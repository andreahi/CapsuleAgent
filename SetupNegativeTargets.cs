using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupNegativeTargets : MonoBehaviour
{
    public GameObject positiveTargetPrefab;
    private static List<GameObject> walls = new List<GameObject>();

    const int wallCount = 20;


    void Start()
    {
        for (int i = 0; i < wallCount; i++)
        {
            Vector3 position = new Vector3(
                Random.value * (TrainingAreaConstants.max + TrainingAreaConstants.min) - TrainingAreaConstants.min,
                0.5f,
                Random.value * (TrainingAreaConstants.max + TrainingAreaConstants.min) - TrainingAreaConstants.min);

            GameObject wallObject = Instantiate(positiveTargetPrefab, position, Quaternion.identity);
            wallObject.GetComponent<Renderer>().material.SetColor("_Color", UnityEngine.Color.red);
            walls.Add(wallObject);
        }

        StartCoroutine(Loop());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator Loop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            _Shuffle();
        }
    }

    private static void _Shuffle()
    {
        var randomVal = Random.Range(0, walls.Count);
        var cordX = Random.value * (TrainingAreaConstants.max + TrainingAreaConstants.min) - TrainingAreaConstants.min;
        var cordZ = Random.value * (TrainingAreaConstants.max + TrainingAreaConstants.min) - TrainingAreaConstants.min;
        if (MapUtils.isInCenterZone(cordX, cordZ))
            return;

        Vector3 position = new Vector3(
            cordX,
            0.5f,
            cordZ);

        walls[randomVal].transform.position = position;
    }

    public static void Shuffle()
    {
        _Shuffle();
    }
}