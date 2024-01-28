using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*there was an issue where the bot can get stuck on walls  
 * - driiving forward but not spinning enough due to friction with wall
 * So i made the walls very slippery :) aand it works well 
 * In the future I need to do some kind of reverse turn State when hitting a wall;
 * Should be easy
 * 
 * 
 */
public class WallCheck : MonoBehaviour //Check if L4 is touching a wall
{

    AI ParentScript;
    CircleCollider2D col;


    // Start is called before the first frame update
    void Start()
    {
        //ParentScript = this.transform.parent.GetComponent<AI>();
        //col = this.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }


    
}
