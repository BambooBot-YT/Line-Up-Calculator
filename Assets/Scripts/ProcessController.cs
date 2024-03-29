using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for accessing UI components

public class ProcessController : MonoBehaviour
{
    [Header("Process Controller")]
    public Button searchButton;

    private void Start()
    {
        // Add listener to the button's onClick event
        searchButton.onClick.AddListener(StartSearch);
    }

    // Attach this method to the button's onClick event in the Unity Editor
    public void StartSearch()
    {
    }
}