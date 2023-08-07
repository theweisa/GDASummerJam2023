using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class DeskWorker : NPC
{
    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textBox == null)
            {
                RuntimeManager.StudioSystem.setParameterByName("NPC_Pitch", Random.Range(0, 25));
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

    void Update()
    {
        if(RageLogic.Instance.fullRage)
        {
            script = new List<string>(){"I'm sorry, you're boned"};
        }
    }
}
