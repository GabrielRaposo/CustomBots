using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class CageScrollList : HorizontalScrollList
{
    public GameObject mainSelection;
    public TextMeshProUGUI descriptionDisplay;
    public Image thumbnailDisplay;

    [System.Serializable]
    public struct CageDisplayInfo
    {
        public string name;
        [TextArea(3,3)] public string description;
        public Sprite thumb;
    }
    public CageDisplayInfo[] cageDisplays;

    private bool setup;
    public int index { get; private set; }
    private EventSystem eventSystem;

    public void Start()
    {
        index = 0;
        UpdateDisplay();
        eventSystem = EventSystem.current;
    }

    private void Update()
    {
        if (eventSystem && eventSystem.currentSelectedGameObject == null)
        {
            eventSystem.SetSelectedGameObject(mainSelection);
        }
    }

    public void UpdateIndexValue(int i)
    {
        index += i;
        index %= cageDisplays.Length;
        if (index < 0) index = cageDisplays.Length - 1;

        StartCoroutine(
            PumpButton(i < 0 ? leftArrow.GetComponent<RectTransform>() : rightArrow.GetComponent<RectTransform>())
        );
        UpdateDisplay();
        eventSystem.SetSelectedGameObject(mainSelection);
    }

    private void UpdateDisplay()
    {
        display.text = cageDisplays[index].name;
        descriptionDisplay.text = cageDisplays[index].description;
        thumbnailDisplay.sprite = cageDisplays[index].thumb;
    }

    public void CallEvent()
    {
        Match.cageID = index;
        Match.Reset();
        SceneTransition transition = SceneTransition.instance;
        if (transition) {
            transition.Call("Match");
        } else {
            SceneTransition.LoadScene("Match");
        }
    }
}
