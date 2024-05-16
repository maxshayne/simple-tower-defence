using System;
using System.Linq;
using TowerDefence.Enemies;
using UnityEngine;

namespace TowerDefence.Damage
{
    public class AoeBullet : Bullet, IDamage
    {
        public float Speed = 10f;
        public float DamagePower;
        public Transform TargetTransform;
        public float Radius = 20;
        
        public override void Configure(Transform target, Action onDestroy)
        {
            TargetTransform = target;
            _transform = transform;
            _onDestroy = onDestroy;
            _layerMask = LayerMask.GetMask("Enemy");
        }
        
        public override void DoDamage(Enemy enemy)
        {
            enemy.Health -= DamagePower;
            _onDestroy?.Invoke();
        }
        
        private void Update()
        {
            _transform.LookAt(TargetTransform);
            _transform.position = Vector3.MoveTowards(
                _transform.position, TargetTransform.position,
                Speed * Time.deltaTime);
        }

        private Collider[] overlapResults = new Collider[50];
        private void OnTriggerEnter(Collider col)
        {
            var cols = Physics.OverlapSphereNonAlloc(_transform.position, Radius, overlapResults,
                _layerMask);
            Debug.Log(cols);
           foreach (var hitCollider in overlapResults)
           {
               if (hitCollider == null) break;
               if (hitCollider.TryGetComponent<Enemy>(out var enemy))
               {
                   DoDamage(enemy);
               }
           }

            gameObject.SetActive(false);
        }
        
        private Action _onDestroy;
        private Transform _transform;
        private int _layerMask;
    }
}