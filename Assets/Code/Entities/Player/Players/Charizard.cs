using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Charizard : CharacterBase
{
    [SerializeField] FloatingHealthBar healthBar;
    public struct attacktimes
    {
        public float clawswipe;
    }

    public attacktimes _attacktimes;

    void Awake()
    {
        player_overhead.Characters.Add(gameObject);
        Debug.Log(Quaternion.identity);
        Hit_Enemies = new List<GameObject>();
        damage_taken = 0;
        totalhealth = 15;
        player_overhead = transform.parent.gameObject.GetComponent<PlayerOverhead>();
        if(player_overhead == null )
        {
            Destroy(gameObject);
            return;
        }
        action = actions_list.IDLE;
        actionable = true;

        healthBar = GameObject.Find("HealthBar").GetComponentInChildren<FloatingHealthBar>();
    }

    void OnTriggerEnter2D(Collider2D other){        
        if (other.gameObject.GetComponent<EnemyProjectile>()){
            damage_taken += 1;

            print("Current Health:" + totalhealth);
            healthBar.UpdateHealthBar(totalhealth-damage_taken, totalhealth);
        }  
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
        if(actionable)
        {
            findTurnDirection();
            //this is where you'd call moves
            //animator.Play("Claw_swipe");
            subaction = subactions_list.Claw_Swipe;
            //print(MathF.Atan2(facing.y, facing.x));
            foreach (GameObject obj in CharacterAttacks[0].objects)
            {
                
                
                Debug.Log(obj);
            }
            transform.LeanRotateZ(-45, .1f);
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.velocity = (_rigidbody2D.velocity * speed) * drag * Time.deltaTime - facing.normalized*8;
            attack_time_total = CharacterAttacks[0].attack_length;
            attack_time_counter = 0;
            actionable = false;
            return;
        }
        else
        {
            //attack specifics such as movement during attacks
            switch(subaction)
            {
                case subactions_list.Claw_Swipe:
                    switch (attack_time_counter)
                    {
                        case < 5:
                            _rigidbody2D.velocity = (_rigidbody2D.velocity * speed) * drag / 2 * Time.deltaTime;
                            break;
                        case 5:
                            transform.LeanRotateZ(50, .23f);
                            CharacterAttacks[0].objects[0].transform.rotation = Quaternion.Euler(new Vector3(0, 0, MathF.Atan2(facing.y, facing.x) *Mathf.Rad2Deg + 45));
                            //CharacterAttacks[0].objects[0].transform.LeanRotateZ(MathF.Atan2(facing.y, facing.x) * Mathf.Rad2Deg + 45, 0);
                            break;
                        case 6:
                            CharacterAttacks[0].objects[0].SetActive(true);
                            break;
                        case 7:
                            Action a = () => Setactiveobj(CharacterAttacks[0].objects[0]);
                            CharacterAttacks[0].objects[0].LeanRotateZ(MathF.Atan2(facing.y, facing.x) * Mathf.Rad2Deg + 135, .23f).setEaseOutExpo().setOnComplete(a);
                            _rigidbody2D.velocity = (_rigidbody2D.velocity * speed) * drag * Time.deltaTime + facing.normalized * 20;
                            break;
                        case > 7:
                            _rigidbody2D.velocity = (_rigidbody2D.velocity) * drag * Time.deltaTime;
                            break;

                    }
                    break;
            }
            attack_time_counter++;

            //check if the move is over here
            if (attack_time_counter > attack_time_total)
            {
                action = actions_list.IDLE;
                subaction = subactions_list.Idle;
                foreach (GameObject obj in CharacterAttacks[0].objects)
                {
                    obj.SetActive(false);
                    obj.transform.rotation = Quaternion.identity;
                }
                transform.LeanRotateZ(0, .1f);
                actionable = true;
                return;
            }
        }
    }

    public void Setactiveobj(GameObject obj)
    {
        obj.SetActive(false);
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
        switch (player_overhead.MovementVector)
        {
            //side view left
            case Vector2 vector when vector.x < 0 && Mathf.Abs(vector.y) < .4f:
                animator.SetInteger("Turn", 1);
                _spriterenderer.flipX = false;
                facing = player_overhead.MovementVector;
                break;
            //side view right
            case Vector2 vector when vector.x > 0 && Mathf.Abs(vector.y) < .4f:
                animator.SetInteger("Turn", 1);
                _spriterenderer.flipX = true;
                facing = player_overhead.MovementVector;
                break;
            //back view
            case Vector2 vector when vector.y > 0:
                animator.SetInteger("Turn", -1);
                facing = player_overhead.MovementVector;
                break;
            //front view
            case Vector2 vector when vector.y < 0:
                animator.SetInteger("Turn", 0);
                facing = player_overhead.MovementVector;
                break;
            default:
                return false;
        }
        return true;
    }

}
