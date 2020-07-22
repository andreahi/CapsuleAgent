using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setup : MonoBehaviour
{
    public GameObject wallPrefab;
    public GameObject bedrockPrefab;

    public static GameObject[,] map = new GameObject[TrainingAreaConstants.range + 1, TrainingAreaConstants.range + 1];
    public static float[,,] mapFloat = new float[TrainingAreaConstants.range + 1, TrainingAreaConstants.range + 1, 1];

    void Start()
    {
        for (var i = 0; i < map.GetLength(0); i++)
        {
            for (var j = 0; j < map.GetLength(1); j++)
            {
                if (Random.Range(0, 10) > 3)
                    continue;
                var xPostion = i - TrainingAreaConstants.min;
                var yPostion = j - TrainingAreaConstants.min;
                if (MapUtils.isInCenterZone(yPostion, xPostion))
                    continue;
                Vector3 position = new Vector3(xPostion, 0.5f, yPostion);

                GameObject wallObject = Instantiate(wallPrefab, position, Quaternion.identity);
                wallObject.GetComponent<Renderer>().material.SetColor("_Color", UnityEngine.Color.grey);

                map[i, j] = wallObject;
                mapFloat[i, j, 0] = 1.0f;
            }
        }


        for (int i = 0; i < TrainingAreaConstants.range; i++)
        {
            Vector3 position = new Vector3(-TrainingAreaConstants.min + 1,
                0.5f,
                i - TrainingAreaConstants.min);

            GameObject wallObject = Instantiate(bedrockPrefab, position, Quaternion.identity);
            wallObject.GetComponent<Renderer>().material.SetColor("_Color", UnityEngine.Color.grey);
        }

        for (int i = 0; i < TrainingAreaConstants.range; i++)
        {
            Vector3 position = new Vector3(TrainingAreaConstants.max - 1,
                0.5f,
                i - TrainingAreaConstants.min);

            GameObject wallObject = Instantiate(bedrockPrefab, position, Quaternion.identity);
            wallObject.GetComponent<Renderer>().material.SetColor("_Color", UnityEngine.Color.grey);
        }

        for (int i = 0; i < TrainingAreaConstants.range; i++)
        {
            Vector3 position = new Vector3(i - TrainingAreaConstants.min,
                0.5f,
                -TrainingAreaConstants.min + 1);

            GameObject wallObject = Instantiate(bedrockPrefab, position, Quaternion.identity);
            wallObject.GetComponent<Renderer>().material.SetColor("_Color", UnityEngine.Color.grey);
        }

        for (int i = 0; i < TrainingAreaConstants.range; i++)
        {
            Vector3 position = new Vector3(i - TrainingAreaConstants.min,
                0.5f,
                TrainingAreaConstants.max - 1);

            GameObject wallObject = Instantiate(bedrockPrefab, position, Quaternion.identity);
            wallObject.GetComponent<Renderer>().material.SetColor("_Color", UnityEngine.Color.grey);
        }


        StartCoroutine(Loop());
    }



    IEnumerator Loop()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            MapUtils.ShuffleBoard(map, mapFloat);
        }
    }


}