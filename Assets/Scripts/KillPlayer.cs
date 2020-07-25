using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    public Camera newCam;
    private float elapsed = 0;
    
    void Update()
    {
        if (newCam)
        {
            elapsed += Time.deltaTime;
            if(elapsed > 10)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                newCam.gameObject.SetActive(true);
                Destroy(this.gameObject);
            }
        }
    }

}
