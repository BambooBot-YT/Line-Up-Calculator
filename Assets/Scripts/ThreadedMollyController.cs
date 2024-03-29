using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;

public struct CalculatePathJob : IJob
{
    public NativeArray<Vector3> positions;
    public NativeArray<Vector3> velocities;
    public Vector3 targetPosition;
    public float throwForce;
    public float gravity;
    public float timeStep;
    public int maxStepCount;
    public bool DEBUG;

    public void Execute()
    {
        for (int i = 0; i < positions.Length; i++)
        {
            Vector3 lastPosition = positions[i];
            Vector3 position = lastPosition;
            Vector3 velocity = velocities[i].normalized * throwForce;
            int step = 0;
            

            while (step++ <= maxStepCount)
            {
                lastPosition = position;
                position += velocity * timeStep;
                velocity += gravity * timeStep * Vector3.down;

                if (Vector3.Distance(position, targetPosition) < 0.1f)
                {
                    // Hit target
                    Debug.Log("Hit target at " + step * timeStep + " seconds");
                    positions[i] = position; // Save the hit position
                    break;
                }

                if (Vector3.Distance(position, targetPosition) < 0.1f)
                {

                    if (DEBUG) ThreadedMollyController.CreateDebugSphere(position, Color.green);
                    break;
                }

                //if (position.y < 0.1f || position.x < -10 || position.x > 10 || position.z < -10 || position.z > 10)
                //{
                //    Debug.Log("Hit bounds at " + time + " seconds");
                //    if (DEBUG) CreateDebugSphere(position, Color.red);
                //    break;
                //}

                if (Physics.Raycast(position, velocity, out RaycastHit hit, velocity.magnitude * timeStep, 3))
                {
                    Debug.Log("Hit wall at " + step * timeStep + " seconds");
                    velocity = Vector3.Reflect(velocity, hit.normal);
                    position = hit.point;
                    if (DEBUG) ThreadedMollyController.CreateDebugSphere(position, Color.blue);

                    if (hit.collider.CompareTag("Target"))
                    {
                        Debug.Log("Hit target at " + step * timeStep + " seconds");
                        if (DEBUG) ThreadedMollyController.CreateDebugSphere(position, Color.green);
                        break;
                    }
                }

                if (DEBUG)
                {
                    Debug.DrawLine(lastPosition, position, Color.blue);
                }

                // Add collision detection and reflection logic here

                // Save the updated position and velocity
                positions[i] = position;
                velocities[i] = velocity;
            }
        }
    }
}

public class ThreadedMollyController : MonoBehaviour
{
    private Vector3[] vectors;

    public Transform origin;
    public Transform target;
    public float throwForce = 1f;

    public int batchSize = 100;
    public int maxStepCount = 1000;
    public int Bounces = 1;

    public bool DEBUG = true;

    public void BeginSolver()
    {
        vectors = GenerateUpperHemispherePoints(1000);

        NativeArray<Vector3> positions = new(vectors.Length, Allocator.TempJob);
        NativeArray<Vector3> velocities = new(vectors.Length, Allocator.TempJob);

        // Initialize positions and velocities
        for (int i = 0; i < vectors.Length; i++)
        {
            positions[i] = origin.position;
            velocities[i] = vectors[i];
        }

        CalculatePathJob pathJob = new CalculatePathJob
        {
            positions = positions,
            velocities = velocities,
            targetPosition = target.position,
            throwForce = throwForce,
            gravity = 9.81f,
            timeStep = 0.01f,
            maxStepCount = maxStepCount,
            DEBUG = DEBUG
        };

        JobHandle handle = pathJob.Schedule();
        handle.Complete();

        // Use the results from the job

        positions.Dispose();
        velocities.Dispose();
    }

    void Start()
    {
        if (DEBUG)
        {
            CreateDebugSphere(origin.position, Color.blue);
            CreateDebugSphere(target.position, Color.green);
        }
        Debug.Log("Starting solver");
        BeginSolver();
    }

    public static Vector3[] GenerateUpperHemispherePoints(int samples)
    {
        List<Vector3> points = new();
        float phi = Mathf.PI * (3 - Mathf.Sqrt(5)); // Golden angle in radians

        for (int i = 0; i < samples; i++)
        {
            float y = 1 - (i / (float)(samples - 1)) * 2; // y goes from 1 to -1
            if (y < 0) continue; // Skip points below the horizontal plane

            float radius = Mathf.Sqrt(1 - y * y); // radius at y

            float theta = phi * i; // golden angle increment

            float x = Mathf.Cos(theta) * radius;
            float z = Mathf.Sin(theta) * radius;

            points.Add(new Vector3(x, y, z));
        }

        return points.ToArray();
    }

    public static void CreateDebugSphere(Vector3 position, Color color)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = position;
        sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        sphere.GetComponent<Renderer>().material.color = color;
    }
}
