using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViperKillJoyMolly : MonoBehaviour
{
    public int manufactureTag;
    public float throwForceMagnitude = 10f; // Constant throw force magnitude
    public float gravity = 9.81f; // Gravity force applied
    public float damping = 0.5f; // Speed reduction factor after each bounce

    [Range(0f, 180f)]
    public float launchAngle = 45f; // Launch angle in degrees (default 45 degrees)

    [Range(0f, 360f)]
    public float horizontalAngle = 0f; // Horizontal angle in degrees

    public int bounceCount = 0;

    private Rigidbody rb;

    public bool thisIsTheOne = false;

    public float hightMultiply = 1;
    public float horizontalMultiply = 1;


    public int type;

    private Transform model;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        model = gameObject.GetComponentInChildren<Transform>();
    }

    public void SetVelocity(Vector3 launchDirection)
    {
        GetComponent<Rigidbody>().velocity = launchDirection.normalized * throwForceMagnitude;
    }

    private void Update()
    {
        rb.AddForce(Vector3.down * gravity * Time.deltaTime);
        model.forward = rb.velocity.normalized;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Molly"))
        {
            Debug.Log("Molly on Molly violence is not the answer");
            return;
        } else
        {
            Destroy(gameObject); // destroy on collision with anything other than another molly
        }
        //rb.velocity = Vector3.Reflect(rb.velocity, collision.contacts[0].normal);
        //rb.velocity *= damping;
        //bounceCount++;

        //if (rb.velocity.magnitude * horizontalMultiply + Mathf.Abs(transform.position.y - spike.transform.position.y) * hightMultiply <= Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(spike.transform.position.x, spike.transform.position.z)))
        //{
        //    Destroy(gameObject);
        //}
    }
}
