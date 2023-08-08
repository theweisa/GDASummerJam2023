using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchHitbox : MonoBehaviour
{
    public List<GameObject> enemiesHit = new List<GameObject>();
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enemiesHit.Contains(collision.gameObject)) {
            enemiesHit.Add(collision.gameObject);
        }
        Debug.Log("Collision detected");
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
