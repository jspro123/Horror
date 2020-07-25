using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctions : MonoBehaviour
{
   

    public void Restart()
    {
        Debug.Log("Restart");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

}
