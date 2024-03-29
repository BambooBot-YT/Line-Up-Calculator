using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MollyReplay : MonoBehaviour
{
    public int manufactureTag;
    public float throwForceMagnitude = 10f; // Constant throw force magnitude
    public float gravity = 9.81f; // Gravity force applied
    public int maxBounces = 4;    // Maximum number of bounces before stopping
    public float bounceSpeedReduction = 0.5f; // Speed reduction factor after each bounce

    [Range(0f, 180f)]
    public float launchAngle = 45f; // Launch angle in degrees (default 45 degrees)

    [Range(0f, 360f)]
    public float horizontalAngle = 0f; // Horizontal angle in degrees

    public int bounceCount = 0;

    private Rigidbody rb;

    public GameObject trail;

    private void Start()
    {
        launchAngle = launchAngle - 270;
        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezeRotation;
        // Calculate the initial velocity based on the launch angle and horizontal angle
        Vector3 launchDirection = Quaternion.Euler(launchAngle, horizontalAngle, 0f) * Vector3.forward;
        Vector3 initialVelocity = launchDirection.normalized * throwForceMagnitude;

        //rb.velocity = initialVelocity * Time.deltaTime;
        rb.velocity = initialVelocity;
    }

    private void Update()
    {

        //rb.velocity = rb.velocity * (Time.deltaTime / timeStabalizer);
        rb.AddForce(Vector3.down * gravity * Time.deltaTime);
        if (bounceCount >= maxBounces)
        {
            // Set velocity and angular velocity to zero to stop the projectile
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
        // Bounce the projectile on collision
        Bounce(collision.contacts[0].normal);
    }

    private void Bounce(Vector3 collisionNormal)
    {
        // Reflect the velocity based on the collision normal
        rb.velocity = Vector3.Reflect(rb.velocity, collisionNormal);

        // Reduce the speed after each bounce
        rb.velocity *= bounceSpeedReduction;

        // Increment the bounce count
        bounceCount++;
    }
}
