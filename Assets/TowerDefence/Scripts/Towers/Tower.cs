using TowerDefence.Damage;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TowerDefence.Towers
{
    public class Tower : MonoBehaviour
    {
        public int TowerCost = 100;
        public int TowerLevel = 1;
        public float SearchRadius = 50f;
        public float ShootingRadius = 50f;
        public float RotationSpeed = 2f;
        public float FireCooldown = 1f;
        public float DamagePower = 10f;
        public GameObject CurrentTarget;
        public GameObject RadiusObject;
        
        public void Configure()
        {
            _enemyLocatorService.Configure(this, head.transform);
            _shootingService.Configure(this, head.transform, pool);
            pool.Configure();
        }

        public void Initialize()
        {

        }

        public void ClickOnTower(BaseEventData data)
        {
            PointerEventData pData = (PointerEventData)data;
            if (!RadiusObject.activeSelf)
            {
                RadiusObject.SetActive(true);
                var scale = RadiusObject.transform.localScale;
                RadiusObject.transform.localScale = new Vector3(scale.x * ShootingRadius, scale.y * ShootingRadius,
                    scale.z * ShootingRadius);
            }
            else
            {
                RadiusObject.SetActive(false);
            }
        }

        private void Update()
        {
            _enemyLocatorService.DoUpdate();
            _shootingService.DoUpdate(_enemyLocatorService.CurrentEnemy);
        }

        [SerializeField] private AmmoPool pool;
        [SerializeField] private GameObject head;
  
        private readonly EnemyLocatorService _enemyLocatorService = new();
        private readonly ShootingService _shootingService = new();
    }
}