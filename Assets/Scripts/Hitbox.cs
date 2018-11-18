using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public int playerID;
    public float damage;
    public bool allignKnockbackWithRotation;
    public Vector2 knockback;
    public Vector2 Knockback
    {
        get
        {
            if (allignKnockbackWithRotation)
            {
                return RaposUtil.RotateVector(knockback, transform.rotation.eulerAngles.z);
            }
            return knockback;
        }

        set
        {
            knockback = value;
        }
    }

    private void Awake()
    {
        transform.tag = "Hitbox";
        gameObject.layer = LayerMask.NameToLayer("Hitbox");
    }
}
