using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private float _destroyTime = 3f;

    public float Speed = 10f;

    public float DamagePower;

    public Transform TargetTransform;
    
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
        if (TargetTransform.gameObject.activeSelf)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetTransform.position,
                Speed * Time.deltaTime);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Bullet triggered with " + collider.name);
        if (collider.tag == "Enemy")
        {
            collider.transform.parent.GetComponent<Enemy>().Health -= DamagePower;
        }
        gameObject.SetActive(false);
    }
}