using UnityEngine;

public class Pellet : MonoBehaviour
{
    public float speed = 0.5f;
    private Rigidbody2D rb;

    void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        this.rb.linearVelocity = transform.up * this.speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit: " + other.CompareTag("Player"));
        Destroy(gameObject);
    }
}
