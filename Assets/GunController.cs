using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public float BulletSpeed = 6f;

    public GameObject Pivot;

    private GameObject _parent;

    private GameObject _bullet;

	// Use this for initialization
	void Start ()
	{
	    _parent = GameObject.FindGameObjectWithTag("BulletStorage");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Fire(Transform target, float power)
    {
        foreach (Transform child in _parent.transform)
        {
            if (child.gameObject.activeSelf) continue;
            var obj = child.gameObject;
            obj.transform.position = Pivot.transform.position;
            var bullet = obj.GetComponent<BulletController>();
            bullet.TargetTransform = target;
            bullet.DamagePower = power;
            obj.SetActive(true);
            //StartCoroutine(Deactivate(bullet));
            break;
        }
    }

    private IEnumerator Deactivate(GameObject obj)
    {

        yield return new WaitForSeconds(2f);

        obj.SetActive(false);
        //Do Function here...
    }
}

/*  public void Fire(Transform target)
    {
        var bullet = (from Transform child in Parent.transform select child.gameObject).FirstOrDefault();
        if (bullet == null) return;
        bullet.transform.parent = null;
        bullet.transform.position = Pivot.transform.position;
        bullet.SetActive(true);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * BulletSpeed;
        StartCoroutine(ReturnToList(bullet));
    }

    private IEnumerator ReturnToList(GameObject obj)
    {

        yield return new WaitForSeconds(2f);
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        obj.transform.parent = Parent.transform;
    }*/
