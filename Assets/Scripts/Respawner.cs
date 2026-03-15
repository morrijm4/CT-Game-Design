using UnityEngine;
using System.Collections;

public class Respawner : MonoBehaviour
{
    public float respawnDelay = 3f;
    public float invulnerabilityTime = 2f;
    public playerController player;
    private Shooter[] shooters;
    private SpriteRenderer[] renderers;
    private Collider2D[] colliders;
    private ParticleSystem[] particleSystems;

    private Vector3 spawnPosition;
    private bool isInvulnerable = false;

    void Awake()
    {
        shooters = player.GetComponentsInParent<Shooter>();
        renderers = player.GetComponentsInChildren<SpriteRenderer>();
        colliders = player.GetComponentsInChildren<Collider2D>();
        particleSystems = player.GetComponentsInChildren<ParticleSystem>();
    }

    void Start()
    {
        this.spawnPosition = this.player.transform.position;
    }

    public void Die(Collider2D other)
    {
        if (!other.CompareTag("Pellet") || this.isInvulnerable) return;

        if (player.isCarryingObject) player.GrabDrop();

        StartCoroutine(RespawnRoutine());

        foreach (var shooter in shooters)
        {
            shooter.SetCount(0);
        }
    }

    void SetTankActive(bool enabled)
    {
        foreach (var renderer in renderers)
        {
            renderer.enabled = enabled;
        }
        foreach (var collider in colliders)
        {
            collider.enabled = enabled;
        }
        foreach (var system in particleSystems)
        {
            system.gameObject.SetActive(enabled);
        }
    }

    IEnumerator RespawnRoutine()
    {
        this.isInvulnerable = true;
        SetTankActive(false);

        yield return new WaitForSeconds(this.respawnDelay);
        player.transform.position = this.spawnPosition;
        SetTankActive(true);

        yield return new WaitForSeconds(this.invulnerabilityTime);
        this.isInvulnerable = false;
    }
}
