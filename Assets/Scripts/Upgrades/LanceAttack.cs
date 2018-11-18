using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanceAttack : Upgrade
{
    public Hitbox bladeHitbox;
    public SpriteRenderer bladeSprite;
    public SpriteRenderer attackEffect;
    public Color normalAttackColor;
    public Color strongAttackColor;

    [Header("Audio References")]
    public AudioSource chargeAudio;
    public AudioSource attackAudio;

    Rigidbody2D rb;
    Animator animator;
    Coroutine chargeCoroutine;

    float charge;
    bool rotationLock;
    bool onCooldown;
    bool maxCharged;

    Shader shaderSprite;
    Shader shaderBlink;

    const float minBlastForce = 3f;
    const float maxBlastForce = 15f;

    public override void Initiate(int playerID, Rigidbody2D rb)
    {
        this.rb = rb;
        animator = GetComponent<Animator>();
        bladeHitbox.playerID = playerID;

        shaderSprite = Shader.Find("Sprites/Default");
        shaderBlink = Shader.Find("GUI/Text Shader");
    }

    public override void Call()
    {
        if (active || onCooldown) return;
        if (chargeCoroutine != null) StopCoroutine(chargeCoroutine);
        chargeCoroutine = StartCoroutine(ChargeEvent());
        chargeAudio.Play();
        active = true;
    }

    private IEnumerator ChargeEvent()
    {
        charge = minBlastForce;
        animator.SetInteger("State", 1);
        maxCharged = false;

        while (charge < maxBlastForce)
        {
            yield return new WaitForFixedUpdate();
            charge += .2f;
            if (charge >= maxBlastForce)
            {
                maxCharged = true;
                StartCoroutine(BlinkColor(bladeSprite, Color.red));
            }
        }
    }

    private IEnumerator BlinkColor(SpriteRenderer sr, Color color)
    {
        sr.color = color;
        sr.material.shader = shaderBlink;

        yield return new WaitForSeconds(.05f);
        sr.color = Color.white;
        sr.material.shader = shaderSprite;
    } 

    public override void RotateAround(Vector3 direction)
    {
        if (rotationLock) return;

        direction = direction.normalized;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    public override void Release()
    {
        if (!active) return;
        if (chargeCoroutine != null) StopCoroutine(chargeCoroutine);
        chargeAudio.Stop();
        attackAudio.Play();
        active = false;
        rb.velocity = Vector2.up * 1;

        if (maxCharged) {
            attackEffect.color = strongAttackColor;
            bladeHitbox.damage = 2;
        } else {
            attackEffect.color = normalAttackColor;
            bladeHitbox.damage = 1;
        }
        animator.SetInteger("State", 2);
        StartCoroutine(RotationLockTimer());
    }

    public override void Interrupt()
    {
        StopAllCoroutines();
        animator.SetInteger("State", 0);
        animator.SetTrigger("Reset");
        chargeAudio.Stop();

        bladeSprite.color = Color.white;
        bladeSprite.material.shader = shaderSprite;

        rotationLock = false;
        active = false;
        onCooldown = false;
    }

    IEnumerator RotationLockTimer()
    {
        rotationLock = true;
        onCooldown = true;
        yield return new WaitForSeconds(.5f);
        rotationLock = false;
        onCooldown = false;
    }
}
