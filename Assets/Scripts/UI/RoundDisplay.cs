using System.Collections;
using UnityEngine;
using TMPro;

public class RoundDisplay : MonoBehaviour
{
    [Header("Values")]
    public int loseSize;
    public int tiedSize;
    public int winSize;

    [Header("Panels")]
    public GameObject matchTypePanel;
    public GameObject resultsBanner;

    [Header("Text Reference")]
    public TextMeshProUGUI matchType;
    public TextMeshProUGUI p1ScoreDisplay;
    public TextMeshProUGUI p2ScoreDisplay;
    public TextMeshProUGUI winnerDisplay;

    [Space(10)]
    public AudioSource scoreAudio;


    public void SetValues(string matchTypeText, int p1Score, int p2Score, bool bump = false, bool playSound = false )
    {
        matchType.text = matchTypeText;

        p1ScoreDisplay.text = p1Score.ToString();
        p2ScoreDisplay.text = p2Score.ToString();

        if (p1Score > p2Score) {
            p1ScoreDisplay.fontSize = winSize;
            p2ScoreDisplay.fontSize = loseSize;
            if (bump)
            {
                StartCoroutine(ScaleBump(p1ScoreDisplay.GetComponent<RectTransform>(), 1.1f));
            }
        } else 
        if (p2Score > p1Score) {
            p1ScoreDisplay.fontSize = loseSize;
            p2ScoreDisplay.fontSize = winSize;
            if (bump)
            {
                StartCoroutine(ScaleBump(p2ScoreDisplay.GetComponent<RectTransform>(), 1.1f));
            }
        } else {
            p1ScoreDisplay.fontSize = tiedSize;
            p2ScoreDisplay.fontSize = tiedSize;
        }

        if (playSound) scoreAudio.Play();
    }

    private IEnumerator ScaleBump(RectTransform r, float targetScale)
    {
        Vector3 originalScale = r.localScale;
        r.localScale = Vector3.one * targetScale;

        float difference = targetScale - originalScale.x; 
        while(Mathf.Abs(r.localScale.x - originalScale.x) > 0.01f)
        {
            yield return new WaitForEndOfFrame();
            r.localScale -= Vector3.one * (difference / 10);
        }
        r.localScale = originalScale;
    }

    public void ShowGameResults(int winner)
    {
        matchTypePanel.SetActive(false);
        winnerDisplay.text = "Player " + winner + " wins!";
        resultsBanner.SetActive(true);
    }
}
