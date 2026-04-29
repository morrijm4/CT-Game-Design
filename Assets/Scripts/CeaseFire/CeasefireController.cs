using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CeasefireController : MonoBehaviour
{
    public GameObject ceasefireUI;
    public GameObject gameOverUI;
    public Respawner respawner;
    public int threshold = 2;
    public List<Tank> tanks;
    public int totalPlayers = 4;
    public float ceasefireCooldown = 5f;
    public TextMeshProUGUI countdown;
    private int _count = 0;
    private HashSet<int> _playerIds = new HashSet<int>();
    private float _ceasefireTimer = 0f;

    private void OnValidate()
    {
        if (ceasefireUI == null)
        {
            Debug.LogWarning("Ceasefire UI reference is not set in the inspector.");
        }
    }

    private void Start()
    {
        StartCooldown();
    }

    private void Update()
    {
        if (_ceasefireTimer > 0)
        {
            _ceasefireTimer -= Time.deltaTime;
            UpdateDisplay();
            return;
        }


        if (ceasefireUI.activeSelf) return;

        int livePlayers = 0;
        for (int playerId = 0; playerId < totalPlayers; playerId++)
        {
            if (tanks[playerId].health.lives <= 0) continue;

            if (Gamepad.all.Count > playerId && Gamepad.all[playerId].buttonNorth.wasPressedThisFrame)
            {
                Ceasefire(playerId);
            }

            livePlayers++;
        }

        if (gameOverUI != null && livePlayers <= 1)
        {
            gameOverUI.SetActive(true);
        }
    }

    public void Clear()
    {
        _count = 0;
        _playerIds.Clear();
    }

    public void Ceasefire(int playerId)
    {
        if (_playerIds.Contains(playerId)) return;

        _playerIds.Add(playerId);
        _count++;

        if (isThresholdReached())
        {
            BeginCeasefire();
        }
    }

    bool isThresholdReached()
    {
        return _count >= threshold;
    }

    void SetTanksActive(bool active)
    {
        foreach (var tank in tanks)
        {
            if (tank.health.lives > 0)
            {
                tank.entity.SetActive(active);
                if (active)
                {
                    tank.input.ActivateInput();
                }
                else
                {
                    tank.input.DeactivateInput();
                }
            }
        }
    }

    void BeginCeasefire()
    {
        ceasefireUI.SetActive(true);
        SetTanksActive(false);
    }

    public void EndCeaseFire()
    {
        ceasefireUI.SetActive(false);
        Clear();
        respawner.RespawnTanks();
        SetTanksActive(true);
        StartCooldown();
    }

    void StartCooldown()
    {
        _ceasefireTimer = ceasefireCooldown;
    }

    void UpdateDisplay()
    {
        if (countdown == null) return;

        if (_ceasefireTimer > 0)
        {
            countdown.text = $"Ceasefire in: {Mathf.CeilToInt(_ceasefireTimer)}s";
        }
        else
        {
            countdown.text = "Press Y to call Ceasefire";
        }
    }
}


[System.Serializable]
public class Tank
{
    public GameObject entity;
    public Health health;
    public PlayerInput input;
}