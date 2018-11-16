using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JawAttack : Upgrade
{
    public Hitbox biteHitbox;
    public Transform socket;

    [Header("Audio References")]
    public AudioSource chargeAudio;
    public AudioSource attackAudio;

    Rigidbody2D rb;
    Animator animator;
    Coroutine chargeCoroutine;
    float gravityScale;
    float charge;
    bool rotationLock;
    bool onCooldown;
    bool maxCharged;

    const float minBlastForce = 3f;
    const float maxBlastForce = 15f;

    public override void Initiate(int playerID, Rigidbody2D rb)
    {
        this.rb = rb;
        gravityScale = .9f;
        animator = GetComponent<Animator>();
        biteHitbox.playerID = playerID;
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
        animator.SetBool("FullCharge", false);
        maxCharged = false;


        while (charge < maxBlastForce)
        {
            yield return new WaitForFixedUpdate();
            charge += .2f;
            if (charge >= maxBlastForce)
            {
                maxCharged = true;
                animator.SetBool("FullCharge", true);
            }
        }
    }

    public override void RotateAround(Vector3 direction)
    {
        if (rotationLock) return;

        direction = direction.normalized;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        if(transform.rotation.eulerAngles.z > 0 && transform.rotation.eulerAngles.z < 180) {
            socket.localRotation = Quaternion.Euler(Vector3.up * 180);
        } else {
            socket.localRotation = Quaternion.Euler(Vector3.up * 0);
        }
        biteHitbox.knockback = direction;
    }

    public override void Release()
    {
        if (!active) return;
        if (chargeCoroutine != null) StopCoroutine(chargeCoroutine);
        chargeAudio.Stop();
        attackAudio.Play();
        active = false;

        if (maxCharged) {
            StartCoroutine(LineMovement(20));
        } else {
            StartCoroutine(LineMovement(10));
        }
        animator.SetInteger("State", 2);
        StartCoroutine(RotationLockTimer());
    }

    IEnumerator LineMovement(float speed)
    {
        Vector3 movementIntensity = Vector3.up * speed;
        rb.gravityScale = 0;
        rb.velocity = RaposUtil.RotateVector(movementIntensity, transform.rotation.eulerAngles.z);
        yield return new WaitForSeconds(.1f);
        rb.gravityScale = gravityScale;
        rb.velocity = Vector2.zero;
    }

    public override void Interrupt()
    {
        StopAllCoroutines();
        animator.SetInteger("State", 0);
        animator.SetTrigger("Reset");
        rb.gravityScale = gravityScale;
        chargeAudio.Stop();

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
