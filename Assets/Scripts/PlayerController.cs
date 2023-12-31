using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using FMODUnity;

public class PlayerController : BaseCharacterController
{
    [Header("Player References")]
    [Space(4)]
    public float punchForce = 50f;
    [HideInInspector] public float basePunchForce;
    public Collider2D punchHitbox;
    public bool canPunch = false;
    public bool canInteract = true;
    public bool rage;
    [HideInInspector] public bool makeRed=true;
    private Color ogColor;
    public Animator fists;
    public List<GameObject> enemiesHit = new List<GameObject>();
    //public Animator LFistAnim;
    //public Animator RFistAnim;
    protected override void Awake() {
        base.Awake();
        basePunchForce = punchForce;
        if (punchHitbox){
            punchHitbox.enabled = false;
            punchHitbox.GetComponent<Renderer>().enabled = false;
        }
        rage = false;
        ogColor = sprite.color;
    }

    void OnMove(InputValue value) {
        Vector2 prevMove = moveDirection;
        moveDirection = value.Get<Vector2>();
    }

    protected override void CheckFlip()
    {
        if (sprite.flipX != (Global.GetMouseWorldPosition()-transform.position).x > 0f) {
            sprite.flipX = !sprite.flipX;
            accessory.flipX = sprite.flipX;
            moveAnim.Flip();
        }
        accessory.flipX = sprite.flipX;
    }

    override protected void FixedUpdate()
    {
        base.FixedUpdate();
        PositionPunchHitbox();
        if (makeRed) {
            SetRed(RageLogic.Instance.rageMeter.value);
        }
    }

    void PositionPunchHitbox() {
        if (!rage) return;
        Vector3 positionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 towardsMouseFromPlayer = positionMouse - transform.position;
        towardsMouseFromPlayer.z = 0;
        towardsMouseFromPlayer = towardsMouseFromPlayer.normalized;
        punchHitbox.transform.position = Vector3.MoveTowards(punchHitbox.transform.position, transform.position + towardsMouseFromPlayer * 2, 16);

        Vector2 mousePos = positionMouse;
        Vector3 prevUp = fists.transform.up;
        fists.transform.up = -(Vector3)(mousePos - (Vector2)fists.transform.position);
    }

    public void SetRed(float percentRed) {
        sprite.color = new Color(
            1f,
            Global.Map(percentRed, 0f, 100f, 1f, 0f),
            Global.Map(percentRed, 0f, 100f, 1f, 0f),
            1f
        );
    }

    void OnFire(InputValue value){
        if (!canPunch) return;
        StartCoroutine(punch());
        //StartCoroutine(punch());
    }

    public void DoPunch() {
        StartCoroutine(punch());
    }

    IEnumerator punch(){
        fists.Play("fistsPunch", -1, 0f);

        FMODUnity.RuntimeManager.PlayOneShot(FMODEventReferences.instance.PunchWhiff);
        Vector3 towardsMouseFromPlayer = ((Vector3)((Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition)) - (Vector2)transform.position)).normalized;
        rb.AddForce(punchForce*towardsMouseFromPlayer, ForceMode2D.Impulse);
        punchHitbox.enabled = true;
        Debug.Log("Punching");
        yield return new WaitForSeconds(0.1f);
        punchHitbox.enabled = false;
        enemiesHit.Clear();
    }

    public void StopPlayer() {
        moveAnim.Stop();
        canMove = false;
        canInteract = false;
        canPunch = false;
    }
    public void ResumePlayer() {
        canMove = true;
        canInteract = true;
        if (rage)
            canPunch = true;

    }
    void makeRage(){
        rage = true;
        punchHitbox.GetComponent<Renderer>().enabled = true;
    }
}
