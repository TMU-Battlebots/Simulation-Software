using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Tilemaps;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
    private Transform meeple;

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
       
    }

    private void Update()
    {

        StateMachine.CurrentEnemyState.Update(); //IMPORTANT
        GetConeInt();

        
    }

    // In the future seperate update and fixed update
    void FixedUpdate()
    {
        StateMachine.CurrentEnemyState.PhysicsUpdate(); //IMPORTANT       
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
    /*place where the marker is to investigate.
    but we don't want to constantly update where this marker is
    Only update when the marker is reached, or L2 is triggered (state changes)
    (hopefully L2 quits
    */
    public void PlaceMarker() 
    {
        if (marker.activeSelf == false) //can only place marker when marketSet is false. market is false only when reached marker
        {
            marker.SetActive(true);
            int i = GetConeInt();
            switch (i)
            {
                case 0: //front
                    meeple = this.gameObject.transform.GetChild(0);
                    marker.transform.position = meeple.transform.position;
                   
                    marker.SetActive(true);
                    break;
                case 1: //left
                    meeple = this.gameObject.transform.GetChild(1);
                    marker.transform.position = meeple.transform.position;

                    marker.SetActive(true);
                    break;
                case 2: //back
                    meeple = this.gameObject.transform.GetChild(2);
                    marker.transform.position = meeple.transform.position;

                    marker.SetActive(true);
                    break;
                case 3: //right
                    meeple = this.gameObject.transform.GetChild(3);
                    marker.transform.position = meeple.transform.position;

                    marker.SetActive(true);
                    break;

            }
        }      
    }
    public void SetMarker(bool b)
    {
        marker.SetActive (b);
    }
    public bool GetMarker()
    {
        return marker.activeSelf;   
    }

    IEnumerator Countdown(float f)
    {

        yield return new WaitForSeconds(f);
        marker.SetActive(false);
    }

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
            Debug.Log("reached");
            marker.SetActive(false);
        }
        
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
        timer = 0f;
        if (timer < 5)
        {
            timer += Time.deltaTime;
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
