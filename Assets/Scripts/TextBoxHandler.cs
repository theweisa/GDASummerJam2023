using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMOD.Studio;
using FMODUnity;

public class TextBoxHandler : MonoBehaviour
{
    public GameObject textBox;
    public TMP_Text text;
    public NPC NPC;

    private float typingSpeed = 0.1f;

    public bool canContinue = true;
    void Start()
    {

    }

    public void Activate()
    {
        PlayerManager.Instance.controller.canMove = false;
        PlayerManager.Instance.controller.moveAnim.Stop();
        canContinue = false;
        textBox.SetActive(true);
        StartCoroutine(DisplayLine(NPC.script[0]));
    }

    public void Deactivate()
    {
        PlayerManager.Instance.controller.canMove = true;
        textBox.SetActive(false);
    }

    private IEnumerator DisplayLine(string line)
    {
        RuntimeManager.StudioSystem.setParameterByName("NPC_Pitch", Random.Range(0, 25));
        text.text = "";
        foreach (char letter in line.ToCharArray())
        {
            text.text += letter;
            RuntimeManager.PlayOneShot(FMODEventReferences.instance.DialogueBlip);
            yield return new WaitForSeconds(typingSpeed);
        }
        canContinue = true;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && canContinue && gameObject.activeSelf)
        {
            Deactivate();
        }
    }
}
