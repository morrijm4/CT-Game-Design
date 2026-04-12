using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

public class Health : MonoBehaviour
{
    public GameObject obj;
    public Text display;
    public int health = 20;

    void OnValidate()
    {
        if (obj == null) Debug.LogError("No game object defined on Health");
    }

    void Start()
    {
        UpdateDisplay();
    }

    public void OnHit(Collider2D collider)
    {
        if (!collider.CompareTag("Pellet")) return;

        if (health > 0) health--;

        if (health == 0 && obj != null)
        {
            Destroy(obj);
            obj = null;
        }

        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        if (display) display.text = health.ToString();
    }

#if UNITY_EDITOR
    void OnEnable()
    {
        EditorApplication.pauseStateChanged += OnPauseStateChanged;
    }

    void OnDisable()
    {
        EditorApplication.pauseStateChanged -= OnPauseStateChanged;
    }

    void OnPauseStateChanged(PauseState state)
    {
        UpdateDisplay();
    }
#endif // UNITY_EDITOR
}
