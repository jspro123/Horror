using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class MonsterNav : MonoBehaviour
{

    public Camera myCam;

    public enum MonsterState { Random, Chase, Stop };

    [Header("Monster Settings")]
    public float monsterWalkSpeed = 1.0f;
    public float monsterChaseSpeed = 4.0f;

    [Header("Float Settings")]
    public float maxHeight = 0.5f;
    public float minHeight = 0.0f;
    public float duration = 4.0f;

    private NavMeshAgent agentProperties;
    private MonsterState currentState = MonsterState.Random;

    private void Start()
    {
        agentProperties = GetComponent<NavMeshAgent>();
        this.transform.position = new Vector3(this.transform.position.x, minHeight, this.transform.position.z);
        Sequence s = DOTween.Sequence();
        s.Append(this.transform.DOMoveY(maxHeight, duration));
        s.Append(this.transform.DOMoveY(minHeight, duration));
        s.SetLoops(-1, LoopType.Restart);
    }

    public void SwitchState(MonsterState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case MonsterState.Random:
                agentProperties.speed = monsterWalkSpeed;
                break;

            case MonsterState.Chase:
                agentProperties.speed = monsterChaseSpeed;
                break;

            case MonsterState.Stop:
                agentProperties.speed = 0;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                agentProperties.SetDestination(hit.point);
            }
        }
        */

        agentProperties.SetDestination(myCam.transform.position);
    }
}
