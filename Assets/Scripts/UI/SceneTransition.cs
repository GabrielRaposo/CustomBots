using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneTransition : MonoBehaviour
{
    public RectTransform[] rects;

    private const int OFFSCREEN_X = 2000;
    public static SceneTransition instance;

	void Awake ()
    {
		if(instance == null) {
            instance = this;
            DontDestroyOnLoad(transform.parent);
        } else {
            Destroy(transform.parent.gameObject);
        }
	}

    public static void LoadScene(string scene)
    {
        if (!scene.Contains("Scene")) scene += "Scene";
        SceneManager.LoadScene(scene);
    }

    public void Call(string scene)
    {
        Time.timeScale = 1;
        StopAllCoroutines();
        if (!scene.Contains("Scene")) scene += "Scene";
        StartCoroutine(TransitionToScene(scene));
    }

    private IEnumerator TransitionToScene(string scene)
    {
        yield return TransitionIn();

        yield return new WaitForEndOfFrame();
        AsyncOperation async = SceneManager.LoadSceneAsync(scene);
        async.allowSceneActivation = false;
        while (async.progress < .9f)
        {
            yield return null;
        }
        async.allowSceneActivation = true;
        //EventSystem.current.enabled = false;

        yield return new WaitForSeconds(.2f);
        yield return TransitionOut();
        //EventSystem.current.enabled = true;
    }

    IEnumerator TransitionIn()
    {
        float time = .2f;
        yield return new WaitForSecondsRealtime(.2f);
        for (int i = 0; i < rects.Length; i++)
        {
            rects[i].DOAnchorPosX(0, time, true);
        }
        yield return new WaitForSeconds(time);
    }

    IEnumerator TransitionOut()
    {
        float time = .2f;
        yield return new WaitForSecondsRealtime(.2f);
        for (int i = 0; i < rects.Length; i++)
        {
            rects[i].DOAnchorPosX((i%2 == 0) ? OFFSCREEN_X : -OFFSCREEN_X, time, true);
        }
        yield return new WaitForSeconds(time);
    }
}
