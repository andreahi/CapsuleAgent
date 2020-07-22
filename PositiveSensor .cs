using System;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class PositiveSensor : ISensor
{
    private Transform transform;
    List<GameObject> viz = new List<GameObject>();
    private float baseX, baseY;
    private GameObject drawPrefab;
    private static bool initiated = false;
    private bool draw = false;
    int mapRange = 15;

    private static float[,,] MapFloat = SetupPositiveTargets.mapFloat;

    public PositiveSensor(Transform transform, GameObject drawPrefab)
    {
        this.transform = transform;
        this.baseX = 60;
        this.baseY = 60;
        this.drawPrefab = drawPrefab;
        //if (!initiated)
        //    draw = true;
        initiated = true;
    }

    public int[] GetObservationShape()
    {
        return new[] {10, 10, 1};
    }

    public int Write(ObservationWriter writer)
    {
        var positionX = transform.position.x;
        var positionZ = transform.position.z;
        foreach (var gameObject in viz)
        {
            Setup.Destroy(gameObject);
        }

        viz.Clear();
        int written = 0;
        for (int ix = 0, i = -mapRange; i < mapRange; i += 3, ix++)
        {
            for (int iz = 0, j = -mapRange; j < mapRange; j += 3, iz++)
            {
                var value = GetMapValue(positionX, i, positionZ, j);

                Vector3 position = new Vector3(baseX + ix, 0.5f, baseY + iz);

                writer[ix, iz, 0] = value;
                written++;
                if (draw)
                {
                    if (Math.Abs(value - 1.0f) < 0.001)
                    {
                        GameObject wallObject = Setup.Instantiate(drawPrefab, position, Quaternion.identity);
                        viz.Add(wallObject);
                    }
                }
            }
        }

        return written;
    }

    private static float GetMapValue(float positionX, int i, float positionZ, int j)
    {
        var index0 = (int) Math.Round(positionX) + TrainingAreaConstants.min + i;
        var index1 = (int) Math.Round(positionZ) + TrainingAreaConstants.min + j;

        if (index0 < 0 || index0 > TrainingAreaConstants.range || index1 < 0 || index1 > TrainingAreaConstants.range)
            return 0.0f;
        return new[]
        {
            MapFloat[index0 + 0, index1, 0],
            MapFloat[index0 + 1, index1, 0],
            MapFloat[index0 + 2, index1, 0],

            MapFloat[index0 + 0, index1 + 1, 0],
            MapFloat[index0 + 1, index1 + 1, 0],
            MapFloat[index0 + 2, index1 + 1, 0],

            MapFloat[index0 + 0, index1 + 2, 0],
            MapFloat[index0 + 1, index1 + 2, 0],
            MapFloat[index0 + 2, index1 + 2, 0],
        }.Max();
    }


    public byte[] GetCompressedObservation()
    {
        throw new System.NotImplementedException();
    }

    void ISensor.Update()
    {
        //Update();
    }

    public void Reset()
    {
        //throw new System.NotImplementedException();
    }

    public SensorCompressionType GetCompressionType()
    {
        return SensorCompressionType.None;
    }

    public string GetName()
    {
        return "agent.map.positive.sensor";
    }
}