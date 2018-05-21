using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    private List<Transform> _path;

    public GameObject[] Enemies;

    public Transform SpawnTransform;

    public float SpawnDelay = 2f;

    public float NextWaveDelay = 40f;    

    public GameObject PathObject;

    // Use this for initialization
    void Start()
    {        
        GameManager.Instance.NextWaveEvent += OnNextWaveEvent;
        _path = new List<Transform>();
        foreach (Transform child in PathObject.transform)
        {
            _path.Add(child);
        }
    }

    private void OnNextWaveEvent()
    {
        SpawnMobs();
    }

    void SpawnMobs()
    {
        switch (GameManager.Instance.WaveNumber)
        {
            case 1:
                for (int i = 0; i < 5; i++)
                {
                    StartCoroutine(ActivateEnemy(0, SpawnDelay * i));
                }
                break;
            case 2:

                for (int i = 0; i < 5; i++)
                {
                    StartCoroutine(ActivateEnemy(0, SpawnDelay * i));
                }
                for (int i = 0; i < 3; i++)
                {
                    StartCoroutine(ActivateEnemy(1, SpawnDelay * i));
                }
                break;
            case 3:

                for (int i = 0; i < 8; i++)
                {
                    StartCoroutine(ActivateEnemy(0, SpawnDelay * i));
                }
                for (int i = 0; i < 3; i++)
                {
                    StartCoroutine(ActivateEnemy(1, SpawnDelay * i));
                }
                break;
            case 4:

                for (int i = 0; i < 15; i++)
                {
                    StartCoroutine(ActivateEnemy(0, SpawnDelay * i));
                }
                break;
            case 5:

                for (int i = 0; i < 3; i++)
                {
                    StartCoroutine(ActivateEnemy(2, SpawnDelay * i));
                }
                for (int i = 0; i < 2; i++)
                {
                    StartCoroutine(ActivateEnemy(2, SpawnDelay * i));
                }
                break;
            case 6:
                for (int i = 0; i < 15; i++)
                {
                    StartCoroutine(ActivateEnemy(0, SpawnDelay * i));
                }
                for (int i = 0; i < 3; i++)
                {
                    StartCoroutine(ActivateEnemy(1, SpawnDelay * i));
                }
                break;
            case 7:
                for (int i = 0; i < 20; i++)
                {
                    StartCoroutine(ActivateEnemy(0, SpawnDelay * i));
                }
                for (int i = 0; i < 5; i++)
                {
                    StartCoroutine(ActivateEnemy(1, SpawnDelay * i));
                }
                for (int i = 0; i < 2; i++)
                {
                    StartCoroutine(ActivateEnemy(1, SpawnDelay * i));
                }
                break;
            case 8:
                for (int i = 0; i < 15; i++)
                {
                    StartCoroutine(ActivateEnemy(0, SpawnDelay * i));
                }
                for (int i = 0; i < 15; i++)
                {
                    StartCoroutine(ActivateEnemy(1, SpawnDelay * i));
                }
                for (int i = 0; i < 5; i++)
                {
                    StartCoroutine(ActivateEnemy(1, SpawnDelay * i));
                }
                break;
            case 9:
                for (int i = 0; i < 20; i++)
                {
                    StartCoroutine(ActivateEnemy(1, SpawnDelay * i));
                }
                for (int i = 0; i < 5; i++)
                {
                    StartCoroutine(ActivateEnemy(2, SpawnDelay * i));
                }
                break;
            case 10:
                for (int i = 0; i < 15; i++)
                {
                    StartCoroutine(ActivateEnemy(2, SpawnDelay * i));
                }
                break;
        }
    }

    private IEnumerator ActivateEnemy(int enemyIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        var enemy = Instantiate(Enemies[enemyIndex], SpawnTransform.position, Quaternion.identity);        
        var enemyComponent = enemy.GetComponent<Enemy>();
        enemyComponent.Path = _path;
        enemy.SetActive(true);
        GameManager.Instance.EnemyCount++;
    }
}