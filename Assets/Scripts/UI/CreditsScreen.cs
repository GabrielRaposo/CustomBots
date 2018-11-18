using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreditsScreen : MonoBehaviour {

    public GameObject mainPanel;
    public GameObject licensesPanel;
    public TextMeshProUGUI buttonLabel;

    bool onMainPanel = true;
	
	public void SwapScreen()
    {
        Debug.Log("call");
        if (onMainPanel) {
            buttonLabel.text = "General";
            mainPanel.SetActive(false);
            licensesPanel.SetActive(true);
        } else {
            buttonLabel.text = "Licenses";
            mainPanel.SetActive(true);
            licensesPanel.SetActive(false);
        }
        onMainPanel = !onMainPanel;
    }
}
