using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    public float MoveSpeed = 40.0f;
    private Rigidbody Cube = null;

    private void Awake()
    {
        Cube = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w"))
        {
            Cube.AddRelativeForce(Vector3.forward * MoveSpeed * Time.deltaTime);
            //Cube.velocity = new Vector3(0, 0, MoveSpeed);
        }
        if (Input.GetKey("s"))
        {
            Cube.AddRelativeForce(-1 * Vector3.forward * MoveSpeed * Time.deltaTime);
            //Cube.velocity = new Vector3(0, 0, -1*MoveSpeed);
        }
        if (Input.GetKey("a"))
        {
            transform.Rotate(Vector3.up * -1 * Time.deltaTime * MoveSpeed);
        }
        if (Input.GetKey("d"))
        {
            transform.Rotate(Vector3.up * Time.deltaTime * MoveSpeed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Ãæµ¹");
    }
}
