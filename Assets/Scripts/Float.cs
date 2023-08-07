using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour
{
    public float floatFactor = 10f;
    // Start is called before the first frame update
    void Start()
    {
        float time = 0.65f + Random.Range(-0.15f, 0.15f);
        LeanTween.moveLocalY(gameObject, transform.localPosition.y+floatFactor, time).setEaseOutQuart().setLoopPingPong();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
