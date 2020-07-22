using UnityEngine;
using System.Collections;

public class MirrorReflectionScript : MonoBehaviour
{
    private MirrorCameraScript childScript;
    private Camera mainCamera;
    private BoxCollider collider;

    [SerializeField]
    private LayerMask layerMask;

    private const int NUMBER_CHECKS = 15;
    private bool visible = false;
    private float timeElapsed = 0;
    private const float intervalLength = 1.0f;

    private void Start()
    {
        childScript = gameObject.transform.parent.gameObject.GetComponentInChildren<MirrorCameraScript>();
        mainCamera = Camera.main;
        collider = mainCamera.GetComponent<BoxCollider>();

        if (childScript == null)
        {
            Debug.LogError("Child script (MirrorCameraScript) should be in sibling object");
        }
    }

    private void RandomCheckIfVisible()
    {
        for (int i = 0; i < NUMBER_CHECKS; i++)
        {
            Vector3 randomPoint = new Vector3(
                Random.Range(collider.bounds.min.x, collider.bounds.max.x),
                Random.Range(collider.bounds.min.y, collider.bounds.max.y),
                Random.Range(collider.bounds.min.z, collider.bounds.max.z));

            Vector3 rayDirection = this.transform.position - randomPoint;
            RaycastHit hit;
            Ray ray = new Ray(mainCamera.transform.position, rayDirection);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.transform.gameObject.tag == this.tag)
                {
                    visible = true;
                    return;
                }
            }
        }

        visible = false;
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed > intervalLength)
        {
            timeElapsed = 0;
            RandomCheckIfVisible();
        }
    }

    private void OnWillRenderObject()
    {
        /*
        if (visible) { childScript.RenderMirror(); }
        else
        {
            Vector3 rayDirection = this.transform.position - mainCamera.transform.position;
            RaycastHit hit;

            if (Physics.Raycast(mainCamera.transform.position, rayDirection, out hit, layerMask))
            {
                if (hit.transform.gameObject.tag == this.tag)
                {
                    childScript.RenderMirror();
                }
            }
        }
        */

        Vector3 rayDirection = this.transform.position - mainCamera.transform.position;
        RaycastHit hit;
        Ray ray = new Ray(mainCamera.transform.position, rayDirection);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.transform.gameObject.tag == this.tag)
            {
                childScript.RenderMirror();
            } else
            {
                RandomCheckIfVisible();
                if(visible) { childScript.RenderMirror(); }
            }
        }
    }
}
