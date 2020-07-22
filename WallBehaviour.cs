using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pickup()
    {
        var x = Random.value * (TrainingAreaConstants.max + TrainingAreaConstants.min) - TrainingAreaConstants.min;
        var z = Random.value * (TrainingAreaConstants.max + TrainingAreaConstants.min) - TrainingAreaConstants.min;

        MapUtils.RemoveObject(Setup.map, Setup.mapFloat, this.gameObject);
        MapUtils.PlaceObject(Setup.map, Setup.mapFloat, x, z, this.gameObject);
    }
}
