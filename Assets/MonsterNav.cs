using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class MonsterNav : MonoBehaviour
{
    // public Camera myCam;
    public GameObject myCam;

    public enum MonsterState { Random, Chase, Stop};

    [Header("Monster Settings")]
    public float monsterWalkSpeed = 1.0f;
    public float monsterChaseSpeed = 4.0f;
    public float monsterCaptureThreshold = 1.0f;

    [Header("Float Settings")]
    public float maxHeight = 0.5f;
    public float minHeight = 0.0f;
    public float duration = 4.0f;

    private NavMeshAgent agentProperties;
    private MonsterState currentState = MonsterState.Random;

    public void FollowMove(float speed) {
        var monsterTransform = GetComponent<Transform>();
        var monsterPosition = monsterTransform.position;
        var targetPosition = myCam.transform.position;
        int dx = 0;
        int dz = 0; 
        if (targetPosition.x > monsterPosition.x) {
            dx = 1;
        } else if (targetPosition.x < monsterPosition.x) {
            dx = -1;
        }
        if (targetPosition.z > monsterPosition.z) {
            dz = 1;
        } else if (targetPosition.z < monsterPosition.z) {
            dz = -1;
        }
        var newPosition = new Vector3(monsterPosition.x + dx*speed, monsterPosition.y, monsterPosition.z + dz*speed);
        monsterTransform.DOMove(newPosition, duration);
    }

    private void Start()
    {
        // agentProperties = GetComponent<NavMeshAgent>();
        // var trans = GetComponent<Transform>();
        // this.transform.position = new Vector3(this.transform.position.x, minHeight, this.transform.position.z);
        // monster.transform.DOMoveX(trans.transform.position.x +5, 5);
        // Sequence s = DOTween.Sequence();
        // s.Append(this.transform.DOMoveY(340.06, 2));
        // s.Append(this.transform.DOMoveY(minHeight, duration));
        // s.SetLoops(-1, LoopType.Restart);
        FollowMove(monsterWalkSpeed);
    }

    public void SwitchState(MonsterState newState)
    {
        currentState = newState;
    }

    // Update is called once per frame
    void Update()
    {
        FollowMove(monsterWalkSpeed);
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
    }
}
