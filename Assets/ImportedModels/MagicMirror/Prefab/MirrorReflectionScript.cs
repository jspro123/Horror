using UnityEngine;
using System.Collections;

public class MirrorReflectionScript : MonoBehaviour
{
    private MirrorCameraScript childScript;
    private Camera mainCamera;

    private void Start()
    {
        childScript = gameObject.transform.parent.gameObject.GetComponentInChildren<MirrorCameraScript>();
        mainCamera = Camera.main;

        if (childScript == null)
        {
            Debug.LogError("Child script (MirrorCameraScript) should be in sibling object");
        }
    }

    private void OnWillRenderObject()
    {
        Vector3 vec = mainCamera.WorldToViewportPoint(this.transform.position);
        RaycastHit hit;
        Vector3 rayDirection = this.transform.position - mainCamera.transform.position;
        if (Physics.Raycast(mainCamera.transform.position, rayDirection, out hit))
        {
            if (hit.transform.gameObject.tag == this.tag)
            {
                childScript.RenderMirror();
            }
        }
    }
}
