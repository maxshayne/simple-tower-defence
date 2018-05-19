using System.Collections;
using UnityEngine;

public class Spawn : MonoBehaviour
{

    public GameObject Parent;

    public float SpawnDelay = 2f;

    public float NextWaveDelay = 40f;

    public int WaveNumber = 0;

    private float time = 0f;    

    // Use this for initialization
    void Start()
    {
       GameManager.Instance.NextWaveEvent += OnNextWaveEvent;        
    }

    private void OnNextWaveEvent()
    {
        SpawnMobs();
    }

    void SpawnMobs()
    {
        switch (WaveNumber)
        {
            case 0:
                WaveNumber++;
                for (int i = 0; i < 10; i++)
                {
                    StartCoroutine(ActivateEnemy(SpawnDelay * i));
                }
                break;
            default:
                WaveNumber++;
                for (int i = 0; i < 10; i++)
                {
                    StartCoroutine(ActivateEnemy(SpawnDelay * i));
                }
                break;
        }
    }

    private IEnumerator ActivateEnemy(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (Transform child in Parent.transform)
        {
            if (child.gameObject.activeSelf) continue;
            var enemy = child.gameObject;
            enemy.transform.position = transform.position;
            enemy.SetActive(true);
            break;
        }
    }
}