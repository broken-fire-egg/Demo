using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.UI;

public class MLTestShootingAI2Agent : Agent
{
    public Text score;
    [SerializeField]
    private SpriteRenderer sr;
    [SerializeField] private MLTestShootingAIAgent opponent;
    //[SerializeField] private Transform targetTransform;
    public Transform agentCenter;
    [SerializeField] private Transform gun;
    [SerializeField] private Transform gunPoint;
    private SpriteRenderer gunsr;
    [SerializeField] private Rigidbody2D rg2d;
    [SerializeField] private MLTestOP op;
    [SerializeField] private Animator animator;
    [SerializeField] private float degree;
    public bool isred_;
    float currentanimationframe = 0f;
    [SerializeField] float current_x = 0f;
    [SerializeField] float current_y = 0f;
    [SerializeField] float ideal_x = 0f;
    [SerializeField] float ideal_y = 0f;

    

    private void Awake()
    {
        gunsr = gun.GetComponent<SpriteRenderer>();
    }
    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-2.5f,2.5f),Random.Range(-3.5f,3.5f));



        op.OffAll();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(ideal_x);
        sensor.AddObservation(ideal_y);
        sensor.AddObservation(transform.position);
        sensor.AddObservation(opponent.transform.position);
        sensor.AddObservation(isred_);
        sensor.AddObservation(op.leftRedBullet);
        sensor.AddObservation(op.leftBlueBullet);
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> des = actionsOut.DiscreteActions;
        ActionSegment<float> con = actionsOut.ContinuousActions;
        int moveX = 0;
        int moveY = 0;
        if (Input.GetKey(KeyCode.W))
            moveY += 1;
        if (Input.GetKey(KeyCode.A))
            moveX -= 1;
        if (Input.GetKey(KeyCode.S))
            moveY -= 1;
        if (Input.GetKey(KeyCode.D))
            moveX += 1;
        switch (moveX)
        {
            case 0: des[0] = 0; break;
            case -1: des[0] = 2; break;
            case 1: des[0] = 1; break;
        }
        switch (moveY)
        {
            case 0: des[1] = 0; break;
            case -1: des[1] = 2; break;
            case 1: des[1] = 1; break;
        }

      Vector3 mouseVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      mouseVector.z = transform.position.z;
      mouseVector = (mouseVector - agentCenter.position).normalized;


        con[0] = mouseVector.x;
        con[1] = mouseVector.y;


        des[2] = Input.GetMouseButton(0) ? 1 : 0;


    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        int moveX = actions.DiscreteActions[0];
        int moveY = actions.DiscreteActions[1];
        Vector2 vec2 = Vector2.zero;
        switch (moveX)
        {
            case 0: vec2.x = 0; break;
            case 1: vec2.x = 1; break;
            case 2: vec2.x = -1; break;
        }
        switch (moveY)
        {
            case 0: vec2.y = 0; break;
            case 1: vec2.y = 1; break;
            case 2: vec2.y = -1; break;
        }
        float moveSpeed = 4f;
        transform.localPosition += new Vector3(vec2.x, vec2.y) * moveSpeed * Time.deltaTime;

        float deg1 = actions.ContinuousActions[0];
        float deg2 = actions.ContinuousActions[1];

        Vector3 gunvec = new Vector2(deg1, deg2);

        current_x = gunvec.x;
        current_y = gunvec.y;

        float deg = Vector2.Angle(Vector2.right, gunvec);
        if (deg2 < 0) deg = 360f - deg;
        degree = deg;

        Vector3 vec3 = agentCenter.transform.position - opponent.agentCenter.transform.position;

        ideal_x = -vec3.normalized.x;
        ideal_y = -vec3.normalized.y;


        if (currentanimationframe > 1f)
            currentanimationframe -= 1f;
        animator.SetFloat("progress", currentanimationframe);
        currentanimationframe += 0.02f;




        //float deg = Random.Range(-1, 1f);
        Quaternion rot = Quaternion.Euler(0, 0, deg);
        if ( degree <= 22.5f || degree > 337.5f)
        {
            animator.Play("PlayerMove1_2");
        }
        else if (degree <= 67.5f)
        {
            animator.Play("PlayerMove1_1");
            
        }
        else if (degree <= 112.5f)
        {
            animator.Play("PlayerMove1_0");
            
        }
        else if (degree <= 157.5f)
        {
            animator.Play("PlayerMove1_7");
            
        }
        else if (degree <= 202.5f)
        {
            animator.Play("PlayerMove1_6");
        }
        else if (degree <= 247.5f)
        {
            animator.Play("PlayerMove1_5");
        }
        else if (degree <= 292.5)
        {
            animator.Play("PlayerMove1_4");
        }
        else if (degree <= 337.5f)
        {
            animator.Play("PlayerMove1_3");
        }

        gun.rotation = Quaternion.Euler(0, 0, deg);

        gunsr.flipY = deg > 90 && deg < 270;

        bool isShot = actions.DiscreteActions[2] == 1;
        if (isShot)
        {
            if(op.Shot(gunPoint.position, rot, isred_))
                AddReward(-0.1f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Bullet>(out Bullet bullet))
        {
            if ((bullet.CompareTag("MLRedBullet") && !isred_) || (bullet.CompareTag("MLBlueBullet") && isred_))
            {
                if (isred_)
                    sr.color = new Color(0, 0, 0.5f);
                else
                    sr.color = new Color(0.5f, 0, 0);
                score.text = (int.Parse(score.text) + 1).ToString();
                AddReward(-100f);
                opponent.AddReward(100f);
                if (opponent.gameObject.activeInHierarchy)
                    opponent.EndEpisode();
                EndEpisode();
            }
        }
        if (collision.TryGetComponent<MLTestWall>(out MLTestWall wall))
        {

            if (isred_)
                sr.color = new Color(0, 0, 0.5f);
            else
                sr.color = new Color(0.5f, 0, 0);
            score.text = (int.Parse(score.text) + 1).ToString();
            AddReward(-100f);
            //opponent.AddReward(100f);
            if (opponent.gameObject.activeInHierarchy)
                opponent.EndEpisode();
            EndEpisode();
        }
    }
}
