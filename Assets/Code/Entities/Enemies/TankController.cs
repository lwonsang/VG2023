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
        
        [SerializeField] private float maxY;
        private Rigidbody2D rb;
        public Transform target;
        private Vector2 moveDirection;
        private float timeBtwShots;
        public float startTimeBtwShots;
        public GameObject projectile;
        

        /*void OnCollisionEnter2D(Collision2D other){
            //reload scene
            
            if(other.gameObject.CompareTag("Player")){
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }*/


        // Start is called before the first frame update
        void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            rb = GetComponent<Rigidbody2D>();
            timeBtwShots = startTimeBtwShots;
        }

        // Update is called once per frame
        void Update()
        {
            if (target)
            {
                Vector3 direction = (target.position - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                rb.rotation = angle + 180;
                moveDirection = direction;
                
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
                    Instantiate(projectile, transform.position + offset, Quaternion.identity);
                    timeBtwShots = startTimeBtwShots;
                }
                else
                {
                    timeBtwShots -= Time.deltaTime;
                }

            }
        }

        private void FixedUpdate()
        {
            rb.MovePosition((Vector2)transform.position + (speed * Time.deltaTime * moveDirection));
            
            if (transform.position.y > maxY) //could set it so that the tank cannot go above a certain point
            {
                transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
            }
        }
    }
}