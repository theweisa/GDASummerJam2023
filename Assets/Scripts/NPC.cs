using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Unity.VisualScripting;

public class NPC : MonoBehaviour
{
    public GameObject textBoxObject;
    public BaseCharacterController controller;
    public TextBoxHandler textBox;
    public List<string> script = new List<string>();
    public float textSpeed;
    public Rigidbody2D rb;
    public bool talkedTo=false;
    public bool Alive;
    public bool Panic;
    public float moveSpeed;
    public Vector2 moveDirection;
    public float rageValue = 10f;
    [Header("try to set to value from 0-25; the higher the squeakier i think")]
    public int pitch = 10;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Punch") return;
        if (!Panic){
            StartCoroutine(GameManager.Instance.StartPanic());
        }
        StartCoroutine(OnHit());
    }

    void FixedUpdate() {
        Vector3 dir = rb.velocity.normalized;
        dir.z = 0f;
        controller.moveDirection = dir;
    }

    public IEnumerator OnHit() {
        Debug.Log("hit!");
        controller.canMove = false;
        controller.moveAnim.Stop();
        rb.drag = 3;
        if (Alive){
            Alive = false;            
        }
        if (!CheckAlive()){
            Debug.Log("Game Over");
        }
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 1f;
        CameraManager.Instance.StartShake(1f, 1f, 2f);
        Vector2 dir = ((Vector2)Global.GetMouseWorldPosition() - (Vector2)PlayerManager.Instance.transform.position).normalized;

        rb.AddForce(dir * 40, ForceMode2D.Impulse);
        yield return null;
    }
    public bool CheckAlive() {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("NPC");
        bool check=false;
        foreach(GameObject go in gos){
            check = check || Global.FindComponent<NPC>(go).Alive;
            print(Global.FindComponent<NPC>(go).Alive + " " + check);
        }
        return check;
    }
    public virtual void Start()
    {
        Alive = true;
        Panic = false;
        rb = rb != null ? rb : Global.FindComponent<Rigidbody2D>(gameObject);
        controller = Global.FindComponent<BaseCharacterController>(gameObject);
    }

    public virtual void OnMouseOver()
    {
        if (GameManager.Instance.gameState != GameState.Wait) return;
        if (Input.GetMouseButtonDown(0) 
            && PlayerManager.Instance.controller.canInteract
            && script.Count != 0)
        {
            if (textBox == null)
            {
                StartCoroutine(ShowTextbox());
                //textBox.Activate();
            }
        }
    }

    public virtual IEnumerator ShowTextbox() {
        PlayerManager.Instance.controller.StopPlayer();
        RuntimeManager.StudioSystem.setParameterByName("NPC_Pitch", pitch);
        var obj = Instantiate(textBoxObject, transform.position, Quaternion.identity, transform);
        PlayerManager.Instance.cameraPosition.position = PlayerManager.Instance.cameraPosition.position + 0.5f*(transform.position - PlayerManager.Instance.transform.position);
        obj.transform.localPosition = new Vector3(1.5f, 1.5f, 0);
        textBox = obj.GetComponent<TextBoxHandler>();
        textBox.NPC = this;
        yield return textBox.Activate();
        yield return TextboxFinished();
    }

    virtual public IEnumerator TextboxFinished() {
        PlayerManager.Instance.cameraPosition.localPosition = Vector2.zero;
        if(!talkedTo)
        {
            talkedTo = true;
            RageLogic.Instance.AddRage(rageValue);
        }
        PlayerManager.Instance.controller.ResumePlayer();
        textBox = null;
        yield return null;
    }

    void StartPanic()
    {
        Panic = true;
        controller.moveAnim.rotateTimer += Random.Range(-0.1f, 0.1f);
        moveSpeed = 0;
        moveDirection = Random.insideUnitCircle.normalized;
        Debug.Log("Panicking" + Panic);
        rb.drag = 0;
        float runForce = 10f + Random.Range(-3f, 3f);
        rb.AddForce(moveDirection * runForce, ForceMode2D.Impulse);
    }
}
