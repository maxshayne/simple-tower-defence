using System;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GunType Type;

    public float BulletSpeed = 6f;

    public GameObject Pivot;

    public GameObject BulletPrefab;

    private GameObject _parent;

    private GameObject _bullet;

	// Use this for initialization
	void Start ()
	{
	    _parent = GameObject.FindGameObjectWithTag("BulletStorage");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Fire(Transform target, float power)
    {
        switch (Type)
        {
            case GunType.Gun:
            case GunType.LaserGun:
                var selected = false;
                foreach (Transform child in _parent.transform)
                {
                    if (child.gameObject.activeSelf) continue;
                    ShootBullet(target, power, child);
                    selected = true;
                    break;
                }

                if (!selected)
                {
                    var obj = Instantiate(BulletPrefab, transform);
                    ShootBullet(target, power, obj.transform);
                }
                break;
            case GunType.Cannon:
                var obj1 = Instantiate(BulletPrefab, null);
                ShootBullet(target, power, obj1.transform);
                break;
        }
    }

    private void ShootBullet(Transform target, float power, Transform child)
    {
        var obj = child.gameObject;
        obj.transform.position = Pivot.transform.position;
        var bullet = obj.GetComponent<Bullet>();
        bullet.TargetTransform = target;
        bullet.DamagePower = power;
        bullet.AoE = Type == GunType.Cannon;
        obj.SetActive(true);
    }
}

public enum GunType
{
    Gun,
    LaserGun,
    Cannon
}