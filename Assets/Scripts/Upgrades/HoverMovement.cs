using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverMovement : Upgrade
{
    [Header("Hover")]
    public ParticleSystem leftHoverPS;
    public ParticleSystem rightHoverPS;
    public AudioSource hoverSound;

    Rigidbody2D rb;
    Coroutine hoverCoroutine;
    bool onCooldown;

    public override void Initiate(int PlayerID, Rigidbody2D rb)
    {
        this.rb = rb;
    }

    public override void Call()
    {
        if (active || onCooldown) return;

        if (hoverCoroutine != null) StopCoroutine(hoverCoroutine);
        hoverCoroutine = StartCoroutine(HoverEvent());
        leftHoverPS.Play();
        rightHoverPS.Play();
        hoverSound.Play();

        active = true;
    }

    private IEnumerator HoverEvent()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            Vector3 movementIntensity = Vector3.up * 15;
            rb.AddForce(RaposUtil.RotateVector(movementIntensity, transform.rotation.eulerAngles.z));
        }
    }

    public override void RotateAround(Vector3 direction)
    {
        float div = 180f / 60f;
        direction = direction.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0 || angle > 180)
        {
            angle -= (angle - 180) * 2;
            angle %= 180;
        }

        transform.rotation = Quaternion.AngleAxis((angle - 90) / div, Vector3.forward);
    }

    public override void Release()
    {
        if (!active) return;
        active = false;

        leftHoverPS.Stop();
        rightHoverPS.Stop();
        hoverSound.Stop();
        if (hoverCoroutine != null) StopCoroutine(hoverCoroutine);
    }

    public override void Interrupt()
    {
        StopAllCoroutines();
        leftHoverPS.Stop();
        rightHoverPS.Stop();
        hoverSound.Stop();
        active = false;

        StartCoroutine(CooldownTimer());
    }


    IEnumerator CooldownTimer()
    {
        onCooldown = true;
        yield return new WaitForSeconds(.3f);
        onCooldown = false;
    }
}
