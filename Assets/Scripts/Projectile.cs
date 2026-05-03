using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 0.5f;
    public bool debug = false;
    public int maxBounces = 1;
    public GameObject explosion;
    private int bounceCount = 0;
    private Rigidbody2D rb;

    void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        this.rb.linearVelocity = transform.up * this.speed * -1;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (debug) Debug.Log("Bullet Collision" + other.collider.tag);
        if (other.collider.CompareTag("ConsumeArea")) return;

        if (bounceCount++ >= maxBounces || other.collider.CompareTag("Player"))
        {
            Destroy(gameObject);
            if (explosion) Instantiate(explosion, transform.position, Quaternion.identity);
        }
        else
        {
            Vector2 normal = other.contacts[0].normal;
            rb.linearVelocity = Vector2.Reflect(rb.linearVelocity, normal);
        }
    }
}
