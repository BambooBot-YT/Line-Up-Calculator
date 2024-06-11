using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

/// <summary>
/// Controls the behavior of Molly projectiles in the game.
/// </summary>
public class MollyController : MonoBehaviour
{
    // Private fields
    /// <summary>
    /// An array of vectors representing points on the upper hemisphere.
    /// </summary>
    private Vector3[] vectors;

    /// <summary>
    /// A GameObject to organize debug objects in the scene hierarchy.
    /// </summary>
    private GameObject debugObjectsFolder;

    /// <summary>
    /// The count of solver points used in the simulation.
    /// </summary>
    private int _solverPointCount;

    // Public fields
    [Header("Debug Settings")]
    /// <summary>
    /// Indicates whether all projectiles should leave a trace. If false, only the successful projectiles will leave a trace.
    /// </summary>
    public bool traceAllProjctiles = false;

    /// <summary>
    /// Determines whether debug spheres should be shown.
    /// </summary>
    public bool showDebug = true;

    /// <summary>
    /// The speed at which the simulation is displayed.
    /// </summary>
    public float simulationDisplaySpeed = 30f;

    /// <summary>
    /// The prefab used for creating debug spheres in the scene.
    /// </summary>
    public GameObject debugSphere;

    /// <summary>
    /// The key used to initiate the simulation.
    /// </summary>
    public KeyCode simulationKey = KeyCode.E;

    [Header("Projectile Settings")]
    /// <summary>
    /// The starting point of the projectile.
    /// </summary>
    public Transform origin;

    /// <summary>
    /// The intended target of the projectile.
    /// </summary>
    public Transform target;

    /// <summary>
    /// The properties of the Molly projectile.
    /// </summary>
    public MollyProperties molly;

    [Header("Performance Settings")]
    /// <summary>
    /// The number of solver points to calculate for the simulation.
    /// </summary>
    public int solverPointsCount = 10000;

    /// <summary>
    /// The maximum number of steps the simulation will calculate.
    /// </summary>
    public float maxStepCount = 1000;

    /// <summary>
    /// The size of each step in the simulation.
    /// </summary>
    public float stepSize = 0.1f;

    [Header("Simulation Information")]
    /// <summary>
    /// The length of time the projectile's trail will be visible in seconds.
    /// </summary>
    public float trailLengthInSeconds = 2f;

    /// <summary>
    /// The total time that has been simulated.
    /// </summary>
    public float cumulativeTime;

    /// <summary>
    /// Used to update values while in play mode.
    /// </summary>
    public void OnValidate()
    {
        if (solverPointsCount != _solverPointCount)
        {
            _solverPointCount = solverPointsCount;
            vectors = GenerateUpperHemispherePoints(_solverPointCount); // Regenerate points whenever editor value changes
        }
    }

    /// <summary>
    /// Initializes the vectors on start.
    /// </summary>
    private void Start()
    {
        vectors = GenerateUpperHemispherePoints(_solverPointCount);
    }

