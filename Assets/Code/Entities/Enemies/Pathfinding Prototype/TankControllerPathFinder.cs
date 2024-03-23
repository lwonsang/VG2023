using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.U2D;

namespace Main{
    public class TankControllerPathFinder : CharacterBase
    {
        [SerializeField] FloatingHealthBar healthBar;
        [SerializeField] TMPController tmpcontroller;
        [SerializeField] private float maxDistanceFromEnemy;
        [SerializeField] private float maxY;
        private Rigidbody2D rb;
        public Vector3 target;
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
        private bool temp = false;
        private float distance;
       List<Vector3> pathList;
        private int pathListIndex;

        Vector3 originalPosition = new Vector3(-10, -10);
        
        // Start is called before the first frame update
        void Start()
        {
            // target = GameObject.FindGameObjectWithTag("Player").transform;
            rb = GetComponent<Rigidbody2D>();
            timeBtwShots = startTimeBtwShots;
            //healthBar = GameObject.Find("TankHealthBar").GetComponentInChildren<FloatingHealthBar>();
            tmpcontroller = FindAnyObjectByType<TMPController>();
            pathList = null;
            pathListIndex = -1;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (Input.GetMouseButtonDown(0)) {
                Vector3 mouseWorldPosition = GetMouseWorldPosition();
                target = mouseWorldPosition;
                Debug.Log("Mouse X: " + mouseWorldPosition.x + " Mouse Y: " + mouseWorldPosition.y);
                pathListIndex = 1;
                pathList = Pathfinding.Instance.FindPath(transform.position, mouseWorldPosition);
            }
            
            // if (pathListIndex > 0);
            // {
            //     Debug.Log("PathList index: " + pathListIndex);
            //     Debug.Log("PathList[index] is not null: " + (pathListIndex > 0));
            //     Vector3 nextcoord = pathList[pathListIndex];
            //     // if same location, increment the pathListIndex and continue
            //     Debug.Log("NextCoord X: " + nextcoord.x + " NextCoord Y: " + nextcoord.y);
            //     pathListIndex = 1;
            //     while ((int) nextcoord.x == (int) transform.position.x && (int) nextcoord.y == (int) transform.position.y && pathListIndex < pathList.Count&& pathList.Count > 1){
                    
            //         Debug.Log("Path same, updating nextcoord");
                    
            //         pathListIndex++;
            //         nextcoord = pathList[pathListIndex];
            //         Debug.Log("NextCoord X: " + nextcoord.x + " NextCoord Y: " + nextcoord.y);
            //     }
            //     Vector3 direction = (nextcoord - transform.position).normalized;
            //     float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg ;
            //     moveDirection = direction;
                
            //     Sprite newSprite = null;
            //     if (angle >= -45 && angle < 45)
            //         newSprite = turnRight;
            //     else if (angle >= 45 && angle < 135)
            //         newSprite = turnUp; 
            //     else if (angle >= 135 && angle < 225)
            //         newSprite = turnLeft; 
            //     else
            //         newSprite = turnDown; 
            //     _spriterenderer.sprite = newSprite;
                
            //     if (direction.x > 0)
            //     {
            //         transform.localScale = new Vector3(1, -1, 1);
            //     }
            //     else
            //     {
            //         transform.localScale = new Vector3(1, 1, 1);
            //     }

            //     if (timeBtwShots <= 0)
            //     {
            //         Vector3 offset = new Vector3(-.1f, 0.5f, 0.0f);
            //         Instantiate(enemyProjectile, transform.position + offset, Quaternion.identity);
            //         timeBtwShots = startTimeBtwShots;
            //     }
            //     else
            //     {
            //         timeBtwShots -= Time.deltaTime;
            //     }

            //     distance = Vector3.Distance(transform.position, target);
            //     if(distance >= maxDistanceFromEnemy){
            //         switch(action)
            //         {
            //             case actions_list.GETTING_HIT:
            //                 Freezeframes();
            //                 break;
            //             case actions_list.HITSTUN:
            //                 Hitstun();
            //                 break;
            //             default:
            //                 currentVelocity = rb.velocity;
            //                 rb.velocity = (currentVelocity + moveDirection * speed) * drag * Time.deltaTime;
            //                 break;
            //         }
            //         //rb.MovePosition((Vector2)transform.position + (speed * Time.deltaTime * moveDirection));
            //     }

            // }
            // if (gettinghit){
            //     gettinghit = false;
            //     damage_taken += 1;
                
            //     healthBar.UpdateHealthBar(totalhealth-damage_taken, totalhealth);
            //     if(totalhealth <= damage_taken)
            //     {
            //         _spriterenderer.sprite = destroyed;
            //         speed = 0;
            //         defeated = true;
                    
            //         gameObject.layer = 0;
            //         rb.angularVelocity = 0f;
            //         foreach (Transform child in transform)
            //         {
            //             Destroy(child.gameObject);
            //         }
            //         Destroy(gameObject, 3f);
            //     }
            // } 
            // if(totalhealth <= damage_taken && temp == false)
            // {
            //     tmpcontroller.onDestroyEnemy();
            //     temp = true;

            // }   
            
            
        }

        public static Vector3 GetMouseWorldPosition(){
            Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
            vec.z = 0f;
            return vec;
        }
        public static Vector3 GetMouseWorldPositionWithZ(){
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        }
        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera){
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }
        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera){
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }
    }


    
}