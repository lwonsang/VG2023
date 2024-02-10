using System;
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
    //I will change this later to be called differently
    public bool attacking;

    public List<GameObject> Characters;

    #endregion

    public void Start()
    {
        //sets the first character as active
        foreach(GameObject character in Characters)
        {
            character.gameObject.SetActive(false);
        }
        Characters[0].SetActive(true);
    }


    #region InputSystem
    public void Movement(InputAction.CallbackContext value)
    {
        MovementVector = value.ReadValue<Vector2>();
    }

    public void Attack(InputAction.CallbackContext value)
    {
        attacking = value.ReadValue<Boolean>();
    }

    #endregion


}
