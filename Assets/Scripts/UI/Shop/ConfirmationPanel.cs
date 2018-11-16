using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfirmationPanel : MonoBehaviour {

    public TextMeshProUGUI label;
    public GameObject winnerCrown;
    public RawImage backRectangle;
    public AudioSource feedbackSound;
    public bool ready { get; private set; }

    public void SetReady(bool ready, bool winner = false)
    {
        this.ready = ready;

        if (!winner)
        {
            if (ready) {
                backRectangle.enabled = false;
                label.color = Color.white;
                label.text = "Ready";
                if (feedbackSound) feedbackSound.Play();
            } else {
                backRectangle.enabled = true;
                label.color = Color.black;
                label.text = "Ready?";
            }
        }

        backRectangle.gameObject.SetActive(!winner);
        winnerCrown.SetActive(winner);
    }
}
