using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

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

    [Header("Health Properties")]
    public float totalhealth;
    public float damage_taken;

    [Header("Attack Logic")]
    public List<Attackdata> CharacterAttacks;
    public List<GameObject> Hit_Enemies;
    public float attack_time_total;
    public float attack_time_counter;
    public bool actionable;


    [Header("Hit Logic")]
    public float freezeframes;
    public float hitstun;

    public enum actions_list {
        IDLE,
        WALKING,
        ATTACKING,
        GETTING_HIT,
        HITSTUN
    }

    public enum facing_direction
    {
        Front,
        Back, 
        Left,
        Right
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

    public facing_direction GetDirection()
    {
        return facing_enum;
    }

    public void SetDirection(facing_direction direction)
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
    
    public void RotateCounterClockwise()
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

    public void RotateClockwise()
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
            _rigidbody2D.velocity = (_rigidbody2D.velocity) * drag * Time.deltaTime;
            freezeframes--;
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

}
