using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [Header("Menu Elements")]
    public GameObject loadingScreen;
    public Image progressBarFill;

    [Header("Loading Settings")]
    public float minimumLoadTime = 0.2f;

    [Header("Audio")]
    public AudioSource menuMusic;
    public AudioSource sfxSource;
    public AudioClip buttonClickSound;

    public void OnPlayClicked()
    {
        PlayClickSound();
        StartCoroutine(LoadGameScene());
    }

    public void OnQuitClicked()
    {
        PlayClickSound();
        StartCoroutine(QuitAfterSound());
    }

    void PlayClickSound()
    {
        if (sfxSource != null && buttonClickSound != null)
        {
            sfxSource.PlayOneShot(buttonClickSound);
        }
    }

    IEnumerator QuitAfterSound()
    {
        // Wait for the click sound to finish before quitting
        yield return new WaitForSeconds(buttonClickSound != null ? buttonClickSound.length : 0.2f);

        Debug.Log("Quit clicked");
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    IEnumerator LoadGameScene()
    {
        // Small delay to let the click sound play
        yield return new WaitForSeconds(0.2f);

        // Store the starting volume for fading
        float startVolume = 0f;
        if (menuMusic != null)
        {
            startVolume = menuMusic.volume;
        }

        // Show the loading screen
        loadingScreen.SetActive(true);

        // Start loading the game scene in the background
        AsyncOperation operation = SceneManager.LoadSceneAsync("Arena");
        operation.allowSceneActivation = false;

        float elapsedTime = 0f;
        float displayedProgress = 0f;

        while (displayedProgress < 1f)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the real loading progress (0 to 1)
            float realProgress = Mathf.Clamp01(operation.progress / 0.9f);

            // Calculate the time-based progress (fills over minimumLoadTime seconds)
            float timedProgress = Mathf.Clamp01(elapsedTime / minimumLoadTime);

            // Use whichever is slower so the bar never jumps ahead
            float targetProgress = Mathf.Min(realProgress, timedProgress);

            // If real loading is done, let the timer catch up smoothly
            if (realProgress >= 1f)
            {
                targetProgress = timedProgress;
            }

            // Smoothly move toward the target progress
            displayedProgress = Mathf.Lerp(displayedProgress, targetProgress, Time.deltaTime * 5f);

            // Snap to 1 when very close
            if (targetProgress >= 0.99f && displayedProgress >= 0.98f)
            {
                displayedProgress = 1f;
            }

            progressBarFill.fillAmount = displayedProgress;

            // Fade out the music as the loading bar fills up
            if (menuMusic != null)
            {
                menuMusic.volume = startVolume * (1f - displayedProgress);
            }

            yield return null;
        }

        // Make sure the music is fully stopped
        if (menuMusic != null)
        {
            menuMusic.volume = 0f;
            menuMusic.Stop();
        }

        // Small pause at full bar before switching scenes
        yield return new WaitForSeconds(0.5f);

        operation.allowSceneActivation = true;
    }
}