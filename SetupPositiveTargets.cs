using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupPositiveTargets : MonoBehaviour
{
    public GameObject positiveTargetPrefab;

    public static GameObject[,] map = new GameObject[TrainingAreaConstants.range + 1, TrainingAreaConstants.range + 1];
    public static float[,,] mapFloat = new float[TrainingAreaConstants.range + 1, TrainingAreaConstants.range + 1, 1];

    void Start()
    {
        for (var i = 0; i < map.GetLength(0); i++)
        {
            for (var j = 0; j < map.GetLength(1); j++)
            {
                if (Random.Range(0, 100) < 99)
                    continue;
                var xPostion = i - TrainingAreaConstants.min;
                var yPostion = j - TrainingAreaConstants.min;
                if (MapUtils.isInCenterZone(yPostion, xPostion))
                    continue;
                Vector3 position = new Vector3(xPostion, 0.5f, yPostion);

                GameObject wallObject = Instantiate(positiveTargetPrefab, position, Quaternion.identity);
                wallObject.GetComponent<Renderer>().material.SetColor("_Color", UnityEngine.Color.grey);

                map[i, j] = wallObject;
                mapFloat[i, j, 0] = 1.0f;
            }
        }


        StartCoroutine(Loop());
    }


    IEnumerator Loop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            MapUtils.ShuffleBoard(map, mapFloat);
        }
    }
}