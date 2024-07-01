using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleMove : MonoBehaviour
{
    [SerializeField] float speed = 5;

    private Rigidbody rgb;

    private Vector3 inputDir;

    void Start()
    {
        rgb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        inputDir = new Vector3(hor, inputDir.y, ver);

        Vector3 moveDirection = transform.TransformDirection(inputDir) * speed;
        moveDirection.y = inputDir.y;

        rgb.velocity = moveDirection;
    }
}