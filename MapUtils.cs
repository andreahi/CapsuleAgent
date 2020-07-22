using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUtils : MonoBehaviour
{
    public static bool isInCenterZone(float yPostion, float xPostion)
    {
        return yPostion > -3 && yPostion < 3 && xPostion > -3 && xPostion < 3;
    }

    public static void ShuffleBoard(GameObject[,] map, float[,,] mapFloat)
    {
        var randomSource0 = Random.Range(0, map.GetLength(0));
        var randomSource1 = Random.Range(0, map.GetLength(1));

        var randomTarget0 = Random.Range(0, map.GetLength(0));
        var randomTarget1 = Random.Range(0, map.GetLength(1));

        var randomSource = map[randomSource0, randomSource1];
        var randomTarget = map[randomTarget0, randomTarget1];

        var xPositionTarget = randomTarget0 - TrainingAreaConstants.min;
        var yPositionTarget = randomTarget1 - TrainingAreaConstants.min;

        var xPositionSource = randomSource0 - TrainingAreaConstants.min;
        var yPositionSource = randomSource1 - TrainingAreaConstants.min;

        if (isInCenterZone(yPositionSource, xPositionSource) || isInCenterZone(yPositionTarget, xPositionTarget))
            return;

        if (randomTarget != null)
            randomTarget.transform.position = new Vector3(xPositionSource, 0.5f, yPositionSource);
        if (randomSource != null)
            randomSource.transform.position = new Vector3(xPositionTarget, 0.5f, yPositionTarget);

        map[randomSource0, randomSource1] = randomTarget;
        map[randomTarget0, randomTarget1] = randomSource;

        mapFloat[randomSource0, randomSource1, 0] = randomTarget == null ? 0.0f : 1.0f;
        mapFloat[randomTarget0, randomTarget1, 0] = randomSource == null ? 0.0f : 1.0f;
    }

    public static void RemoveObject(GameObject[,] map, float[,,] mapFloat, GameObject wall)
    {
        for (var i = 0; i < map.GetLength(0); i++)
        {
            for (var j = 0; j < map.GetLength(1); j++)
            {
                if (ReferenceEquals(map[i, j], wall))
                {
                    map[i, j] = null;
                    mapFloat[i, j, 0] = 0.0f;
                }
            }
        }
    }


    public static bool PlaceObject(GameObject[,] map, float[,,] mapFloat, float positionX, float positionZ,
        GameObject wall)
    {
        if (MapUtils.isInCenterZone(positionX, positionZ))
        {
            for (var i = 0; i < map.GetLength(0); i++)
            {
                for (var j = 0; j < map.GetLength(1); j++)
                {
                    var xPostion = i - TrainingAreaConstants.min;
                    var yPostion = j - TrainingAreaConstants.min;
                    if (MapUtils.isInCenterZone(yPostion, xPostion))
                        continue;

                    Vector3 placementPosition = new Vector3(xPostion, 0.5f, yPostion);
                    wall.transform.position = placementPosition;

                    map[i, j] = wall;
                    mapFloat[i, j, 0] = 1.0f;

                    wall.SetActive(true);
                    return true;
                }
            }
        }

        if (map[(int) positionX + TrainingAreaConstants.min, (int) positionZ + TrainingAreaConstants.min] == null)
        {
            Vector3 placementPosition = new Vector3(positionX, 0.5f, positionZ);
            wall.transform.position = placementPosition;

            map[(int) positionX + TrainingAreaConstants.min, (int) positionZ + TrainingAreaConstants.min] =
                wall;
            mapFloat[(int) positionX + TrainingAreaConstants.min, (int) positionZ + TrainingAreaConstants.min, 0] =
                1.0f;

            wall.SetActive(true);
            return true;
        }

        return false;
    }
}