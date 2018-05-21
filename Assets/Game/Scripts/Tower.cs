using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tower : MonoBehaviour
{

    private Transform _transform;

    private GameObject _head;

    private GameObject _body;

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

    public GameObject RadiusObject;


    void Awake()
    {
        _transform = transform;
        _head = gameObject.transform.GetChild(0).gameObject;
        _gun = _head.transform.GetChild(0).gameObject;
        _body = gameObject.transform.GetChild(1).gameObject;
        _defaultColors = new List<Color>
        {
            _body.GetComponent<MeshRenderer>().material.color,
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
                if (CurrentTarget == null || !CurrentTarget.activeSelf)
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

    public void Colorize(Color color)
    {
        _body.GetComponent<MeshRenderer>().material.color = color;
        _head.GetComponent<MeshRenderer>().material.color = color;
        //_gun.GetComponent<MeshRenderer>().material.color = color;
        foreach (Transform child in _head.transform)
        {
            child.GetComponent<MeshRenderer>().material.color = color;
        }
    }

    public void RestoreColors()
    {
        _body.GetComponent<MeshRenderer>().material.color = _defaultColors[0];
        _head.GetComponent<MeshRenderer>().material.color = _defaultColors[1];
        //_gun.GetComponent<MeshRenderer>().material.color = _defaultColors[2];
        foreach (Transform child in _head.transform)
        {
            child.GetComponent<MeshRenderer>().material.color = _defaultColors[2];
        }
    }

    private void Fire()
    {
        foreach (Transform child in _head.transform)
        {
            child.GetComponent<Gun>().Fire(CurrentTarget.transform, DamagePower);
        }
    }

    private void FindEnemies()
    {
        var hitColliders = Physics.OverlapSphere(_transform.position, SearchRadius).Where(obj=>obj.tag == "Enemy");
        var maxDist = float.MaxValue;
        foreach (var hitCollider in hitColliders)
        {
            var dist = Vector3.Distance(_transform.position, hitCollider.transform.position);
            if (!(dist < maxDist)) continue;
            maxDist = dist;
            CurrentTarget = hitCollider.gameObject;
        }
        if (CurrentTarget != null)
            State = TowerState.Firing;        
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