using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class PositiveSensorComponent : SensorComponent
{
    public GameObject drawPrefab;

    // Start is called before the first frame update
    public override ISensor CreateSensor()
    {
        var transform1 = this.gameObject.GetComponent<CapsuleAgent>().transform;

        var positiveSensor = new PositiveSensor(transform1, drawPrefab);
        return positiveSensor;
    }

    public override int[] GetObservationShape()
    {
        return new[] {10, 10, 1};
    }
}
