using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Respawner : MonoBehaviour
{
    public float respawnDelay = 3f;
    public float invulnerabilityTime = 5f;
    public GameObject player;
    public Text display;
    private playerController playerController;
    private Shooter[] shooters;
    private SpriteRenderer[] renderers;
    private Collider2D[] colliders;
    private ParticleSystem[] particleSystems;
    private int count = 0;

    private Vector3 spawnPosition;
    private bool isInvulnerable = false;

    void Awake()
    {
        playerController = player.GetComponent<playerController>();
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

        StartCoroutine(RespawnRoutine());

        if (playerController.isCarryingObject) playerController.GrabDrop();

        foreach (var shooter in shooters)
        {
            shooter.SetCount(0);
        }

        count++;
        if (display) display.text = "x " + count.ToString();
    }

    void SetTankActive(bool enabled)
    {
        playerController.disableGrabbing = !enabled;
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
