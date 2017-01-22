using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalWave : MonoBehaviour
{
    public EInputType InputType;
    public int Id { get; set; }

    public float Speed = 1f;
    public float DieTime;
    public float robotSpeed;
    public float DelayForReplicating = 1;

    private Rigidbody2D _Rigidbody;
    private bool _CanBeReplicated;

    void Awake()
    {
        _CanBeReplicated = true;
        _Rigidbody = GetComponent<Rigidbody2D>();
	}

    void OnEnable()
    {
        Invoke("DestroySignal", DieTime);
    }

    void OnDisable()
    {
        CancelInvoke("DestroySignal");
    }

    public void DestroySignal()
    {
        _CanBeReplicated = true;
        gameObject.SetActive(false);
    }

    public void SetReplicationOff()
    {
        _CanBeReplicated = false;
    }

    public void Send(Vector2 direction, EInputType input, int id)
    {
        InputType = input;
        Id = id;

        switch (input)
        {
            case EInputType.Jump:
                GetComponent<ParticleSystemRenderer>().material.SetColor("_TintColor", Color.green);
                break;
            case EInputType.Left:
                GetComponent<ParticleSystemRenderer>().material.SetColor("_TintColor", Color.red);
                break;
            case EInputType.Right:
                GetComponent<ParticleSystemRenderer>().material.SetColor("_TintColor", Color.blue);
                break;
        }

        _Rigidbody.AddForce(direction * Speed, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Robot"))
            _DestroyAfterEffects();

        else if (collider.CompareTag("NotBouncyBlock"))
            _DestroyAfterEffects();

        else if (collider.CompareTag("Inverser"))
            _InverseInput();

        else if (collider.CompareTag("SpeedBlock"))
            _ChangeSpeed(collider);

        else if (collider.CompareTag("MultiplierBlock") && _CanBeReplicated)
            _MultiplySignal(collider);

        else if (collider.CompareTag("RedirectBlock"))
            _RedirectSignal(collider);

        else if (collider.CompareTag("RandomRedirectBlock"))
            _RandomRedirectSignal(collider);
    }

    private void _RedirectSignal(Collider2D collider)
    {
        var angle = collider.transform.rotation.eulerAngles.z;
        var direction = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up;
        _Rigidbody.velocity = Vector2.zero;

        transform.position = collider.transform.position;

        Send(direction, InputType, Id);
    }

    private void _RandomRedirectSignal(Collider2D collider)
    {
        _RedirectSignal(collider);

        var rotation = Random.rotationUniform;

        rotation.x = 0;
        rotation.y = 0;

        collider.transform.rotation = rotation;
    }

    private void _MultiplySignal(Collider2D collider)
    {
        var emitter = collider.GetComponent<Emitter>();

        var angle = Quaternion.LookRotation(Vector3.forward, _Rigidbody.velocity).eulerAngles.z;
        emitter.SendReplicated(angle, transform.position, robotSpeed, Speed, InputType, Id);

        DestroySignal();
    }

    private void _ChangeSpeed(Collider2D collider)
    {
        var speedBlock = collider.GetComponent<SpeedBlock>();
        
        _Rigidbody.velocity *= speedBlock.SpeedMultiplier;
        robotSpeed *= speedBlock.RobotSpeedMultiplier;
    }

    private void _InverseInput()
    {
        if (InputType == EInputType.Left)
            InputType = EInputType.Right;
        else if (InputType == EInputType.Right)
            InputType = EInputType.Left;

        var direction = _Rigidbody.velocity.normalized;
        _Rigidbody.velocity = Vector2.zero;
        Send(direction, InputType, Id);
    }

    private void _DestroyAfterEffects()
    {
        DestroySignal();
    }
}
