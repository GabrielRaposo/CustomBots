using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class StylizedUIButton : MonoBehaviour, ISubmitHandler
{
    public RawImage backRectangle;
    public TextMeshProUGUI label;
    public bool iAmASelfCenteredBitch;

    private EventSystem eventSystem;
    protected Color selectedColor = Color.white;
    protected Color deselectedColor = Color.black;

    public void OnSubmit(BaseEventData eventData)
    {
        backRectangle.enabled = false;
        label.color = Color.white;
    }

    void OnAwake()
    {
        Hide();
    }

    void Start()
    {
        eventSystem = EventSystem.current;
    }
	
	void Update ()
    {
        if (iAmASelfCenteredBitch && eventSystem.currentSelectedGameObject == null)
        {
            eventSystem.SetSelectedGameObject(label.gameObject);
        }	
	}

    public void CallEvent(string s)
    {
        Hide();
        iAmASelfCenteredBitch = false;

        SceneTransition transition = SceneTransition.instance;
        if (transition) {
            transition.Call(s);
        } else {
            SceneTransition.LoadScene(s);
        }
        enabled = false;
    }

    public void Highlight()
    {
        backRectangle.color = selectedColor;
        backRectangle.enabled = true;
        label.color = deselectedColor;
    }

    public void Hide()
    {
        backRectangle.color = deselectedColor;
        backRectangle.enabled = false;
        label.color = selectedColor;
    }
}
