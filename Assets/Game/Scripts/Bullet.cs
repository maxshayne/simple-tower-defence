using System.Collections;
using System.Linq;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private float _destroyTime = 3f;

    private Transform _transform;

    public float Speed = 10f;

    public float DamagePower;

    public bool AoE;

    public float AoERedius = 20;

    public Transform TargetTransform;

    void Awake()
    {
        _transform = transform;
    }

    void OnEnable()
    {
        StartCoroutine(Deactivate());
    }

    private IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(_destroyTime);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (TargetTransform == null || !TargetTransform.gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        else
        {
            _transform.LookAt(TargetTransform);
            _transform.position = Vector3.MoveTowards(_transform.position, TargetTransform.position,
                Speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (AoE)
        {
            var hitColliders = Physics.OverlapSphere(_transform.position, AoERedius).Where(obj => obj.tag == "Enemy");
            foreach (var hitCollider in hitColliders)
            {
                var enemy = hitCollider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.Health -= DamagePower;
                }
            }
        }
        else
        {
            Debug.Log("Bullet triggered with " + collider.name);
            if (collider.tag == "Enemy")
            {
                collider.transform.GetComponent<Enemy>().Health -= DamagePower;
            }
        }

        gameObject.SetActive(false);
    }
}