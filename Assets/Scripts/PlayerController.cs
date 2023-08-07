using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : BaseCharacterController
{
    [Header("Player References")]
    [Space(4)]
    public Collider2D punchHitbox;
    public bool canPunch = false;
    public bool canInteract = true;
    public bool rage;
    protected override void Awake() {
        base.Awake();
        if (punchHitbox){
            punchHitbox.enabled = false;
            punchHitbox.GetComponent<Renderer>().enabled = false;
        }
        rage = false;
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

    void Update()
    {
        Vector3 positionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 towardsMouseFromPlayer = positionMouse - transform.position;
        towardsMouseFromPlayer.z = 0;
        towardsMouseFromPlayer = towardsMouseFromPlayer.normalized;
        punchHitbox.transform.position = Vector3.MoveTowards(punchHitbox.transform.position, transform.position + towardsMouseFromPlayer * 4, 16);
    }

    void OnFire(InputValue value){
        if (!canPunch) return;
        StartCoroutine(punch());
    }

    IEnumerator punch(){
        FMODUnity.RuntimeManager.PlayOneShot(FMODEventReferences.instance.PunchWhiff);
        Vector3 positionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 towardsMouseFromPlayer = positionMouse - transform.position;
        towardsMouseFromPlayer.z = 0;
        towardsMouseFromPlayer = towardsMouseFromPlayer.normalized;
        rb.AddForce(50f*towardsMouseFromPlayer, ForceMode2D.Impulse);
        //transform.position = Vector3.MoveTowards(transform.position, transform.position + towardsMouseFromPlayer * 2, 16);
        punchHitbox.enabled = true;
        Debug.Log("Punching");
        yield return new WaitForSeconds(.5f);
        punchHitbox.enabled = false;
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
