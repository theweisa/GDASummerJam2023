using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        PlayerController.Instance.canMove = false;
        canContinue = false;
        textBox.SetActive(true);
        StartCoroutine(DisplayLine(NPC.script[0]));
    }

    public void Deactivate()
    {
        PlayerController.Instance.canMove = true;
        textBox.SetActive(false);
    }

    private IEnumerator DisplayLine(string line)
    {
        text.text = "";
        foreach (char letter in line.ToCharArray())
        {
            text.text += letter;
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
