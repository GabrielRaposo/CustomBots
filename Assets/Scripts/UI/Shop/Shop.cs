using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ActionType
{
    Movement,
    Attack,
    Special
}

public class Shop : MonoBehaviour
{
    [Header("Installs")]
    public List<UpgradeInstall> movementInstalls;
    public List<UpgradeInstall> attackInstalls;

    [Header("UI components")]
    public GameObject panelPrefab;
    public Transform panelsGroup;

    [Header("Exit panel")]
    public GameObject exitConfirmationPanel;
    public Button firstSelection;

    private InstallationPanel leftPanel;
    private InstallationPanel rightPanel;

    private bool[] readyPlayers = new bool[2];
    private MatchManager matchManager; 
    private Player leftPlayer;
    private Player rightPlayer;

    public void Setup(MatchManager matchManager, Player leftPlayer, Player rightPlayer)
    {
        this.matchManager = matchManager;
        this.leftPlayer = leftPlayer;
        this.rightPlayer = rightPlayer;
        panelPrefab.gameObject.SetActive(false);

        GameObject lPanel = Instantiate(panelPrefab, panelsGroup);
        lPanel.SetActive(true);
        leftPanel = lPanel.GetComponent<InstallationPanel>();
        leftPanel.SetPanels(this, 1, leftPlayer,  Vector3.left * 500);

        GameObject rPanel = Instantiate(panelPrefab, panelsGroup);
        rPanel.SetActive(true);
        rightPanel = rPanel.GetComponent<InstallationPanel>();
        rightPanel.SetPanels(this, 2, rightPlayer, Vector3.right * 500);

    }

    public void CallInstall(Player player, int i, ActionType type)
    {
        GameObject upgradeObject;
        switch (type)
        {
            default:
            case ActionType.Movement:
                upgradeObject = Instantiate(movementInstalls[i].prefab);
                player.InstallMovementUpgrade(upgradeObject);
                if (player.ID == 1) {
                    PlayerConfigurations.player1.movement = i;
                } else {
                    PlayerConfigurations.player2.movement = i;
                }
                break;

            case ActionType.Attack:
                upgradeObject = Instantiate(attackInstalls[i].prefab);
                player.InstallAttackUpgrade(upgradeObject);
                if (player.ID == 1) {
                    PlayerConfigurations.player1.attack = i;
                } else {
                    PlayerConfigurations.player2.attack = i;
                }
                break;

        }   
    }

    public void SetPanelReady(int id, Player player, int movementIndex, int attackIndex)
    {
        CallInstall(player, movementIndex, ActionType.Movement);
        CallInstall(player, attackIndex, ActionType.Attack);

        if(id == 1) readyPlayers[0] = true;
        else        readyPlayers[1] = true;

        foreach(bool b in readyPlayers)
        {
            if (!b) return;
        }

        leftPanel.DisablePreview();
        rightPanel.DisablePreview();

        matchManager.StartCoroutine(matchManager.StartMatch());
    }

    public void ProhibitSetup(int id)
    {
        if (id == 1)
        {
            SetPanelReady(1, leftPlayer, PlayerConfigurations.player1.movement, PlayerConfigurations.player1.attack);
            leftPanel.ProhibitInstallation();
        }

        if (id == 2)
        {
            SetPanelReady(2, rightPlayer, PlayerConfigurations.player2.movement, PlayerConfigurations.player2.attack);
            rightPanel.ProhibitInstallation();
        }
    }

    public void PauseInstallations(bool value)
    {
        if (value) {
            exitConfirmationPanel.SetActive(true);
            firstSelection.Select();
        } else {
            exitConfirmationPanel.SetActive(false);
        }
        leftPanel.enabled = !value;
        rightPanel.enabled = !value;
    }
}
