using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    
    public void Play()
    {
        Debug.Log("Game Started");
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Debug.Log("Game Ended");
        Application.Quit();
    }
}
