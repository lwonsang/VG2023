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
    public SpriteRenderer _spriterenderer;

    [Header("Physics")]
    public float drag;
    public float speed;
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

    public actions_list action;
    [System.Serializable]
    public struct Attackdata
    {
        public List<GameObject> objects;
        public float attack_length;
    }
    public enum subactions_list
    {
        Idle,
        Claw_Swipe
    }
    public subactions_list subaction;

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
            Hitstun();
            return;
        }
        else if (freezeframes > 0)
            freezeframes--;
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
            freezeframes--;
        else
        {
            //same here
            hitstun = 0;
        }
    }
    #endregion

}
