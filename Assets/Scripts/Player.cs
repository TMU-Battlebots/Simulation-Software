using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public class Player : MonoBehaviour
{


    [SerializeField] Rigidbody2D rb;
    [SerializeField] float movementSpeed;
    [SerializeField] float boostSpeed;
    float currentSpeed;
    [SerializeField] float turnSpeed;

    [SerializeField] Vector2 inputVector;

    //Enemy Bot
    bool toggleEnemy = true;
    [SerializeField] GameObject enemy;


    //vars
    float accelInputs = 0;
    float steerInputs = 0;
    float rotationAngle = 0;

    // Start is called before the first frame update c
    void Start()
    {
        // rb = new Rigidbody2D();
        currentSpeed = movementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        inputVector.x = Input.GetAxisRaw("Horizontal");
        inputVector.y = Input.GetAxisRaw("Vertical");
        SetInputs(inputVector);
        OnorOff();

        QuitGame();

    }

    void FixedUpdate()
    {
        AccelSystem();
        SteerSystem();
        Boost();
    }

    void AccelSystem()
    {
        //transform.up = fwd //accel and steer inputs is 0/1 boolean dictating when a force is applied
        Vector2 engineForceVector = transform.up * accelInputs * currentSpeed;
        rb.AddForce(engineForceVector, ForceMode2D.Impulse);


        
    }
    void SteerSystem()
    {
      rotationAngle -= steerInputs * turnSpeed;
        rb.MoveRotation(rotationAngle);
    }

    void SetInputs(Vector2 inputVector)
    {
        steerInputs = inputVector.x;
        accelInputs = inputVector.y;
    }

    void OnorOff()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            toggleEnemy = !toggleEnemy;
            enemy.SetActive(toggleEnemy);

        }
    }

    void Boost()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = boostSpeed;
        }
        else
        {
            currentSpeed = movementSpeed; 
        }
    }

    void QuitGame()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Application.Quit();
            Debug.Log("Quitting game... ");
        }
    }


}
