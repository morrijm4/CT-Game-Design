using UnityEngine;
using System.Collections;

public class TankRespawner : MonoBehaviour
{
    public float respawnDelay = 3f;
    public float invulnerabilityTime = 2f;

    private Vector3 spawnPosition;
    private bool isInvulnerable = false;

    // References
    private SpriteRenderer spriteRenderer;
    private Collider2D bodyCollider;

    void Awake()
    {
        Collider2D[] colliders = GetComponents<Collider2D>();

        foreach (var col in colliders)
        {
            if (!col.isTrigger) this.bodyCollider = col;
        }

        this.spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        this.spawnPosition = this.transform.position;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Pellet") || this.isInvulnerable) return;
        else this.Die();
    }

    void Die()
    {
        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        this.spriteRenderer.enabled = false;
        this.bodyCollider.enabled = false;
        yield return new WaitForSeconds(this.respawnDelay);

        // Start invulnerability
        this.isInvulnerable = true;

        // Respawn
        this.transform.position = this.spawnPosition;
        this.spriteRenderer.enabled = true;
        this.bodyCollider.enabled = true;

        yield return new WaitForSeconds(this.invulnerabilityTime);
        this.isInvulnerable = false;
    }
}
