using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [Header("Player")]
    public Transform aim;
    public SpriteRenderer ledsIn;
    public SpriteRenderer ledsOut;
    public ParticleSystem damageBurst;

    [Header("Action components")]
    public Upgrade movementUpgrade;
    public Upgrade attackUpgrade;

    [Header("Audio References")]
    public AudioSource damageAudio;
    [Space(10)]

    [HideInInspector] public GameObject movementUpgradeObject;
    [HideInInspector] public GameObject attackUpgradeObject;

    enum State { Menu, Idle, Stunned }
    private State state;

    public int ID { get; private set; }
    public string inputIndex = "Key0_";

    private delegate void InputDelegate();
    private InputDelegate inputDelegate;
    private InputDelegate rotationDelegate;

    Rigidbody2D rb;
    Health health;
    private float originalGravity;
    Coroutine stunCoroutine;
    bool invincible;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (movementUpgrade) movementUpgradeObject = movementUpgrade.gameObject;
        if(attackUpgrade) attackUpgradeObject = attackUpgrade.gameObject;

        inputDelegate = Empty;
        rotationDelegate = Empty;
    }

    public void Init(int ID, string inputIndex, Color color, Health health)
    {
        this.ID = ID;
        this.inputIndex = inputIndex;
        ledsIn.color = ledsOut.color = color;
        this.health = health;

        if (inputIndex.StartsWith("Key")) {
            inputDelegate = KeyboardInputs;
            rotationDelegate = AxisSpin;
        } else
        if (inputIndex.StartsWith("Mou")) {
            inputDelegate = KeyboardInputs;
            rotationDelegate = MouseSpin;
        } else {
            inputDelegate = JoystickInputs;
            rotationDelegate = AxisSpin;
        }

        //só pra funcionar em testes rápidos sem menu
        movementUpgrade.Initiate(ID, rb);
        attackUpgrade.Initiate(ID, rb);

        originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
    }

    public void InstallMovementUpgrade(GameObject upgradeObject)
    {
        if (movementUpgradeObject) Destroy(movementUpgradeObject);
        upgradeObject.transform.parent = transform;
        movementUpgradeObject = upgradeObject;
        movementUpgradeObject.transform.position = transform.position;
        movementUpgrade = upgradeObject.GetComponent<Upgrade>();

        movementUpgrade.Initiate(ID, rb);
    }

    public void InstallAttackUpgrade(GameObject upgradeObject)
    {
        Destroy(attackUpgradeObject);
        upgradeObject.transform.parent = transform;
        attackUpgradeObject = upgradeObject;
        attackUpgradeObject.transform.position = transform.position;
        attackUpgrade = upgradeObject.GetComponent<Upgrade>();

        attackUpgrade.Initiate(ID, rb);
    }

    public void SetIdleState()
    {
        rb.gravityScale = originalGravity;

        Vector3 direction = (ID == 1) ? Vector3.right : Vector3.left;
        movementUpgrade.RotateAround(direction);
        attackUpgrade.RotateAround(direction);
        RotateAround(aim, direction);

        health.Init(this, attackUpgrade.health + movementUpgrade.health, ledsIn.color);

        state = State.Idle;
    } 

    void Update () {
        if (state != State.Idle || Time.timeScale < .5f) return;

        inputDelegate();
        rotationDelegate();
    }

    private void Empty() { }

    private void KeyboardInputs()
    {   
        if (Input.GetButton(inputIndex + "Move")) {
            movementUpgrade.Call();
        } else {
            movementUpgrade.Release();
        }

        if (Input.GetButton(inputIndex + "Attack")) {
            attackUpgrade.Call();
        } else {
            attackUpgrade.Release();
        }
    }

    private void JoystickInputs()
    {
        if (Input.GetAxisRaw(inputIndex + "Move") < 0 || Input.GetButton(inputIndex + "Move") ) {
            movementUpgrade.Call();
        } else {
            movementUpgrade.Release();
        }

        if (Input.GetButton(inputIndex + "Attack")) {
            attackUpgrade.Call();
        } else {
            attackUpgrade.Release();
        }
    }

    private void AxisSpin()
    {
        Vector2 movementInput = Vector2.zero;
        movementInput += Input.GetAxisRaw(inputIndex + "Horizontal") * Vector2.right;
        movementInput += Input.GetAxisRaw(inputIndex + "Vertical")   * Vector2.up;

        if (movementInput != Vector2.zero)
        {
            movementUpgrade.RotateAround(movementInput);
            attackUpgrade.RotateAround(movementInput);
            RotateAround(aim, movementInput);
        }
    }

    private void MouseSpin()
    {
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition -= transform.position;
        targetPosition = Vector3.Normalize(targetPosition);

        movementUpgrade.RotateAround(targetPosition);
        attackUpgrade.RotateAround(targetPosition);
        RotateAround(aim, targetPosition);
    }

    public void RotateAround(Transform t, Vector3 direction)
    {
        direction = direction.normalized;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        t.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bumper"))
        {
            Bumper bumper = collision.gameObject.GetComponent<Bumper>();
            if (bumper)
            {
                rb.velocity += bumper.GetPush(); 
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!transform.CompareTag("Player") || invincible) return;

        if (collision.CompareTag("Hitbox")) {
            Hitbox hitbox = collision.GetComponent<Hitbox>();
            if(hitbox && hitbox.playerID != ID)
            {
                if (health) health.TakeDamage((int)hitbox.damage);
                SetKnockback(hitbox.allignKnockbackWithRotation, hitbox.Knockback, collision.transform.position);
            }
        } else 
        if (collision.CompareTag("Bullet"))
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if(bullet && bullet.playerID != ID)
            {
                if (bullet) health.TakeDamage((int)bullet.damage);
                SetKnockback(false, bullet.knockback, collision.transform.position);
                bullet.Destroy();
            }
        }
    }

    private void SetKnockback(bool allign, Vector2 knockback, Vector2 origin)
    {
        state = State.Stunned;
        damageBurst.Play();
        damageAudio.Play();

        if (Mathf.Abs(knockback.x) < 1)
        {
            if (transform.position.x < origin.x)
                knockback += Vector2.left;
            else
                knockback += Vector2.right;
        }
        knockback.y = 0;
        knockback += Vector2.up;
        rb.velocity = knockback * 2;
        

        movementUpgrade.Interrupt();
        attackUpgrade.Interrupt();

        if (stunCoroutine != null) StopCoroutine(stunCoroutine);
        stunCoroutine = StartCoroutine(StunnedTimer());
    }

    IEnumerator StunnedTimer()
    {
        state = State.Stunned;
        invincible = true;
        yield return new WaitForSeconds(.3f);

        invincible = false;
        yield return new WaitForSeconds(.3f);
        state = State.Idle;
    }

    public IEnumerator Die()
    {
        inputDelegate = Empty;
        rotationDelegate = Empty;

        ledsIn.color = ledsOut.color = Color.black;
        GetComponent<Breakable>().Break(true);
        movementUpgradeObject.GetComponent<Breakable>().Break();
        attackUpgradeObject.GetComponent<Breakable>().Break();

        aim.gameObject.SetActive(false);

        CameraEffects cam = Camera.main.GetComponent<CameraEffects>();
        if (cam)
        {
            cam.StopAllCoroutines();
            cam.StartCoroutine(cam.Shake());
        }

        Time.timeScale = .01f;
        yield return new WaitForSecondsRealtime(.3f);
        Time.timeScale = 1f;
    }
}
