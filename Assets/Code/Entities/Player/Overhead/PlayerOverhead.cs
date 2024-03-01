using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerOverhead : MonoBehaviour
{
    #region Variables
    [Header("Define whether this is first player or not")]
    public bool is_player_one;
    public bool pressAttack;
    public bool characterswappress;
    public bool ability1press;
    public bool ability2press;
    public bool ability3press;

    public float swaptime;
    [HideInInspector] public float timer_for_swap = 0;

    public Vector2 MovementVector;
    //I will change this later to be called differently

    public List<GameObject> Characters;
    public CharacterBase activecharacter;
    public CinemachineTargetGroup targetgroup;
    public GameObject target_for_enemies;

    [System.Serializable]
    public struct CharacterUIGameObjects
    {
        public Image icon_image;
        public Image icon_background;
        public Image sliderimage;
    }
    public List<CharacterUIGameObjects> Icon_Gameobjects;
    public struct CharacterUIData
    {
        public Sprite icon;
        public Color background_color;
    }
    public List<CharacterUIData> Character_UIData = new List<CharacterUIData>();

    #endregion

    public void Start()
    {
        //sets the first character as active
        foreach(GameObject character in Characters)
        {
            character.gameObject.GetComponent<CharacterBase>().SetOut();
            Debug.Log(character);
        }

        for(int i = 0; i < Character_UIData.Count; i++)
        {
            if (Icon_Gameobjects.Count < i)
                return;
            Debug.Log(i + " " + Character_UIData[i].background_color);
            if(Character_UIData[i].background_color != null)
                Icon_Gameobjects[i].icon_background.color = Character_UIData[i].background_color;
            if (Character_UIData[i].icon != null)
                Icon_Gameobjects[i].icon_image.sprite = Character_UIData[i].icon;
        }
        activecharacter = Characters[0].GetComponent<CharacterBase>();

        if(targetgroup == null)
        {
            targetgroup = FindAnyObjectByType<CinemachineTargetGroup>();
        }
        if(activecharacter != null)
        {
            ChooseCharacter(0);
            activecharacter.SetIn();
        }
    }

    #region Character_Functions

    public void ChooseCharacter(int number)
    {
        targetgroup.RemoveMember(activecharacter.transform);
        activecharacter.SetOut();
        Characters[number].transform.position = activecharacter.transform.position;
        Debug.Log(Characters[number]);
        activecharacter = Characters[number].GetComponent<CharacterBase>();
        activecharacter.SetIn();
        targetgroup.AddMember(activecharacter.transform, 1, 8);
        timer_for_swap = 0;
        return;
    }

    #endregion



    #region InputSystem
    public void Movement(InputAction.CallbackContext value)
    {
        MovementVector = value.ReadValue<Vector2>();
    }

    public void Attack(InputAction.CallbackContext value)
    {
        pressAttack = value.ReadValueAsButton();
        if (pressAttack)
        {
            activecharacter.action = CharacterBase.actions_list.ATTACKING;
        }
    }

    public void Character1(InputAction.CallbackContext value)
    {
        if (timer_for_swap < swaptime)
            return;
        characterswappress = value.ReadValueAsButton();
        if (!characterswappress)
        {
            return;
        }
        ChooseCharacter(0);
        return;
    }

    public void Character2(InputAction.CallbackContext value)
    {
        if (timer_for_swap < swaptime)
            return;
        characterswappress = value.ReadValueAsButton();
        if (!characterswappress)
        {
            return;
        }
        ChooseCharacter(1);
        Debug.Log(Characters.Count);
        return;
    }

    public void Ability1(InputAction.CallbackContext value)
    {
        ability1press = value.ReadValueAsButton();
    }

    public void Ability2(InputAction.CallbackContext value)
    {
        ability2press = value.ReadValueAsButton();
    }

    public void Ability3(InputAction.CallbackContext value)
    {
        ability3press = value.ReadValueAsButton();
    }


    #endregion


}
