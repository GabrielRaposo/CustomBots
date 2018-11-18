using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInputPanel : MonoBehaviour {

    public TextMeshProUGUI playerLabel;
    public RawImage backRectangle;
    public Image openRectangle;
    public RawImage currentInputRectangle;
    public TextMeshProUGUI currentInputDisplay;
    public TextMeshProUGUI playerLabelDisplay;
    public TextMeshProUGUI tipDisplay;
    public AudioSource confirmationSound;
    public ColorSwapPanel colorSwapPanel;

    public void SetLabel(int i)
    {
        playerLabel.text = "Player " + (i + 1);
    }

    public void SetDeselectedState()
    {
        backRectangle.enabled = false;
        openRectangle.enabled = false;
        currentInputRectangle.enabled = false;
        currentInputDisplay.enabled = false;
        playerLabelDisplay.enabled = true;
        playerLabelDisplay.color = Color.white;
        tipDisplay.gameObject.SetActive(false);

        colorSwapPanel.gameObject.SetActive(false);
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

        colorSwapPanel.gameObject.SetActive(false);
    }

    public void SetReadyState(int index, string controllerOption, string playerInput)
    {
        openRectangle.GetComponent<RectTransform>().anchoredPosition += Vector2.up * 50;
        backRectangle.enabled = false;
        openRectangle.enabled = false;
        currentInputRectangle.enabled = true;
        currentInputDisplay.enabled = true;
        currentInputDisplay.color = Color.black;
        currentInputDisplay.text = controllerOption;
        playerLabelDisplay.color = Color.white;
        tipDisplay.gameObject.SetActive(false);
        if (confirmationSound) confirmationSound.Play();

        colorSwapPanel.gameObject.SetActive(true);
        colorSwapPanel.Init(index + 1, playerInput);
    }
}
