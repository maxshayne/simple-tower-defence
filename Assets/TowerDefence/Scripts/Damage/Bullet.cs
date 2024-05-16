using System;
using TowerDefence.Enemies;
using UnityEngine;

namespace TowerDefence.Damage
{
    public abstract class Bullet : MonoBehaviour, IDamage
    {
        public abstract void Configure(Transform target, Action onDestroy);
        public abstract void DoDamage(Enemy enemy);
    }
}