using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositiveTargetBehaviour : MonoBehaviour
{

    public void pickup()
    {
        if (Random.Range(0, 10) > 8)
        {
            var x = Random.value * (TrainingAreaConstants.max + TrainingAreaConstants.min) - TrainingAreaConstants.min;
            var z = Random.value * (TrainingAreaConstants.max + TrainingAreaConstants.min) - TrainingAreaConstants.min;

            MapUtils.RemoveObject(SetupPositiveTargets.map, SetupPositiveTargets.mapFloat, this.gameObject);
            MapUtils.PlaceObject(SetupPositiveTargets.map, SetupPositiveTargets.mapFloat, x, z, this.gameObject);
        }

    }
}
