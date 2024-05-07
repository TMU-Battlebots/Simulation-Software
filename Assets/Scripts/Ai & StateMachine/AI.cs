using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Tilemaps;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/*
 * The core of the Enemy AI, where all components are connected
 * 
 */
public class AI : MonoBehaviour
{
    //Us/AI
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float AImoveSpeed;
    [SerializeField] private float AImoveSpeedSlow;
    [SerializeField] private float AIinvestigateSpeed;
    [SerializeField] private float AImoveSpeedCenter;
    [SerializeField] private float turningSpeed; //7 //Also uysed for tracking
    [SerializeField] private float slowTrackSpeed;
    //[SerializeField] float AIturnSpeed;

    [SerializeField] private int stateName;

    //bool isHittingWall = false;


    //Starting position
    [SerializeField] private GameObject startPosition;

    //Target
    [SerializeField] private GameObject targetObj;
    Vector2 detectionVector;
    [SerializeField] private bool targetLocked;
    float lockOnSpeed;
    float lockOnSpeedSlow;

    bool idleBool = false;
    private Transform pingLocation;

    //LockOnCircle
    [SerializeField] private GameObject marker;
    [SerializeField] private GameObject notLockOn;

    //CenterTarget
    [SerializeField] private GameObject centerObj;
    private float timer;


    //Movement 
    Vector2 engineForceVector;
    [SerializeField] private float rotationModifier; //10
    
    [SerializeField] private float L1turnSpeed; //Speed at which is orients to move to center
    
    Quaternion q;
    VisionCone visionCone;
    int[] state;

    //for Health and Damage system
    [SerializeField] CombatSystem combatSystem;
    [SerializeField] GameObject explosion;
    bool aliveState = true;

    //Combat ("Multiplayer")
    [SerializeField] int index = 0; //unique id for each bot, 0 = ai, 1 = p1, 2 = p2
    private GameObject organizerObj;
    private GameOrganizer organizer;


    /*Variables for our state machine
    These are automatically implemented by C#, 
    Normally, this would be
    public StateMachine StateMachine{
        get { return  }
        set { this = that idk }
     */
    public StateMachine StateMachine { get; private set; }
    public SearchTargetState SearchTargetState { get; private set; }
    public FollowTargetState FollowTargetState { get; private set; }
    public OffensiveState OffensiveState { get; private set; }
    public RamAttkState RamAttkState { get; private set; }
    public IdleState IdleState { get; private set; }
/*
 * 
 * 
 */
    void Awake()
    {
        
        StateMachine = new StateMachine();
        //Create instance of new State classes
        IdleState = new IdleState(this, StateMachine);
        SearchTargetState = new SearchTargetState(this, StateMachine);
        FollowTargetState = new FollowTargetState(this, StateMachine);  
        OffensiveState = new OffensiveState(this, StateMachine);    
        RamAttkState = new RamAttkState(this, StateMachine);    

    }

    private void OnEnable()
    {
        //returns to starting position
        this.transform.position = startPosition.transform.position;
        marker.SetActive(false);
    }

    private void Start()
    {
        visionCone = this.GetComponent<VisionCone>();
        StateMachine.Initialize(SearchTargetState); //IMPORTANT

        organizerObj = GameObject.FindWithTag("Organizer");
        organizer = organizerObj.GetComponent<GameOrganizer>();
        organizer.UpdateScores(); //idk how to make organizer call this when a scene restarts (not onEnable somehow) so this'll do.


    }

    private void Update()
    {

        StateMachine.CurrentEnemyState.Update(); //IMPORTANT
        GetConeInt();
        if (combatSystem.DeathCheck() == true)
        {
            aliveState = false;
            Instantiate(explosion, this.transform);
            organizer.PlayerLoses(index);

            enabled = false;
            //Debug.Log("die");
        }


    }

    // In the future seperate update and fixed update
    void FixedUpdate()
    {
        StateMachine.CurrentEnemyState.PhysicsUpdate(); //IMPORTANT
        if (organizer.StopSignal() == true)
        {
            enabled = false;
        }
    }
    //returns int of the cone
    //first index better for ENTERING CONE
    //last index better for TRANSITIONING AND PRIORITIZING LATER CONES
    //i think adding some kind of FIFO stack system can help this by checking the most recently changed bool?

    //yea its using a duplicate method in VisionCone
    //rmb to combine once done tinkering w Austin



    public int GetConeInt()
    {
        state = visionCone.GetCone();
        return state[1];
    }

    public GameObject GetTargetObj()
    {
        return targetObj;
    }

    public int GetStateName()
    {
        return stateName;
    }

    public void SetCurrentStateName(int name)
    {
        stateName = name;
    }

 
    //some gnarly reuse of code below, but since the bot can trigger multiple L's, I cant think of any way to reduce it... then again im kinda ass
    public bool IsL1()
    {
        int l1 = GetConeInt();
        if (l1 >= 0 && l1 <= 3) //any of L1 sensors triggered
        {
            //Debug.Log("Isl1");
            return true;
        }
        else
        {
            return false;
        }
    }


    public bool IsL3()
    {
        int l3 = GetConeInt();
        if (l3 >= 8 && l3 <= 9) //any of L3+ sensors triggered
        {
            //Debug.Log("Isl3");
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsL4()
    {
        int l4 = GetConeInt();
        if (l4 == 9) //L4 sensors triggered
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsL2()
    {
        int l2 = GetConeInt();
        if (l2 >= 4 && l2 <= 7) //any of L2 sensors triggered
        {
            //Debug.Log("Isl2");
            return true;
        }
        else
        {
            return false;
        }
    }
    /*Used by other states to jump to idle state
     * If statement works as a toggle.
     * 
     */
    public bool IsIdle()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            idleBool = !idleBool;
            return idleBool;
        }
        return idleBool;
    }

    //Generic track target
    public void TrackTarget(float trackingSpeed)
    {
        Vector3 vectorToTarget = targetObj.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
        q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * trackingSpeed);
    }

