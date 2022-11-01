using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    public static GameManager Instance;
    void Awake() {
         if(Instance==null)
            Instance=this;
    }

    public bool isPaused=false;

    public void GoToMenu(){
        Time.timeScale=1;
        isPaused=false;
        SceneManager.LoadScene("MainMenu");
    }

    public void AddStar(int starCount){
        int currentStarCount=PlayerPrefs.GetInt("Star",0);
        PlayerPrefs.SetInt("Star",currentStarCount+starCount);
    }
}
