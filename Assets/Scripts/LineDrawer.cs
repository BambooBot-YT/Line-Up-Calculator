using UnityEngine;
using System.Collections.Generic;

public class LineDrawer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();

    void Start()
    {
        // Create a LineRenderer and set its initial properties
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    void Update()
    {
        // Draw a line from the origin to the current mouse position
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            //points.Add(Vector3.zero); // Start point at the origin
            points.Add(mousePos); // End point at the mouse position
            UpdateLineRenderer();
        }
    }

    void UpdateLineRenderer()
    {
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }
}
