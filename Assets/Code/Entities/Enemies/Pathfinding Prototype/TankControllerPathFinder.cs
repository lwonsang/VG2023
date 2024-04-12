using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
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
        private bool moving = false;
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
                rb.velocity = Vector3.zero;
                
                pathList = null;
                PathfindingOld.Instance.BlahBlahBlahTest();
                pathList = PathfindingOld.Instance.FindPath(transform.position, mouseWorldPosition);

                
                distance = Vector3.Distance(transform.position, target);
                if(distance >= 5){
                    Debug.Log("Pathfinding...");
                    PathfindingOld.Instance.FindPath(transform.position, mouseWorldPosition);
                }
                else{
                    // x distance closer than y distance - back up in the x axis
                    if(Math.Abs(target.x - transform.position.x) < Math.Abs(target.y - transform.position.y)){
                        // if target x is larger, back up in the -x direction
                        if(target.x > transform.position.x){
                            target.x = transform.position.x - 5;
                            pathList = PathfindingOld.Instance.FindPath(transform.position, target);
                        }
                        // if target x is smaller, back up in the +x direction
                        else if(target.x < transform.position.x){
                            target.x = transform.position.x + 5;
                            pathList = PathfindingOld.Instance.FindPath(transform.position, target);
                        }
                    }
                    else{
                        // if target y is larger, back up in the -y direction
                        if(target.y > transform.position.y){
                            target.y = transform.position.y - 5;
                            pathList = PathfindingOld.Instance.FindPath(transform.position, target);
                        }
                        // if target x is smaller, back up in the +x direction
                        else if(target.y < transform.position.y){
                            target.y = transform.position.y + 5;
                            pathList = PathfindingOld.Instance.FindPath(transform.position, target);
                        }
                    }
                }
                // Debug.Log("PathList:");
                // for(int i = 0; i < PathList)

                if (pathList != null){
                    if (pathList.Count > 0){
                        pathListIndex = 1;
                    }
                }
            }
            
            if (pathListIndex > 0)
            {
                Debug.Log("PathList index: " + pathListIndex);
                // Debug.Log("index is larger than 0: " + (pathListIndex > 0));
                Vector3 nextcoord = pathList[pathListIndex];
                nextcoord.x += originalPosition.x;
                nextcoord.y += originalPosition.y;
                // nextcoord.x = (int) nextcoord.x;
                // nextcoord.y = (int) nextcoord.y;
                // if same location, increment the pathListIndex and continue
                Debug.Log("Current Coord X: " + transform.position.x + " CurrentCoord Y: "+ transform.position.y);
                Debug.Log("NextCoord X: " + nextcoord.x + " NextCoord Y: " + nextcoord.y);
                // pathListIndex = 1;
                while ((int) nextcoord.x == (int) transform.position.x && (int) nextcoord.y == (int) transform.position.y && pathListIndex < pathList.Count&& pathList.Count > 1){
                    
                    Debug.Log("Path same, updating nextcoord");
                    
                    pathListIndex++;
                    nextcoord = pathList[pathListIndex];
                    nextcoord.x += originalPosition.x;
                    nextcoord.y += originalPosition.y;
                    // nextcoord.x = (int) nextcoord.x;
                    // nextcoord.y = (int) nextcoord.y;
                    Debug.Log("NextCoord X: " + nextcoord.x + " NextCoord Y: " + nextcoord.y);
                }
                
                Vector3 direction = (nextcoord - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg ;
                Debug.Log("angle: " + angle);
                moveDirection = direction;
                
                Sprite newSprite = null;
                if (angle >= -45 && angle < 45)
                    newSprite = turnRight;
                else if (angle >= 45 && angle < 135)
                    newSprite = turnUp; 
                else if (angle >= 135 && angle < 225)
                    newSprite = turnLeft; 
                else
                    newSprite = turnDown; 
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

                distance = Vector3.Distance(transform.position, target);
                if(distance >= maxDistanceFromEnemy){
                    switch(action)
                    {
                        case actions_list.GETTING_HIT:
                            Freezeframes();
                            break;
                        case actions_list.HITSTUN:
                            Hitstun();
                            break;
                        default:
                            Debug.Log("Moving...");
                            Debug.Log("Speed: " + speed);
                            currentVelocity = rb.velocity;
                            rb.velocity = (currentVelocity + moveDirection * speed) * drag * Time.deltaTime;
                            break;
                    }
                    //rb.MovePosition((Vector2)transform.position + (speed * Time.deltaTime * moveDirection));
                }
                else{
                    rb.velocity = Vector3.zero;
                    pathListIndex = -1;
                }
            }
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