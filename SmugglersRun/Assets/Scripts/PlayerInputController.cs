using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerInputController : MonoBehaviour, PlayerControlls.IPlayerMovementActions
{
    private PlayerControlls.PlayerMovementActions actions;
    //refrence to the action map we are implementing
    private Rigidbody _rigidBody;
    //refrence to the action map we are implementing
    private Animator _animator;
    //refrence to the action map we are implementing


    [SerializeField] private float _speedModifier = 3f;
    //modify how much the player inputs are being multiplied by
    [SerializeField] private int[] vertInputHistory = { 0, 0 };
    //input history for the prox sensors
    [SerializeField] int vertInputIndex = 0;
    //index for input tracking
    private int prox1 = 2;
    //top sensor data value, set for initialization
    private int prox2 = 2;
    //bottom sensor data value, set for initialization

    //Player movement Limits
    private float _xMin = -8.5f;
    private float _xMax = 3.5f;
    private float _yMin = -5.0f;
    private float _yMax = 7.5f;

    //Booleans to track if directional inputs are detected
    private bool _rightPressed;
    private bool _leftPressed;


    private float _verticalForce;
    //strength of vertical direction influence
    private float _horizontalForce;
    //strength of horizontal direction influence

    //animator values
    private float _horizMovement;
    private float _vertMovement;
    private float _dampTime;


    // Start is called before the first frame update
    void Start()
    {
        //Get a refrence to our rigidbody
        _rigidBody = GetComponent<Rigidbody>();
        //Get a refrence to our rigidbody
        _animator = GetComponent<Animator>();
        //Get a reference to our action map
        actions = new PlayerControlls().PlayerMovement;
        //Enable our actions
        actions.Enable();
        //Set callbacks to those actions (automatically handled since we derive from IWristMenuActions)
        actions.SetCallbacks(this);

    }

    public void FixedUpdate()
    {
        //Store user input as a movement vector
        Vector3 m_Input = new Vector3(_horizontalForce * _speedModifier, _verticalForce * _speedModifier, transform.position.z);

        //Apply the clamped movement vector to the current position, which is
        //multiplied by deltaTime and speed for a smooth MovePosition
        _rigidBody.MovePosition(Clamp(transform.position + m_Input * Time.deltaTime));

        //set animator values to match movement values
        _animator.SetFloat("HorizMovement", _horizMovement, _dampTime, Time.fixedDeltaTime);
        _animator.SetFloat("VertMovement", _vertMovement, _dampTime, Time.fixedDeltaTime);

        //slowly reset vertical momentum
        if (_verticalForce > -2) { _verticalForce -= 0.2f; }
        if (_verticalForce < -2) { _verticalForce += 0.2f; }


    }

    //on right input in action map
    public void OnRight(InputAction.CallbackContext context)
    {
        //when the key is pressed
        if (context.performed)
        {
            //set the key as pressed
            _rightPressed = true;
            //check if the other direction is pressed
            if (_leftPressed)
            {
                //if it is, zero out the movement
                Neutral();
                return;
            }
            //if not, move desired direction
            MoveRight();

        }
        //when the key is released
        else if (context.canceled)
        {
            //set the key as released
            _rightPressed = false;
            //check if the other direction is pressed
            if (_leftPressed)
            {
                //if it is, move desired direction
                MoveLeft();
                return;
            }
            //if not, zero out the movement
            Neutral();
        }

    }

    //on left input in action map
    public void OnLeft(InputAction.CallbackContext context)
    {
        //when the key is pressed
        if (context.performed)
        {
            //set the key as pressed
            _leftPressed = true;
            //check if the other direction is pressed
            if (_rightPressed)
            {
                //if it is, zero out the movement
                Neutral();
                return;
            }
            //if not, move desired direction
            MoveLeft();
        }
        //when the key is released
        else if (context.canceled)
        {
            //set the key as released
            _leftPressed = false;
            //check if the other direction is pressed
            if (_rightPressed)
            {
                //if it is, move desired direction
                MoveRight();
                return;
            }
            //if not, zero out the movement
            Neutral();
        }
    }

    //on up input in action map
    public void OnUp(InputAction.CallbackContext context)
    {
        //when the scroll is detected
        if (context.performed)
        {
            //if scroll is positive
            if (context.ReadValue<float>() > 0)
            {
                //add vertical force up, and set animation values for up
                _verticalForce = 3;
                _vertMovement = 8;
                _dampTime = 0.01f;
            }
            //if scroll is negitive
            else if (context.ReadValue<float>() < 0)
            {
                //add vertical force down, and set animation values for down
                _verticalForce = -3;
                _vertMovement = -8;
                _dampTime = 0.01f;
            }
        }
        //when the scroll stops
        else if (context.canceled)
        {
            _vertMovement = 0;
            _dampTime = 0.2f;
        }
    }

    //move left functionality
    private void MoveLeft()
    {
        _horizontalForce = -2;
        _horizMovement = -2;
        _dampTime = 0.2f;
    }
    //move right functionality
    private void MoveRight()
    {
        _horizontalForce = 2;
        _horizMovement = 2;
        _dampTime = 0.2f;
    }
    //neutral functionality
    private void Neutral()
    {
        _horizontalForce = 0;
        _horizMovement = 0;
        _dampTime = 0.05f;
    }

    private Vector3 Clamp(Vector3 value)
    {
        //Clamps the player movement between boundries so the player cannot fall out of the tunnel
        value = new Vector3(Mathf.Clamp(value.x, _xMin, _xMax), Mathf.Clamp(value.y, _yMin, _yMax), transform.position.z);
        return value;
    }


    void OnMessageArrived(string msg)
    {
        string[] datas = msg.Split(",");
        //splits data between ,
        
        int leftVal = int.Parse(datas[0]);
        int rightVal = int.Parse(datas[1]);

        int temp1 = int.Parse(datas[2]);
        //top prox sensor temp for comparison
        int temp2 = int.Parse(datas[3]);
        //bottom prox sensor temp for comparison

        ArduinoLeft(leftVal);
        ArduinoRight(rightVal);

        if (prox1 != temp1)
        {
            prox1 = temp1;
            vertInputHistory[vertInputIndex % 2] = 1;
            vertInputIndex = (vertInputIndex % 2) + 1;
            ArduinoUp();
        }
        //if the input is different than the last value
        if (prox2 != temp2)
        {
            prox2 = temp2;
            vertInputHistory[vertInputIndex % 2] = 2;
            vertInputIndex = (vertInputIndex % 2) + 1;
            ArduinoUp();
        }
        

    }

    //on right input in action map
    public void ArduinoRight(int inputValue)
    {
        //when the key is pressed
        if (inputValue > 500)
        {
            //set the key as pressed
            _rightPressed = true;
            //check if the other direction is pressed
            if (_leftPressed)
            {
                //if it is, zero out the movement
                Neutral();
                return;
            }
            //if not, move desired direction
            MoveRight();

        }
        //when the key is released
        else if (inputValue < 500)
        {
            //set the key as released
            _rightPressed = false;
            //check if the other direction is pressed
            if (_leftPressed)
            {
                //if it is, move desired direction
                MoveLeft();
                return;
            }
            //if not, zero out the movement
            Neutral();
        }

    }

    //on left input in action map
    public void ArduinoLeft(int inputValue)
    {
        //when the key is pressed
        if (inputValue > 500)
        {
            //set the key as pressed
            _leftPressed = true;
            //check if the other direction is pressed
            if (_rightPressed)
            {
                //if it is, zero out the movement
                Neutral();
                return;
            }
            //if not, move desired direction
            MoveLeft();
        }
        //when the key is released
        else if (inputValue < 500)
        {
            //set the key as released
            _leftPressed = false;
            //check if the other direction is pressed
            if (_rightPressed)
            {
                //if it is, move desired direction
                MoveRight();
                return;
            }
            //if not, zero out the movement
            Neutral();
        }
    }

    //on up input in action map
    public void ArduinoUp()
    {
        //if scroll is positive
        if (vertInputHistory[(vertInputIndex - 1) % 2] > vertInputHistory[vertInputIndex % 2])
        {
            //add vertical force up, and set animation values for up
            _verticalForce = 4;
            _vertMovement = 1;
            _dampTime = 0.5f;
        }
        //if scroll is negitive
        else if (vertInputHistory[(vertInputIndex - 1) % 2]  < vertInputHistory[vertInputIndex % 2])
        {
            //add vertical force down, and set animation values for down
            _verticalForce = -4;
            _vertMovement = -1;
            _dampTime = 0.5f;
        }
        //if scroll is equal
        else
        {
            return;
        }
    }
}