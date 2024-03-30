using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class MollyController : MonoBehaviour
{
    private Vector3[] vectors;
    private GameObject debugObjects;
    private int _solverPointCount;

    [Header("Debug Settings")]
    public bool traceAllProjctiles = false;
    public bool showDebugSpheres = true;
    public float simulationDisplaySpeed = 30f;
    public GameObject debugSphere;
    public KeyCode simulationKey = KeyCode.E;

    [Header("Projectile Settings")]
    public Transform origin;
    public Transform target;
    public float throwForce = 1f;
    public float gravity = 9.81f;
    public float dampening = 0.4f;
    public int targetBounces = 4;
    public float targetRadius = 3f;

    [Header("Performance Settings")]
    public int solverPointsCount = 10000;
    public float maxStepCount = 1000;
    public float stepSize = 0.01f;

    [Header("Simulation Information")]
    public float trailLengthInSeconds = 2f;
    public float simulatedTime;

    public void OnValidate()
    {
        if (solverPointsCount != _solverPointCount)
        {
            _solverPointCount = solverPointsCount;
            vectors = GenerateUpperHemispherePoints(_solverPointCount); // Regenerate points whenever editor value changes
        }
    }
    private void Start()
    {
        vectors = GenerateUpperHemispherePoints(_solverPointCount);
    }

    private void Update()
    {
        if (Input.GetKeyDown(simulationKey))
        {
            BeginSolver();
        }
    }

    private IEnumerator CalculatePath(Vector3 vector, bool trace=false)
    {
        Vector3 lastPosition;
        Vector3 position = origin.position;
        Vector3 velocity = vector.normalized * throwForce;
        float time = 0;
        int step = 0;
        int bounces = 0;
        while (step++ <= maxStepCount)
        {
            lastPosition = position;
            position += velocity * stepSize;
            velocity += gravity * stepSize * Vector3.down;
            time += stepSize;
            simulatedTime += stepSize;

            // TODO: define bound to kill simulations.
            //if (position.y < 0.1f || position.x < -10 || position.x > 10 || position.z < -10 || position.z > 10)
            //{
            //    Debug.Log("Hit bounds at " + time + " seconds");
            //    if (DEBUG) CreateDebugSphere(position, Color.red);
            //    break;
            //}

            if (Physics.Raycast(position, velocity, out RaycastHit hit, velocity.magnitude * stepSize, 3))
            {
                position = hit.point;
                if (hit.collider.CompareTag("Target"))
                {
                    Debug.Log("Hit target at " + time + " seconds");
                    if (showDebugSpheres) CreateDebugSphere(position, Color.green);

                    if (showDebugSpheres && !trace) StartCoroutine(CalculatePath(vector, true));
                    if (showDebugSpheres && trace)
                    {
                        Debug.DrawLine(lastPosition, position, Color.blue, 1000);
                    }
                    break;
                } else {

                    Debug.Log("Hit wall at " + time + " seconds");
                    velocity = Vector3.Reflect(velocity, hit.normal);
                    if (showDebugSpheres && trace) CreateDebugSphere(position, Color.blue);
                    if (bounces++ == targetBounces)
                    {
                        if (Vector3.Distance(position, target.position) < targetRadius)
                        {
                            Debug.Log("Hit target at " + time + " seconds");
                            if (showDebugSpheres) CreateDebugSphere(position, Color.green);
                        }
                        break;
                    };
                    velocity *= dampening;

                }
            }

            if (showDebugSpheres && trace)
            {
                Debug.DrawLine(lastPosition, position, Color.blue, 500);
            }

            if (trace) yield return new WaitForSeconds(1f / simulationDisplaySpeed);
            else if (step % 100 == 0) yield return null;
        }
        yield return null;
    }

    private void CreateDebugSphere(Vector3 position, Color color)
    {
        // folder for debug spheres
        // get from instanced object
        GameObject sphere = Instantiate(debugSphere);
        sphere.transform.position = position;
        sphere.GetComponent<Renderer>().material.color = color;
        sphere.gameObject.layer = 2;
        sphere.transform.parent = debugObjects.transform;
    }

    public void BeginSolver()
    {
        if (showDebugSpheres)
        {
            if (debugObjects != null)
                Destroy(debugObjects);
            debugObjects = new GameObject("DebugObjects");
            CreateDebugSphere(origin.position, Color.blue);
            CreateDebugSphere(target.position, Color.green);
        }
        for (int i = 0; i < vectors.Length; i++)
        {
            StartCoroutine(CalculatePath(vectors[i], traceAllProjctiles));
        }
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
}

