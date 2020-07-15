using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class MonsterNav : MonoBehaviour
{

    public Camera myCam;

    public enum MonsterState { Random, Chase, Stop};

    [Header("Monster Settings")]
    public GameObject player;
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

        //Update agent settings
        switch (currentState)
        {
            case MonsterState.Random:
                agentProperties.speed = monsterWalkSpeed;
                break;

            case MonsterState.Stop:
                agentProperties.speed = 0;
                break;

            case MonsterState.Chase:
                agentProperties.speed = monsterChaseSpeed;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        agentProperties.SetDestination(player.transform.position);
    }
}
