using System;
using TowerDefence.Enemies;
using UnityEngine;

namespace TowerDefence.Damage
{
    public class SimpleBullet : Bullet
    {
        public float Speed = 10f;
        public float DamagePower;

        public override void Configure(Transform target, Action onDestroy)
        {
            _target = target;
            _transform = transform;
          
            _targetPosition = target.position;
            _onDestroy = onDestroy;
        }
        
        public override void DoDamage(Enemy enemy)
        {
            enemy.Health -= DamagePower;
            gameObject.SetActive(false);
            _onDestroy?.Invoke();
        }
        
        private void Update()
        {
            _transform.LookAt(_target);
            _transform.position = Vector3.MoveTowards(
                _transform.position, _targetPosition,
                Speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider col)
        {
            Debug.Log($"Bullet triggered with {col.name}");
            if (!col.TryGetComponent<Enemy>(out var enemy)) 
                return;
            DoDamage(enemy);
        }
        
        private Transform _target;
        private Vector3 _targetPosition;
        private Action _onDestroy;
        private Transform _transform;
    }
}