    /// <summary>
    /// Begins the solver if the simulation key is pressed.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(simulationKey))
        {
            if (debugObjectsFolder != null)
                Destroy(debugObjectsFolder);
            debugObjectsFolder = new("DebugObjects");
            for (int i = 0; i < vectors.Length; i++)
            {
                CalculatePath(vectors[i]);
            }
        }
    }

    /// <summary>
    /// Calculates the projectile path and handles the simulation logic.
    /// </summary>
    /// <param name="vector">The initial direction and force of the projectile.</param>
    /// <param name="trace">Whether to trace the projectile path.</param>
    /// <returns>An IEnumerator for coroutine management.</returns>
    private IEnumerator CalculatePathWithTrace(Vector3 vector)
    {
        GameObject lineStrip = new("LineStrip");
        lineStrip.transform.parent = debugObjectsFolder.transform;
        LineRenderer lineRenderer = lineStrip.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended"));
        lineRenderer.startColor = Color.blue;
        lineRenderer.endColor = Color.blue;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        List<Vector3> points = new()
        {
            origin.position
        };
        Vector3 position = origin.position;
        Vector3 velocity = vector.normalized * molly.throwForce;
        float time = 0;
        int step = 0;
        int bounces = 0;
        while (step++ <= maxStepCount)
        {
            position += velocity * stepSize;
            velocity += molly.gravity * stepSize * Vector3.down;
            time += stepSize;
            if (Physics.Raycast(position, velocity, out RaycastHit hit, velocity.magnitude * stepSize, 3))
            {
                position = hit.point;
                velocity = Vector3.Reflect(velocity, hit.normal);
                if (bounces++ == molly.targetBounces)
                {
                    if (Vector3.Distance(position, target.position) < molly.targetRadius)
                    {
                        CreateDebugSphere(position, Color.green);
                    }
                    break;
                };
                velocity *= molly.dampening;
            }

            if (showDebug)
            {
                points.Add(position);
                lineRenderer.positionCount = points.Count;
                lineRenderer.SetPositions(points.ToArray());
            }

            if ((molly.useFloorIncline && IsFloor(hit.normal)) || velocity.magnitude < molly.terminationVelocity)
            {
                break;
            }

            yield return new WaitForSeconds(1f / simulationDisplaySpeed);
        }
        yield return null;
    }

    private void CalculatePath(Vector3 vector)
    {
        Vector3 position = origin.position;
        Vector3 velocity = vector.normalized * molly.throwForce;
        float time = 0;
        int step = 0;
        int bounces = 0;
        while (step++ <= maxStepCount)
        {
            position += velocity * stepSize;
            velocity += molly.gravity * stepSize * Vector3.down;
            time += stepSize;
            if (Physics.Raycast(position, velocity, out RaycastHit hit, velocity.magnitude * stepSize, 3))
            {
                position = hit.point;
                velocity = Vector3.Reflect(velocity, hit.normal);
                if (bounces++ == molly.targetBounces)
                {
                    if (Vector3.Distance(position, target.position) < molly.targetRadius)
                    {
                        LineupCamera.addLineup(origin.position, vector);
                        if (showDebug) StartCoroutine(CalculatePathWithTrace(vector));
                    }
                    break;
                };

                if (velocity.magnitude < molly.terminationVelocity || (molly.useFloorIncline && IsFloor(hit.normal)))
                {
                    if (Vector3.Distance(position, target.position) < molly.targetRadius)
                    {
                        LineupCamera.addLineup(origin.position, vector);
                        if (showDebug) StartCoroutine(CalculatePathWithTrace(vector));
                    }
                }

                velocity *= molly.dampening;
            }
        }
    }

    /// <summary>
    /// Creates a debug sphere at the specified position and color.
    /// </summary>
    /// <param name="position">The position to create the sphere at.</param>
    /// <param name="color">The color of the sphere.</param>
    private void CreateDebugSphere(Vector3 position, Color color)
    {
        GameObject sphere = Instantiate(debugSphere);
        sphere.transform.position = position;
        sphere.GetComponent<Renderer>().material.color = color;
        sphere.gameObject.layer = 2;
        sphere.transform.parent = debugObjectsFolder.transform;
    }

    /// <summary>
    /// Generates points on the upper hemisphere for projectile simulation.
    /// </summary>
    /// <param name="samples">The number of points to generate.</param>
    /// <returns>An array of Vector3 points.</returns>
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

    /// <summary>
    /// Checks if the floor incline is within acceptable limits.
    /// </summary>
    /// <param name="normal">The normal vector of the floor surface.</param>
    /// <returns>True if the incline is acceptable, false otherwise.</returns>
    private bool IsFloor(Vector3 normal)
    {
        return Mathf.Abs(Vector3.Angle(Vector3.up, normal)) > molly.floorInclineAngle;
    }
}