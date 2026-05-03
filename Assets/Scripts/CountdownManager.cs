using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CountdownManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject countdownOverlay;
    public TextMeshProUGUI countdownText;
    [Header("Settings")]
    public int countdownFrom = 3;
    public float timeBetweenNumbers = 1f;
    public string goText = "GO!";
    public float goDisplayTime = 0.5f;
    [Header("Audio")]
    public AudioSource sfxSource;
    public AudioClip countdownBeep;
    public AudioClip goSound;
    public AudioSource arenaMusic;

    void OnEnable()
    {
        Time.timeScale = 0f;
        StartCoroutine(RunCountdown());
    }

    IEnumerator RunCountdown()
    {
        for (int i = countdownFrom; i > 0; i--)
        {
            countdownText.text = i.ToString();
            if (sfxSource != null && countdownBeep != null)
                sfxSource.PlayOneShot(countdownBeep);

            countdownText.transform.localScale = Vector3.one * 1.5f;
            float elapsed = 0f;
            while (elapsed < timeBetweenNumbers)
            {
                elapsed += Time.unscaledDeltaTime;
                float scale = Mathf.Lerp(1.5f, 1f, elapsed / timeBetweenNumbers);
                countdownText.transform.localScale = Vector3.one * scale;
                yield return null;
            }
        }

        countdownText.text = goText;
        if (sfxSource != null && goSound != null)
            sfxSource.PlayOneShot(goSound);

        countdownText.transform.localScale = Vector3.one * 1.5f;
        float goElapsed = 0f;
        while (goElapsed < goDisplayTime)
        {
            goElapsed += Time.unscaledDeltaTime;
            float scale = Mathf.Lerp(1.5f, 1f, goElapsed / goDisplayTime);
            countdownText.transform.localScale = Vector3.one * scale;
            yield return null;
        }

        Time.timeScale = 1f;
        countdownOverlay.SetActive(false);

        if (arenaMusic != null)
            arenaMusic.Play();
    }
}