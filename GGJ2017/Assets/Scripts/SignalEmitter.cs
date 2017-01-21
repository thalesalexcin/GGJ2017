using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalEmitter : MonoBehaviour
{
    public SignalWave SignalWavePrefab;

    [Range(15, 360)]
    public int Angle = 360;
    public int NumberOfSignals = 3;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < NumberOfSignals; i++)
            {
                var angleBetweenSignals = i * (Angle / NumberOfSignals);
                var signal = Instantiate<SignalWave>(SignalWavePrefab);

                Quaternion.AngleAxis(angleBetweenSignals, new Vector3(0,0,1));

                //signal.Send();
            }
        }	
	}
}
