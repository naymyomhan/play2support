using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ColorSwitchMenu : MonoBehaviour
{   
    public TextMeshProUGUI highScore;

    void Start(){
        highScore.text="High Score :" + PlayerPrefs.GetInt("Best",0);
    }

    public void PlayColorSwitch(){
        SceneManager.LoadScene("ColorSwitchGame");
    }
}
