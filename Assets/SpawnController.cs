using System.Collections;
using UnityEngine;

public class SpawnController : MonoBehaviour
{

    public GameObject Parent;

    public float SpawnDelay = 2f;

    public float NextWaveDelay = 40f;

    public int WaveNumber = 0;

    private float time = 0f;

    // Use this for initialization
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > NextWaveDelay)
        {
            time = 0f;
            Spawn();
        }
    }

    void Spawn()
    {
        switch (WaveNumber)
        {
            case 0:
                WaveNumber++;
                for (int i = 0; i < 10; i++)
                {
                    Invoke("ActivateEnemy", SpawnDelay*i);
                }
                break;
            default:
                WaveNumber++;
                for (int i = 0; i < 10; i++)
                {
                    Invoke("ActivateEnemy", SpawnDelay * i);
                }
                break;
        }
    }

    private void ActivateEnemy()
    {
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