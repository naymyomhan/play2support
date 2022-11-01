using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadSliced : MonoBehaviour
{
    
    private float thrust = 500f;
    Rigidbody2D rb;

    void Start()
    {
		rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * thrust);
    }
}
