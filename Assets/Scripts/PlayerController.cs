using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : BaseCharacterController
{
    [Header("Player References")]
    [Space(4)]
    public Collider2D punchHitbox;

    protected override void Awake() {
        base.Awake();
        if (punchHitbox) punchHitbox.enabled = false;
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

    void OnFire(InputValue value){
        StartCoroutine(punch());
    }

    IEnumerator punch(){
        Vector3 positionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 towardsMouseFromPlayer = positionMouse - transform.position;
        towardsMouseFromPlayer = towardsMouseFromPlayer.normalized;
        punchHitbox.transform.position = transform.position;
        punchHitbox.transform.position = Vector3.MoveTowards(punchHitbox.transform.position, punchHitbox.transform.position + towardsMouseFromPlayer * 6, 16);
        rb.AddForce(50f*towardsMouseFromPlayer, ForceMode2D.Impulse);
        //transform.position = Vector3.MoveTowards(transform.position, transform.position + towardsMouseFromPlayer * 2, 16);
        punchHitbox.enabled = true;
        Debug.Log("Punching");
        yield return new WaitForSeconds(.5f);
        punchHitbox.enabled = false;
    }
}
