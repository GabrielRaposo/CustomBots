using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonAttack : Upgrade
{
    public Transform chargePos;
    public ParticleSystem chargeEffect;
    public ParticleSystem releaseEffect;

    [Header("Audio References")]
    public AudioSource chargeAudio;
    public AudioSource shootAudio;

    BulletPool bulletPool;
    Rigidbody2D rb;
    Animator animator;
    Coroutine chargeCoroutine;
    int playerID;
    float charge;

    bool rotationLock;
    bool onCooldown;

    GameObject currentBullet;

    public override void Initiate(int playerID, Rigidbody2D rb)
    {
        this.playerID = playerID;
        this.rb = rb;
        animator = GetComponent<Animator>();
        bulletPool = BulletPool.instance;
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
        float
            minSize = .4f,
            maxSize = .8f;

        charge = minSize;
        animator.SetBool("Charge", true);
        chargeEffect.Play();

        currentBullet = bulletPool.GetFromPool();
        currentBullet.transform.parent = chargePos;
        currentBullet.transform.position = chargePos.position;
        Bullet b = currentBullet.GetComponent<Bullet>();
        b.ChargeSetup(playerID, 1);
        b.GetComponent<SpriteRenderer>().color = Color.white;
        currentBullet.SetActive(true);

        while (charge < maxSize)
        {
            b.ChargeSetup(playerID, charge);
            yield return new WaitForFixedUpdate();
            charge += .005f;
        }
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
        if (chargeCoroutine != null) StopCoroutine(chargeCoroutine);
        animator.SetBool("Charge", false);
        chargeEffect.Stop();
        chargeAudio.Stop();
        shootAudio.Play();
        releaseEffect.Play();

        Vector2 pushback = Vector2.down * charge * 4;
        rb.velocity = RaposUtil.RotateVector(pushback, transform.rotation.eulerAngles.z) + Vector2.up;

        Bullet b = currentBullet.GetComponent<Bullet>();
        if (b)
        {
            Vector3 velocity = Vector3.up * 10;
            velocity = RaposUtil.RotateVector(velocity, transform.rotation.eulerAngles.z);
            b.ShootSetup(playerID, 1, velocity / 10, (1 - (.7f * charge)) * velocity);
            b.ReturnToLegalCustody();
        }
        currentBullet = null;
        StartCoroutine(RotationLock());
    }

    public override void Interrupt()
    {
        StopAllCoroutines();
        animator.SetBool("Charge", false);
        chargeEffect.Stop();
        animator.SetTrigger("Reset");
        chargeAudio.Stop();

        rotationLock = false;
        active = false;
        onCooldown = false;
        if(currentBullet) currentBullet.GetComponent<Bullet>().Destroy();
    }

    IEnumerator RotationLock()
    {
        rotationLock = true;
        onCooldown = true;
        yield return new WaitForSeconds(.7f);
        onCooldown = false;
        rotationLock = false;
    }
}

