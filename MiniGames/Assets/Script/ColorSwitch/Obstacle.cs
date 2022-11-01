using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    void Update()
    {
        if(Mathf.Abs(CsPlayer.Instance.transform.position.y-transform.position.y)>50){
            Destroy(gameObject);
        }
    }
}
