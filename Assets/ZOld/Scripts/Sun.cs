using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0.05f, 0, 0);
    }
}
