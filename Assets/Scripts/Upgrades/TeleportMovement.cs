using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportMovement : Upgrade
{
    [Header("Charge")]
    public ParticleSystem chargePS;
    public SpriteRenderer chargeArrow;
    public AudioSource chargeAudio;
    public Gradient chargeColor;

    [Header("Aim")]
    public Transform aim;
    public Transform aimAnchor;
    public ParticleSystem vanishPS;
    public ParticleSystem revealPS;
    public AudioSource teleportAudio;

    public Animator machineSpin;

    Rigidbody2D rb;
    Coroutine aimCoroutine;
    Coroutine cooldownCoroutine;
    SpriteRenderer aimVisual;
    float originalGravity;
    bool rotationLock;
    bool onCooldown;

    public override void Initiate(int PlayerID, Rigidbody2D rb)
    {
        this.rb = rb;
        //originalGravity = rb.gravityScale;
        originalGravity = .9f;
        aimVisual = aim.GetComponent<SpriteRenderer>();
        aimVisual.enabled = false;
        UpdateDotChain(0);
    }

    public override void Call()
    {
        if (active || onCooldown) return;

        if (aimCoroutine != null) StopCoroutine(aimCoroutine);
        aimCoroutine = StartCoroutine(AimEvent());
        chargePS.Play();
        chargeAudio.Play();
        machineSpin.speed = 3;

        rb.velocity = Vector2.zero;
        rb.gravityScale = originalGravity / 10;

        active = true;
    }

    private IEnumerator AimEvent()
    {
        float
            speed = .05f,
            distance = 0,
            maxDistance = 3;

        aim.position = transform.position;
        aim.localPosition += Vector3.up * 2;
        aimVisual.enabled = true;
        while (distance < maxDistance)
        {
            UpdateDotChain(distance / maxDistance);
            yield return new WaitForFixedUpdate();
            aim.localPosition += Vector3.up * speed;
            distance += speed;
        }
    }

    private void UpdateDotChain(float percentage)
    {
        int minSize = 8,
            maxSize = 18;
        chargeArrow.size = new Vector2(.25f, (minSize + percentage * (maxSize - minSize)) * -1);

        Color color = chargeColor.Evaluate(percentage);
        color.a = chargeColor.Evaluate(percentage).a;
        chargeArrow.color = color;
    }

    public override void RotateAround(Vector3 direction)
    {
        if (rotationLock) return;

        direction = direction.normalized;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        aimAnchor.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        chargeArrow.transform.rotation = aimAnchor.rotation;
    }

    public override void Release()
    {
        if (!active) return;
        active = false;

        UpdateDotChain(0);
        chargePS.Stop();
        chargeAudio.Stop();
        teleportAudio.Play();
        machineSpin.speed = 1;
        if (aimCoroutine != null) StopCoroutine(aimCoroutine);

        aimVisual.enabled = false;
        Vector2 chargedPosition = aim.position;

        while (Vector2.Distance(aim.position, transform.position) > .1f)
        {
            if(isOffscreen(aim.position) || 
                Physics2D.CircleCast(aim.position, .1f, Vector2.zero, 1f, 1 << LayerMask.NameToLayer("Solid")) )
            {
                aim.position += (transform.position - aim.position) / 10; 
                //10 é número arbitrário, que significa o número máximo de check que fará antes de parar  
            } else break;
        }
        Vector3 oldPosition = rb.transform.position;
        rb.transform.position = aim.position;
        rb.velocity = Vector2.zero;
        rb.gravityScale = originalGravity;

        revealPS.Play();
        vanishPS.transform.position = oldPosition;
        vanishPS.Play();

        aim.position = rb.transform.position;
        rotationLock = false;

        if (cooldownCoroutine != null) StopCoroutine(cooldownCoroutine);
        cooldownCoroutine = StartCoroutine(CooldownTimer());
    }

    private bool isOffscreen(Vector2 position)
    {
        float x = 5.2f, y = 3.2f;
        if (Mathf.Abs(position.x) > x || Mathf.Abs(position.y) > y) return true;
        return false;
    }

    public override void Interrupt()
    {
        StopAllCoroutines();
        aimVisual.enabled = false;
        UpdateDotChain(0);
        chargePS.Stop();
        chargeAudio.Stop();
        machineSpin.speed = 1;
        aim.position = transform.position;
        rb.gravityScale = originalGravity;
        rotationLock = false;
        active = false;
        onCooldown = false;
    }

    IEnumerator CooldownTimer()
    {
        onCooldown = true;
        yield return new WaitForSeconds(.5f);
        onCooldown = false;
    }
}
