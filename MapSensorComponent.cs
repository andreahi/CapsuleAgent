using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class MapSensorComponent : SensorComponent
{
    public override ISensor CreateSensor()
    {
        var transform1 = this.gameObject.GetComponent<CapsuleAgent>().transform;
        var mapSensor = new MapSensor(transform1);
        return mapSensor;
    }

    public override int[] GetObservationShape()
    {
        return new[] {10, 10, 1};
    }
}