using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NBMollyReplay : MonoBehaviour
{
    public int manufactureTag;
    public float throwForceMagnitude = 10f; // Constant throw force magnitude
    public float gravity = 9.81f; // Gravity force applied
    public float bounceSpeedReduction = 0.5f; // Speed reduction factor after each bounce

    [Range(0f, 180f)]
    public float launchAngle = 45f; // Launch angle in degrees (default 45 degrees)

    [Range(0f, 360f)]
    public float horizontalAngle = 0f; // Horizontal angle in degrees

    public Rigidbody rb;
    public Spike data;
    private GameObject spike;

    public bool thisIsTheOne = false;

    public float hightMultiply = 1;
    public float horizontalMultiply = 1;

    public bool done = false;

    public GameObject trail;

    private void Start()
    {
        data = GameObject.FindGameObjectWithTag("spike").GetComponent<Spike>();

        launchAngle = data.vertical;
        horizontalAngle = data.horizontal;

        launchAngle = launchAngle - 270;
        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezeRotation;
        
        Vector3 launchDirection = Quaternion.Euler(launchAngle, horizontalAngle, 0f) * Vector3.forward;
        Vector3 initialVelocity = launchDirection.normalized * throwForceMagnitude;

        rb.velocity = initialVelocity;
    }

    private void Update()
    {
        rb.AddForce(Vector3.down * gravity * Time.deltaTime);
        if (done)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            Instantiate(trail, transform.position, Quaternion.identity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Bounce(collision.contacts[0].normal);
    }

    private void Bounce(Vector3 collisionNormal)
    {
        rb.velocity = Vector3.Reflect(rb.velocity, collisionNormal);
        rb.velocity *= bounceSpeedReduction;
    }

    private void OnTriggerEnter()
    {
        done = true;
    }
}
