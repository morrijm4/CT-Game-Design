using UnityEngine;

public class TankShooter : MonoBehaviour
{
    public GameObject pellet;
    public Transform muzzle;

    public void ShootPellet()
    {
        if (this.pellet == null)
        {
            Debug.LogError("Pellet is not defined!");
            return;
        }
        if (this.muzzle == null)
        {
            Debug.LogError("Muzzle is not defined!");
            return;
        }

        Instantiate(this.pellet, this.muzzle.position, this.transform.rotation);
    }
}
