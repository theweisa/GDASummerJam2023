using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveText : MonoBehaviour
{
    public TMP_Text text;
    private Vector3 targetScale;
    void Start()
    {
        gameObject.SetActive(false);
        targetScale = gameObject.transform.localScale;
    }

    public void ActivateObjective(string objectiveText)
    {
        text.text = objectiveText;
        gameObject.SetActive(true);
        StartCoroutine(AnimatePopIn());
    }

    public void DeactivateObjective()
    {
        StartCoroutine(AnimateShrinkOut());
    }

    private IEnumerator AnimatePopIn(){
        gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
        LeanTween.scale(gameObject, targetScale, 0.4f);
        yield return null;
    }

    private IEnumerator AnimateShrinkOut()
    {
        LeanTween.scale(gameObject, new Vector3(0f, 0f, 0f), 0.3f);
        yield return new WaitForSeconds(0.3f);
        text.text = "";
        gameObject.SetActive(false);
    }
}