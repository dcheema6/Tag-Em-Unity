using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] float rateOfFire;
    [SerializeField] Projectile projectile;

    [HideInInspector]
    public Transform muzzle;

    float nextFireAllowed;
    public bool canFire;

    void Awake()
    {
        muzzle = transform.Find("Muzzle");

        if (muzzle == null)
            muzzle = transform;
    }

    public virtual void Fire()
    {
        if(Time.time < nextFireAllowed || !canFire)
            return;

        nextFireAllowed = Time.time + rateOfFire;

        Instantiate(projectile, muzzle.position, muzzle.rotation);
    }
}
