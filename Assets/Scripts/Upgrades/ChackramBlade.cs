using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChackramBlade : MonoBehaviour
{
    private ChackramAttack origin;
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;

    public ParticleSystem windEffect;
    public Transform visualComponent;
    public AudioSource attackAudio;

    public float rotationSpeed;
    private float multiplier = 1;
    public int playerID = -1;
    bool launched;

    void Awake () {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _collider.enabled = false;
    }

    public void Init(int playerID)
    {
        this.playerID = playerID;
    }

    private void Update()
    {
        visualComponent.Rotate(Vector3.forward * (rotationSpeed * multiplier));
        if (launched && Vector3.Distance(transform.position, origin.transform.position) < .5f)
        {
            Reattach();
        }
    }

    public void Launch(ChackramAttack origin, Vector3 direction)
    {
        this.origin = origin;
        transform.parent = null;

        attackAudio.Play();
        _rigidbody.velocity = direction;
        _collider.enabled = true;
        multiplier = 5;
        launched = false; 
        StartCoroutine(TimerToStop(transform.position, 3));
    }

    //fazer nos updates bonitinho depois quando puder
    private IEnumerator TimerToStop(Vector3 originalPosition, float maxDistance)
    {
        windEffect.Play();
        for(int i = 0; i < 15; i++)
        {
            yield return new WaitForFixedUpdate();
            if(i == 5)
                launched = true;

            if(Mathf.Abs(transform.position.x) > 7.5f || Mathf.Abs(transform.position.y) > 4.5f)
                _rigidbody.velocity /= 30;
        }
        launched = true;
        windEffect.Stop();
        _rigidbody.velocity /= 30;
        //_rigidbody.velocity = Vector2.zero;

        yield return new WaitForSeconds(1);
        //yield return new WaitUntil(() => callBack);
        //callBack = false;

        float returnSpeed = .2f;
        while (Vector3.Distance(transform.position, origin.transform.position) > returnSpeed)
        {
            yield return new WaitForFixedUpdate();
            transform.position -= Vector3.Normalize(transform.position - origin.transform.position) * returnSpeed;
        }
        Reattach();
    }

    private void Reattach()
    {
        windEffect.Stop();
        StopAllCoroutines();
        transform.position = origin.transform.position;
        transform.parent = origin.transform;
        _collider.enabled = false;
        _rigidbody.velocity = Vector2.zero;
        multiplier = 1;
        origin.AttachBlade();
        launched = false;
    }
}
