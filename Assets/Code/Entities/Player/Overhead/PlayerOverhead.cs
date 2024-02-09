using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOverhead : MonoBehaviour
{
    #region Variables
    [Header("Define whether this is first player or not")]
    public bool is_player_one;

    public Vector2 MovementVector;

    #endregion

    public InputAction inputsys;

    void Start()
    {
        if(is_player_one)
        {

        }
        //inputsys.Enable();
    }

    void FixedUpdate()
    {
            //Debug.Log(inputsys.ReadValue<Vector2>() + " " + Time.deltaTime);
    }

    public void Movement(InputAction.CallbackContext value)
    {
        MovementVector = value.ReadValue<Vector2>();
        //Debug.Log(MovementVector);
    }



}
