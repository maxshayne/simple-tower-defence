using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float Speed = 10f;

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
        gameObject.SetActive(false);
    }
}