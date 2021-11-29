using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MLTestSubjectAgent : Agent
{

    [SerializeField] private Transform targetTransform;
    [SerializeField] private SpriteRenderer sr;
    public override void OnEpisodeBegin()
    {
        transform.localPosition = Vector3.zero;
        
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];
        float movespeed = 3f;

        transform.position += new Vector3(moveX, moveY) * Time.deltaTime * movespeed;

        //Debug.Log(actions.DiscreteActions[0]);
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<MLTestGoal>(out MLTestGoal goal))
        {
            SetReward(1f);
            sr.color = Color.green;
            EndEpisode();
        }
        if (collision.TryGetComponent<MLTestWall>(out MLTestWall wall))
        {
            SetReward(-1f);
            sr.color = Color.red;
            EndEpisode();
        }
    }
}
