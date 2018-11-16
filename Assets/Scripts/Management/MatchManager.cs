using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviour {

    public bool skipShop;
    public bool skipRoundDisplay;

    [Header("UI Reference")]
    public Shop shop;
    public RoundDisplay roundDisplay;
    public BattleStartPanel battleStartPanel;
    public Health player1Health;
    public Health player2Health;
    public PauseManager pauseManager;

    [Header("Player Stuff")]
    public GameObject playerPrefab;
    public Color player1Color;
    public Color player2Color;

    private GameObject player1;
    private GameObject player2;
    private SceneTransition sceneTransition;

    private string player1input;
    private string player2input;

    private SoundtrackManager soundtrackManager;

    private enum State { Shop, Intro, Playing, Outro }
    private State state;

    void Start ()
    {
        sceneTransition = SceneTransition.instance;
        soundtrackManager = SoundtrackManager.instance;

        player1 = Instantiate(playerPrefab, Vector3.left  * 3, Quaternion.identity, transform);
        player2 = Instantiate(playerPrefab, Vector3.right * 3, Quaternion.identity, transform);
        Destroy(playerPrefab);

        player1Health.matchManager = this;
        player2Health.matchManager = this;

        PlayerConfigurations.BaseSetup();
        PlayerConfigurations.player1.color = player1Color;
        PlayerConfigurations.player2.color = player2Color;

        player1.GetComponent<Player>().Init(1, player1input = PlayerConfigurations.player1.input, player1Color, player1Health);
        player2.GetComponent<Player>().Init(2, player2input = PlayerConfigurations.player2.input, player2Color, player2Health);
        battleStartPanel.HideAll();

        if (!skipShop) {
            roundDisplay.gameObject.SetActive(false);
            if (shop) {
                if (!soundtrackManager.PlayFadedBattleTheme(1))
                {
                    soundtrackManager.SetFocus(true);
                }
                
                shop.gameObject.SetActive(true);
                shop.Setup(this, player1.GetComponent<Player>(), player2.GetComponent<Player>());
            }
            shop.ProhibitSetup(Match.lastWinnerID);
            state = State.Shop;
        } else {
            StartCoroutine(StartMatch());
        }
    }

    private void Update()
    {
        if(state == State.Playing)
        {
            if (Input.GetButtonDown(player1input + "Start") || 
                Input.GetButtonDown(player2input + "Start") ||
                Input.GetKeyDown(KeyCode.Escape))
            {
                pauseManager.ToggleActivation();
            }
        }   
    }

    public IEnumerator StartMatch()
    {
        yield return new WaitForSeconds(1f);
        shop.gameObject.SetActive(false);
        if (!skipRoundDisplay)
        {
            roundDisplay.gameObject.SetActive(true);
            roundDisplay.SetValues("First to win " + Match.maxScore, Match.player1Score, Match.player2Score);
            state = State.Intro;

            yield return new WaitForSeconds(2f);
            roundDisplay.gameObject.SetActive(false);
            battleStartPanel.gameObject.SetActive(true);
            battleStartPanel.Call();
        }
        soundtrackManager.SetFocus(false);
        player1.GetComponent<Player>().SetIdleState();
        player2.GetComponent<Player>().SetIdleState();
        state = State.Playing;
    }

    public IEnumerator EndMatch(int loserID)
    {
        if (state != State.Outro)
        {
            state = State.Outro;
            yield return new WaitForSeconds(1);
            yield return UpdateScore((loserID == 1) ? 2 : 1);
            soundtrackManager.SetFocus(true);

            int winner = Match.Winner();
            if (winner < 0) {
                if (sceneTransition) sceneTransition.Call(SceneManager.GetActiveScene().name);
                else                 SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            } else {
                roundDisplay.ShowGameResults(winner);
            }
        }
    }

    IEnumerator UpdateScore(int winnerID)
    {
        roundDisplay.gameObject.SetActive(true);
        roundDisplay.SetValues("Player " + winnerID + " wins", Match.player1Score, Match.player2Score);
        yield return new WaitForSeconds(.5f);

        if (winnerID == 1) {
            Match.player1Score++;
            Match.lastWinnerID = 1;
        } else {
            Match.player2Score++;
            Match.lastWinnerID = 2;
        }
        roundDisplay.SetValues("Player " + winnerID + " wins", Match.player1Score, Match.player2Score, true, true);
        yield return new WaitForSeconds(1);
    }
}
