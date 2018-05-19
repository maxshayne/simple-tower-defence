using UnityEngine;

public class Billboard : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }

}