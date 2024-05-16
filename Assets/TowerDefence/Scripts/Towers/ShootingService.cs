using System.Collections;
using TowerDefence.Damage;
using TowerDefence.Enemies;
using TowerDefence.Pool;
using UnityEngine;

namespace TowerDefence.Towers
{
    public class ShootingService
    {
       // public Enemy CurrentEnemy { get; set; }
        
        public void Configure(MonoBehaviour coroutineRunner, Transform view, AmmoPool pool)
        {
            _runner = coroutineRunner;
            _view = view;
            _guns = _view.GetComponentsInChildren<Gun>();
            foreach (var gun in _guns)
            {
                gun.Configure(pool);
            }
        }
 
        
        public void DoUpdate(Enemy currentEnemy)
        {
            if (currentEnemy != null && currentEnemy.gameObject.activeSelf)
            {
                var lookPos = currentEnemy.transform.position - _view.transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                _view.transform.rotation = Quaternion.Slerp(
                    _view.transform.rotation, rotation, Time.deltaTime * RotationSpeed);
                if (_currentFireCooldown >= FireCooldown)
                {
                    _currentFireCooldown = 0f;
                    Debug.Log("FIRE!");
                    Fire(currentEnemy);
                }
                else
                {
                    _currentFireCooldown += Time.deltaTime;
                }
                CheckDistance(currentEnemy); 
            }
        }
        
        private void CheckDistance(Enemy currentEnemy)
        {
            var dist = Vector3.Distance(_view.position, currentEnemy.transform.position);
            if (dist > _shootingRadius)
            {
                currentEnemy = null;
            }
        }
        
        private void Fire(Enemy currentEnemy)
        {
            foreach (var gun in _guns)
            {
                gun.Fire(currentEnemy.transform, DamagePower);
            } 
        }

        private float _shootingRadius = 50f;
        private float _currentFireCooldown;
        public float FireCooldown = 1f;
        public float DamagePower = 10f;
        public float RotationSpeed = 2f;
        private Transform _view;
        private MonoBehaviour _runner;
        private Gun[] _guns;
    }
}