using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour
{
    Rigidbody rgb;
    void Start()
    {
        rgb = GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        rgb.velocity = new Vector3(0, 0, 11f);
    }
}
