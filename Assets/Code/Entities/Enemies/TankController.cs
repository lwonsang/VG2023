using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

namespace Main{
    public class TankController : MonoBehaviour
    {
        [SerializeField] float speed = 1.0f;
        [SerializeField] private float maxY;
        private Rigidbody2D rb;
        public Transform target;
        private Vector2 moveDirection;
        

        void OnCollisionEnter2D(Collision2D other){
            //reload scene
            
            if(other.gameObject.CompareTag("Player")){
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            rb = GetComponent<Rigidbody2D>();
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