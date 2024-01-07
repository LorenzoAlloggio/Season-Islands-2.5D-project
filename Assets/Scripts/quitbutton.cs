using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quitbutton : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            Application.Quit();
            Debug.Log("Quit!");
        }
    }
}
