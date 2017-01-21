using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum EInputType
{
    None = 0,
    Right,
    Left,
    Jump
}

public class SignalEmitter : MonoBehaviour
{
    public SignalWave SignalWavePrefab;
    public GameObject SignalsHolder;
    public bool IsUsingJoystick;

    [Range(15, 360)]
    public int Angle = 360;
    public int NumberOfSignals = 3;

    private int _CurrentId = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        _SetEmmiterRotation();
        _SendSignals();
    }

    private void _SendSignals()
    {
        EInputType input = EInputType.None;

        if (Input.GetAxisRaw("Fire1") > 0.5f)
            input = EInputType.Right;

        if (Input.GetAxis("Fire2") > 0.5f)
            input = EInputType.Left;

        if (Input.GetAxis("Fire3") > 0.5f)
            input = EInputType.Jump;

        if (input != EInputType.None)
            _SendSignals(input, _CurrentId++);
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

    private void _SendSignals(EInputType input, int id)
    {
        for (int i = 0; i < NumberOfSignals; i++)
        {
            var angleBetweenSignals = 0;
            var angle = transform.rotation.eulerAngles.z;
            if (NumberOfSignals > 1)
            {
                angleBetweenSignals = Angle / (NumberOfSignals - 1);
                angle = angle + (i * angleBetweenSignals) - (Angle / 2);
            }

            var signal = Instantiate<SignalWave>(SignalWavePrefab);
            
            signal.transform.position = transform.position;
            signal.transform.parent = SignalsHolder.transform;

            var direction = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up;
            signal.Send(direction, input, id);
        }
    }
}
