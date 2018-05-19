using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float Speed = 10f;

    public float DamagePower;

    public Transform TargetTransform;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (TargetTransform != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetTransform.position,
                Speed * Time.deltaTime);
        }        
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Bullet triggered with " + collider.name);
        if (collider.tag == "Enemy")
        {
            collider.transform.parent.GetComponent<EnemyController>().Health -= DamagePower;
        }
        gameObject.SetActive(false);
    }
}