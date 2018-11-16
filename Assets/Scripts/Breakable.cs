using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour {

    public SpriteRenderer[] renderers;
    public Collider2D[] colliders;
    public Color destroyedColor;
    public ParticleSystem effect;

	public void Break(bool spin = false)
    {
        foreach(SpriteRenderer r in renderers)
        {
            r.color = destroyedColor;
        }

        foreach(Collider2D c in colliders)
        {
            c.enabled = false;
        }

        if (effect)
        {
            effect.Play();
        }

        if (spin)
        {
            StartCoroutine(Spin());
        }
    }

    private IEnumerator Spin()
    {
        float rotationSpeed = Random.Range(-5f, 5f);
        while (true)
        {
            yield return new WaitForFixedUpdate();
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
    }
}
