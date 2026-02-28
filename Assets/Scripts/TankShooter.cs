using UnityEngine;

public class TankShooter : MonoBehaviour
{
    public GameObject pellet;
    public GameObject bomb;
    public Transform muzzle;
    public int maxPellets = 10;
    private playerController playerController;

    void Awake()
    {
        this.playerController = GetComponent<playerController>();
    }

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
        if (this.playerController == null)
        {
            Debug.LogError("playerController is not attached!");
            return;
        }

        if (this.playerController.capital > this.maxPellets)
            this.playerController.capital = maxPellets;

        if (this.playerController.bombs > 0)
        {
            this.ShootBomb();
        }
        else if (this.playerController.capital > 0)
        {
            Instantiate(this.pellet, this.muzzle.position, this.transform.rotation);
            this.playerController.capital -= 1;
        }
    }

    public void ShootBomb()
    {
        if (this.bomb == null)
        {
            Debug.LogError("Bomb is not defined!");
            return;
        }
        if (this.muzzle == null)
        {
            Debug.LogError("Muzzle is not defined!");
            return;
        }
        if (this.playerController == null)
        {
            Debug.LogError("playerController is not attached!");
            return;
        }

        if (this.playerController.bombs > 0)
        {
            Instantiate(this.bomb, this.muzzle.position, this.transform.rotation);
            this.playerController.bombs -= 1;
        }
    }
}
