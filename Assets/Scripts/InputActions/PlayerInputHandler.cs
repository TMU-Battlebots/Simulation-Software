using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction; //using static eliminates need to call type name everytime u wanna use its references (Math.Pow -> just Pow)


public class PlayerInputHandler : MonoBehaviour
{
    //this script converts input from InputActions into vectors for the Player script to use physics.
    //useful for the new input system becauyse that shit is dog ass
    private Player playerScript;
    [SerializeField] PlayerInputs playerInputs;
    private PlayerInput playerInput;

    private Vector2 boostVect;
    
    /*okay how this currently works:
     * In the pvp scene, there are 3 PLayerInput objects w this script and PlayerInput 'Input Action' component that corresponds to the 1 keyboard (index 0), and 2 Controllers (index 1, 2)
     * Each of these input action component has an index that will be referenced below on Start.
     * They will match the index of the PlayerInput component to the index of the player bots to pair
     * If you wanna use controllers v controllers, have index be 1 and 2 for the Player bots.
     * cuz index 0 is being mapped to keyboard, and also automatically mapped to AI.
     * If you wanna use 1 controller vs 1 keyboard, have index be 0 and 1 for the Player bots.
     * this is fucked i know.
     * 
     * */
    
    void Start()
    {
        /*alternative movement system where a new PlayerInput system is created everytime a new controller is used
         * , but since this call the input action directly, u cant seperate different input devices (?)
    
        playerScript = GetComponent<Player>();
        playerInputs = new PlayerInputs();
        playerInputs.Enable();
        if (playerInputs != null) { 
            playerScript.Boost2(playerInputs.Movement.Boost.ReadValue<float>());
        }
        */
        playerInput = GetComponent<PlayerInput>(); //get this playerInput component to access its playerIndex (automatically created)
        var players = FindObjectsOfType<Player>(); //get Player bots into an array. Each player script has a way to access its index
        var index = playerInput.playerIndex; //get playerInput index
        playerScript = players.FirstOrDefault(m => m.GetPlayerIndex() == index);
        /* FirstOrDefault: Returns the first element of the sequence that satisfies a condition or a default value if no such element is found
         * Condition being that the index of the Player script matches the index of the PlayerInput
         * If match, instantiate that script privately, so whenever we use playerScript it is directly in reference to a specific bot with a specific index.
         */
    }
    /*this method is called by the Player Input component on the Player prefab. 
     *Basically used to listen to the Input Action bindings of Event Players
     *there's a bug where it wasnt registering Input Actions from KB if controller is set up
     *Fix was idk, add a keyboard scheme but dont use it??? remove and readd keyboard support in input settings??
     *
     */
    public void OnMove(InputValue context) 
    {
        if (playerScript != null) //check this if not null point ref idk
        {
            playerScript.SetInputs(context.Get<Vector2>());
        }
        
    }

    /*Currently only registering on perform - the moment its pressed, not registering hold...
     * okay workaround fix is that somehow, Vector2 gets polled constantly, so i just made the button act like a vector2...
     */
    public void OnBoost(InputValue context)
    {
        if (playerScript != null)
        {
            boostVect = context.Get<Vector2>();
            playerScript.Boost2(boostVect.y);
            //Debug.Log("pluh!");
        }

        
    }
}
