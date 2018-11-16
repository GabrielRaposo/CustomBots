using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCOM : MonoBehaviour
{
    [Header("Player")]
    public Transform aim;

    [Header("Action components")]
    public Upgrade movementUpgrade;
    public Upgrade attackUpgrade;

    public ActionType actionType;

    [HideInInspector] public GameObject movementUpgradeObject;
    [HideInInspector] public GameObject attackUpgradeObject;

    Rigidbody2D rb;
    float currentRotation;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (movementUpgrade) movementUpgradeObject = movementUpgrade.gameObject;
        if (attackUpgrade) attackUpgradeObject = attackUpgrade.gameObject;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        movementUpgrade.Initiate(0, rb);
        attackUpgrade.Initiate(0, rb);

        StartCoroutine(LookAround());
        if (actionType == ActionType.Movement) {
            StartCoroutine(TimedMove());
        } else
        if (actionType == ActionType.Attack) {
            StartCoroutine(TimedAttack());
        }
    }

    IEnumerator LookAround()
    {
        while (true)
        {
            currentRotation += Random.Range(0, 6) * 45;
            Vector2 direction = RaposUtil.RotateVector(Vector2.up, currentRotation);
            SpinComponents(direction);
            yield return new WaitForSeconds(Random.Range(.5f, 1.5f));
        }
    }

    IEnumerator TimedMove()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1.5f, 2.5f));
            movementUpgrade.Call();
            yield return new WaitForSeconds(.5f);
            movementUpgrade.Release();
        }
    }

    IEnumerator TimedAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2,3));
            attackUpgrade.Call();
            yield return new WaitForSeconds(1);
            attackUpgrade.Release();
        }
    }

    private void SpinComponents(Vector2 direction)
    {
        movementUpgrade.RotateAround(direction);
        attackUpgrade.RotateAround(direction);
        RotateAround(aim, direction);
    }

    public void RotateAround(Transform t, Vector3 direction)
    {
        direction = direction.normalized;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        t.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }
}
