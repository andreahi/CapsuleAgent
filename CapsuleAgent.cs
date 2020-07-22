using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Color = UnityEngine.Color;
using Random = UnityEngine.Random;

public class CapsuleAgent : Agent
{
    private UnityEngine.Color agentColor = new UnityEngine.Color(.5f, 1.0f, 1.0f);
    private Rigidbody _rBody;
    public float speed = 10;

    private GameObject lastCollidedWall = null;
    private GameObject lastCollidedPositive = null;
    private int pickedUpWalls = 0;
    private GameObject pickedUpPositive = null;
    private List<GameObject> touchedObjects = new List<GameObject>();

    private const int maxWallsPickUp = 5;
    private int stepCountSinceActive = 0;
    private float messageA = 0.0f;
    private float messageB = 0.0f;
    private float messageC = 0.0f;

    private float targetSecret;
    private float secretOutput;

    void Start()
    {
        this.GetComponent<Renderer>().material.SetColor("_Color", agentColor);
    }


    public override void OnEpisodeBegin()
    {
        stepCountSinceActive = 0;
        _rBody = GetComponent<Rigidbody>();

        // If the Agent fell, zero its momentum
        this._rBody.angularVelocity = Vector3.zero;
        this._rBody.velocity = Vector3.zero;

        Vector3 position = new Vector3(
            0 + Random.value * 3,
            1f,
            0 + Random.value * 3);

        this.transform.position = position;

        if (Random.Range(0, 10) > 8)
        {
            for (int i = 0; i < 10000; i++)
            {
                MapUtils.ShuffleBoard(Setup.map, Setup.mapFloat);
                MapUtils.ShuffleBoard(SetupPositiveTargets.map, SetupPositiveTargets.mapFloat);
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        stepCountSinceActive = 0;
        if (col.gameObject.CompareTag("Walls"))
        {
            lastCollidedWall = col.gameObject;
        }

        if (col.gameObject.CompareTag("PositiveTarget"))
        {
            lastCollidedPositive = col.gameObject;
        }
    }

    void OnCollisionExit(Collision col)
    {
        stepCountSinceActive = 0;

        if (col.gameObject.CompareTag("Walls"))
        {
            lastCollidedWall = null;
        }

        if (col.gameObject.CompareTag("PositiveTarget"))
        {
            lastCollidedPositive = null;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(this.transform.position.x / TrainingAreaConstants.max);
        sensor.AddObservation(this.transform.position.z / TrainingAreaConstants.max);
        sensor.AddObservation(this._rBody.velocity.x / 10.0f);
        sensor.AddObservation(this._rBody.velocity.z / 10.0f);
        sensor.AddObservation(this.pickedUpWalls / maxWallsPickUp);
        sensor.AddObservation(this.pickedUpPositive != null ? 1.0f : 0.0f);
        sensor.AddObservation(MapUtils.isInCenterZone(this.transform.position.x, this.transform.position.z)
            ? 1.0f
            : 0.0f);       
        
        sensor.AddObservation(targetSecret);
        
        sensor.AddObservation(this.GetMessageA());
        sensor.AddObservation(this.GetMessageB());
        sensor.AddObservation(this.GetMessageC());
        foreach (var capsuleAgent in AgentSetup.agents)
        {
            if (this == capsuleAgent)
            {
                continue;
            }
            sensor.AddObservation(capsuleAgent.GetMessageA());
            sensor.AddObservation(capsuleAgent.GetMessageB());
            sensor.AddObservation(capsuleAgent.GetMessageC());
        }
    }

    private float GetMessageA()
    {
        return messageA;
    }

    private float GetMessageB()
    {
        return messageB;
    }

    private float GetMessageC()
    {
        return messageC;
    }


    public override void OnActionReceived(float[] vectorAction)
    {
        stepCountSinceActive += 1;

        // Fell off platform
        if (this.transform.localPosition.y < 0)
        {
            addReward(-0.01f);
            EndEpisode();
            return;
        }

        if (stepCountSinceActive > 1000)
        {
            //EndEpisode();
            //addReward(-0.1f);
            //return;
        }


        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0] * 10;
        controlSignal.z = vectorAction[1] * 10;
        _rBody.AddForce(controlSignal * speed);


        if (vectorAction[2] > 0.0 && lastCollidedWall != null && pickedUpWalls < maxWallsPickUp)
        {
            lastCollidedWall.GetComponent<WallBehaviour>().pickup();
            pickedUpWalls += 1;

            addReward(0.001f);
        }


        if (pickedUpWalls > 0 && MapUtils.isInCenterZone(this.transform.position.x, this.transform.position.z))
        {
            pickedUpWalls = 0;
            addReward(0.001f);
        }


        if (lastCollidedPositive != null && pickedUpPositive == null)
        {
            lastCollidedPositive.GetComponent<PositiveTargetBehaviour>().pickup();
            pickedUpPositive = lastCollidedPositive;
            addReward(0.1f);
            
            //this.GetComponent<Renderer>().material.SetColor("_Color", new UnityEngine.Color(.5f, 1.0f, 0.5f));

        }

        if (pickedUpPositive != null && MapUtils.isInCenterZone(this.transform.position.x, this.transform.position.z))
        {
            pickedUpPositive = null;
            addReward(1.0f);
            
            //this.GetComponent<Renderer>().material.SetColor("_Color", agentColor);
        }

        this.messageA = vectorAction[3];
        this.messageB = vectorAction[4];
        this.messageC = vectorAction[5];

        this.secretOutput = vectorAction[3];
        
        this.GetComponent<Renderer>().material.SetColor("_Color", new UnityEngine.Color((messageA + 1) / 2, 0, 0));

    }

    public void addReward(float reward)
    {
        this.stepCountSinceActive = 0;
        foreach (var capsuleAgent in AgentSetup.agents)
        {
            capsuleAgent.AddReward(reward);
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
    }

    public void SetSecretTarget(float secret)
    {
        this.targetSecret = secret;
    }
    public float GetSecretOutput()
    {
        return this.secretOutput;
    }
}

//var agentPosition = this.transform.position;
//var positionX = (int) Mathf.Round(agentPosition.x) +
//                Mathf.Sign(vectorAction[6]) * Mathf.Ceil(Mathf.Abs(vectorAction[6]));
//var positionZ = (int) Mathf.Round(agentPosition.z) +
//                Mathf.Sign(vectorAction[7]) * Mathf.Ceil(Mathf.Abs(vectorAction[7]));