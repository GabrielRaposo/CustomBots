using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public int playerID;
    public float damage;
    public Vector2 knockback;
    public bool allignKnockbackWithRotation;

    private void Awake()
    {
        transform.tag = "Hitbox";
        gameObject.layer = LayerMask.NameToLayer("Hitbox");

        if (allignKnockbackWithRotation)
        {
            knockback = RaposUtil.RotateVector(knockback, transform.rotation.eulerAngles.z);
        }
    }
}
