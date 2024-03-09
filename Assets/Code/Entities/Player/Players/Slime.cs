using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Slime: CharacterBase
{
    public GameManagerScript gameManager;
    private bool isDead;
    [SerializeField] MainFloatingHealthBar healthBar;

    public enum subactions_list
    {
        Idle,
        Slime_tornado,
        Silme_split
    }
    public subactions_list subaction;

    private float tornadotimer = 0;
    public List<GameObject> children_slime;
    public GameObject father_slime;
    public bool isChild;
    public float recombine_timer;
    public float recombine_total;
    public bool recombinable;
    public float current_scale;

    void Awake()
    {
        Hit_Enemies = new List<GameObject>();
        action = actions_list.IDLE;
        actionable = true;
        current_scale = transform.localScale.z;
        if (isChild)
        {
            return;
        }
        player_overhead = transform.parent.gameObject.GetComponent<PlayerOverhead>();
        player_overhead.Characters.Add(gameObject);
        PlayerOverhead.CharacterUIData thing = new PlayerOverhead.CharacterUIData();
        thing.background_color = new Color(.4f, .6f, 1f);
        thing.icon = _spriterenderer.sprite;
        player_overhead.Character_UIData.Add(thing);
        if (player_overhead == null )
        {
            Destroy(gameObject);
            return;
        }
        

        GameObject healthbargameobject = GameObject.Find("HealthBar");
        if(healthbargameobject != null )
        {
            healthBar = healthbargameobject.GetComponentInChildren<MainFloatingHealthBar>();
        }
    }

    private void Start()
    {
        if(isChild)
        {
            recombine_timer = 0;
            recombinable = false;
            cooldown_delegate += RecombineCooldown;
            current_scale = transform.localScale.z;
        }
    }

    void OnTriggerEnter2D(Collider2D other){        
        if (other.gameObject.GetComponent<EnemyProjectile>()){
            damage_taken += 1;

            // print("Current Health:" + (totalhealth-damage_taken));
            healthBar.UpdateHealthBar(totalhealth-damage_taken, totalhealth);

            if(totalhealth <= damage_taken && !isDead)
            {
                isDead = true;
                gameManager.gameOver();
                // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }  
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.Equals(father_slime)) {
            if (!recombinable)
                return;

        }
    }

    public override void FixedUpdate()
    {
        Cooldowns();
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
            Hit_Enemies = new List<GameObject>();
            findTurnDirection();
            subaction = subactions_list.Slime_tornado;
            tornadotimer = 0;
            attack_time_total = CharacterAttacks[0].attack_length;
            attack_time_counter = 0;
            actionable = false;
            return;
        }
        else
        {
            //attack specifics such as movement during attacks
            switch (subaction)
            {
                case subactions_list.Slime_tornado:
                    _rigidbody2D.velocity = (_rigidbody2D.velocity + player_overhead.MovementVector * .45f * speed) * drag * Time.deltaTime;
                    switch(attack_time_counter)
                    {
                        case 5:
                            CharacterAttacks[0].objects[0].SetActive(true);
                            break;
                        case 120:
                            CharacterAttacks[0].objects[1].SetActive(true);
                            break;
                        case 400:
                            CharacterAttacks[0].objects[2].SetActive(true);
                            break;
                    }

                    transform.eulerAngles = transform.eulerAngles + Vector3.forward * Mathf.Min(attack_time_counter, 180f) / 7f;
                    //transform.LeanRotateZ(transform.eulerAngles.z + 45f, 20f/Mathf.Min(attack_time_counter, 180f));
                    attack_time_counter++;
                    tornadotimer += Mathf.Min(attack_time_counter, 180f) / 5f;
                    if (tornadotimer > 144f)
                    {
                        RotateCounterClockwise();
                        ResetAttackList();
                        tornadotimer = 0;
                        foreach (GameObject obj in CharacterAttacks[0].objects)
                        {
                            obj.GetComponentInChildren<CircleCollider2D>().enabled = false;
                        }
                        Action action = () =>
                        {
                            foreach (GameObject obj in CharacterAttacks[0].objects)
                            {
                                obj.GetComponentInChildren<CircleCollider2D>().enabled = true;
                            }
                        };
                        gameObject.LeanDelayedCall(Time.deltaTime, action);
                    }
                    if (!player_overhead.pressAttack)
                    {
                        foreach (GameObject obj in CharacterAttacks[0].objects)
                        {
                            obj.SetActive(false);
                            obj.transform.rotation = Quaternion.identity;
                        }
                        action = actions_list.IDLE;
                        subaction = subactions_list.Idle;
                        transform.LeanRotateZ(0, .1f);
                        actionable = true;
                        return;
                    }
                    return;
            }
            attack_time_counter++;
            //check if the move is over here
            if (attack_time_counter > attack_time_total)
            {
                switch(subaction)
                {
                    case subactions_list.Silme_split:
                        foreach (GameObject obj in CharacterAttacks[1].objects)
                        {
                            obj.SetActive(false);
                            obj.transform.rotation = Quaternion.identity;
                        }
                        break;
                }
                action = actions_list.IDLE;
                subaction = subactions_list.Idle;
                transform.LeanRotateZ(0, .1f);
                actionable = true;
                return;
            }
        }
    }

    public void Setactiveobj(GameObject obj)
    {
        print(obj);
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
        //for slime specifically, front = 0, back = 1, back_tilted = 2, front tilted = 3

        switch (player_overhead.MovementVector)
        {
            //side view left front
            case Vector2 vector when vector.x < 0 && Mathf.Abs(vector.y) <= Mathf.Abs(vector.x):
                if(vector.y > 0)
                {
                    animator.SetInteger("Turn_direction", 2);
                    facing_enum = facing_direction.Tilted_Left_Back;
                }
                else
                {
                    animator.SetInteger("Turn_direction", 3);
                    facing_enum = facing_direction.Tilted_Left_Front;
                }
                _spriterenderer.flipX = true;
                facing = player_overhead.MovementVector;
                break;
            //side view right front
            case Vector2 vector when vector.x > 0 && Mathf.Abs(vector.y) <= Mathf.Abs(vector.x):
                if (vector.y > 0)
                {
                    animator.SetInteger("Turn_direction", 2);
                    facing_enum = facing_direction.Tilted_Right_Back;
                }
                else
                {
                    animator.SetInteger("Turn_direction", 3);
                    facing_enum = facing_direction.Tilted_Right_Front;
                }
                _spriterenderer.flipX = false;
                facing = player_overhead.MovementVector;
                break;
            //back view
            case Vector2 vector when vector.y > 0:
                animator.SetInteger("Turn_direction", 1);
                facing = player_overhead.MovementVector;
                facing_enum = facing_direction.Back;
                break;
            //front view
            case Vector2 vector when vector.y < 0:
                animator.SetInteger("Turn_direction", 0);
                facing = player_overhead.MovementVector;
                facing_enum = facing_direction.Front;
                break;
            default:
                return false;
        }
        return true;
    }

    #region Helper
    public void ResetAttackList()
    {
        Hit_Enemies.Clear();
    }
    public void RecombineCooldown()
    {
        if(recombine_timer < recombine_total)
        {
            return;
        }
        else
        {
            recombinable = true;
            cooldown_delegate -= RecombineCooldown;
        }
    }

    public void Recombine(float size)
    {
    }
    #endregion

    #region Override Basics

    override public void SetOut()
    {
        foreach (GameObject obj in CharacterAttacks[0].objects)
        {
            obj.SetActive(false);
            Debug.Log("snom");
        }
        action = actions_list.OUT;
        _spriterenderer.enabled = false;
        hitbox.enabled = false;
        _rigidbody2D.velocity = Vector2.zero;
    }
    override public facing_direction GetDirection()
    {
        return facing_enum;
    }

    override public void SetDirection(facing_direction direction)
    {
        switch (direction)
        {
            //side view left back
            case facing_direction.Tilted_Left_Back:
                animator.SetInteger("Turn_direction", 2);
                _spriterenderer.flipX = true;
                facing_enum = facing_direction.Tilted_Left_Back;
                break;
            //side view right back
            case facing_direction.Tilted_Right_Back:
                animator.SetInteger("Turn_direction", 2);
                _spriterenderer.flipX = false;
                facing_enum = facing_direction.Tilted_Right_Back;
                break;
            case facing_direction.Tilted_Left_Front:
                animator.SetInteger("Turn_direction", 3);
                _spriterenderer.flipX = true;
                facing_enum = facing_direction.Tilted_Left_Front;
                break;
            case facing_direction.Tilted_Right_Front:
                animator.SetInteger("Turn_direction", 3);
                _spriterenderer.flipX = false;
                facing_enum = facing_direction.Tilted_Right_Front;
                break;
            //back view
            case facing_direction.Back:
                animator.SetInteger("Turn_direction", 1);
                facing_enum = facing_direction.Back;
                break;
            //front view
            case facing_direction.Front:
                animator.SetInteger("Turn_direction", 0);
                facing_enum = facing_direction.Front;
                break;
            default:
                return;
        }
    }

    override public void RotateCounterClockwise()
    {
        switch (facing_enum)
        {
            case facing_direction.Front:
                SetDirection(facing_direction.Tilted_Right_Front);
                break;
            case facing_direction.Tilted_Right_Front:
                SetDirection(facing_direction.Tilted_Right_Back);
                break;
            case facing_direction.Tilted_Right_Back:
                SetDirection(facing_direction.Back);
                break;
            case facing_direction.Back:
                SetDirection(facing_direction.Tilted_Left_Back);
                break;
            case facing_direction.Tilted_Left_Back:
                SetDirection(facing_direction.Tilted_Left_Front);
                break;
            case facing_direction.Tilted_Left_Front:
                SetDirection(facing_direction.Front);
                break;
        }
    }

    override public void RotateClockwise()
    {
        switch (facing_enum)
        {
            case facing_direction.Front:
                SetDirection(facing_direction.Tilted_Left_Front);
                break;
            case facing_direction.Tilted_Left_Front:
                SetDirection(facing_direction.Tilted_Left_Back);
                break;
            case facing_direction.Tilted_Left_Back:
                SetDirection(facing_direction.Back);
                break;
            case facing_direction.Back:
                SetDirection(facing_direction.Tilted_Right_Back);
                break;
            case facing_direction.Tilted_Right_Back:
                SetDirection(facing_direction.Tilted_Right_Front);
                break;
            case facing_direction.Tilted_Right_Front:
                SetDirection(facing_direction.Front);
                break;
        }
    }
    #endregion

}
