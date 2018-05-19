using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{

    private Transform _transform;

    private GameObject _head;

    private GameObject _gun;

    private float _currentFireCooldown;

    private List<Color> _defaultColors;

    public TowerState State;

    public int TowerCost = 100;

    public int TowerLevel = 1;

    public float SearchRadius = 50f;

    public float ShootingRadius = 50f;

    public float RotationSpeed = 2f;

    public float FireCooldown = 1f;

    public float DamagePower = 10f;

    public GameObject CurrentTarget;


    void Awake()
    {
        _transform = transform;
        _head = gameObject.transform.GetChild(0).gameObject;
        _gun = _head.transform.GetChild(0).gameObject;
        _defaultColors = new List<Color>
        {
            gameObject.GetComponent<MeshRenderer>().material.color,
            _head.GetComponent<MeshRenderer>().material.color,
            _gun.GetComponent<MeshRenderer>().material.color
        };
    }

    // Use this for initialization
    void Start()
    {
        State = TowerState.Placing;
    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case TowerState.Patroling:
                _head.transform.Rotate(Vector3.up * 20 * Time.deltaTime);
                FindEnemies();
                break;
            case TowerState.Firing:
                if (CurrentTarget == null)
                {
                    State = TowerState.Patroling;
                    break;
                }
                var lookPos = CurrentTarget.transform.position - _head.transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                _head.transform.rotation = Quaternion.Slerp(_head.transform.rotation, rotation, Time.deltaTime * RotationSpeed);
                if (_currentFireCooldown >= FireCooldown)
                {
                    _currentFireCooldown = 0f;
                    Debug.Log("FIRE!");
                    Fire();
                }
                else
                {
                    _currentFireCooldown += Time.deltaTime;
                }
                CheckDistance();
                break;
        }

    }

    public void Colorize(Color color)
    {
        gameObject.GetComponent<MeshRenderer>().material.color = color;
        _head.GetComponent<MeshRenderer>().material.color = color;
        _gun.GetComponent<MeshRenderer>().material.color = color;
    }

    public void RestoreColors()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = _defaultColors[0];
        _head.GetComponent<MeshRenderer>().material.color = _defaultColors[1];
        _gun.GetComponent<MeshRenderer>().material.color = _defaultColors[2];
    }

    private void Fire()
    {
        _gun.GetComponent<GunController>().Fire(CurrentTarget.transform, DamagePower);
    }

    private void FindEnemies()
    {
        var hitColliders = Physics.OverlapSphere(_transform.position, SearchRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag != "Enemy") continue;
            CurrentTarget = hitCollider.gameObject;
            State = TowerState.Firing;
            break;
        }
    }

    private void CheckDistance()
    {
        var dist = Vector3.Distance(_transform.position, CurrentTarget.transform.position);
        if (dist > ShootingRadius && State == TowerState.Firing)
        {
            CurrentTarget = null;
            State = TowerState.Patroling;
        }
    }
}


public enum TowerState
{
    Placing,
    Patroling,
    Firing
}