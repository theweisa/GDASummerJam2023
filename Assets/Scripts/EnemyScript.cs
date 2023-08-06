using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public bool Alive;
    public bool Panic;
    public float moveSpeed;
    public Vector2 moveDirection;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Panic){
            Debug.Log("Panic?" + Panic);
            GameObject[] gos;
            gos = GameObject.FindGameObjectsWithTag("NPC");

            foreach(GameObject go in gos){
                go.SendMessage("StartPanic");
            }
        }
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
        Panic = false;
        rb = rb != null ? rb : Global.FindComponent<Rigidbody2D>(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        if (Panic && Alive){
            Debug.Log("moving" + moveDirection + moveSpeed);
            rb.AddForce(moveDirection * moveSpeed);
        }
    }

    void StartPanic()
    {
        Panic = true;
        moveSpeed = 10;
        moveDirection = Random.insideUnitCircle.normalized;
        Debug.Log("Panicking" + Panic);
    }
}
