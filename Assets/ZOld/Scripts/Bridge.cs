using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.eulerAngles = new Vector3(0, 90, 0);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
}
