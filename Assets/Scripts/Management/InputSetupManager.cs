using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputSetupManager : MonoBehaviour {

    [Header("Main")]
    public MenuNavigationManager navigationManager;
    public GameObject playerPanelPrefab;
    public Transform panelGroupAnchor;

    [Header("After setup")]
    public GameObject buttonsPanel;
    public StylizedUIButton firstSelection;

    private List<PlayerInputPanel> playerPanels;

    private int controllerSetup = 1;
    private string p1Controller;

    private string[] inputOptions = { "Key_", "Mou_", "Joy0_", "Joy1_" };

    void Start ()
    {
        PlayerConfigurations.BaseSetup();

        playerPanels = new List<PlayerInputPanel>();
        for (int i = 0; i < 2; i++)
        {
            GameObject panelObject = Instantiate(playerPanelPrefab, panelGroupAnchor);
            panelObject.SetActive(true);

            PlayerInputPanel pip = panelObject.GetComponent<PlayerInputPanel>();
            if (pip)
            {
                pip.SetLabel(i);
                if (i == 0) pip.SetSelectedState();
                else        pip.SetDeselectedState();
                playerPanels.Add(pip);
            }
        }
        playerPanelPrefab.SetActive(false);
    }
	
	void Update () {
		switch(controllerSetup)
        {
            default:
            case 1: //setup controle 1
                foreach(string s in inputOptions)
                {
                    if (Input.GetButtonDown(s + "Move"))
                    {
                        Setup(1, s);
                        break;
                    }
                }
                break;
            
            case 2: //setup controle 2
                foreach (string s in inputOptions)
                {
                    if (p1Controller != s && Input.GetButtonDown(s + "Move"))
                    {
                        Setup(2, s);
                        break;
                    }
                }
                break;

            case 3: //final setup
                buttonsPanel.SetActive(true);
                firstSelection.iAmASelfCenteredBitch = true;
                EventSystem.current.SetSelectedGameObject(null);
                if (navigationManager) navigationManager.enabled = true;
                enabled = false;
                break;
        }

	}

    private void Setup(int ID, string inputIndex)
    {
        string s = string.Empty;
        if (inputIndex.Contains("Key")) s = "keyboard"; else 
        if (inputIndex.Contains("Mou")) s = "mouse";    else
                                        s = "joystick";

        if (ID == 1) {
            PlayerConfigurations.player1.input = inputIndex;
            p1Controller = inputIndex;

            playerPanels[0].SetReadyState(0, s, inputIndex);
            playerPanels[1].SetSelectedState();
        } else {
            PlayerConfigurations.player2.input = inputIndex;

            playerPanels[1].SetReadyState(1, s, inputIndex);
        }

        controllerSetup++;
    }
}
