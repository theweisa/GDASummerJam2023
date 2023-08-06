using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public bool Alive;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (Alive){
            if (collision.gameObject.name == "Square"){
                Debug.Log("died");
                Alive = false;
            }
        }
    }
    void Start()
    {
        Alive = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
