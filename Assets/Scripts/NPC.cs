using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Unity.VisualScripting;

public class NPC : MonoBehaviour
{
    public GameObject textBoxObject;
    public GameObject interactPrompt;
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
    public Animator anim;
    [Header("try to set to value from 0-25; the higher the squeakier i think")]
    public int pitch = 10;

    protected float hitstun = 0.21f;
    Coroutine sporadicMove;

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

    public virtual IEnumerator OnHit() {
        controller.accessory.gameObject.SetActive(false);
        Debug.Log("hit!");
        controller.moveAnim.Stop();
        FMODUnity.RuntimeManager.PlayOneShot(FMODEventReferences.instance.PunchHit);        
        controller.canMove = false;
        
        anim.SetBool("dead", true);
        Vector2 dir = ((Vector2)Global.GetMouseWorldPosition() - (Vector2)PlayerManager.Instance.transform.position).normalized;
        rb.AddForce(dir * 40, ForceMode2D.Impulse);
        rb.drag = 3;
        if (Alive){
            Alive = false;   
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(hitstun);
            Time.timeScale = 1f;
            CameraManager.Instance.StartShake(60f, 0.4f, 500f);
        }
        else {
            CameraManager.Instance.StartShake(30f, 0.4f, 100f);
        }
        

        if (!CheckAlive()){
            Debug.Log("Game Over");
            StartCoroutine(GameManager.Instance.PostRage());
        }
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
        anim = Global.FindComponent<Animator>(gameObject);
        sporadicMove = StartCoroutine(SporadicMovement());
    }

    public virtual IEnumerator SporadicMovement() {
        float time = 3f + Random.Range(0f, 3f);
        while (GameManager.Instance.gameState != GameState.PanicStart) {
            yield return new WaitForSeconds(time);
            if (GameManager.Instance.gameState == GameState.PanicStart) continue;
            time = 3f + Random.Range(0f, 3f);
            Vector2 dir = Random.insideUnitCircle.normalized;
            float force = 5f + Random.Range(0f, 3f);
            rb.AddForce(force*dir, ForceMode2D.Impulse);
        }
        FacePlayer();
    }

    public virtual void OnMouseOver()
    {
        if (GameManager.Instance.gameState != GameState.Wait) return;
        if (interactPrompt != null && script.Count != 0) {
            interactPrompt.SetActive(true);
        }
        if (Input.GetMouseButtonDown(0) 
            && PlayerManager.Instance.controller.canInteract
            && script.Count != 0)
        {
            if (textBox == null)
            {
                FacePlayer();
                StartCoroutine(ShowTextbox());
                //textBox.Activate();
            }
        }
    }
    public virtual void OnMouseExit() {
        if (interactPrompt.gameObject.activeSelf) {
            interactPrompt.SetActive(false);
        }
    }
    public void FacePlayer() {
        if (controller.sprite.flipX != (PlayerManager.Instance.transform.position-transform.position).x > 0f) {
            controller.sprite.flipX = !controller.sprite.flipX;
            controller.accessory.flipX = controller.sprite.flipX;
            controller.moveAnim.Flip();
        }
    }   

    public virtual IEnumerator ShowTextbox() {
        if (sporadicMove != null)
            StopCoroutine(sporadicMove);
        if (interactPrompt)
            interactPrompt.SetActive(false);
        controller.rb.velocity = Vector2.zero;
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
        if (sporadicMove != null)
            StopCoroutine(sporadicMove);
        sporadicMove = StartCoroutine(SporadicMovement());
        textBox = null;
        yield return null;
    }

    void StartPanic()
    {
        if (!Alive) {
            Panic = true;
            return;
        }
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
