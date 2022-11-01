using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{   
    public GameObject leftHeadSlicedPrefab;
    public GameObject rightHeadSlicedPrefab;
    public GameObject upperHeadSlicedPrefab;
    public GameObject lowerHeadSlicedPrefab;
    public GameObject bloodPrefab;
    public float startForce=10f;

    private GameObject leftHead;
    private GameObject rightHead;

    Quaternion headRotation;

    Rigidbody2D rb;

    private float force=200f;

    void Start(){
        rb=GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up*startForce,ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D col) {
        // Vector3 direction= (col.transform.position - transform.position).normalized;

        Quaternion rotation = Quaternion.LookRotation(Blade.Instance.direction);
        rotation.y=0f;
        rotation.x=0f;
        if(rotation.z < 0){
            headRotation=Quaternion.Euler(0, 0, ((float)(rotation.z*-60)+45f));
        }else{
            headRotation=Quaternion.Euler(0, 0, ((float)(rotation.z*60)-45f));
        }
        if(col.tag=="Blade"){
            int randNo=Random.Range (1, 3);
            if(randNo==1){
                leftHead = Instantiate(leftHeadSlicedPrefab,transform.position,headRotation);
                rightHead = Instantiate(rightHeadSlicedPrefab,transform.position,headRotation);
                leftHead.GetComponent<Rigidbody2D>().AddForce(transform.right * -force);
                rightHead.GetComponent<Rigidbody2D>().AddForce(transform.right * force);
            }else{
                leftHead = Instantiate(upperHeadSlicedPrefab,transform.position,headRotation);
                rightHead = Instantiate(lowerHeadSlicedPrefab,transform.position,headRotation);
                leftHead.GetComponent<Rigidbody2D>().AddForce(transform.right * force);
                rightHead.GetComponent<Rigidbody2D>().AddForce(transform.right * -force);
            }
            Instantiate(bloodPrefab,transform.position,rotation);

            Destroy(gameObject);
            Destroy(leftHead,1f);
            Destroy(rightHead,1f);
        }
    }
}
