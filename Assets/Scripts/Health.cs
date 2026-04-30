using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;




#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

public class Health : MonoBehaviour
{
    public GameObject obj;

    public PlayerInput input;
    public Respawner respawner;
    public GameObject explosion;
    public AudioClip explosionSound;
    public Text display;
    public int startingHealth = 1;
    public int lives = 3;
    private int health;


    void OnValidate()
    {
        if (obj == null) Debug.LogError("No game object defined on Health");
    }

    void Start()
    {
        health = startingHealth;
        UpdateDisplay();
        respawner.AddSpwawnPosition(transform.position);
    }

    public void OnHit(Collider2D collider)
    {
        if (!collider.CompareTag("Pellet")) return;
        if (health > 0) health--;

        if (health == 0 && obj != null)
        {
            Die();
        }

    }

    public void Die()
    {
        obj.SetActive(false);
        input.DeactivateInput();
        lives--;

        if (explosion) Instantiate(explosion, obj.transform.position, Quaternion.identity);
        if (explosionSound) AudioSource.PlayClipAtPoint(explosionSound, obj.transform.position, 1f);

        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        if (display) display.text = lives.ToString();
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
