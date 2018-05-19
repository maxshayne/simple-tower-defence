using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private float _health;

    private Transform _transform;

    private List<GameObject> _waypoints;

    private List<GameObject> _points;

    public GameObject CurrentPoint;

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

    public float MaxHealth = 100;

    public float Speed = 1;

    public delegate void OnHealthChangeDelegate(float val);

    public event OnHealthChangeDelegate HealthChangeEvent;


    void Awake()
    {
        _transform = transform;
        _waypoints = GameObject.FindGameObjectsWithTag("Waypoint").ToList();  //знаю, что юзать GameObject.Find'ы всякие сродни стрельбе в колено, но т.к. времени мало, пришлось, не судите строго
        _points = _waypoints;
        Health = MaxHealth;
        HealthChangeEvent += OnHealthChangeEvent;
    }

    private void OnHealthChangeEvent(float val)
    {
        Debug.Log("Health changed by" + val);
        if (val <= 0)
        {
           Restore();
        }
    }

    void OnEnable()
    {
        CurrentPoint = FindNearest(_points);
        _waypoints.Remove(CurrentPoint);
    }

    // Update is called once per frame
    void Update()
    {

        if (CurrentPoint != null)
        {
            var curPos = _transform.position;
            _transform.position = Vector3.MoveTowards(curPos, CurrentPoint.transform.position, Time.deltaTime * Speed);
        }
    }

    private GameObject FindNearest(List<GameObject> list)
    {
        var nearest = list.First();
        foreach (var o in list.Where(c => c.transform.position.magnitude != nearest.transform.position.magnitude))
        {
            var dist1 = Vector3.Distance(nearest.transform.position, _transform.position);
            var dist2 = Vector3.Distance(o.transform.position, _transform.position);
            if (dist2 < dist1)
            {
                nearest = o;
            }
            else if (Math.Abs(dist2 - dist1) < 1)
            {
                Debug.Log("Found two equal distances");
                if (nearest.transform.position.z == o.transform.position.z)
                {
                    Debug.Log("Randomize time!");
                    var rand = new System.Random();
                    var res = rand.Next(100);
                    if (res % 2 == 0)
                        nearest = o;
                }
                else if (nearest.transform.position.z > o.transform.position.z)
                {
                    nearest = o;
                }
            }
        }
        //Debug.Log("Nearest point: " + nearest.transform.position);
        return nearest;
    }

    private void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("Triggered with " + collider.tag);
        switch (collider.tag)
        {
            case "Waypoint":
                var list = _waypoints.Where(c => c.transform.position.magnitude !=
                                                CurrentPoint.transform.position.magnitude)
                    .ToList();
                CurrentPoint = FindNearest(list);
                _waypoints.Remove(CurrentPoint);
                break;
            case "Endpoint":
                Game.Instance.LifeCount--;
                Restore();
                break;
        }
    }

    private void Restore()
    {
        Health = MaxHealth;
        CurrentPoint = null;
        gameObject.SetActive(false);
    }
}