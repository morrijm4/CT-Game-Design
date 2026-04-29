using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using NUnit.Framework;

public class Respawner : MonoBehaviour
{
    public List<GameObject> tanks;
    private List<Vector3> positions = new List<Vector3>();

    public void AddSpwawnPosition(Vector3 position)
    {
        positions.Add(position);
    }

    public void RespawnTanks()
    {
        ShufflePositions();
        for (int i = 0; i < tanks.Count && i < positions.Count; i++)
        {
            tanks[i].transform.position = positions[i];
        }
    }

    void ShufflePositions()
    {
        for (int i = positions.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (positions[i], positions[j]) = (positions[j], positions[i]);
        }
    }
}
