using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInputPanel : MonoBehaviour {

    public RawImage backRectangle;
    public Image openRectangle;
    public RawImage currentInputRectangle;
    public TextMeshProUGUI currentInputDisplay;
    public TextMeshProUGUI playerLabelDisplay;
    public TextMeshProUGUI tipDisplay;
    public AudioSource confirmationSound;

    public void SetDeselectedState()
    {
        backRectangle.enabled = false;
        openRectangle.enabled = false;
        currentInputRectangle.enabled = false;
        currentInputDisplay.enabled = false;
        playerLabelDisplay.enabled = true;
        playerLabelDisplay.color = Color.white;
        tipDisplay.gameObject.SetActive(false);
    }

    public void SetSelectedState()
    {
        backRectangle.enabled = true;
        openRectangle.enabled = true;
        currentInputRectangle.enabled = false;
        currentInputDisplay.enabled = true;
        currentInputDisplay.color = Color.white;
        currentInputDisplay.text = " keyboard \n or \n mouse \n or \n joystick ";
        playerLabelDisplay.color = Color.black;
        tipDisplay.gameObject.SetActive(true);
    }

    public void SetReadyState(string controllerOption)
    {
        backRectangle.enabled = false;
        openRectangle.enabled = false;
        currentInputRectangle.enabled = true;
        currentInputDisplay.enabled = true;
        currentInputDisplay.color = Color.black;
        currentInputDisplay.text = controllerOption;
        playerLabelDisplay.color = Color.white;
        tipDisplay.gameObject.SetActive(false);
        if (confirmationSound) confirmationSound.Play();
    }
}
