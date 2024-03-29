using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscapeMenuManager : MonoBehaviour
{
    [Header("Escape Menu UI")]
    [Tooltip("link the escape menu root object (should be disabled by default)")]
    public GameObject escapeMenuUI;

    [Space(10)]
    [Header("Buttons")]
    [Tooltip("link each button (this is semi hard coded)")]
    public Button ExitButton;
    public Button SaveButton;
    public Button ResetButton;
    public Button ShareButton;
    private bool isEscapeMenuActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isEscapeMenuActive)
                Resume();
            else
                Pause();
        }

        ExitButton.onClick.AddListener(OnExit);
        SaveButton.onClick.AddListener(OnSave);
        ResetButton.onClick.AddListener(OnReset);
        ShareButton.onClick.AddListener(OnShare);
    }

    public void Pause()
    {
        escapeMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isEscapeMenuActive = true;
    }

    public void Resume()
    {
        escapeMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isEscapeMenuActive = false;
    }

    public void OnExit()
    {
        Application.Quit();
    }

    public void OnSave()
    {
        // Save the game
    }

    public void OnReset()
    {
        // Reset the game
    }

    public void OnShare()
    {
        // Share the game
    }
}