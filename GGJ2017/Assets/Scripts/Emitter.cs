using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emitter : MonoBehaviour
{
    public SignalWave SignalWavePrefab;
    public GameObject SignalsHolder;

    [Range(15, 360)]
    public int Angle;
    public int NumberOfSignals;
	
	public void Send(int initialAngle, Vector3 initialPosition, EInputType type, int id, bool replicated = false)
    {
        for (int i = 0; i < NumberOfSignals; i++)
        {
            var angleBetweenSignals = 0;
            var angle = initialAngle;
            if (NumberOfSignals > 1)
            {
                angleBetweenSignals = Angle / (NumberOfSignals - 1);
                angle = angle + (i * angleBetweenSignals) - (Angle / 2);
            }

            var signal = Instantiate<SignalWave>(SignalWavePrefab);

            signal.transform.position = initialPosition;
            signal.transform.parent = SignalsHolder.transform;

            if (replicated)
                signal.SetReplicationOff();


            var direction = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up;
            signal.Send(direction, type, id);
        }
    }

    public void SendReplicated(int initialAngle, Vector3 initialPosition, EInputType inputType, int id)
    {
        Send(initialAngle, initialPosition, inputType, id, true);
    }
}
