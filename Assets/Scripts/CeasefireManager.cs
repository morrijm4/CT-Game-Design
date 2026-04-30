using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;

public class CeasefireManager : MonoBehaviour
{
    public CeasefireController ceasefireController;

    [Header("Panels")]
    public Image leftPanel;
    public Image rightPanel;

    [Header("Tank Colors")]
    public Image leftTankImage;
    public Image leftBomb;
    public Image rightTankImage;
    public Image rightBomb;

    [Header("Timer")]
    public TextMeshProUGUI timerText;
    public float countdownDuration = 5f;
    public float delay = 3f;

    [Header("Result")]
    public GameObject resultBanner;
    public TextMeshProUGUI resultText;

    [Header("Score Display")]
    public TextMeshProUGUI leftScore;
    public TextMeshProUGUI leftTanksRemain;
    public TextMeshProUGUI rightScore;
    public TextMeshProUGUI rightTanksRemain;

    [Header("Colors")]
    public Color neutralColor = Color.green;
    public Color greenColor = Color.green;
    public Color redColor = Color.red;

    [Header("Tanks")]
    public List<TankData> tanks;

    public bool debug = false;

    public static CeasefireManager Instance { get; private set; }

    enum Choice { None, Cooperate, Attack }

    private Choice leftPlayerChoice = Choice.None;
    private Choice rightPlayerChoice = Choice.None;
    private bool gameOver = false;
    private float timeLeft;
    private List<(int, int)> _pairs = new List<(int, int)>();

    void Awake()
    {
        Instance = this;
    }


    void OnEnable()
    {
        RemoveDeadTanks();
        GeneratePairs();
        ShufflePairs();
        ResetGame();
    }

    void Update()
    {
        if (gameOver) return;

        ReadInput();

        timeLeft -= Time.deltaTime;
        UpdateTimerDisplay();

        if (timeLeft <= 0 || (leftPlayerChoice != Choice.None && rightPlayerChoice != Choice.None))
        {
            timeLeft = 0;
            UpdateTimerDisplay();
            EndGame();
        }
    }

    void ReadInput()
    {
        int lhs = GetLHS();
        int rhs = GetRHS();

        if (Gamepad.all.Count > lhs && leftPlayerChoice == Choice.None)
        {
            if (Gamepad.all[lhs].buttonWest.wasPressedThisFrame)
                leftPlayerChoice = Choice.Cooperate;  // X button
            else if (Gamepad.all[lhs].buttonEast.wasPressedThisFrame)
                leftPlayerChoice = Choice.Attack;     // B button
        }

        if (Gamepad.all.Count > rhs && rightPlayerChoice == Choice.None)
        {
            if (Gamepad.all[rhs].buttonWest.wasPressedThisFrame)
                rightPlayerChoice = Choice.Cooperate;  // X button
            else if (Gamepad.all[rhs].buttonEast.wasPressedThisFrame)
                rightPlayerChoice = Choice.Attack;     // B button
        }

        if (debug) Debug.Log($"P1 Choice: {leftPlayerChoice} | P2 Choice: {rightPlayerChoice}");
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
        if (leftPlayerChoice == Choice.None) leftPlayerChoice = Choice.Cooperate;
        if (rightPlayerChoice == Choice.None) rightPlayerChoice = Choice.Cooperate;

        if (debug) Debug.Log($"Final Choices - P1: {leftPlayerChoice}, P2: {rightPlayerChoice}");

        // Apply panel colors
        leftPanel.color = leftPlayerChoice == Choice.Cooperate ? greenColor : redColor;
        rightPanel.color = rightPlayerChoice == Choice.Cooperate ? greenColor : redColor;

        // Calculate scores and result message
        string message = "";

        int lhsScore = 0;
        int rhsScore = 0;

        if (leftPlayerChoice == Choice.Cooperate && rightPlayerChoice == Choice.Cooperate)
        {
            lhsScore = 3;
            rhsScore = 3;
            message = "CEASEFIRE!\nBoth players cooperate.";
        }
        else if (leftPlayerChoice == Choice.Cooperate && rightPlayerChoice == Choice.Attack)
        {
            lhsScore = 0;
            rhsScore = 5;
            message = "BETRAYAL!\nP2 attacks, P1 cooperates.";
        }
        else if (leftPlayerChoice == Choice.Attack && rightPlayerChoice == Choice.Cooperate)
        {
            lhsScore = 5;
            rhsScore = 0;
            message = "BETRAYAL!\nP1 attacks, P2 cooperates.";
        }
        else
        {
            lhsScore = 1;
            rhsScore = 1;
            message = "WAR!\nBoth players attack.";
        }

        tanks[GetLHS()].shooter.Add(lhsScore); 
        tanks[GetRHS()].shooter.Add(rhsScore);

        // Display scores on panels
        UpdateScores();

        // Show result banner
        // resultBanner.SetActive(true);
        // resultText.text = message;

        if (debug) Debug.Log("P1 Score: " + lhsScore + " | P2 Score: " + rhsScore);

        _pairs.RemoveAt(0);

        if (_pairs.Count == 0)
        {
            StartCoroutine(EndCeasefireAfterDelay());
        } else
        {
            StartCoroutine(ResetGameAfterDelay());
        }
    }

    int GetLHS()
    {
        return _pairs[0].Item1;
    }

    int GetRHS()
    {
        return _pairs[0].Item2;
    }



    public void ResetGame()
    {
        leftPlayerChoice = Choice.None;
        rightPlayerChoice = Choice.None;
        gameOver = false;
        timeLeft = countdownDuration;

        leftPanel.color = neutralColor;
        rightPanel.color = neutralColor;

        // resultBanner.SetActive(false);

        int lhs = GetLHS();
        int rhs = GetRHS();

        if (debug) Debug.Log($"Starting new round: Tank {lhs} vs Tank {rhs}");

        Color lhsColor = tanks[lhs].renderer.color;
        Color rhsColor = tanks[rhs].renderer.color;
        
        leftTankImage.color = lhsColor;
        rightTankImage.color = rhsColor;

        leftBomb.color = lhsColor;
        rightBomb.color = rhsColor;

        leftTanksRemain.text = tanks[lhs].health.lives.ToString();
        rightTanksRemain.text = tanks[rhs].health.lives.ToString();

        UpdateScores();
    }

    void UpdateScores()
    {
        int lhs = GetLHS();
        int rhs = GetRHS();
        if (leftScore != null) leftScore.text = tanks[lhs].shooter.GetCount().ToString();
        if (rightScore != null) rightScore.text = tanks[rhs].shooter.GetCount().ToString();
    }

    void RemoveDeadTanks()
    {
        tanks.RemoveAll(t => t.health.lives <= 0);
    }

    void GeneratePairs()
    {
        _pairs.Clear();
        for (int i = 0; i < tanks.Count; i++)
        {
            for (int j = i + 1; j < tanks.Count; j++)
            {
                _pairs.Add((i, j));
            }
        }
    }

    void ShufflePairs()
    {
        for (int i = _pairs.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (_pairs[i], _pairs[randomIndex]) = (_pairs[randomIndex], _pairs[i]);
        }
    }

    IEnumerator ResetGameAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        ResetGame();
    }

    IEnumerator EndCeasefireAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        ceasefireController.EndCeaseFire();
    }
}


[System.Serializable]
public class TankData
{
    public PelletShooter shooter;
    public Health health;
    public SpriteRenderer renderer;
}