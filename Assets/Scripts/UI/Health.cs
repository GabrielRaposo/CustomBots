using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    public Image gem;
    public Image valueDisplay;
    public Image delayedValueDisplay;
    public Gradient gradient;

    [HideInInspector] public MatchManager matchManager;
    public int value { get; private set; }

    private Player player;
    private int maxValue;

	public void Init(Player player, int maxValue, Color color)
    {
        this.player = player;
        this.maxValue = maxValue;
        gem.color = color;
        value = maxValue;
        UpdateBar();
    }

    private void UpdateBar()
    {
        if (valueDisplay && maxValue > 0)
        {
            float percent = (float)value / maxValue;
            valueDisplay.fillAmount = percent;
            if(delayedValueDisplay.fillAmount <= valueDisplay.fillAmount) {
                delayedValueDisplay.fillAmount = valueDisplay.fillAmount;
            } else {
                StopAllCoroutines();
                StartCoroutine(DelayedUpdate(delayedValueDisplay, valueDisplay));
            }
            valueDisplay.color = gradient.Evaluate(percent);
        }
    }

    private IEnumerator DelayedUpdate(Image delayedBar, Image targetBar)
    {
        yield return new WaitForSeconds(.7f);
        float decreaseValue = (targetBar.fillAmount - delayedBar.fillAmount) / 10;
        decreaseValue = Mathf.Abs(decreaseValue);

        while (delayedBar.fillAmount > targetBar.fillAmount)
        {
            yield return new WaitForFixedUpdate();
            delayedBar.fillAmount -= decreaseValue;
        }
        delayedBar.fillAmount = targetBar.fillAmount;
    }

    public void TakeDamage(int damage)
    {
        value -= damage;
        if (value <= 0)
        {
            value = 0;
            if (matchManager)
            {
                matchManager.StartCoroutine(matchManager.EndMatch(player.ID));
            }

            player.StopAllCoroutines();
            player.StartCoroutine(player.Die());

            AudioSource finalHitEcho;
            if (finalHitEcho = GetComponent<AudioSource>())
            {
                finalHitEcho.Play();
            }
        }
        UpdateBar();
    }
}
