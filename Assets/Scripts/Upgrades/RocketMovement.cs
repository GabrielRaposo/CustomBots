using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMovement : Upgrade
{
    [Header("Charge")]
    public ParticleSystem chargePS;
    public SpriteRenderer chargeArrow;
    public AudioSource chargeAudio;
    public Gradient chargeColor;

    [Header("Launch")]
    public ParticleSystem launchPS;
    public AudioSource launchAudio;

    Rigidbody2D rb;
    Animator animator;
    Coroutine chargeCoroutine;
    float charge;
    bool rotationLock;
    bool onCooldown;

    public override void Initiate(int PlayerID, Rigidbody2D rb)
    {
        this.rb = rb;
        animator = GetComponent<Animator>();
    }

    public override void Call()
    {
        if (active || onCooldown) return;

        if (chargeCoroutine != null) StopCoroutine(chargeCoroutine);
        chargeCoroutine = StartCoroutine(ChargeEvent());
        chargePS.Play();
        animator.SetBool("Charge", true);
        chargeAudio.Play();

        active = true;
    }

    private IEnumerator ChargeEvent()
    {
        float 
            minBlastForce = 3f,
            maxBlastForce = 10f;

        charge = minBlastForce;
        while(charge < maxBlastForce)
        {
            UpdateChargeArrow(charge / (maxBlastForce - minBlastForce));
            yield return new WaitForFixedUpdate();
            charge += .2f;
        }
    }

    private void UpdateChargeArrow(float percentage)
    {
        int maxSize = 3;
        chargeArrow.size = new Vector2(1, percentage * maxSize);

        Color color = chargeColor.Evaluate(percentage);
        color.a = chargeColor.Evaluate(percentage).a;
        chargeArrow.color = color;
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
        active = false;

        UpdateChargeArrow(0);
        chargePS.Stop();
        launchPS.Play();
        chargeAudio.Stop();
        launchAudio.Play();
        animator.SetBool("Charge", false);
        if (chargeCoroutine != null) StopCoroutine(chargeCoroutine);
        Vector3 movementIntensity = Vector3.up * charge;
        rb.velocity = RaposUtil.RotateVector(movementIntensity, transform.rotation.eulerAngles.z);
        rotationLock = false;
        StartCoroutine(CooldownTimer());
    }

    public override void Interrupt()
    {
        StopAllCoroutines();
        UpdateChargeArrow(0);
        chargePS.Stop();
        chargeAudio.Stop();
        charge = 0;
        rotationLock = false;
        onCooldown = false;
        active = false;
    }


    IEnumerator CooldownTimer()
    {
        onCooldown = true;
        yield return new WaitForSeconds(.3f);
        onCooldown = false;
    }
}
