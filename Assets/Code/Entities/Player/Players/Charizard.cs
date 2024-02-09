using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charizard : CharacterBase
{
    void Start()
    {
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
                idle();
                break;
            case actions_list.ATTACKING:
                attacking();
                break;
            case actions_list.WALKING:
                walking();
                break;

            default: 
                break;
        }
    }

    public void idle()
    {
        //idle as in idle animation
        action = actions_list.WALKING;
    }

    public void attacking()
    {
        return;
    }

    public void walking()
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
                break;
        }

    }

}
