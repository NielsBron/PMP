using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] public GameObject PauseMenuUI;
    [SerializeField] public GameObject TestPanel;
    [SerializeField] public GameObject LoadingCanvas;
    
    [SerializeField] public GameObject Player;
    
    [SerializeField] private AudioMixer myAudioMixer;
    [SerializeField] GenerationManager GenerationManagerScript; 

    public bool isTesting;
    public static bool GameIsPaused = false;

    public void SetVolume (float Volume)
    {
        myAudioMixer.SetFloat("Volume", Mathf.Log10(Volume) * 20);
    }

    public void Start()
    {
        if(isTesting == true)
        {
            TestPanel.SetActive(true);
        }
        
        if(isTesting == false)
        {
            LoadingCanvas.SetActive(true);
            TestPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            StartGame();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            } 
            else
            {
                Pause();
            }
        }
    }
    
    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        GameIsPaused = true;
        Player.GetComponent<FirstPersonController>().enabled=false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
          
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        GameIsPaused = false;
        Player.GetComponent<FirstPersonController>().enabled=true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("Resumed Game");
    }
    
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
    
    public void Settings()
    {
        Debug.Log("Settings pressed");
    }

    public void StartGame()
    {
        StartCoroutine(StartGameFunction());
    }

    IEnumerator StartGameFunction()
    {
        yield return new WaitForSeconds(1.0f);
        GenerationManagerScript.GenerateWorld();
        yield return new WaitForSeconds(3.0f);
        LoadingCanvas.SetActive(false);
        GenerationManagerScript.SpawnPlayer();
    }

/*
    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && onMenu == false && PlayerIsAlive == true)
        {
            PlayerToMenu();
            Debug.Log("Escape Button Pressed");
            onMenu = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && onMenu == true && PlayerIsAlive == true)
        {
            MenuToPlayer();
            Debug.Log("Escape Button Pressed");
            onMenu = false;
        }
    }
    
    public void PlayerToMenu()
    {
        Debug.Log("Menu Opened");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PlayerCanvas.SetActive(true);
    }

    public void MenuToPlayer()
    {
        Debug.Log("Menu closed");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerCanvas.SetActive(false);
    }*/
}
