using System.Collections;
using System.Linq;
using TowerDefence.Enemies;
using UnityEngine;

namespace TowerDefence.Towers
{
    public class EnemyLocatorService
    {
        public Enemy CurrentEnemy { get; set; }
        
        public void Configure(MonoBehaviour coroutineRunner, Transform view)
        {
            _runner = coroutineRunner;
            _view = view;
        }
        
        public void DoUpdate()
        {
            if (CurrentEnemy != null)
                return;
            FindEnemies();
            Rotate();
        }
        

        private void Rotate()
        {
            if (CurrentEnemy != null)
                return;
            _view.Rotate(Vector3.up * (20 * Time.deltaTime));
        }

        private void FindEnemies()
        {
            var hitColliders = Physics.OverlapSphere(_view.position, SearchRadius).Where(obj=>obj.tag == "Enemy");
            var maxDist = float.MaxValue;
            foreach (var hitCollider in hitColliders)
            {
                var dist = Vector3.Distance(_view.position, hitCollider.transform.position);
                if (!(dist < maxDist)) continue;
                maxDist = dist;
                CurrentEnemy = hitCollider.gameObject.GetComponent<Enemy>();
                break;
            }
        }
        
        private MonoBehaviour _runner;
        private Transform _view;
        private float SearchRadius = 50f;
    }
}