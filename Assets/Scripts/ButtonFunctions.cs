using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("Horror");
    }

    public void Quit()
    {
        Application.Quit();
    }

}
