﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject uiCanvas;
    public GameObject leftCardPanel;
    public GameObject rightCardPanel;
    public GameObject gameEndPanel;

    public Text staminaText;
    public Text healthText;
    public Text gameEndText;
    public Text drillHealthText;
    public Text drillTimerText;

    private bool paused;
    // Start is called before the first frame update
    void Start()
    {
        paused = false;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu() {
        if (paused) {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        } else {
            pausePanel.SetActive(true);
            Time.timeScale = 0.5f;
        }
        paused = !paused;
    }

    public void Quit() {
        Application.Quit();
    }
}
