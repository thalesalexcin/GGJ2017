using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        var position = GetWorldPositionOnPlane(Camera.main, Input.mousePosition, 0);
        var direction = position - transform.position;
        var rotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = rotation;

        EInputType input = EInputType.None;

        if (Input.GetKeyDown(KeyCode.RightArrow))
            input = EInputType.Right;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            input = EInputType.Left;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            input = EInputType.Jump;

        if(input != EInputType.None)
            _SendSignals(input, _CurrentId++);
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
