using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCOM : MonoBehaviour
{
    [Header("Player")]
    public Transform aim;

    [Header("Leds")]
    public SpriteRenderer LEDsIn;
    public SpriteRenderer LEDsOut;

    [Header("Action components")]
    public Upgrade movementUpgrade;
    public Upgrade attackUpgrade;

    public ActionType actionType;
    public bool startActive = true;

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

        if (startActive)
        {
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

    public void InstallMovementUpgrade(GameObject upgradeObject)
    {
        RemoveMovementUpgrade();
        upgradeObject = Instantiate(upgradeObject);
        upgradeObject.transform.parent = transform;
        upgradeObject.transform.localScale = Vector3.one;
        movementUpgradeObject = upgradeObject;
        movementUpgradeObject.transform.position = transform.position;
        movementUpgrade = upgradeObject.GetComponent<Upgrade>();
        movementUpgrade.Initiate(0, GetComponent<Rigidbody2D>());
    }

    public void InstallAttackUpgrade(GameObject upgradeObject)
    {
        RemoveAttackUpgrade();
        upgradeObject = Instantiate(upgradeObject);
        upgradeObject.transform.parent = transform;
        upgradeObject.transform.localScale = Vector3.one;
        attackUpgradeObject = upgradeObject;
        attackUpgradeObject.transform.position = transform.position;
        attackUpgrade = upgradeObject.GetComponent<Upgrade>();
        attackUpgrade.Initiate(0, GetComponent<Rigidbody2D>());
    }

    public void RemoveMovementUpgrade()
    {
        if (movementUpgradeObject) Destroy(movementUpgradeObject);
    }

    public void RemoveAttackUpgrade()
    {
        if (attackUpgradeObject) Destroy(attackUpgradeObject);
    }

    public void SetLEDsColor(Color color)
    {
        if (LEDsIn)  LEDsIn.color  = color;
        if (LEDsOut) LEDsOut.color = color;
    }
}
