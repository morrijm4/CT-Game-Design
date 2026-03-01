using UnityEngine;

public class ProjectileSupplier : MonoBehaviour
{
    public int bombCost = 4;

    public void AddPellet(playerController player)
    {
        player.GetComponent<PelletShooter>()?.Increment();
    }
    public void AddBomb(playerController player)
    {
        PelletShooter pelletShooter = player.GetComponent<PelletShooter>();
        if (pelletShooter == null)
        {
            Debug.LogError("Cannot find pellet shooter");
            return;
        }
        int pellets = pelletShooter.GetCount();
        if (pellets < bombCost) return;
        pelletShooter.SetCount(pellets - bombCost);
        player.GetComponentInParent<BombShooter>()?.Increment();
    }
}
