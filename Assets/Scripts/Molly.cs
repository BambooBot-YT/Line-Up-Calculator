using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molly : MonoBehaviour
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

    public BigBrainBrimiBoyBigBrain FatherBigBrainBrimiBoyBigBrain;

    private GameObject spike;

    public bool thisIsTheOne = false;

    public float hightMultiply = 1;
    public float horizontalMultiply = 1;

    public int type;

    private void Start()
    {
        FatherBigBrainBrimiBoyBigBrain = GameObject.FindGameObjectWithTag("Player").GetComponent<BigBrainBrimiBoyBigBrain>();
        
        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezeRotation;
        // Calculate the initial velocity based on the launch angle and horizontal angle
        Vector3 launchDirection = Quaternion.Euler(0f, 0f, 0f) * Vector3.forward;
        Vector3 initialVelocity = launchDirection.normalized * throwForceMagnitude;
        rb.velocity = initialVelocity;
        spike = GameObject.FindGameObjectWithTag("spike");
    }

    private void Update()
    {
        rb.AddForce(Vector3.down * gravity * Time.deltaTime);
    }


    private void OnCollisionEnter(Collision collision)
    {
        Bounce(collision.contacts[0].normal);
    }

    private void Bounce(Vector3 collisionNormal)
    {
        rb.velocity = Vector3.Reflect(rb.velocity, collisionNormal);
        rb.velocity *= bounceSpeedReduction;
        bounceCount++;
        if (bounceCount >= maxBounces)
        {
            // Set velocity and angular velocity to zero to stop the projectile
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            if (Vector3.Distance(transform.position, spike.transform.position) < 2)
            {
                thisIsTheOne = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        if (bounceCount <= 2)
        {
            if (rb.velocity.magnitude * horizontalMultiply + Mathf.Abs(transform.position.y - spike.transform.position.y) * hightMultiply <= Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(spike.transform.position.x, spike.transform.position.z)))
            {
                Destroy(gameObject);
            }
        }
        
    }
}
