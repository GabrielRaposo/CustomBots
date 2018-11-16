using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackManager : MonoBehaviour
{
    [Header("References")]
    public AudioSource menuTheme;
    public AudioSource battleTheme;
    public AudioSource battleThemeFaded;

    public float maxVolume;

    public static SoundtrackManager instance;

    private void Awake()
    {
        if(instance == null) {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void PlayMenuTheme()
    {
        if (!menuTheme.isPlaying)
        {
            menuTheme.volume = 0;
            StartCoroutine(FadeIn(menuTheme, maxVolume, 1));
        }
        battleTheme.Stop();
        battleThemeFaded.Stop();
    }

    public bool PlayFadedBattleTheme(float delay)
    {
        menuTheme.Stop();
        if (battleThemeFaded.isPlaying) return false;

        battleTheme.volume = 0;
        battleThemeFaded.volume = 0;

        battleTheme.Play();
        battleThemeFaded.Play();

        StartCoroutine(FadeIn(battleThemeFaded, maxVolume, delay));
        return true;
    }

    public void SetFocus(bool toMenu)
    {
        if (toMenu) {
            StartCoroutine(FadeOut(battleTheme));
        } else {
            StartCoroutine(FadeIn(battleTheme, maxVolume, 0));
        }
    }

    private IEnumerator FadeIn(AudioSource au, float targetVolume, float delay)
    {
        if (!au.isPlaying)
        {
            au.PlayDelayed(delay);
            yield return new WaitForSeconds(delay);
        }
        while(au.volume < targetVolume)
        {
            yield return new WaitForEndOfFrame();
            au.volume += (targetVolume / 60);
        }
        au.volume = targetVolume;
    }

    private IEnumerator FadeOut(AudioSource au)
    {
        float volume = au.volume;
        while (au.volume > 0)
        {
            yield return new WaitForEndOfFrame();
            au.volume -= (volume / 60);
        }
        au.volume = 0;
        //au.Stop();
    }

    private IEnumerator SwitchFocusEvent(AudioSource toHigh, AudioSource toLow)
    {
        int numberOfSteps = 60;
        while(toHigh.volume < maxVolume)
        {
            yield return new WaitForEndOfFrame();
            toHigh.volume += (maxVolume / numberOfSteps);
            toLow.volume -= (maxVolume / numberOfSteps); 
        }
        toHigh.volume = maxVolume;
        toLow.volume = 0;
    }
}
