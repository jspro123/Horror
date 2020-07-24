using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;

public class MonsterManager : MonoBehaviour
{

    public Camera myCam;

    public enum MonsterState { Random, Chase, Stop };

    [Header("Waypoint Settings")]
    public List<GameObject> waypoints;
    [Tooltip("How long the monster should wait if they teleport here. ")]
    public float waitStartDuration;
    [Tooltip("How long the monster should wait once they make it here. ")]
    public float waitEndDuration;

    [Header("Monster Settings")]
    public float monsterWaypointSpeed = 1.0f;
    public float monsterChaseSpeed = 4.0f;
    [Range(0.0f ,1.0f)]
    public float wayPointProbability = 0;
    [Range(0.0f, 1.0f)]
    public float chaseProbability = 0;
    public float monsterChaseDurationMin;
    public float monsterChaseDurationMax;
    [Tooltip("How long the monster should wait once they stop chasing. ")]
    public float chaseEndDuration;

    [Header("Float Settings")]
    public float maxHeight = 0.5f;
    public float minHeight = 0.0f;
    public float duration = 4.0f;

    private NavMeshAgent agentProperties;
    private MonsterState currentState = MonsterState.Random;
    private Vector3 currentDestination;
    private bool coroutineEnded = false;
    
    private void Start()
    {
        agentProperties = GetComponent<NavMeshAgent>();
        /*
        this.transform.position = new Vector3(this.transform.position.x, minHeight, this.transform.position.z);
        Sequence s = DOTween.Sequence();
        s.Append(this.transform.DOMoveY(maxHeight, duration));
        s.Append(this.transform.DOMoveY(minHeight, duration));
        s.SetLoops(-1, LoopType.Restart);
        */
        coroutineEnded = true;
    }

    private bool Arrived()
    {
        if (!agentProperties.pathPending)
        {
            if (agentProperties.remainingDistance <= agentProperties.stoppingDistance)
            {
                if (!agentProperties.hasPath || agentProperties.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private IEnumerator WaypointsMovement()
    {
        GameObject start = waypoints[Random.Range(0, waypoints.Count)];
        GameObject end = waypoints[Random.Range(0, waypoints.Count)];

        this.transform.position = start.transform.position;
        agentProperties.isStopped = true;
        agentProperties.speed = monsterWaypointSpeed;
        yield return new WaitForSeconds(waitStartDuration);

        agentProperties.isStopped = false;
        agentProperties.SetDestination(end.transform.position);

        while (!Arrived()) { yield return null; }
        yield return new WaitForSeconds(waitEndDuration);
        coroutineEnded = true;
        yield break;
    }

    private IEnumerator ChaseMovement()
    {
        float chaseDuration = Random.Range(monsterChaseDurationMin, monsterChaseDurationMax);
        float elapsedTime = 0;
        agentProperties.speed = monsterChaseSpeed;

        while(elapsedTime < chaseDuration)
        {
            elapsedTime += Time.deltaTime;
            agentProperties.SetDestination(myCam.transform.position);
        }

        yield return new WaitForSeconds(chaseEndDuration);
        coroutineEnded = true;
        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        if (coroutineEnded)
        {
            coroutineEnded = false;
            float roll = Random.Range(0.0f, 1.0f);

            if(roll < wayPointProbability)
            {
                StartCoroutine(WaypointsMovement());
            }
            else
            {
                StartCoroutine(ChaseMovement());
            }
        }
    }

    private void OnValidate()
    {
        if (wayPointProbability + chaseProbability > 1.0f)
        {
            chaseProbability = 1 - wayPointProbability;
        }
    }
}
