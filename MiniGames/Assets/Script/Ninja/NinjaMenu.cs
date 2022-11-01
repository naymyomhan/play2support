using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NinjaMenu : MonoBehaviour
{
    public void PlayNinja(){
        SceneManager.LoadScene("NinjaGame");
    }
}
