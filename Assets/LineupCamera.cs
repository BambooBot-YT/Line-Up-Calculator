using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class LineupCamera : MonoBehaviour
{
    private static List<Tuple<Vector3, Vector3>> lineups = new();
    public Camera mainCamera;
    private static int currentLineupIndex = 0;

    public static void addLineup(Vector3 position, Vector3 rotation)
    {
        lineups.Add(new Tuple<Vector3, Vector3>(position, rotation));
    }

    public static void clearLineups()
    {
        lineups.Clear();
    }

    public void loadLineup(int index)
    {
        if (index < 0 || index >= lineups.Count)
        {
            Debug.LogError("Invalid lineup index");
            return;
        }

        Tuple<Vector3, Vector3> lineup = lineups[index];
        Vector3 position = lineup.Item1;
        Vector3 rotation = lineup.Item2;
        transform.rotation = Quaternion.LookRotation(rotation);
        transform.position = position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))    
        {
            mainCamera.gameObject.SetActive(false);
            currentLineupIndex = (currentLineupIndex + 1) % lineups.Count;
            loadLineup(currentLineupIndex);
        } else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            mainCamera.gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            GameObject debugObjects = GameObject.Find("DebugObjects");
            debugObjects.SetActive(!debugObjects.activeSelf);
        }
    }
}
