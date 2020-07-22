using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine.Serialization;

public class RollerAgent : Agent
{
    private Rigidbody _rBody;
    public float speed = 10;
    public Transform positiveTarget;
    public Transform negativeTarget;

    void Start()
    {
        _rBody = GetComponent<Rigidbody>();
    }


    public override void OnEpisodeBegin()
    {
        if (this.transform.localPosition.y < 0)
        {
            // If the Agent fell, zero its momentum
            this._rBody.angularVelocity = Vector3.zero;
            this._rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 0.5f, 0);
        }

        // Move the target to a new spot
        positiveTarget.localPosition = new Vector3(Random.value * 8 - 4,
            0.5f,
            Random.value * 8 - 4);
        
        // Move the target to a new spot
        negativeTarget.localPosition = new Vector3(Random.value * 8 - 4,
            0.5f,
            Random.value * 8 - 4);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        //sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(this.transform.rotation);
    }


    public override void OnActionReceived(float[] vectorAction)
    {
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];
        _rBody.AddForce(controlSignal * speed);

        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, positiveTarget.localPosition);

        // Reached target
        if (distanceToTarget < 1.2f)
        {
            SetReward(1.0f);
            EndEpisode();
        }
        float distanceToNegativeTarget = Vector3.Distance(this.transform.localPosition, negativeTarget.localPosition);
        // Reached negative target
        if (distanceToNegativeTarget < 1.2f)
        {
            EndEpisode();
        }

        // Fell off platform
        if (this.transform.localPosition.y < 0)
        {
            EndEpisode();
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
    }
}