using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
            attack_time_counter++;
            switch(attack_time_counter)
            {
                case < 5:
                    break;
                case 7:
                    //_rigidbody2D.velocity = (_rigidbody2D.velocity * speed) * drag * Time.deltaTime + attackdirection * 10;
                    break;

            }
            if(attack_time_counter > attack_time_total)
            {
                action = actions_list.IDLE;
                subaction = subactions_list.Idle;
                foreach (GameObject obj in CharacterAttacks[0].objects)
                {
                    obj.SetActive(false);
                    obj.transform.rotation = Quaternion.identity;
                }
                return;
            }
        }
        else
        {
            subaction = subactions_list.Claw_Swipe;
            print(MathF.Atan2(facing.y, facing.x));
            foreach (GameObject obj in CharacterAttacks[0].objects)
            {
                obj.transform.LeanRotateZ(MathF.Atan2(facing.y,facing.x)*Mathf.Rad2Deg + 45, 0);
                obj.SetActive(true);
                obj.LeanRotateZ(MathF.Atan2(facing.y, facing.x) * Mathf.Rad2Deg + 135, .28f).setEaseOutExpo();
            }
            _rigidbody2D.velocity = Vector2.zero;
            //_rigidbody2D.velocity = (_rigidbody2D.velocity * speed) * drag * Time.deltaTime - attackdirection*2;*/
            attack_time_total = CharacterAttacks[0].attack_length;
            attack_time_counter = 0;
            return;
        }
    }

    public void Walking()
    {
        Vector2 newspeed = (_rigidbody2D.velocity + player_overhead.MovementVector * speed) * drag * Time.deltaTime;
        _rigidbody2D.velocity = newspeed;

        if(!findTurnDirection())
        {
            action = actions_list.IDLE;
            subaction = subactions_list.Idle;
        }
    }

    public bool findTurnDirection()
    {
        facing = player_overhead.MovementVector;
        switch (player_overhead.MovementVector)
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
                return false;
        }
        return true;
    }

}
