using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour {

    [Header("References")]
    public GameObject screen;
    public StylizedUIButton selfCenteredBitch;

    public bool isActive { get; private set; }
    private EventSystem eventSystem;


    private void Awake()
    {
        isActive = false;
        screen.SetActive(false);
        eventSystem = EventSystem.current;
    }

    public void ToggleActivation()
    {
        eventSystem.SetSelectedGameObject(null);
        selfCenteredBitch.iAmASelfCenteredBitch = false;
        isActive = !isActive;
        if (isActive) {
            Time.timeScale = 0;
            screen.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(DelayToSelect());
        } else {
            Time.timeScale = 1;
            screen.SetActive(false);
        }
    }

    private IEnumerator DelayToSelect()
    {
        yield return new WaitForSecondsRealtime(.25f);
        selfCenteredBitch.iAmASelfCenteredBitch = true;
    }
}
