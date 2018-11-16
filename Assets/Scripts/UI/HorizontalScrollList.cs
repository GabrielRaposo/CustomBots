using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HorizontalScrollList : MonoBehaviour {

    public TextMeshProUGUI display;
    public Button leftArrow;
    public Button rightArrow;
    public RawImage backRectangle;

    protected Color selectedColor = Color.white;
    protected Color deselectedColor = Color.black;

    protected void OnEnable()
    {
        backRectangle.color = selectedColor;
        backRectangle.enabled = true;
        display.color = deselectedColor;
        leftArrow.GetComponent<Image>().color = deselectedColor;
        rightArrow.GetComponent<Image>().color = deselectedColor;
    }

    protected void OnDisable()
    {
        backRectangle.color = deselectedColor;
        backRectangle.enabled = false;
        display.color = selectedColor;
        leftArrow.GetComponent<Image>().color = selectedColor;
        rightArrow.GetComponent<Image>().color = selectedColor;
    }

    protected IEnumerator PumpButton(RectTransform t)
    {
        float delta = .4f;
        t.localScale = Vector3.one * (1 - delta);
        int numSteps = 5;
        for (int i = 0; i < numSteps; i++)
        {
            yield return new WaitForEndOfFrame();
            t.localScale += Vector3.one * (delta / numSteps);
        }
        t.localScale = Vector3.one;
    }
}
