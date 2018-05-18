using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public List<GameObject> Waypoints;

    public GameObject CurrentPoint;

    public float Speed = 1;

    private Transform _transform;

    private List<GameObject> _points;

    void Awake()
    {
        _transform = transform;
        Waypoints = GameObject.FindGameObjectsWithTag("Waypoint").ToList();
        _points = Waypoints;
    }

    void OnEnable()
    {
        CurrentPoint = FindNearest(_points);
        Waypoints.Remove(CurrentPoint);
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
                var list = Waypoints.Where(c => c.transform.position.magnitude !=
                                                CurrentPoint.transform.position.magnitude)
                    .ToList();
                CurrentPoint = FindNearest(list);
                Waypoints.Remove(CurrentPoint);
                break;
            case "Endpoint":
                CurrentPoint = null;
                gameObject.SetActive(false);
                break;
        }
    }
}