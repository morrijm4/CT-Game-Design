using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneFadeIn : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private Image overlay;

    void Awake()
    {
        if (overlay == null)
            overlay = GetComponent<Image>();

        overlay.color = new Color(0, 0, 0, 1f);
        overlay.raycastTarget = false;
    }

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            overlay.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        overlay.color = new Color(0, 0, 0, 0f);
        gameObject.SetActive(false);
    }
}