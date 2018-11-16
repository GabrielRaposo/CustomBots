using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InstallationPanel : MonoBehaviour {

    public TextMeshProUGUI label;

    [Header("Panels Control")]
    public GameObject inputPanel;
    public ConfirmationPanel confirmationPanel;

    [Header("Input")]
    public TextMeshProUGUI actionDisplay;
    public UpgradeScrollList upgradeList;

    [Header("Current Displays")]
    public UpgradeSlotUI currentMovementDisplay;
    public UpgradeSlotUI currentAttackDisplay;

    ActionType[] actionTypes = { ActionType.Movement, ActionType.Attack };

    private Shop shop;
    private int id;
    private Player player;
    private string playerInputIndex;
    private int index;
    private bool onCooldown;
    private AudioSource installationSound;
    private bool isReady;

    private void Start()
    {
        installationSound = GetComponent<AudioSource>();
    }

    public void SetPanels(Shop shop, int id, Player player, Vector3 position)
    {
        this.shop = shop;
        this.id = id;
        this.player = player;

        playerInputIndex = player.inputIndex;
        GetComponent<RectTransform>().anchoredPosition = position;

        inputPanel.SetActive(true);
        confirmationPanel.gameObject.SetActive(false);
        index = 0;
        SetActionDisplay();

        SetUpgradeLabels();

        label.text = "Player " + id;
    }

    public void SetUpgradeLabels()
    {
        if(Match.lastWinnerID != player.ID) {
            currentMovementDisplay.Free();
            currentAttackDisplay.Free();
        } else
        {
            int index;
            if (id == 1) {
                index = PlayerConfigurations.player1.movement;
                if (index > -1) SetInstallDisplay(ActionType.Movement, index);

                index = PlayerConfigurations.player1.attack;
                if (index > -1) SetInstallDisplay(ActionType.Attack, index);
            } else {
                index = PlayerConfigurations.player2.movement;
                if (index > -1) SetInstallDisplay(ActionType.Movement, index);

                index = PlayerConfigurations.player2.attack;
                if (index > -1) SetInstallDisplay(ActionType.Attack, index);
            }
        }
    }

    private void Update()
    {
        if (playerInputIndex.Contains("Mou") || isReady) return;

        if (Input.GetButtonDown(playerInputIndex + "Move") || Input.GetButtonDown(playerInputIndex + "Start"))
        {
            GoToPanel(1);
        } else 
        if (Input.GetButtonDown(playerInputIndex + "Attack"))
        {
            ReturnToPanel(-1);
        }

        float horInput = Input.GetAxisRaw(playerInputIndex + "Horizontal");
        if (horInput > 0) {
            upgradeList.UpdateIndexValue(1);
        } else
        if (horInput < 0) {
            upgradeList.UpdateIndexValue(-1);
        }
    }

    private void SetActionDisplay()
    {
        switch (actionTypes[index])
        {
            default:
            case ActionType.Movement:
                upgradeList.Setup(player, shop.movementInstalls, ActionType.Movement);
                actionDisplay.text = "Movement";
                break;

            case ActionType.Attack:
                upgradeList.Setup(player, shop.attackInstalls, ActionType.Attack);
                actionDisplay.text = "Attack";
                break;
        }
    }

    public void ProhibitInstallation()
    {
        isReady = true;
        index = 99;
        GoToPanel();
    }

    public void GoToPanel(int i = 0)
    {
        index += i;
        switch (index)
        {
            case 1: //instala movement e ativa o próximo
                if (installationSound) installationSound.Play();
                SetInstallDisplay(ActionType.Movement, upgradeList.index);
                SetActionDisplay();
                break;

            case 2: //instala attack e ativa o próximo
                if (installationSound) installationSound.Play();
                SetInstallDisplay(ActionType.Attack, upgradeList.index);
                inputPanel.SetActive(false);
                confirmationPanel.gameObject.SetActive(true);
                confirmationPanel.SetReady(false);
                break;

            case 3: //confirma ready e avisa 
                confirmationPanel.SetReady(true);
                shop.SetPanelReady(id, player, currentMovementDisplay.upgradeIndex, currentAttackDisplay.upgradeIndex);
                isReady = true;
                break;

            case 99: //prohibited
                inputPanel.SetActive(false);
                confirmationPanel.gameObject.SetActive(true);
                confirmationPanel.SetReady(true, true);
                //pergunta o index para completar as caixinhas
                break;
        }
    }

    public void ReturnToPanel(int i = 0)
    {
        index += i;
        switch (index)
        {
            case -1:
                index = 0;
                shop.PauseInstallations(true);
                break;

            case 0: //retorna para a tela de movement
                if (installationSound) installationSound.Play();
                currentMovementDisplay.Free();
                SetActionDisplay();
                break;

            case 1: //retorna para a tela de attack
                if (installationSound) installationSound.Play();
                currentAttackDisplay.Free();
                inputPanel.SetActive(true);
                confirmationPanel.gameObject.SetActive(false);
                break;
        }
    }

    public void SetInstallDisplay(ActionType actionType, int index)
    {
        switch (actionType)
        {
            case ActionType.Movement:
                currentMovementDisplay.SetUpgrade(index, shop.movementInstalls[index]);
                break;

            case ActionType.Attack:
                currentAttackDisplay.SetUpgrade(index, shop.attackInstalls[index]);
                break;
        }
    }
}