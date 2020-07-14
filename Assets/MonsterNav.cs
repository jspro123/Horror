using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class MonsterNav : MonoBehaviour
{

    public Camera myCam;
    public NavMeshAgent agent;

    [Header("Float Settings")]
    public float maxHeight = 0.5f;
    public float minHeight = 0.0f;
    public float duration = 4.0f;

    private void Start()
    {
        this.transform.position = new Vector3(this.transform.position.x, minHeight, this.transform.position.z);
        Sequence s = DOTween.Sequence();
        s.Append(this.transform.DOMoveY(maxHeight, duration));
        s.Append(this.transform.DOMoveY(minHeight, duration));
        s.SetLoops(-1, LoopType.Restart);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
    }
}
