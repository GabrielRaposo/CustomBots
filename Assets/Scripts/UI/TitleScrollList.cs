using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitleScrollList : HorizontalScrollList
{
    public GameObject mainSelection;

    private const int SIZE = 3;
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
        index %= SIZE;
        if (index < 0) index = SIZE - 1;

        StartCoroutine(
            PumpButton(i < 0 ? leftArrow.GetComponent<RectTransform>() : rightArrow.GetComponent<RectTransform>())
        );
        UpdateDisplay();
        eventSystem.SetSelectedGameObject(mainSelection);
    }

    private void UpdateDisplay()
    {
        string s = string.Empty;
        switch (index)
        {
            default:
            case 0: s = "Play"; break;
            case 1: s = "Credits"; break;
            case 2: s = "Exit"; break;
        }
        display.text = s;
    }

    public void CallEvent()
    {
        SceneTransition transition = SceneTransition.instance;
        switch (index)
        {
            case 0: //Play
                if (transition) {
                    transition.Call("Controls");
                } else {
                    SceneTransition.LoadScene("Controls");
                }
                enabled = false;
                break;

            case 1: //Credits
                if (transition) {
                    transition.Call("Credits");
                } else {
                    SceneTransition.LoadScene("Credits");
                }
                enabled = false;
                break;

            case 2: //Quit
                Debug.Log("Quit.");
                Application.Quit();
                enabled = false;
                break;
        }
    }
}
