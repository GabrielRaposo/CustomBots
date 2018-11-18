using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPreviewPanel : MonoBehaviour
{
    const int MAX_VALUE = 8;

    public Image permValueDisplay;
    public Image valueDisplay;

    private int moveUpgradeValue;
    public int MoveUpgradeValue
    {
        get
        {
            return moveUpgradeValue;
        }
        set
        {
            moveUpgradeValue = value;
            UpdateDisplay();
        }
    }

    private int attackUpgradeValue;
    public int AttackUpgradeValue
    {
        get
        {
            return attackUpgradeValue;
        }
        set
        {
            attackUpgradeValue = value;
            UpdateDisplay();
        }
    }

    void Start ()
    {
        UpdateDisplay();
        permValueDisplay.fillAmount = 0;
    }

    private void UpdateDisplay()
    {
        valueDisplay.fillAmount = (float)(moveUpgradeValue + attackUpgradeValue) / MAX_VALUE;
    }

    public void UpdatePermDisplay()
    {
        permValueDisplay.fillAmount = valueDisplay.fillAmount;
    }
}
