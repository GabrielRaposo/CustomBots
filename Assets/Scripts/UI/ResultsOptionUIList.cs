using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ResultsOptionUIList : HorizontalScrollList
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

        UpdateDisplay();
        eventSystem.SetSelectedGameObject(mainSelection);
    }

    private void UpdateDisplay()
    {
        string s = string.Empty;
        switch (index)
        {
            default:
            case 0: s = "Rematch"; break;
            case 1: s = "Change Cage"; break;
            case 2: s = "Quit"; break;
        }
        display.text = s;
    }

    public void CallEvent()
    {
        SceneTransition transition = SceneTransition.instance;
        switch (index)
        {
            case 0: //Rematch
                Match.Reset();
                if (transition) transition.Call("Match");
                else SceneManager.LoadScene("Match");
                enabled = false;
                break;

            case 1: //Change cage
                if (transition) transition.Call("Cage");
                else SceneManager.LoadScene("Cage");
                enabled = false;
                break;

            default:
            case 2: //Quit
                if (transition) transition.Call("Title");
                else SceneManager.LoadScene("Title");
                enabled = false;
                break;
        }
    }
}
