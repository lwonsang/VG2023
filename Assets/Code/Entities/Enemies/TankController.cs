using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.U2D;

namespace Main{
    public class TankController : CharacterBase
    {
        [SerializeField] FloatingHealthBar healthBar;
        
        [SerializeField] private float maxY;
        private Rigidbody2D rb;
        public Transform target;
        private Vector2 moveDirection;
        private float timeBtwShots;
        public float startTimeBtwShots;
        public GameObject enemyProjectile;
        public Sprite turnLeft;
        public Sprite turnRight;
        public Sprite turnUp;
        public Sprite turnDown;
        public Sprite destroyed;
        private bool defeated = false;

        // If the target is too far from the enemy, move closer. False by default, set during update.
        // until we actually implement this mechanic, it will be set to true always
        private Boolean TooFarAwayFromEnemy = true;
        
                
        
        
        
        // Start is called before the first frame update
        void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            rb = GetComponent<Rigidbody2D>();
            timeBtwShots = startTimeBtwShots;
            healthBar = GameObject.Find("TankHealthBar").GetComponentInChildren<FloatingHealthBar>();

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            
            if (target && !defeated)
            {
                Vector3 direction = (target.position - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg ;
                // rb.rotation = angle + 180;
                moveDirection = direction;
                
                Sprite newSprite = null;
                if (angle >= -45 && angle < 45)
                    newSprite = turnRight; // Left sprite
                else if (angle >= 45 && angle < 135)
                    newSprite = turnUp; // Up sprite
                else if (angle >= 135 && angle < 225)
                    newSprite = turnLeft; // Right sprite
                else
                    newSprite = turnDown; // Down sprite

                // Set the new sprite
                _spriterenderer.sprite = newSprite;
                
                if (direction.x > 0)
                {
                    transform.localScale = new Vector3(1, -1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }

                if (timeBtwShots <= 0)
                {
                    Vector3 offset = new Vector3(-.1f, 0.5f, 0.0f);
                    Instantiate(enemyProjectile, transform.position + offset, Quaternion.identity);
                    timeBtwShots = startTimeBtwShots;
                }
                else
                {
                    timeBtwShots -= Time.deltaTime;
                }

            }
            if (gettinghit){
                gettinghit = false;
                print("tank being hit");
                damage_taken += 1;

                print("Tank Current Health:" + (totalhealth-damage_taken));
                healthBar.UpdateHealthBar(totalhealth-damage_taken, totalhealth);
                if(totalhealth <= damage_taken)
                {
                    _spriterenderer.sprite = destroyed;
                    speed = 0;
                    defeated = true;
                    gameObject.layer = 0;
                    rb.angularVelocity = 0f;
                    foreach (Transform child in transform)
                    {
                        Destroy(child.gameObject);
                    }
                    Destroy(gameObject, 10f);
                }
                
            } 
            
            if(TooFarAwayFromEnemy){
                switch(action)
                {
                    case actions_list.GETTING_HIT:
                        Freezeframes();
                        break;
                    case actions_list.HITSTUN:
                        Hitstun();
                        break;
                    default:
                        currentVelocity = rb.velocity;
                        rb.velocity = (currentVelocity + moveDirection * speed) * drag * Time.deltaTime;
                        break;
                }
                //rb.MovePosition((Vector2)transform.position + (speed * Time.deltaTime * moveDirection));
            }
        }


    }
}