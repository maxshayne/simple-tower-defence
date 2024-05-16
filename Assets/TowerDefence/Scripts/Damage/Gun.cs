using TowerDefence.Pool;
using UnityEngine;

namespace TowerDefence.Damage
{
    public class Gun : MonoBehaviour
    {
        public GameObject Pivot;

        public void Configure(IPool<Bullet> pool)
        {
            _pool = pool;
        }

        public void Fire(Transform target, float power)
        {
            var ammo = _pool.Take();
            ammo.Configure(target, () =>
            {
                ammo.gameObject.SetActive(false);
                _pool.Return(ammo);
            });
            ShootBullet(target, ammo);
        }

        private void ShootBullet(Transform target, Bullet ammo)
        { 
            ammo.transform.position = Pivot.transform.position;
            ammo.gameObject.SetActive(true);
        }
        
        private GameObject _parent;
        private GameObject _bullet;
        private IPool<Bullet> _pool;
    }
}