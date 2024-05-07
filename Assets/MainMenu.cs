using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameObject organizerObj;
    private GameOrganizer organizer;

    private void Start()
    {
        organizerObj = GameObject.FindWithTag("Organizer");
        organizer = organizerObj.GetComponent<GameOrganizer>();
    }

    public void Play2PLayer()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        organizer.SetLives();
        organizer.SetPVPMode(true);


    }


    public void PlayAI()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        organizer.SetLives();
        organizer.SetPVPMode(false);

    }

}
