using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : BaseCharacterController
{
    public NPC npc;
    protected override void FixedUpdate() {
        moveDirection = rb.velocity.normalized;
        CheckFlip();
        if (npc.Alive) {
            if (moveDirection.magnitude != 0f) {
                moveAnim.Move();
            }
            else {
                moveAnim.Stop();
            }
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        npc = Global.FindComponent<NPC>(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
