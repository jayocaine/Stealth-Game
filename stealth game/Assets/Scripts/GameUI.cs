﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public GameObject gamesLoseUI;
    public GameObject gameWinUI;
    bool gameIsOver;
 
    void Start()
    {
        Guard.OnGuardHasSpottedPlayer += ShowGameLoseUI;
        FindObjectOfType<Player>().OnReachedEndOfLevel += ShowGameWinUI;
    }

  
    void Update()
    {
        if (gameIsOver) 
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire2") )
            {
                SceneManager.LoadScene(0);
            }
        }
    }
    void ShowGameWinUI() 
    {
        OnGameOver(gameWinUI);

    }
    void ShowGameLoseUI()
    {
        OnGameOver(gamesLoseUI);
    }
    void OnGameOver(GameObject gameOverUI)
    {
        gameOverUI.SetActive(true);
        gameIsOver = true;
        Guard.OnGuardHasSpottedPlayer -= ShowGameLoseUI;
        FindObjectOfType<Player>().OnReachedEndOfLevel -= ShowGameWinUI;

    }
}