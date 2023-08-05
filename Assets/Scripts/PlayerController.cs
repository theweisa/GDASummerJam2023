using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Parameters")]
    [Space(4)]
    public Rigidbody2D rb;
    public float moveSpeed;

    [Header("Run-time variables")]
    [Space(4)]
    public Vector2 moveDirection;

    void Awake() {
        rb = rb != null ? rb : Global.FindComponent<Rigidbody2D>(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {
        rb.AddForce(moveDirection * moveSpeed);
    }

    void OnMove(InputValue value) {
        moveDirection = value.Get<Vector2>();
    }
}
