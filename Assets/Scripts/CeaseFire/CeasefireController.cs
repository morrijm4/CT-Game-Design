using System.Collections.Generic;
using UnityEngine;

public class CeasefireController : MonoBehaviour
{
    public GameObject ceasefireUI;
    public int threshold = 2;
    private int _count = 0;
    private HashSet<int> _playerIds = new HashSet<int>();

    private void OnValidate()
    {
        if (ceasefireUI == null)
        {
            Debug.LogWarning("Ceasefire UI reference is not set in the inspector.");
        }
    }

    public void Clear()
    {
        _count = 0;
        _playerIds.Clear();
    }

    public void Increment(int playerId)
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

    void BeginCeasefire()
    {
        ceasefireUI.SetActive(true);
        // Deactivate player controls
    }

    public void EndCeaseFire()
    {         
        ceasefireUI.SetActive(false);
        Clear();
        // Reactivate player controls
    }
}
