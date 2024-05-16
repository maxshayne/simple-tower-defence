using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.Enemies
{
    public class Enemy : MonoBehaviour
    {
        public Image HealthBar;
        
        public Transform CurrentPoint;
        public float Health
        {
            get => _health;
            set
            {
                if (Math.Abs(_health - value) < 0.001f) return;
                _health = value;
                HealthChangeEvent?.Invoke(_health);
            }
        }

        public int Reward = 30;
        public float StartHealth = 100;
        public float Speed = 1;

        public delegate void OnHealthChangeDelegate(float val);
        public event OnHealthChangeDelegate HealthChangeEvent;

        public void Configure(List<Transform> path)
        {
            _path = path;
            _transform = transform;                
            Health = StartHealth;
            HealthChangeEvent += OnHealthChangeEvent;
        }
        
        private void OnHealthChangeEvent(float val)
        {
            Debug.Log("Health changed by " + val);
            HealthBar.fillAmount = val / StartHealth;
            //  Debug.Log("HealthBar.fillAmount: " + HealthBar.fillAmount);
            if (val <= 0 && isAlive)
            {            
                Die();
            }
        }

        void OnEnable()
        {
            CurrentPoint = _path.First();
            _wayIndex = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if (CurrentPoint == null) return;
            var curPos = _transform.position;
            _transform.position = Vector3.MoveTowards(curPos, CurrentPoint.transform.position, Time.deltaTime * Speed);
            var dist = Vector3.Distance(curPos, CurrentPoint.transform.position);
            if (dist < 1)
                SelectNewWayPoint();
        }

        private void OnTriggerEnter(Collider collider)
        {
            switch (collider.tag)
            {
                case "Endpoint":
                    GameManager.Instance.LifeCount--;
                    Die();
                    break;
            }
        }

        private void SelectNewWayPoint()
        {        
            CurrentPoint = _path[_wayIndex];
            _wayIndex++;
        }

        private void Die()
        {
            isAlive = false;
            GameManager.Instance.Money += Reward;
            GameManager.Instance.EnemyCount--;
            Destroy(gameObject);
        }
        
        private List<Transform> _path;
        private float _health;
        private Transform _transform;
        private int _wayIndex;
        private bool isAlive = true;
    }
}