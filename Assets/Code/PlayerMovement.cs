using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main{
    public class PlayerMovement : MonoBehaviour
    {

        public float currentHealth = 10f;
        public float maxHealth = 10f;

        [SerializeField] FloatingHealthBar healthBar;



        //broken code: wont print when there is a collision????
        void OnTriggerEnter2D(Collider2D other){
            if (other.gameObject.GetComponent<TankController>()){
                print("Collision with Enemy");
                // currentHealth -= 1;

                // print("Current Health:" + currentHealth);
                // healthBar.UpdateHealthBar(currentHealth, maxHealth);
            }
            
        }
        void OnTriggerExit2D(Collider2D other) {
            print("Exit Collision with " + other.gameObject.name);
        }

        // Start is called before the first frame update
        void Start()
        {
            currentHealth = maxHealth;
            healthBar = GameObject.Find("HealthBar").GetComponentInChildren<FloatingHealthBar>();
        }

        // Update is called once per frame
        void Update()
        {

            //temp
            if (Input.GetMouseButtonDown(0)) {
                print("Space pressed");
                currentHealth -= 1;

                print("Current Health:" + currentHealth);
                healthBar.UpdateHealthBar(currentHealth, maxHealth);
            }

            //Slowed down movement from 0.2f
            //Move Up
            if (Input.GetKey(KeyCode.UpArrow)) {
                transform.position += new Vector3(0, 0.05f, 0);
                // print("MOVING UP");
            }

            //Move Down
            if (Input.GetKey(KeyCode.DownArrow)) {
                transform.position += new Vector3(0, -0.05f, 0);
            }

            //Move Left
            if (Input.GetKey(KeyCode.LeftArrow)) {
                transform.position += new Vector3(-0.05f, 0, 0);
            }

            //Move Right
            if (Input.GetKey(KeyCode.RightArrow)) {
                transform.position += new Vector3(0.05f, 0, 0);
            }
        }
    }
}