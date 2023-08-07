using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class NPC : MonoBehaviour
{
    public GameObject textBoxObject;
    public TextBoxHandler textBox;
    public List<string> script = new List<string>();
    public float textSpeed;
    public Rigidbody2D rb;
    public bool Alive;
    public bool Panic;
    public float moveSpeed;
    public Vector2 moveDirection;

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
        if (collision.gameObject.tag == "Punch"){
            Debug.Log(collision.gameObject.tag);
            if (Alive){
                Alive = false;
                rb.drag = 1;
                GameObject[] gos;
                gos = GameObject.FindGameObjectsWithTag("NPC");
                bool check = false;
                foreach(GameObject go in gos){
                    check = check || Global.FindComponent<NPC>(go).Alive;
                    print(Global.FindComponent<NPC>(go).Alive + " " + check);
                }
                if (!check){
                    Debug.Log("Game Over");
                }
            }
            rb.AddForce(collision.gameObject.transform.parent.GetComponent<Rigidbody2D>().velocity.normalized * 700);
        }
    }
    void Start()
    {
        Alive = true;
        Panic = false;
        rb = rb != null ? rb : Global.FindComponent<Rigidbody2D>(gameObject);
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textBox == null)
            {
                RuntimeManager.StudioSystem.setParameterByName("NPC_Pitch", Random.Range(0, 25));
                var obj = Instantiate(textBoxObject, transform.position, Quaternion.identity);
                textBox = obj.GetComponent<TextBoxHandler>();
                textBox.NPC = this;
                textBox.transform.SetParent(transform);
                textBox.transform.Translate(0f, 4f, 0f);
                textBox.Activate();
            }
            else if(textBox.isActiveAndEnabled == false)
            {
                textBox.Activate();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {

    }

    void StartPanic()
    {
        Panic = true;
        moveSpeed = 500;
        moveDirection = Random.insideUnitCircle.normalized;
        Debug.Log("Panicking" + Panic);
        rb.drag = 0;
        rb.AddForce(moveDirection * moveSpeed);
    }
}
