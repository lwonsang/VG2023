using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charizard : CharacterBase
{
    void Awake()
    {
        player_overhead.Characters.Add(gameObject);
        Debug.Log(Quaternion.identity);
        Hit_Enemies = new List<GameObject>();
        damage_taken = 0;
        player_overhead = transform.parent.gameObject.GetComponent<PlayerOverhead>();
        if(player_overhead == null )
        {
            Destroy(gameObject);
            return;
        }
        action = actions_list.IDLE;
    }

    void FixedUpdate()
    {
        switch(action)
        {
            case actions_list.IDLE:
                Idle();
                break;
            case actions_list.ATTACKING:
                Attacking();
                break;
            case actions_list.WALKING:
                Walking();
                break;

            default: 
                break;
        }
    }

    public void Idle()
    {
        //idle as in idle animation
        action = actions_list.WALKING;
        subaction = subactions_list.Idle;
    }

    public void Attacking()
    {
        if(subaction == subactions_list.Claw_Swipe)
        {
            return;
        }
        subaction = subactions_list.Claw_Swipe;
        MyAttacks[0].SetActive(true);
        return;
    }

    public void Walking()
    {
        Vector2 newspeed = (_rigidbody2D.velocity + player_overhead.MovementVector * speed) * drag * Time.deltaTime;
        _rigidbody2D.velocity = newspeed;

        switch(player_overhead.MovementVector)
        {
            //side view left
            case Vector2 vector when vector.x < 0 && Mathf.Abs(vector.y) < .4f:
                animator.SetInteger("Turn", 1);
                _spriterenderer.flipX = false;
                break;
            //side view right
            case Vector2 vector when vector.x > 0 && Mathf.Abs(vector.y) < .4f:
                animator.SetInteger("Turn", 1);
                _spriterenderer.flipX = true;
                break;
            //back view
            case Vector2 vector when vector.y > 0:
                animator.SetInteger("Turn", -1);
                break;
            //front view
            case Vector2 vector when vector.y < 0:
                animator.SetInteger("Turn", 0);
                break;
            default:
                action = actions_list.IDLE;
                subaction = subactions_list.Idle;
                break;
        }

    }

}
