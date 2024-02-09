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

    public enum actions_list {
        IDLE,
        WALKING,
        ATTACKING
    }

    public actions_list action;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
