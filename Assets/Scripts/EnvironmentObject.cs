using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class EnvironmentObject : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Rigidbody2D rb;
    public int sortLayerFactor = 2;
    void Awake() {
        sprite = sprite != null ? sprite : Global.FindComponent<SpriteRenderer>(gameObject);
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sprite.sortingOrder = transform.position.y < PlayerManager.Instance.controller.transform.position.y ? PlayerManager.Instance.controller.sprite.sortingOrder+sortLayerFactor : PlayerManager.Instance.controller.sprite.sortingOrder-sortLayerFactor;
    }

    void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.tag != "Punch") return;
        if (rb == null) return;
        RuntimeManager.PlayOneShot(FMODEventReferences.instance.ChairHit);
        CameraManager.Instance.StartShake(20f, 0.4f, 800f);
        Vector2 dir = ((Vector2)Global.GetMouseWorldPosition() - (Vector2)PlayerManager.Instance.transform.position).normalized;
        rb.AddForce(dir * 40, ForceMode2D.Impulse);
    }
    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag != "Punch") return;
        if (rb == null) return;
        RuntimeManager.PlayOneShot(FMODEventReferences.instance.ChairHit);
        CameraManager.Instance.StartShake(20f, 0.4f, 800f);
        Vector2 dir = ((Vector2)Global.GetMouseWorldPosition() - (Vector2)PlayerManager.Instance.transform.position).normalized;
        rb.AddForce(dir * 10f, ForceMode2D.Impulse);
    }
}
