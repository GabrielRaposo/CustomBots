using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingHazard : CageHazard {

    public float maxDistance;
    public float duration;
	
    public override void Call()
    {
        if(transform.position.x > 0) {
            transform.DOMoveX(-maxDistance, duration);
        } else {
            transform.DOMoveX(maxDistance, duration);
        }
    }
}
