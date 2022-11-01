using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwitchAudio : MonoBehaviour
{
    public static ColorSwitchAudio Instance;
    void Awake() {
         if(Instance==null)
            Instance=this;
    }

    [SerializeField]private AudioSource themeSong;
    [SerializeField]private AudioSource click;
    [SerializeField]private AudioSource jump;
    [SerializeField]private AudioSource colorChange;
    [SerializeField]private AudioSource eatStar;
    [SerializeField]private AudioSource die;

    public void PlayTheme(){
        themeSong.Play();
    }

    public void Click(){
        click.Play();
    }

    public void Jump(){
        jump.Play();
    }

    public void ColorChange(){
        colorChange.Play();
    }

    public void EatStar(){
        eatStar.Play();
    }

    public void Die(){
        die.Play();
    }
}
