using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;

public class CharacterBase : MonoBehaviour
{
    #region Variables

    [Header("Inspector Elements")]
    public PlayerOverhead player_overhead;
    public Rigidbody2D _rigidbody2D;
    public Collider2D hitbox;
    public Animator animator;
    public AnimationClip[] clips;
    public SpriteRenderer _spriterenderer;

    [Header("Physics")]
    public float drag;
    public float speed;
    public bool gettinghit;
    public Vector2 facing;
    public float weight;
    public Vector2 currentVelocity;


    [Header("Health Properties")]
    public float totalhealth;
    public float damage_taken;

    // public float evoCounter;s
    // public float evoLevel;

    [Header("Attack Logic")]
    public List<Attackdata> CharacterAttacks;
    public List<GameObject> Hit_Enemies;
    public float attack_time_total;
    public float attack_time_counter;
    public bool actionable;


    [Header("Hit Logic")]
    public float freezeframes;
    public float hitstun;


    //cooldown logic
    public delegate void CharacterCooldowns();
    public CharacterCooldowns cooldown_delegate = null;

    public enum actions_list
    {
        IDLE,
        WALKING,
        ATTACKING,
        GETTING_HIT,
        HITSTUN,
        OUT
    }

    public enum facing_direction
    {
        Front,
        Back,
        Left,
        Right,
        Tilted_Left_Front,
        Tilted_Right_Front,
        Tilted_Left_Back,
        Tilted_Right_Back,
    }

    public facing_direction facing_enum;

    public actions_list action;
    [System.Serializable]
    public struct Attackdata
    {
        public List<GameObject> objects;
        public float attack_length;
    }
    /*
        //this is a template
        public enum subactions_list
        {
            Idle
        }
        public subactions_list subaction;*/

    #endregion

    #region Defaults


    public virtual void FixedUpdate()
    {
        _rigidbody2D.velocity = (_rigidbody2D.velocity) * drag * Time.deltaTime;
    }

    #endregion

    #region Universal_Statements

    virtual public facing_direction GetDirection()
    {
        return facing_enum;
    }

    virtual public void SetDirection(facing_direction direction)
    {
        switch (direction)
        {
            //side view left
            case facing_direction.Left:
                animator.SetInteger("Turn", 1);
                _spriterenderer.flipX = false;
                facing_enum = facing_direction.Left;
                break;
            //side view right
            case facing_direction.Right:
                animator.SetInteger("Turn", 1);
                _spriterenderer.flipX = true;
                facing_enum = facing_direction.Right;
                break;
            //back view
            case facing_direction.Back:
                animator.SetInteger("Turn", -1);
                facing_enum = facing_direction.Back;
                break;
            //front view
            case facing_direction.Front:
                animator.SetInteger("Turn", 0);
                facing_enum = facing_direction.Front;
                break;
            default:
                return;
        }
    }

    virtual public void RotateCounterClockwise()
    {
        switch (facing_enum)
        {
            case facing_direction.Front:
                SetDirection(facing_direction.Right);
                break;
            case facing_direction.Back:
                SetDirection(facing_direction.Left);
                break;
            case facing_direction.Left:
                SetDirection(facing_direction.Front);
                break;
            case facing_direction.Right:
                SetDirection(facing_direction.Back);
                break;
        }
    }

    virtual public void RotateClockwise()
    {
        switch (facing_enum)
        {
            case facing_direction.Front:
                SetDirection(facing_direction.Left);
                break;
            case facing_direction.Back:
                SetDirection(facing_direction.Right);
                break;
            case facing_direction.Left:
                SetDirection(facing_direction.Back);
                break;
            case facing_direction.Right:
                SetDirection(facing_direction.Front);
                break;
        }
    }

    #endregion

    #region Hit_logic

    /// <summary>
    /// These two are called in the HitboxData class, which also sets the actions
    /// </summary>
    //when you hit something with an attack, the frames when you and your opponent are both frozen, for helping to sell the weight of the hit
    public void Freezeframes()
    {
        if (freezeframes <= 0)
        {
            action = actions_list.HITSTUN;
            gettinghit = true;
            _rigidbody2D.velocity = Vector2.zero;
            Hitstun();
            return;
        }
        else if (freezeframes > 0)
        {
            freezeframes--;
            gettinghit = false;
        }

        else
        {
            //this should theoretically never happen but its an edge case just in case
            freezeframes = 0;
        }
    }

    //when you're inactionable, to allow for potential combos or just to make combat feel more powerful
    public void Hitstun()
    {
        if (hitstun <= 0)
        {
            action = actions_list.IDLE;
            actionable = true;
            return;
        }
        else if (hitstun > 0)
        {
            _rigidbody2D.velocity = currentVelocity * .95f;
            hitstun--;
        }
        else
        {
            //same here
            hitstun = 0;
        }
    }

    public void Reset_Hit_List()
    {
        Hit_Enemies = new List<GameObject>();
    }
    #endregion

    #region Character_selection_logic
    virtual public void SetOut()
    {
        action = actions_list.OUT;
        _spriterenderer.enabled = false;
        hitbox.enabled = false;
        _rigidbody2D.velocity = Vector2.zero;
    }

    virtual public void SetIn()
    {
        action = actions_list.IDLE;
        _spriterenderer.enabled = true;
        hitbox.enabled = true;
        transform.rotation = Quaternion.identity;
        actionable = true;
        if (player_overhead != null)
        {
            Debug.Log("CHILD");
            player_overhead.target_for_enemies.transform.SetParent(transform);
        }
    }

    #endregion

    #region Cooldown_Management

    public void Cooldowns()
    {
        //cooldown_delegate();
        if(player_overhead.timer_for_swap <= player_overhead.swaptime)
        {
            player_overhead.timer_for_swap += 1;
            foreach (PlayerOverhead.CharacterUIGameObjects obj in player_overhead.Icon_Gameobjects)
            {
                obj.sliderimage.fillAmount = player_overhead.timer_for_swap / player_overhead.swaptime;
                Debug.Log(player_overhead.timer_for_swap + " " + player_overhead.swaptime);
            }
        }
    }



    #endregion

}
