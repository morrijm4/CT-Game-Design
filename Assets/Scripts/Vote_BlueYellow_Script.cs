using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class CeasefireManager : MonoBehaviour
{
    [Header("Panels")]
    public Image leftPanel;
    public Image rightPanel;

    [Header("Timer")]
    public TextMeshProUGUI timerText;
    public float countdownDuration = 5f;

    [Header("Result")]
    public GameObject resultBanner;
    public TextMeshProUGUI resultText;

    [Header("Score Display")]
    public TextMeshProUGUI BlueScore;
    public TextMeshProUGUI YellowScore;

    [Header("Colors")]
    public Color neutralColor = Color.green;
    public Color greenColor = Color.green;
    public Color redColor = Color.red;

    public static int BluePlayerScore { get; private set; }
    public static int YellowPlayerScore { get; private set; }

    public static CeasefireManager Instance { get; private set; }

    enum Choice { None, Cooperate, Attack }

    private Choice BluePlayerChoice = Choice.None;
    private Choice YellowPlayerChoice = Choice.None;
    private bool gameOver = false;
    private float timeLeft;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        ResetGame();
    }

    void Update()
    {
        if (gameOver) return;

        ReadInput();

        timeLeft -= Time.deltaTime;
        UpdateTimerDisplay();

        if (timeLeft <= 0)
        {
            timeLeft = 0;
            UpdateTimerDisplay();
            EndGame();
        }
    }

    void ReadInput()
    {
        if (Gamepad.all.Count > 0 && BluePlayerChoice == Choice.None)
        {
            if (Gamepad.all[0].buttonWest.wasPressedThisFrame)
                BluePlayerChoice = Choice.Cooperate;  // X button
            else if (Gamepad.all[0].buttonEast.wasPressedThisFrame)
                BluePlayerChoice = Choice.Attack;     // B button
        }

        if (Gamepad.all.Count > 1 && YellowPlayerChoice == Choice.None)
        {
            if (Gamepad.all[1].buttonWest.wasPressedThisFrame)
                YellowPlayerChoice = Choice.Cooperate;  // X button
            else if (Gamepad.all[1].buttonEast.wasPressedThisFrame)
                YellowPlayerChoice = Choice.Attack;     // B button
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerText.color = timeLeft <= 2f ? Color.red : Color.white;
    }

    void EndGame()
    {
        gameOver = true;

        // Default to cooperate if no input was made
        if (BluePlayerChoice == Choice.None) BluePlayerChoice = Choice.Cooperate;
        if (YellowPlayerChoice == Choice.None) YellowPlayerChoice = Choice.Cooperate;

        // Apply panel colors
        leftPanel.color = BluePlayerChoice == Choice.Cooperate ? greenColor : redColor;
        rightPanel.color = YellowPlayerChoice == Choice.Cooperate ? greenColor : redColor;

        // Calculate scores and result message
        string message = "";

        if (BluePlayerChoice == Choice.Cooperate && YellowPlayerChoice == Choice.Cooperate)
        {
            BluePlayerScore = 3;
            YellowPlayerScore = 3;
            message = "CEASEFIRE!\nBoth players cooperate.";
        }
        else if (BluePlayerChoice == Choice.Cooperate && YellowPlayerChoice == Choice.Attack)
        {
            BluePlayerScore = 1;
            YellowPlayerScore = 5;
            message = "BETRAYAL!\nP2 attacks, P1 cooperates.";
        }
        else if (BluePlayerChoice == Choice.Attack && YellowPlayerChoice == Choice.Cooperate)
        {
            BluePlayerScore = 5;
            YellowPlayerScore = 1;
            message = "BETRAYAL!\nP1 attacks, P2 cooperates.";
        }
        else
        {
            BluePlayerScore = 2;
            YellowPlayerScore = 2;
            message = "WAR!\nBoth players attack.";
        }

        // Display scores on panels
        if (BlueScore != null) BlueScore.text = BluePlayerScore.ToString();
        if (YellowScore != null) YellowScore.text = YellowPlayerScore.ToString();

        // Show result banner
        resultBanner.SetActive(true);
        resultText.text = message;

        Debug.Log("P1 Score: " + BluePlayerScore + " | P2 Score: " + YellowPlayerScore);
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Vote_BlueGreen");
    }
    public void ResetGame()
    {
        BluePlayerChoice = Choice.None;
        YellowPlayerChoice = Choice.None;
        gameOver = false;
        timeLeft = countdownDuration;

        leftPanel.color = neutralColor;
        rightPanel.color = neutralColor;

        resultBanner.SetActive(false);

        if (BlueScore != null) BlueScore.text = "";
        if (YellowScore != null) YellowScore.text = "";
    }
}