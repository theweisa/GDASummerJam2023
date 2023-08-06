using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Parameters")]
    [Space(4)]
    public Rigidbody2D rb;
    public Collider2D punchHitbox;
    public float moveSpeed;

    [Header("Run-time variables")]
    [Space(4)]
    public Vector2 moveDirection;

    void Awake() {
        rb = rb != null ? rb : Global.FindComponent<Rigidbody2D>(gameObject);
        if (punchHitbox) punchHitbox.enabled = false;
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

    void OnFire(InputValue value){
        StartCoroutine(punch());
    }

    IEnumerator punch(){
        Vector3 positionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 towardsMouseFromPlayer = positionMouse - transform.position;
        towardsMouseFromPlayer = towardsMouseFromPlayer.normalized;
        punchHitbox.transform.position = transform.position;
        punchHitbox.transform.position = Vector3.MoveTowards(punchHitbox.transform.position, punchHitbox.transform.position + towardsMouseFromPlayer * 6, 16);
        transform.position = Vector3.MoveTowards(transform.position, transform.position + towardsMouseFromPlayer * 2, 16);
        punchHitbox.enabled = true;
        Debug.Log("Punching");
        yield return new WaitForSeconds(.5f);
        punchHitbox.enabled = false;
    }
}
