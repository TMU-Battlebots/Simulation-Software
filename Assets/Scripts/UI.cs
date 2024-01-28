using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI textCone;
    [SerializeField] TextMeshProUGUI textState;
    [SerializeField] TextMeshProUGUI textStateDesc;
    [SerializeField] GameObject AI;

    [SerializeField]
    String[] ConeNameID = new String[9];
    [SerializeField]
    String[] StateNameID = new String[4];
    [SerializeField]
    String[] StateDescID = new String[4];




    VisionCone ParentScript;
    AI AIParentScript;

    int[] cones;
    int stateIndex;

    void Start()
    {
        ParentScript = AI.GetComponent<VisionCone>();
        AIParentScript = AI.GetComponent<AI>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
        UpdateCone();

    }

    void UpdateCone()
    {
        cones = ParentScript.GetCone();
        switch (cones[1])
        {
            case 0:
                textCone.text = ConeNameID[0];
                break;

            case 1:
                textCone.text = ConeNameID[1];
                break;

            case 2:
                textCone.text = ConeNameID[2];
                break;

            case 3:
                textCone.text = ConeNameID[3];
                break;
            case 4:
                textCone.text = ConeNameID[4];
                break;

            case 5:         
                textCone.text = ConeNameID[5];
                break;

            case 6:
                textCone.text = ConeNameID[6];
                break;

            case 7:
                textCone.text = ConeNameID[7];
                break;

            case 8:
                textCone.text = ConeNameID[8];
                break;

            case 9:
                textCone.text = ConeNameID[9];
                break;

            default:
                textCone.text = "No bot detected";
                break;
        }
    }

    void UpdateState()
    {
        stateIndex = AIParentScript.GetStateName();
        switch (stateIndex)
        {
            case 0:
                textState.text = StateNameID[0];
                textStateDesc.text = StateDescID[0];
                break;

            case 1:
                textState.text = StateNameID[1];
                textStateDesc.text = StateDescID[1];
                break;

            case 2:
                textState.text = StateNameID[2];
                textStateDesc.text = StateDescID[2];
                break;

            case 3:
                textState.text = StateNameID[3];
                textStateDesc.text = StateDescID[3];
                break;
        }
    }
}

 