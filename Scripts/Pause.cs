using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

    public GameObject FPSController;

    private void Start()
    {
        ResumeGame();
    }


    public void Quit()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        FPSController.GetComponent<FPSController>().enabled = false;

        for (int i = 0; i < transform.parent.childCount; i++)
        {
            transform.parent.GetChild(i).gameObject.SetActive(true);
        }
        gameObject.SetActive(false);
    }

    public void ResumeGame()
    {

        FPSController.GetComponent<FPSController>().enabled = true;

        for (int i = 0; i < transform.parent.childCount; i++)
        {
            transform.parent.GetChild(i).gameObject.SetActive(false);
        }
        gameObject.SetActive(true);
    }
}
