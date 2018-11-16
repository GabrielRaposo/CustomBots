using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleStartPanel : MonoBehaviour {

    public RawImage backRectangle; 
    public TextMeshProUGUI label;
    public AudioSource battleStartSound;

	public void HideAll()
    {
        StopAllCoroutines();
        backRectangle.enabled = false;
        label.enabled = false;
    }

    public void Call()
    {
        if (battleStartSound) battleStartSound.Play();
        backRectangle.enabled = true;
        StartCoroutine(FlashText(18));
    }

    private IEnumerator FlashText(float numFlashes)
    {
        for (int i = 0; i < numFlashes; i++)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            label.enabled = !label.enabled;
        }
        HideAll();
    }

}
