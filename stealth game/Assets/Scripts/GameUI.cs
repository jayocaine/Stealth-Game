using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public GameObject gamesLoseUI;
    public GameObject gameWinUI;
    bool gameIsOver;
    bool hasWon;
    bool hasLost;
 
    void Start()
    {
        Guard.OnGuardHasSpottedPlayer += ShowGameLoseUI;
        FindObjectOfType<Player>().OnReachedEndOfLevel += ShowGameWinUI;
    }

  
    void Update()
    {
        //exit to main menu on presing escape
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            LevelLockManager.MainMenu();
        }

        if (gameIsOver && hasWon) 
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire2") )
            {
                LevelLockManager.Next();

            }
        }
        if(gameIsOver && hasLost) 
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire2"))
            {
                LevelLockManager.MainMenu();
                
            }
        }
    }
    void ShowGameWinUI() 
    {
        OnGameOver(gameWinUI);
         hasWon = true;

    }
     void ShowGameLoseUI()
    {
        OnGameOver(gamesLoseUI);
         hasLost = true;
    }
    void OnGameOver(GameObject gameOverUI)
    {
        //Ignore this if the scene has already been destroyed
        if ( gameOverUI == null) {
            return;
        }

        gameOverUI.SetActive(true);
        gameIsOver = true;
        Guard.OnGuardHasSpottedPlayer -= ShowGameLoseUI;
        FindObjectOfType<Player>().OnReachedEndOfLevel -= ShowGameWinUI;

    }
}
