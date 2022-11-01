using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{      
    
    public TextMeshProUGUI starCount;

    void Start(){
        starCount.text=PlayerPrefs.GetInt("Star",0).ToString();
        AudioManager.Instance.PlayTheme();
    }

}
