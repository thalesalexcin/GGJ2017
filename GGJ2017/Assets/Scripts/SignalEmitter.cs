using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public enum EInputType
{
    Right,
    Left,
    Jump
}

public class SignalEmitter : MonoBehaviour
{
    public float TimeBetweenSameInput = 0.3f;

    private Emitter _Emitter;
    private Dictionary<EInputType, float> _timersByInput;
    private int _CurrentId = 0;
    private AudioManager _AudioManager;
    private bool _hasSent;

    void Awake()
    {
        _Emitter = GetComponent<Emitter>();
        _AudioManager = FindObjectOfType<AudioManager>();
    }

    // Use this for initialization
    void Start ()
    {
        _timersByInput = new Dictionary<EInputType, float>();

        foreach (EInputType inputType in Enum.GetValues(typeof(EInputType)))
            _timersByInput.Add(inputType, 0);
	}
	
	// Update is called once per frame
	void Update ()
    {
        _SetEmmiterRotation();
        _SendSignals();
    }

    private void _SendSignals()
    {
        List<EInputType> inputs = new List<EInputType>();
        _hasSent = false;

        _timersByInput[EInputType.Right] -= Time.deltaTime;
        _timersByInput[EInputType.Left] -= Time.deltaTime;
        _timersByInput[EInputType.Jump] -= Time.deltaTime;

        if (Input.GetAxisRaw("Fire1") > 0.5f)
        {
            inputs.Add(EInputType.Right);
            
        }
        //else
        //    _timersByInput[EInputType.Right] = 0;

        if (Input.GetAxis("Fire2") > 0.5f)
        {
            inputs.Add(EInputType.Left);
        }
        //else
        //    _timersByInput[EInputType.Left] = 0;

        if (Input.GetAxis("Fire3") > 0.5f)
        {
            inputs.Add(EInputType.Jump);
        }
        //else
        //    _timersByInput[EInputType.Jump] = 0;

        foreach (var input in inputs)
        {
            if (_timersByInput[input] <= 0)
            {
                _Emitter.Send(transform.rotation.eulerAngles.z, transform.position, input, _CurrentId++);
                _timersByInput[input] = TimeBetweenSameInput;
                _hasSent = true;
            }
        }

        if (_hasSent)
            _AudioManager.Play(EAudioType.Wave);
    }

    private void _SetEmmiterRotation()
    {
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        var direction = new Vector2(x, y);

        if (x == 0 && y == 0)
        {
            var position = GetWorldPositionOnPlane(Camera.main, Input.mousePosition, 0);
            direction = position - transform.position;
        }

        var rotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = rotation;
    }

    public static Vector3 GetWorldPositionOnPlane(Camera camera, Vector3 screenPosition, float z)
    {
        Ray ray = camera.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }
}
