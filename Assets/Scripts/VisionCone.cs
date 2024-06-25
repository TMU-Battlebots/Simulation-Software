using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    //having trouble keeping up with detection when theres fast movement, 
    //might be due to having only 1 int to update 9 different states
    //maybe splitting it up will help
    //ok didnt rlly work or idk how to figure it out
    //need to find a better way to manage states
    //update: bunch of if states on an array :(
    //breakthrough while rethinking things.


    
    //HOW I MADE THE VISION CONES
    //1. Create child object with polygoncollider
    //2. Attach ChildCone script and assign an ID in inspector
    //3. Make sure coneState is updated to include a bool for it
    //HOW IT WORKS
    //Each child object and script connects to this VisionCone script
    //When they touch Target, it calls the functions here
    //function here updates the bool

    [SerializeField] BoxCollider2D Target;
    ChildCone childcorn;

    //CONE STATES: index of coneState array corresponds to a cone.
    //Note for later: maybe update to a List to make it easier to create new cones?
    [SerializeField]  public  bool[] coneState = { false, false, false, false, false, false, false, false, false, false };
    //These values are used to display which element in coneState is true;
    int firstTrueIndex;
    int lastTrueIndex;
    [SerializeField] int[] coneStateTrue = {0, 0};
    

    //Use this to get which cone is returning true
    //returns as an 2x1 array cuz theres potentially 2 cones that are
    //detecting target
    public int[] GetCone()
    {        
        firstTrueIndex = Array.IndexOf(coneState, true);
        lastTrueIndex = Array.LastIndexOf(coneState, true);
        coneStateTrue[0] = firstTrueIndex;
        coneStateTrue[1] = lastTrueIndex;
        return coneStateTrue;
    }

    public void SetConeStateT(int id)
    {
        coneState[id] = true;    
    }

    public void SetConeStateF(int id)
    {
        coneState[id] = false;
    }

    public BoxCollider2D GetTarget()
    {
        return Target;
    }
    //press button to turn on and off all renders
    


}

//old cone size
//L1 pos 22.9, xy 11
//L2 pos 12.6, xy 6
