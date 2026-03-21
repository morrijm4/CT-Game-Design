using UnityEngine;
using UnityEngine.UI;

public class Shooter : MonoBehaviour
{
    public CircleCollider2D projectile;
    public Transform muzzle;
    public int count = 0;
    public int maxProjectiles = 0;
    public Text display;

    public float shootCooldown = 1f;

    public bool debug = false;
    private float nextShootTime = 0f;

    public void Shoot()
    {
        if (count <= 0) return;
        if (Time.time < nextShootTime) return;

        Vector3 offset = muzzle.rotation * Vector3.forward * projectile.radius;
        Instantiate(projectile, muzzle.position + offset, muzzle.rotation);
        nextShootTime = Time.time + shootCooldown;
        Decrement();
        if (debug) Debug.Log("Projectile shot. " + count + " left.");
    }

    public int Increment()
    {
        return SetCount(GetCount() + 1);
    }

    public int Decrement()
    {
        return SetCount(GetCount() - 1);
    }

    public int GetCount()
    {
        return count;
    }

    public int SetCount(int newCount)
    {
        if (MaxProjectilesEnabled() && newCount > maxProjectiles)
            count = maxProjectiles;
        else if (newCount < 0)
            count = 0;
        else
            count = newCount;

        UpdateDisplay();

        return count;
    }

    bool MaxProjectilesEnabled()
    {
        return maxProjectiles != 0;
    }

    void UpdateDisplay()
    {
        if (!display) return;
        display.text = "x " + GetCount().ToString();
    }

    void OnValidate()
    {
        if (shootCooldown < 0f)
            shootCooldown = 0f;
        if (projectile == null)
            Debug.LogError("Projectile must be defined!");
        if (muzzle == null)
            Debug.LogError("Muzzle must be defined!");
        if (count > maxProjectiles)
            count = maxProjectiles;
        if (count < 0)
            count = 0;
    }
}
