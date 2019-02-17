using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public bool gameOver = true;
    public bool muteSoundFX = false;


    //Player (Single player Mode)
    [SerializeField]
    private Player _playerPrefab;
    [SerializeField]
    private GameObject _mobileControls;
    //UI Manager
    private UIManager _uiManager;
    //Spawn Manager
    private SpawnManager _spawnManager;


    // Use this for initialization
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }


    public void StartGame()
    {
        //set gameOver to be false
        gameOver = false;
        Time.timeScale = 1;

        //hide the Title Screen and display player's lives
        _uiManager.HideTitleScreen();

        //reset and display the Score
        _uiManager.score = 0;
        _uiManager.scoreText.text = "Score: " + _uiManager.score;

        //add the player with 3 lives
        Instantiate(_playerPrefab, transform.position = new Vector3(0, -2.5f, 0), Quaternion.identity);
        //display the number of lives
        _uiManager.UpdateLives(_playerPrefab.lives);

#if UNITY_ANDROID
        //display controls for mobile game
        _mobileControls.SetActive(true);
#endif
        
        //display the Best Score if there's a previous best one
        _uiManager.bestScore = PlayerPrefs.GetInt("BestScore");
        if (_uiManager.bestScore > 0)
        {
        _uiManager.bestScoreText.text = "Best Score: " + _uiManager.bestScore;
        }

        //start spawning enemies and powerups
        if (_spawnManager != null)
        {
            _spawnManager.StartSpawnRoutines();
        }
    }


    //Pauses the Game
    public void PauseGame()
    {
        Animator pauseMenu = GameObject.Find("Pause_Menu_Panel").GetComponent<Animator>();
        if(pauseMenu != null)
        {
            pauseMenu.updateMode = AnimatorUpdateMode.UnscaledTime;
            pauseMenu.SetBool("isPaused", true);
        }
        _uiManager.pauseButton.SetActive(false);
        Time.timeScale = 0;
    }

    //Resumes a paused Game
    public void ResumeGame()
    {
        Animator pauseMenu = GameObject.Find("Pause_Menu_Panel").GetComponent<Animator>();
        if (pauseMenu != null)
        {
            pauseMenu.SetBool("isPaused", false);
        }
        _uiManager.pauseButton.SetActive(true);
        Time.timeScale = 1;
    }

    //Controls the Background Music
    public void MusicControl()
    {
        AudioSource bgMusic = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        bgMusic.mute = !bgMusic.mute;
    }

    //Mute/Unmute SoundFX
    public void SoundFXControl()
    {
        muteSoundFX = !muteSoundFX;
    }


    //Quit Game
    public void QuitGame()
    {
        //hides the pause menu
        Animator pauseMenu = GameObject.Find("Pause_Menu_Panel").GetComponent<Animator>();
        if (pauseMenu != null)
        {
            pauseMenu.SetBool("isPaused", false);
        }

        //set time scale back to normal
        Time.timeScale = 1;

        //destroys instantiated clones
        var clones = GameObject.FindGameObjectsWithTag("Clone");
        foreach (var item in clones)
        {
            Destroy(item);
        }

        var player = GameObject.FindGameObjectWithTag("Player");
        Destroy(player);
  

        //stop spawning enemies and powerups immediately
        _spawnManager.StopSpawnRoutines();

        //set game over to true
        gameOver = true;

        //hides controls and shows title screen after 1sec
        StartCoroutine("TitleScreenDelayOnQuit");
    }

    private IEnumerator TitleScreenDelayOnQuit()
    {
        yield return new WaitForSeconds(1.0f);
        _mobileControls.SetActive(false);
        _uiManager.ShowTitleScreen();
    }

    //Ends the Game
    public void GameOver()
    {
        //Check if we need to update the best score
        _uiManager.CheckBestScore();

        //destroy all the instantiated clones
        var clones = GameObject.FindGameObjectsWithTag("Clone");
        foreach (var item in clones)
        {
            Destroy(item);
        }

        //stop spawning enemies and powerups immediately
        _spawnManager.StopSpawnRoutines();

        //set the gameOver state to true after all animations are over
        StartCoroutine(EndGame());
    }

    //Delays the gameOver state and the Title Screen
    //Gives time to all animations to be over and all instantiated objects to be destroyed
    public IEnumerator EndGame()
    {
        yield return new WaitForSeconds(3.0f);
        gameOver = true;
        _mobileControls.SetActive(false);
        _uiManager.ShowTitleScreen();

    }

}