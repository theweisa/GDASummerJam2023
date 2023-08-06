using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public GameObject textBoxObject;
    public TextBoxHandler textBox;
    public List<string> script = new List<string>();
    public float textSpeed;
    void Start()
    {
        
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textBox == null)
            {
                var obj = Instantiate(textBoxObject, transform.position, Quaternion.identity);
                textBox = obj.GetComponent<TextBoxHandler>();
                textBox.NPC = this;
                textBox.transform.SetParent(transform);
                textBox.transform.Translate(0f, 4f, 0f);
                textBox.Activate();
            }
            else if(textBox.isActiveAndEnabled == false)
            {
                textBox.Activate();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
