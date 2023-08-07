using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistScript : MonoBehaviour
{
    public Vector3 parentUp;
    // Start is called before the first frame update
    void Start()
    {
        parentUp = transform.parent.transform.up;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ResetParentPosition() {
        transform.parent.transform.up = parentUp;
    }
}
