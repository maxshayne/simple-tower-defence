using System.Collections.Generic;
using TowerDefence.Pool;
using UnityEngine;

namespace TowerDefence.Damage
{
    public class AmmoPool : MonoBehaviour, IPool<Bullet>
    {
        [SerializeField] private Bullet itemPrefab;
       
        public void Configure()
        {
            _items = new Queue<Bullet>();
            Expand();
        }
        
        public Bullet Take()
        {
            if (_items.TryDequeue(out var item))
                return item;
            Expand();
            return _items.Dequeue();
        }

        public void Return(Bullet item)
        {
            item.gameObject.SetActive(false);
            _items.Enqueue(item);
        }
        
        private void Expand()
        {
            for (int i = 0; i < ExpandSize; i++)
            {
                var item = Instantiate(itemPrefab, transform);
                item.gameObject.SetActive(false);
                _items.Enqueue(item);
            }
        }
        
        private Queue<Bullet> _items;
        private const int ExpandSize = 20;
    }
}