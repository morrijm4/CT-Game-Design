using UnityEngine;
using System.Collections;

public class TankRespawner : MonoBehaviour
{
    public float respawnDelay = 3f;
    public float invulnerabilityTime = 2f;
    public GameObject tank;
    private Shooter[] shooters;
    private SpriteRenderer[] renderers;
    private Collider2D[] colliders;
    private ParticleSystem[] particleSystems;

    private Vector3 spawnPosition;
    private bool isInvulnerable = false;

    void Awake()
    {
        shooters = tank.GetComponents<Shooter>();
        renderers = tank.GetComponentsInChildren<SpriteRenderer>();
        colliders = tank.GetComponentsInChildren<Collider2D>();
        particleSystems = tank.GetComponentsInChildren<ParticleSystem>();
    }

    void Start()
    {
        this.spawnPosition = this.tank.transform.position;
    }

    public void Die(Collider2D other)
    {
        if (!other.CompareTag("Pellet") || this.isInvulnerable) return;

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
        tank.transform.position = this.spawnPosition;
        SetTankActive(true);

        yield return new WaitForSeconds(this.invulnerabilityTime);
        this.isInvulnerable = false;
    }
}
