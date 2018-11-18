using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeScrollList : HorizontalScrollList
{
    [Space(30)]
    public HealthPreviewPanel healthPreviewPanel;
    public TextMeshProUGUI descriptionDisplay;
    public AudioSource selectionMoveSound;

    private PlayerCOM playerPreview;
    private List<UpgradeInstall> list;
    private ActionType currentType;
    private bool setup;
    private string playerInputIndex;
    private bool onCooldown;
    public int index { get; private set; }

    public void Setup (Player player, PlayerCOM playerPreview, List<UpgradeInstall> list, ActionType currentType)
    {
        this.playerPreview = playerPreview;
        this.list = list;
        this.currentType = currentType;
        playerInputIndex = player.inputIndex;

        GetStartingIndex(player);
        UpdateDisplay();
        setup = true;
    }

    private void GetStartingIndex(Player player)
    {
        switch (currentType)
        {
            case ActionType.Movement:
                if (player.ID == 1) {
                    index = PlayerConfigurations.player1.movement;
                } else {
                    index = PlayerConfigurations.player2.movement;
                }
                break;

            case ActionType.Attack:
                if (player.ID == 1) {
                    index = PlayerConfigurations.player1.attack;
                } else {
                    index = PlayerConfigurations.player2.attack;
                }
                break;
        }
        if(index < 0) index = 0;
    }

    public void UpdateIndexValue(int i)
    {
        if (!setup || onCooldown) return;

        index += i;
        index %= list.Count;
        if (index < 0) index = list.Count - 1;

        if (selectionMoveSound) selectionMoveSound.Play();
        StartCoroutine(
            PumpButton( i < 0 ? leftArrow.GetComponent<RectTransform>() : rightArrow.GetComponent<RectTransform>())
        );
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        display.text = list[index].name;
        descriptionDisplay.text = list[index].description;

        Upgrade upgrade = list[index].prefab.GetComponent<Upgrade>();
        switch (currentType)
        {
            case ActionType.Movement:
                healthPreviewPanel.MoveUpgradeValue = upgrade.health;
                playerPreview.InstallMovementUpgrade(list[index].prefab);
                playerPreview.RemoveAttackUpgrade();
                break;

            case ActionType.Attack:
                healthPreviewPanel.AttackUpgradeValue = upgrade.health;
                playerPreview.InstallAttackUpgrade(list[index].prefab);
                break;
        }

        StartCoroutine(CooldownTimer());
    }

    private IEnumerator CooldownTimer()
    {
        onCooldown = true;
        yield return new WaitForSeconds(.2f);
        onCooldown = false;
    }

    public UpgradeInstall GetInstall()
    {
        return list[index];
    }

}
