using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterController : MonoBehaviour
{
    [Header("References")]
    [Space(4)]
    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    public SpriteRenderer accessory;
    public MoveAnimation moveAnim;

    [Header("Parameters")]
    [Space(4)]
    public float moveSpeed;

    [Header("Run-time variables")]
    [Space(4)]
    public Vector2 moveDirection;
    public bool canMove=true;
    protected virtual void Awake() {
        rb = rb != null ? rb : Global.FindComponent<Rigidbody2D>(gameObject);
        sprite = sprite != null ? sprite : Global.FindComponent<SpriteRenderer>(gameObject);
        moveAnim = moveAnim != null ? moveAnim : Global.FindComponent<MoveAnimation>(gameObject);
        accessory = accessory != null ? accessory : Global.FindComponent<SpriteRenderer>(transform.Find("Accessory").gameObject);
    }
    protected virtual void FixedUpdate() {
        if (!canMove) return;
        rb.AddForce(moveDirection * moveSpeed);
    }
}
