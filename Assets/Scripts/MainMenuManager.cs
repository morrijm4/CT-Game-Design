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
    public float minimumLoadTime = 3f; // How many seconds the bar takes to fill

    public void OnPlayClicked()
    {
        StartCoroutine(LoadGameScene());
    }

    public void OnQuitClicked()
    {
        Debug.Log("Quit clicked");
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    IEnumerator LoadGameScene()
    {
        // Show the loading screen
        loadingScreen.SetActive(true);

        // Start loading the game scene in the background
        AsyncOperation operation = SceneManager.LoadSceneAsync("GAL_01");
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

            // Use whichever is slower — so the bar never jumps ahead
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

            yield return null;
        }

        // Small pause at full bar before switching scenes
        yield return new WaitForSeconds(0.5f);

        operation.allowSceneActivation = true;
    }
}