using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Respawner : MonoBehaviour
{
    public List<Position> tanks;
    public Transform initialTurret;
    private List<StartPosition> positions = new List<StartPosition>();

    public void AddSpwawnPosition(StartPosition position)
    {
        positions.Add(position);
    }

    public void RespawnTanks()
    {
        ShufflePositions();
        for (int i = 0; i < tanks.Count && i < positions.Count; i++)
        {
            tanks[i].entity.transform.position = positions[i].entityPosition;
            tanks[i].entity.transform.rotation = positions[i].entityRotation;
            tanks[i].turret.transform.rotation = positions[i].turretRotation;
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

[System.Serializable]
public class Position
{
    public GameObject entity;
    public GameObject turret;
}

[System.Serializable]
public class StartPosition
{
    public Vector3 entityPosition;
    public Quaternion entityRotation;
    public Quaternion turretRotation;
}