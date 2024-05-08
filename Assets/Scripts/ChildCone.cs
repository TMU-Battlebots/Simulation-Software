using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEditor;
using UnityEngine;

public class ChildCone : MonoBehaviour
{

    //This integer corresponds to the Cone name in the array.
    [SerializeField] int coneID;
    VisionCone ParentScript;
    SpriteRenderer rend;
    PolygonCollider2D col;
    bool b;
    [SerializeField] bool toggleOn;
    Color detectColor;
    Color normColor;


    //try to keep code here light since atleast 9 objects uses it. Put GetComponent in Start since it can be taxing.
    void Start()
    {
        //Getting VisionCone through transform.parent 
        ParentScript = this.transform.parent.GetComponent<VisionCone>();
        rend = this.GetComponent<SpriteRenderer>();
        //I prefer to use SerializeField (heard its faster too) but for automation i did one here
        col = this.GetComponent<PolygonCollider2D>();
        detectColor = new Color(1f, 0, 0.4f, 0.4f);
        normColor = new Color(0.3f, 1f, 1f, 0.3f);

    }
    void Update()
    {
        ChildConeCheck();
        ToggleRender();
    }

    void ChildConeCheck()
    {
        if (col.IsTouching(ParentScript.GetTarget()))
        {
            ParentScript.SetConeStateT(coneID);
            SetRender(true);
            rend.color = detectColor;

        }
        else
        {
            ParentScript.SetConeStateF(coneID);
            if (toggleOn == false)
            {
                SetRender(false);
            }
            rend.color = normColor;


        }
    }

    void SetRender(bool render)
    {
        this.rend.enabled = render;
    }


    void ToggleRender()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            b = !b;
            toggleOn = !toggleOn;
            this.rend.enabled = b;
        }
    }
}
