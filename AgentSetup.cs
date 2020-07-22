using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AgentSetup : MonoBehaviour
{
    private int agentCount = 3;

    public static List<CapsuleAgent> agents = new List<CapsuleAgent>();
    public GameObject agentPrefab;

    void Start()
    {
        for (int i = 0; i < agentCount; i++)
        {
            Vector3 position = new Vector3(
                Random.value * (TrainingAreaConstants.max + TrainingAreaConstants.min) - TrainingAreaConstants.min,
                0.5f,
                Random.value * (TrainingAreaConstants.max + TrainingAreaConstants.min) - TrainingAreaConstants.min);

            GameObject agentObject = Instantiate(agentPrefab, position, Quaternion.identity);
            agents.Add(agentObject.GetComponent<CapsuleAgent>());
        }

        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        float secretTarget = 10000.0f;

        while (true)
        {
            yield return new WaitForSeconds(5);
            float totalReward = 0.0f;
            foreach (var capsuleAgent in agents)
            {
                if (Math.Abs(capsuleAgent.GetSecretOutput() - secretTarget) < 0.1)
                {
                    totalReward += 0.5f;
                }

                capsuleAgent.SetSecretTarget(0.0f);
            }

            agents[0].addReward(totalReward);

            yield return new WaitForSeconds(10);

            secretTarget = Random.value;
            agents[0].SetSecretTarget(secretTarget);
            agents[1].SetSecretTarget(secretTarget);
            agents[2].SetSecretTarget(secretTarget);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}