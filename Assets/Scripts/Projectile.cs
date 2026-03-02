using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 0.5f;
    public bool debug = false;
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
        if (other.CompareTag("ConsumeArea")) return;
        Destroy(gameObject);
        if (debug) Debug.Log("Bullet Destroyed");
    }
}
