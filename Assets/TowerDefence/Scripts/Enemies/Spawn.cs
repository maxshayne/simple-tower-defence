using System.Collections;
using System.Collections.Generic;
using TowerDefence.Data;
using UnityEngine;

namespace TowerDefence.Enemies
{
    public class Spawn : MonoBehaviour
    {
        public GameObject[] Enemies;
        public Transform SpawnTransform;
        public GameObject PathObject;

        public void Configure(LevelInfo levelInfo)
        {
            GameManager.Instance.NextWaveEvent += OnNextWaveEvent;
            _path = new List<Transform>();
            foreach (Transform child in PathObject.transform)
            {
                _path.Add(child);
            }
            _waves = levelInfo.Waves;
        }

        private void OnNextWaveEvent()
        {
            SpawnMobs();
        }

        private void SpawnMobs()
        {
            var waveIndex = GameManager.Instance.WaveIndex;
            if (waveIndex < 0 || waveIndex >= _waves.Count)
            {
                Debug.LogError($"level info doesn't contain info about index: [{waveIndex.ToString()}]");
                return;
            }
            var waveInfo = _waves[waveIndex];
            foreach (var mobsInfo in waveInfo.MobsInfos)
            {
                for (var i = 0; i < mobsInfo.Count; i++)
                {
                    StartCoroutine(ActivateEnemy(mobsInfo.Id, waveInfo.SpawnDelayBetweenMobs * i));
                }
            }
        }

        private IEnumerator ActivateEnemy(int enemyIndex, float delay)
        {
            yield return new WaitForSeconds(delay);
            var enemyObject = Instantiate(Enemies[enemyIndex], SpawnTransform.position, Quaternion.identity);        
            var enemy = enemyObject.GetComponent<Enemy>();
            enemy.Configure(_path);
            enemyObject.SetActive(true);
            GameManager.Instance.EnemyCount++;
        }

        private List<WaveInfo> _waves;
        private List<Transform> _path;
    }
}