    //Different Tracking speeds;
    public void TrackingTarget()
    {
        TrackTarget(turningSpeed);
    }

    public void TrackingTargetSlowly()
    {
        TrackTarget(slowTrackSpeed);
    }
    //Generic move forward function that  inputs a direction
    public void MoveForward()
    {
        engineForceVector = transform.up  * AImoveSpeed;
        rb.AddForce(engineForceVector, ForceMode2D.Impulse);
    }

    public void MoveSlowly()
    {
        engineForceVector = transform.up * AImoveSpeedSlow;
        rb.AddForce(engineForceVector, ForceMode2D.Impulse);
    }

    public void SetL1TurnSpeed(float s)
    {
        slowTrackSpeed = s;
    }

    //At center bot has best view with no dead zones
    //also easier to be the one rotating and tracking rather than
    //moving around
    public void MoveToCenter()
    {
        timer = 0f; 
        if (timer < 3)
        {
            
            timer += Time.deltaTime;
            TrackTarget(L1turnSpeed);
        }
        
        float distance = Vector2.Distance(centerObj.transform.position, this.transform.position);
        if (distance >= 2f)
        {
            rb.AddForce(transform.up * AImoveSpeedCenter, ForceMode2D.Impulse);
        }
    }


    /*place where the marker is to investigate in relation to where our bot is.
    but we don't want to constantly update where this marker is
    So, update when the marker is reached, or L2 is triggered (state changes)
    (hopefully L2 quits
    */
    public void PlaceMarker() 
    {
        if (marker.activeSelf == false) //can only place marker when marketSet is false. marker is false only when reached marker
        {
            //Debug.Log("placingMarker");
            marker.SetActive(true);
            int i = GetConeInt();
            switch (i)
            {
                case 0: //front
                    pingLocation = this.gameObject.transform.GetChild(0);
                    marker.transform.position = pingLocation.transform.position;
                   
                    marker.SetActive(true);
                    break;
                case 1: //left
                    pingLocation = this.gameObject.transform.GetChild(1);
                    marker.transform.position = pingLocation.transform.position;

                    marker.SetActive(true);
                    break;
                case 2: //back
                    pingLocation = this.gameObject.transform.GetChild(2);
                    marker.transform.position = pingLocation.transform.position;

                    marker.SetActive(true);
                    break;
                case 3: //right
                    pingLocation = this.gameObject.transform.GetChild(3);
                    marker.transform.position = pingLocation.transform.position;

                    marker.SetActive(true);
                    break;

            }
        }      
    }

    //Generic Getter Setters for Marker state.
    //Toggles the gameObject Marker state when SearchTargetState jumps to Other
    public void SetMarker(bool b)
    {
        marker.SetActive (b);
    }
    public bool GetMarker()
    {
        return marker.activeSelf;   
    }

    /*Uses a short x to give bot some turning time.
     * 
     */
    public void InvestigateMarker()
    {
 
            timer = 0f;
            if (timer < 1)
            {

                timer += Time.deltaTime;
                Vector3 vectorToTarget = marker.transform.position - transform.position;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
                q = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * L1turnSpeed);
            }

            float distance = Vector2.Distance(marker.transform.position, this.transform.position);
            if (distance >= 5f)
            {
                rb.AddForce(transform.up * AIinvestigateSpeed, ForceMode2D.Impulse);
        }
        else if(distance < 5f) //when uve reached target
        {
            //Debug.Log("reached");
            marker.SetActive(false);
        }


        
    }

    public bool GetState()
    {
        return aliveState;
    }

    /// <summary>
    /// BEYOND LIES DOG SHIT
    /// </summary>
    /// <returns></returns>
    /*
    public float GetTimerFast()
    {
        return lockOnSpeed;
    }

    public float GetTimerSlow() {  return lockOnSpeedSlow; }

    public void StartTargetLock(float countdownTime)
    {
        if (GetLock() == false)
        {
            StartCoroutine(CountdownCoroutine(countdownTime));
        }
       
    }

    // Coroutine for the countdown
    private IEnumerator CountdownCoroutine(float countdownTime)
    {
        Debug.Log("Countdown started!");

        // Wait for the specified time
        yield return new WaitForSeconds(countdownTime);


        TrackingTarget();
        // Code to execute after the countdown
        //SetLock(true);
        //DrawTarget();
    }

    public void SetLock(bool idleBool)
    {
        targetLocked = idleBool;
    }

    public bool GetLock()
    {
        return targetLocked;
    }

    void DrawTarget()
    {
        if (targetLocked == true)
        {
            marker.transform.position = targetObj.transform.position;
        }
        else
        {
            marker.transform.position = notLockOn.transform.position;
        }
        
    }



    /// <summary>
    /// Not used
    /// </summary>
    /// <param name="obj"></param>
    public void MoveTo(GameObject obj)
    {
        x = 0f;
        if (x < 5)
        {
            x += Time.deltaTime;
            Turning(obj);
            rb.AddForce(transform.up * 40, ForceMode2D.Impulse);
        }
    }

    void Turning(GameObject obj )
    {
        Vector3 vectorToTarget = obj.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
        q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * L1turnSpeed);
    }

    */









}
