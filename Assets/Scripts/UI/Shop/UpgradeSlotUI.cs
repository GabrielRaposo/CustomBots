using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeSlotUI : MonoBehaviour {

    public TextMeshProUGUI label;
    public Image box;
    public TextMeshProUGUI display;

    [HideInInspector] public int upgradeIndex;
    [HideInInspector] public UpgradeInstall upgradeInstall;

    public void Free()
    {
        Color fadedWhite = new Color(1, 1, 1, .2f);
        box.color = fadedWhite;
        label.color = fadedWhite;
        display.text = string.Empty;
        upgradeInstall = null;
    }

    public void SetUpgrade(int upgradeIndex, UpgradeInstall upgradeInstall)
    {
        this.upgradeIndex = upgradeIndex;
        this.upgradeInstall = upgradeInstall;
        Color fullWhite = new Color(1, 1, 1, 1);
        box.color = fullWhite;
        label.color = fullWhite;
        display.text = upgradeInstall.name;
        StartCoroutine(Pump());
    }

    private IEnumerator Pump()
    {
        Vector3 originalScale = transform.localScale;
        transform.localScale = originalScale + (Vector3.one * .2f);
        while(transform.localScale.x > originalScale.x)
        {
            yield return null;
            transform.localScale -= Vector3.one * .02f;
        }
        transform.localScale = originalScale;
    }
}
