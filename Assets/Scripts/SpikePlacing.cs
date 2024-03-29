using UnityEngine;
using System.Collections;


public class SpikePlacing : MonoBehaviour
{
    private bool hasMoved = false; // Track if the object has already moved
    private bool isMoving = false;
    public GameObject objectToDisappear;

    void Update()
    {
        // Check for mouse click and whether the object hasn't moved yet
        if (Input.GetMouseButtonDown(0) && !hasMoved)
        {
            // Get the position of the mouse pointer in the world
            Vector3 targetPosition = GetMouseWorldPosition();

            // Start moving the object to the mouse position
            StartCoroutine(MoveCoroutine(targetPosition));

            // Make the other object disappear
            if (objectToDisappear != null)
            {
                objectToDisappear.SetActive(false);
            }

            // Set hasMoved to true to prevent further movement
            hasMoved = true;
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        // Get mouse position in screen space
        Vector3 mousePosition = Input.mousePosition;

        // Set the distance from the camera along the local z-axis
        float distanceFromCamera = 20f;

        // Cast a ray from the mouse position into the scene
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        // Check if the ray intersects with any objects in the scene
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // Return the hit point as the target position (with adjusted y-coordinate)
            return new Vector3(hit.point.x, transform.position.y, hit.point.z);
        }

        // If no intersection, return a default position based on mouse position and camera distance
        return ray.origin + ray.direction * distanceFromCamera;
    }

    IEnumerator MoveCoroutine(Vector3 targetPosition)
    {
        isMoving = true;

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            // Move the object to the target position using Unity's built-in Lerp function
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f); // Adjust the speed by modifying the multiplier (e.g., 5f)
            yield return null;
        }

        // Ensure the object reaches exactly the target position
        transform.position = targetPosition;
        isMoving = false;
    }
}
