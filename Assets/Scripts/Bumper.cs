using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    public float force = 6;
    public Sprite spriteBase;
    public Sprite spriteHighlight;
    public ParticleSystem bumpEffect;
    public AudioSource bumpAudio;

    private SpriteRenderer _renderer;
    private Collider2D _collider;
    private Vector3 originalScale;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        originalScale = transform.localScale;
    }

    public Vector2 GetPush()
    {
        bumpEffect.Play();
        bumpAudio.Play();
        StartCoroutine(Bump());
        Vector2 pushDirection = RaposUtil.RotateVector(Vector3.up, transform.rotation.eulerAngles.z);
        return pushDirection * force;
    }

    private IEnumerator Bump()
    {
        if (_collider) _collider.enabled = false;
        _renderer.sprite = spriteHighlight;

        transform.localScale = originalScale + Vector3.up * .2f;
        while(transform.localScale.y > originalScale.y)
        {
            yield return new WaitForEndOfFrame();
            transform.localScale -= Vector3.up * .05f;
        }
        transform.localScale = originalScale;

        if (_collider) _collider.enabled = true;
        _renderer.sprite = spriteBase;
    }
}
