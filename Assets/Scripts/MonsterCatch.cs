using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterCatch : MonoBehaviour
{

    [SerializeField]
    private MonsterManager monsterNav;
    [SerializeField]
    private GameObject player;

    private int playerLayer;
    private bool inCollider = false;

    private void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject collidedWith = other.gameObject;
        if (collidedWith.layer == playerLayer)
        {
            inCollider = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject collidedWith = other.gameObject;
        if (collidedWith.layer == playerLayer)
        {
            inCollider = false;
        }
    }

    private void Update()
    {
        if (inCollider)
        {
            RaycastHit hit;
            Vector3 rayDirection = player.transform.position - monsterNav.transform.position;
            if (Physics.Raycast(monsterNav.transform.position, rayDirection, out hit))
            {
                if (hit.transform.gameObject.layer == playerLayer)
                {
                    SceneManager.LoadScene("CorridorMonster");
                }
                else
                {
                    //Obstacle in the way
                }
            }
        }
    }

}
