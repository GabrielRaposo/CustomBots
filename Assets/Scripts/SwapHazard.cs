using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapHazard : CageHazard {

    [Header("Body components")]
    public GameObject bumperComponent;
    public GameObject spikeComponent;

    [Header("Values")]
    public float hiddenPosition;
    public float revealedPosition;

    bool spikesActive;

    public override void Call()
    {
        if (spikesActive) {
            StartCoroutine( HideAndReveal(spikeComponent.transform, bumperComponent.transform) );
        } else {
            StartCoroutine( HideAndReveal(bumperComponent.transform, spikeComponent.transform) );
        }
        spikesActive = !spikesActive;
    }

    private IEnumerator HideAndReveal(Transform toHide, Transform toReveal)
    {
        float distanceValue = (revealedPosition - hiddenPosition) / 15;
        while (toHide.localPosition.y > hiddenPosition)
        {
            yield return new WaitForEndOfFrame();
            toHide.localPosition += Vector3.down * distanceValue;
            toReveal.localPosition += Vector3.up * distanceValue;
        }
        toHide.localPosition = Vector2.up * hiddenPosition;
        toReveal.localPosition = Vector2.up * revealedPosition;
    }
}
