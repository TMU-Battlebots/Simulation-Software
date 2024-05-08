using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOrganizer : MonoBehaviour
{

    GameObject scoreBoard1; //very shitty way of doing this, i can iterate thru a loop to find all child objects...
    GameObject scoreBoard2;
    GameObject scoreBoardAI;

    TextMeshProUGUI p1lives;
    TextMeshProUGUI p2lives;
    TextMeshProUGUI aIlives;


    [SerializeField] int player1lives = 3;
    [SerializeField] int player2lives = 3;
    [SerializeField] int playerAIlives = 3;

    [SerializeField] GameObject P1WINS; //oh as long as the gameobejcts are prefab n u set them in the bluye prefab screen it works!!
    [SerializeField] GameObject P2WINS;
    [SerializeField] GameObject AIWINS;


    int winner;
    bool pleaseStop = false;
    
    [SerializeField] bool PVP; //Set this flag to true for PVP, false for PVE - determines winning check
    //probably a better way to do this

    /*this is getting horrible holy...
    idea is to have a prefab game object script combo i can use for both pve and pvp
    once a player wins, its object will call this script, which will do stuff
    stuff to do: 
    update score (best of 3)
    restart scene if noone won yet while still keeping scores
    holds our pause/restart button aswell?
    */

    public static GameOrganizer instance; //other scripts can access this unique script/object


    private void Awake()
    {
        if (instance != null) //makes sure theres no duplicate gameobjects
        {
            Destroy(gameObject);
            return;
        }

        instance = this; //other scripts can access this unique script/object
        DontDestroyOnLoad(gameObject); //so this object n its script persists
    }

    private void OnEnable()
    {
        //Debug.Log("OnEnableCalled");
        UpdateScores(); //this doesnt really work so I have to call it by other objects
    }

    private void Update()
    {
        //temporary sols
        if (Input.GetKey(KeyCode.L)) {
            SetLives();
            RestartScene();
        }
        if (Input.GetKey(KeyCode.O))
        {
            SetLives();
            BackToMenuScene();
        }
    }

    public void SetLives()
    {
        player1lives = 3;
        player2lives = 3;
        playerAIlives = 3;
    }

    public void SetPVPMode(bool mode)
    {
        if (mode == true)
        {
            PVP = true;
        }
        else if (mode == false) 
        {
            PVP = false;
        }
    }



    public void PlayerLoses(int index) //Accessed by Core scripts to update UI and live count.
    {
        pleaseStop = true;
        switch (index)
        {
            case 0: //ai
                playerAIlives -= 1;

                aIlives.text = playerAIlives.ToString();

                break;
            case 1: //player 1
                player1lives -= 1;

                p1lives.text = player1lives.ToString();

                break;
            case 2: //player 2
                player2lives -= 1;

                p2lives.text = player2lives.ToString();

                break;
        }

        CheckGame();

    }


    private void CheckGame(){

        if (playerAIlives == 0 || player1lives == 0 || player2lives == 0) //if someone has lost, 
        {
            Debug.Log("Winner Found");
            if (PVP == true) //determine who won. Have 2 seperate methods whether its pvp or pve. 
            {
                winner = PVPWinner();
            }
            else if (PVP == false)
            {

                Debug.Log("PVE");
                winner = PVEWinner();
            }

            DisplayWinningBanner(winner); //and put that bot on top!!!!!!!!!!!

            Invoke(nameof(BackToMenuScene), 2.9f);
            
        }
        //if not
        Invoke(nameof(RestartScene), 3f); // Adjust the delay time as needed

    }

    /*Absolutely horrible find with tag abuse. needa learn how to traverse child objects soon.
 * that drag n drop on inspector thing doesnt work for persistent objects when a new scene loads, so u must get components via code with this.
 */
    public void UpdateScores()
    {
        //Debug.Log("InsideOnEnableCalled"); 
        try
        {
            scoreBoardAI = GameObject.FindWithTag("ScoringAI");
            aIlives = scoreBoardAI.GetComponent<TextMeshProUGUI>();

            scoreBoard1 = GameObject.FindWithTag("Scoring1");
            p1lives = scoreBoard1.GetComponent<TextMeshProUGUI>();

            scoreBoard2 = GameObject.FindWithTag("Scoring2");
            p2lives = scoreBoard2.GetComponent<TextMeshProUGUI>();

            aIlives.text = playerAIlives.ToString();
            p1lives.text = player1lives.ToString();
            p2lives.text = player2lives.ToString();
        }
        catch (System.Exception ex)
        {
            // Code to handle the exception
            Debug.LogError("COULDNT FIND SCOREBOARDS PROLLY CUZ WE OUTSIDEE: " + ex.Message);
        }
        
    }

    private void RestartScene()
    {
        pleaseStop = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void BackToMenuScene()
    {

        pleaseStop = false;
        SceneManager.LoadScene(0); //index 0 must always be main manu
    }


    /*I realize i dont need have 2 seperate methods and a check i can just have one big checker for all 3 ints lmao but i got dooky bra
     * in rn from that cps test
     * 
     */
    public int PVPWinner() //use to determine index of winning player
    {
       if (player1lives > player2lives)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    public int PVEWinner()
    {
        if (player1lives > playerAIlives)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    private void DisplayWinningBanner(int index)
    {
        Debug.Log("DisplayWinningBanner" + index);
        // Get the screen dimensions
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Calculate the middle position
        Vector3 middlePosition = Camera.main.ScreenToWorldPoint(new Vector3(screenWidth / 2f, screenHeight / 2f, Camera.main.nearClipPlane));


        switch (index)
        {
            case 0: //ai wins
                Instantiate(AIWINS, middlePosition, Quaternion.identity);
                Debug.Log("ai wins!!");
                
                break;

            case 1: //Player 1 wins
                Instantiate(P1WINS, middlePosition, Quaternion.identity);
                Debug.Log("p1 wins");
                break;

            case 2: //Player 2 wins
                Instantiate(P2WINS, middlePosition, Quaternion.identity);
                Debug.Log("p2 wins");
                break;

            default:
                Debug.Log("how");
                break;
        }
            

    }
    //called by other players so they can stop do damage and cheat
    //really shitty ngl but its 3:38am
    public bool StopSignal() 
    {
        if (pleaseStop == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }



}
