using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour
{
    public float floatFactor = 10f;
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.moveLocalY(gameObject, transform.localPosition.y+floatFactor, 0.65f).setEaseOutQuart().setLoopPingPong();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
