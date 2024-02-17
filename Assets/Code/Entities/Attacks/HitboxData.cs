using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements;

public class HitboxData : MonoBehaviour
{
    [HideInInspector] public GameObject Owner;
    [HideInInspector] public GameObject Opponent;

    public float attackangle;
    public float damage;
    public float knockback;
    public float knockbackgrowth;
    public float freezeframes;
    public float hitstun;

    [HideInInspector] public bool reset_attacker;
    public void Awake()
    {
        Owner = transform.parent.parent.gameObject;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject == Owner)
        {
            return;
        }
        else if (collision.gameObject.CompareTag("Enemy_Hitbox"))
        {
            CharacterBase owner_script = Owner.GetComponent<CharacterBase>();
            if (owner_script == null)
            {
                return;
            }
            else
            {
                //if already hit enemy, return
                if (owner_script.Hit_Enemies.Contains(collision.gameObject))
                {
                    return;
                }
            }
            Opponent = collision.gameObject;
            CharacterBase opponent_script = collision.gameObject.GetComponent<CharacterBase>();

            /*if (opponent_script.Hit_Enemies.Contains(Opponent))
            {
                return;
            }*/

            opponent_script.action = CharacterBase.actions_list.GETTING_HIT;
            opponent_script.gettinghit = true;
            // Debug.Log(opponent_script.damage_taken);
            
            //angle and direction calculation
            opponent_script.transform.rotation = Quaternion.identity;

            Vector2 facing = Vector2.one;

            

            if (owner_script != null)
            {
                facing = owner_script.player_overhead.MovementVector.normalized;

            }

            Opponent.LeanCancel();
            opponent_script.damage_taken += damage;
            Debug.Log(opponent_script.damage_taken);
            float total_knockback = knockback + Mathf.Log(opponent_script.damage_taken, 2) * knockbackgrowth;
            opponent_script._rigidbody2D.AddForce(total_knockback * facing);
        }
        else
        {
            Debug.Log(collision.name);
            Debug.Log(collision.gameObject.tag);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
