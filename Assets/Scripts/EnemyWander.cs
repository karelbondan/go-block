using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder.Shapes;

public class EnemyWander : MonoBehaviour
{
    private RunTimeReferences runTimeReferences;

    Vector3 backLeftPos;
    Vector3 backRightPos;
    Vector3 frontLeftPos;
    Vector3 frontRightPos;

    NavMeshAgent agent;

    public float minWaitingTime, maxWaitingTime, firstFrame;

    float timeElapsed = 0;
    float waitingTime = 0;
    int framePassed = 0;

    bool hasArrived = true;

    void Start()
    {
        runTimeReferences = GameObject.Find("Game Manager").GetComponent<RunTimeReferences>();
        backLeftPos = runTimeReferences.backLeft.transform.position;
        backRightPos = runTimeReferences.backRight.transform.position;
        frontLeftPos = runTimeReferences.frontLeft.transform.position;
        frontRightPos = runTimeReferences.frontRight.transform.position;

        agent = GetComponent<NavMeshAgent>();

        firstFrame = Time.time;
    }

    void Update()
    {
        RaycastHit hit;

        if (timeElapsed > waitingTime)
        {
            // if agent has not arrived to dest and the waiting time has passed, 
            // stop the agent for 5 secs before going to other dest. 
            if (!hasArrived)
            {
                agent.destination  = agent.transform.position + agent.transform.forward * 3f;
                hasArrived = true;
                waitingTime = 5f;
            } 
            else
            {
                Vector3 position = new Vector3(Random.Range(backLeftPos.x, backRightPos.x), 0, Random.Range(backRightPos.z, frontRightPos.z));
            
                if (Physics.Raycast(position + new Vector3(0, 100.0f, 0), Vector3.down, out hit, 200.0f))
                {
                    hasArrived = false;
                    agent.destination  = hit.transform.position;
                    //waitingTime = Random.Range(minWaitingTime, maxWaitingTime);
                    waitingTime = 5f;
                }
                else
                {
                    Debug.Log("there seems to be no ground at this position");
                }
                
            }
            timeElapsed = 0;
            firstFrame = Time.time;
        } 
        else if (agent.remainingDistance < agent.stoppingDistance + 2)
        {
            if (!hasArrived && framePassed > 5)
            {
                hasArrived = true;
                framePassed = 0;

                // update position after arriving at the destination after 5 seconds of idling. 
                waitingTime = 5f;
                timeElapsed = 0;
                firstFrame = Time.time;
            }
            framePassed++; 
        }

        timeElapsed = Time.time - firstFrame;
    }












    //public float speed = 3.0f;
    //public float obstacleRange = 5.0f;
    //public float rotSpeed = 10.0f;

    //// stuck in a corner prevention
    //bool isMoving = true;
    //int movingCount = 0;

    //Quaternion direction;       // wandering direction
    //bool isRotating = false;    // rotate over a number of frames

    //// Start is called before the first frame update
    //void Start()
    //{
    //    // start in a random direction
    //    float angle = Random.Range(-180.0f, 180.0f);
    //    direction = Quaternion.LookRotation(Quaternion.Euler(0.0f, angle, 0.0f) * transform.forward);
    //    isRotating = true;
    //}

    //void OnDrawGizmos()
    //{
    //    // draw a red line gizmo to indicate collision avoidance distance
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(transform.position, transform.position + transform.forward * obstacleRange);
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    // if the agent is rotating
    //    if (isRotating)
    //    {
    //        // force the agent to stop moving
    //        isMoving = false;

    //        // rotate the agent over several frames
    //        transform.rotation = Quaternion.Slerp(transform.rotation,
    //                               direction, rotSpeed * Time.deltaTime);

    //        // if the agent within a certain angle of the correct direction
    //        if (Quaternion.Angle(transform.rotation, direction) < 1.0f)
    //        {
    //            isRotating = false;
    //        }
    //    }
    //    else
    //    {
    //        // the agent is moving, reset the moving count too
    //        isMoving = true;
    //        movingCount = 0;

    //        // move the agent
    //        transform.Translate(0, 0, speed * Time.deltaTime);
    //    }

    //    // collision avoidance
    //    Ray ray = new Ray(transform.position, transform.forward);
    //    RaycastHit hit;

    //    // cast a sphere to check whether it collides with anything
    //    if (Physics.SphereCast(ray, 0.75f, out hit))
    //    {
    //        // if the collision is within the collision avoidance range
    //        if (hit.distance < obstacleRange)
    //        {
    //            // choose a random angle
    //            float angle = Random.Range(-110.0f, 110.0f);

    //            // check each time the agent is stuck.
    //            // if stuck more than 5 frames then force rotate it 45 degrees
    //            if (!isMoving)
    //            {
    //                movingCount++;
    //                if (movingCount > 5)
    //                {
    //                    angle = 45.0f;
    //                }
    //            }

    //            // set the direction based on the angle
    //            direction = Quaternion.LookRotation(Quaternion.Euler(0.0f, angle, 0.0f) * transform.forward);
    //            isRotating = true;
    //        }
    //    }
    //}
}
