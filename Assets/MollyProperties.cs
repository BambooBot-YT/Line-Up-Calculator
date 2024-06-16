using System;
using UnityEngine;

/// <summary>
/// Defines the properties of a Molly projectile.
/// </summary>
public class MollyProperties : MonoBehaviour
{
    [Header("Projectile Settings")]
    /// <summary>
    /// The initial force applied to the projectile when thrown.
    /// </summary>
    public float throwForce = 1f;

    /// <summary>
    /// The gravitational force applied to the projectile.
    /// </summary>
    public float gravity = 9.81f;

    /// <summary>
    /// The dampening factor applied to the projectile's velocity after a bounce.
    /// </summary>
    public float dampening = 0.4f;

    /// <summary>
    /// The number of bounces the projectile can make before stopping.
    /// </summary>
    public int targetBounces = 4;

    /// <summary>
    /// The radius within which the projectile must land to be considered as hitting the target.
    /// </summary>
    public float targetRadius = 3f;

    /// <summary>
    /// Determines whether the floor's incline should be considered in the projectile's behavior.
    /// </summary>
    public Boolean useFloorIncline = false;

    /// <summary>
    /// The maximum angle of incline of the floor that the projectile can handle.
    /// </summary>
    public float floorInclineAngle = 20f;

    public float terminationVelocity = 0.5f;
}