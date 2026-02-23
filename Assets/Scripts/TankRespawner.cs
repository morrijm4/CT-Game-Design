using UnityEngine;
using System.Collections;

public class TankRespawner : MonoBehaviour
{
    public float respawnDelay = 3f;
    public float invulnerabilityTime = 2f;

    private Vector3 spawnPosition;
    private bool isInvulnerable = false;

    // References
    private SpriteRenderer sr;
    private Collider2D body;

    void Awake()
    {
        Collider2D[] colliders = GetComponents<Collider2D>();

        foreach (var col in colliders)
        {
            if (!col.isTrigger) this.body = col;
        }

        this.sr = GetComponentInChildren<SpriteRenderer>();
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
        this.sr.enabled = false;
        this.body.enabled = false;
        yield return new WaitForSeconds(this.respawnDelay);

        // Start invulnerability
        this.isInvulnerable = true;

        // Respawn
        this.transform.position = this.spawnPosition;
        this.sr.enabled = true;
        this.body.enabled = true;

        yield return new WaitForSeconds(this.invulnerabilityTime);
        this.isInvulnerable = false;
    }
}
