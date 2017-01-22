using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emitter : MonoBehaviour
{
    public SignalWave SignalWavePrefab;

    [Range(15, 360)]
    public int Angle;
    public int NumberOfSignals;
	

	public void Send(float initialAngle, Vector3 initialPosition, EInputType type, int id, bool replicated = false)
    {
        _Send(initialAngle, initialPosition, SignalWavePrefab.robotSpeed, type, id, replicated);
    }

    private void _Send(float initialAngle, Vector3 initialPosition, float robotSpeed, EInputType type, int id, bool replicated)
    {
        for (int i = 0; i < NumberOfSignals; i++)
        {
            var angleBetweenSignals = 0f;
            var angle = initialAngle;
            if (NumberOfSignals > 1)
            {
                angleBetweenSignals = (float) Angle / (NumberOfSignals - 1);
                angle = angle + (i * angleBetweenSignals) - (Angle / 2);
            }

            var signal = SignalWavePooling.Current.GetPooled();

            signal.gameObject.SetActive(true);
            signal.transform.position = initialPosition;
            signal.robotSpeed = robotSpeed;

            if (replicated)
                signal.SetReplicationOff();


            var direction = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up;
            signal.Send(direction, type, id);
        }
    }

    public void SendReplicated(float initialAngle, Vector3 initialPosition, float robotSpeed, EInputType inputType, int id)
    {
        _Send(initialAngle, initialPosition, robotSpeed, inputType, id, true);
    }
}
