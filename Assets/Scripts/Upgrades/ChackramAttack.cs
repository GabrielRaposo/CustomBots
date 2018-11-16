using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChackramAttack : Upgrade
{
    public ChackramBlade blade;
    public Hitbox hitbox;

    [Header("Audio References")]
    public AudioSource reattachSound;

    Rigidbody2D rb;

    bool detached;
    bool maxCharged;
    bool buttonHeld;

    public override void Initiate(int playerID, Rigidbody2D rb)
    {
        this.rb = rb;
        hitbox.playerID = playerID;
        blade.Init(playerID);
    }

    public override void Call()
    {
        if (detached || buttonHeld) return;
        buttonHeld = true;
        //attackAudio.Play();

        Vector2 velocity = Vector3.up * 10;
        velocity = RaposUtil.RotateVector(velocity, transform.rotation.eulerAngles.z);
        blade.Launch(this, velocity + rb.velocity);
        detached = true;
    }

    public override void RotateAround(Vector3 direction)
    {
        direction = direction.normalized;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    public override void Release()
    {
        if (!detached)
        {
            buttonHeld = false;
        }
    }

    public override void Interrupt()
    {
    }

    public void AttachBlade()
    {
        reattachSound.Play();
        detached = false;
    }
}
