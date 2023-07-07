using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class planeController : MonoBehaviour
{
    Rigidbody rb;
    public float velocity;
    public float acceleration = 1;
    public float yawSpeed = 180;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        velocity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float z = Input.GetAxis("Vertical");
        float y = Input.GetAxis("Horizontal");
        float x = Input.GetAxis("Jump");
        velocity += x*Time.deltaTime;
        rb.velocity = Quaternion.Euler(transform.eulerAngles)*new Vector3(0,0,velocity);
        float temp = transform.eulerAngles.x;

        if(transform.rotation.eulerAngles.z > 90)
        {
            transform.eulerAngles -= new Vector3(0,y*Time.deltaTime*yawSpeed,0);
        }
        else
        {
            transform.eulerAngles += new Vector3(0,y*Time.deltaTime*yawSpeed,0);
        }
        transform.Rotate(-z*Time.deltaTime*yawSpeed,0,0);
    }
}
