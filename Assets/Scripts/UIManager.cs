using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Lives
    public GameObject playerLives;
    public Sprite[] lives; //array containing all possible states
    public Image currentLives; //image that represents the current number of lives

    //pause
    public GameObject pauseButton;
    public GameObject pausePanel;

    /*options
    public GameObject optionsButton;
    public GameObject optionsMenu;*/

    //Score
    public int score = 0;
    public Text scoreText;
    public int bestScore = 0;
    public Text bestScoreText;

    //Title Screen
    public GameObject titleScreen;
    public GameObject pressStart;

    //Update Image that represents Player's lives
    public void UpdateLives(int livesCount)
    {
        currentLives.sprite = this.lives[livesCount];
    }


    //Update Game score
    public void UpdateScore()
    {
        score += 10;
        scoreText.text = "Score: " + score;
    }

    //Check for Best Score
    public void CheckBestScore()
    {
        if(score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
            bestScoreText.text = "Best Score: " + bestScore;
        }
    }

    //Show Title Screen after 3 secs, so we can wait for the explosion animations to be finished
    public void ShowTitleScreen()
    {
        titleScreen.SetActive(true);
        pressStart.SetActive(true);
        //optionsButton.SetActive(true);
        playerLives.SetActive(false);
        pauseButton.SetActive(false);
    }
    //Hide Title Screen
    public void HideTitleScreen()
    {
        titleScreen.SetActive(false);
        pressStart.SetActive(false);
        //optionsButton.SetActive(false);
        playerLives.SetActive(true);
        pauseButton.SetActive(true);
    }

    /*
    //Open Options Menu
    public void OpenOptionsMenu()
    {
        titleScreen.SetActive(false);
        pressStart.SetActive(false);
        optionsButton.SetActive(false);
        optionsMenu.SetActive(true);
    }

    //Close Options Menu
    public void CloseOptionsMenu()
    {
        titleScreen.SetActive(true);
        pressStart.SetActive(true);
        optionsButton.SetActive(true);
        optionsMenu.SetActive(false);
    }
    */
}
