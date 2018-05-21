using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    private float _health;

    private Transform _transform;

    private int _waveIndex;

    public Image HealthBar;

    public List<Transform> Path;

    public Transform CurrentPoint;

    public float Health
    {
        get { return _health; }
        set
        {
            if (_health == value) return;
            _health = value;
            if (HealthChangeEvent != null)
                HealthChangeEvent(_health);
        }
    }

    public int Reward = 30;

    public float StartHealth = 100;

    public float Speed = 1;

    public delegate void OnHealthChangeDelegate(float val);

    public event OnHealthChangeDelegate HealthChangeEvent;


    void Awake()
    {
        _transform = transform;                
        Health = StartHealth;
        HealthChangeEvent += OnHealthChangeEvent;
    }

    private void OnHealthChangeEvent(float val)
    {
        Debug.Log("Health changed by " + val);
        HealthBar.fillAmount = val / StartHealth;
      //  Debug.Log("HealthBar.fillAmount: " + HealthBar.fillAmount);
        if (val <= 0)
        {
            Die();
        }
    }

    void OnEnable()
    {
        CurrentPoint = Path.First();
        _waveIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentPoint != null)
        {
            var curPos = _transform.position;
            _transform.position = Vector3.MoveTowards(curPos, CurrentPoint.transform.position, Time.deltaTime * Speed);
            var dist = Vector3.Distance(curPos, CurrentPoint.transform.position);
            if (dist < 1)
                SelectNewWayPoint();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("Triggered with " + collider.tag);
        switch (collider.tag)
        {
            //case "Waypoint":
            //    SelectNewWayPoint();
            //    break;
            case "Endpoint":
                GameManager.Instance.LifeCount--;
                Die();
                break;
        }
    }

    private void SelectNewWayPoint()
    {        
        CurrentPoint = Path[_waveIndex];
        _waveIndex++;
    }

    private void Die()
    {
        GameManager.Instance.Money += Reward;
        GameManager.Instance.EnemyCount--;
        Destroy(gameObject);
    }
}