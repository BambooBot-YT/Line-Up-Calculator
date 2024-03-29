using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathWall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the death wall has a rigidbody
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // If it has a rigidbody, destroy the object
            Destroy(other.gameObject);
        }
    }
}
