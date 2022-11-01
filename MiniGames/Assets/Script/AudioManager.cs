using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{   
    public static AudioManager Instance;
    void Awake() {
         if(Instance==null)
            Instance=this;
    }

    [SerializeField]private AudioSource themeSong;
    [SerializeField]private AudioSource click;

    public void PlayTheme(){
        themeSong.Play();
    }

    public void Click(){
        click.Play();
    }
}
