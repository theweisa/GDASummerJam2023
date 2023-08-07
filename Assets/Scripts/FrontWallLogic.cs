using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontWallLogic : MonoBehaviour
{
    public SpriteRenderer sprite;
    private Color ogColor;
    void Awake() {
        sprite = Global.FindComponent<SpriteRenderer>(gameObject);
        ogColor = sprite.color;
    }
    void OnTriggerEnter2D(Collider2D coll) {
        //if (coll.GetComponent<Rigidbody2D>() == null) return;
        if (coll.gameObject.tag != "Player") return;
        Color c = ogColor;
        c.a = 0.25f;
        LeanTween.color(gameObject, c, 0.2f);

    }

    void OnTriggerExit2D(Collider2D coll) {
        //if (coll.GetComponent<Rigidbody2D>() == null) return;
        if (coll.gameObject.tag != "Player") return;
        Debug.Log("trigger exit: " + coll);
        LeanTween.color(gameObject, ogColor, 0.2f);
    }
}
