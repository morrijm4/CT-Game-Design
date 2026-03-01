using UnityEngine;

public class Shooter : MonoBehaviour
{
    public CircleCollider2D projectile;
    public Transform muzzle;
    public int count = 0;
    public int maxProjectiles = 0;

    public bool debug = false;

    public void Shoot()
    {
        if (count <= 0) return;
        Vector3 offset = transform.rotation * Vector3.forward * projectile.radius;
        Instantiate(projectile, muzzle.position + offset, transform.rotation);
        count -= 1;
        if (debug) Debug.Log("Projectile shot. " + count + " left.");
    }

    public int Increment()
    {
        return SetCount(GetCount() + 1);
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

        return count;
    }

    bool MaxProjectilesEnabled()
    {
        return maxProjectiles != 0;
    }

    void OnValidate()
    {
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
