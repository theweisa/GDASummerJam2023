using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentObject : MonoBehaviour
{
    public SpriteRenderer sprite;
    void Awake() {
        sprite = sprite != null ? sprite : Global.FindComponent<SpriteRenderer>(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sprite.sortingOrder = transform.position.y < PlayerManager.Instance.controller.transform.position.y ? PlayerManager.Instance.controller.sprite.sortingOrder+1 : PlayerManager.Instance.controller.sprite.sortingOrder-1;
    }
}
