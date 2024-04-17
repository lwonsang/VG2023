using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.U2D;

namespace Main{
    public class TankController : CharacterBase
    {
        [SerializeField] FloatingHealthBar healthBar;
        [SerializeField] TMPController tmpcontroller;
        [SerializeField] private float maxDistanceFromEnemy;
        [SerializeField] private float maxY;
        private Rigidbody2D rb;
        // public Transform target;

        public Transform playerTarget;
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
        private AudioSource _audioSource;
        public AudioClip _audio;
        private bool playedSound = false;

        public Vector3 target;
        
        private bool moving = false;
       List<Vector3> pathList;
        private int pathListIndex;

        private bool traveling = false;
        private int shouldmove = 0;

        private bool tooClose = false;

        float randomTimeFrame1;

        Vector3 originalPosition = new Vector3(-35, -18);
        
        // Start is called before the first frame update
        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            // target = GameObject.FindGameObjectWithTag("Player").transform;
            // target = GameObject.FindGameObjectWithTag("Player").transform;
            rb = GetComponent<Rigidbody2D>();
            timeBtwShots = startTimeBtwShots;
            //healthBar = GameObject.Find("TankHealthBar").GetComponentInChildren<FloatingHealthBar>();
            tmpcontroller = FindAnyObjectByType<TMPController>();
            pathList = null;
            pathListIndex = -1;
            randomTimeFrame1 = UnityEngine.Random.Range(0f, 3f);
        }

        // Update is called once per frame

       float timePassed = 0f;
        void Update()
        {
            // Re-evaluate where you are every 3-5 seconds
            timePassed += Time.deltaTime;
            if(timePassed > randomTimeFrame1)
            {
                playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
                target = playerTarget.position;
                // debug.log("Player X: " + target.x + " Player Y: " + target.y);
                rb.velocity = Vector3.zero;
                
                pathList = null;
                // PathfindingOld.Instance.BlahBlahBlahTest();

                
                distance = Vector3.Distance(transform.position, target);
                if(distance >= 5){
                    tooClose = false;
                    // debug.log("Pathfinding...");
                    pathList = PathfindingOld.Instance.FindPath(transform.position, target);
                }
                else{
                    // debug.log("Too Close!");
                    tooClose = true;
                    // x distance closer than y distance - back up in the x axis
                    if(Math.Abs(target.x - transform.position.x) < Math.Abs(target.y - transform.position.y)){
                        // if target x is larger, back up in the -x direction
                        if(target.x > transform.position.x){
                            target.x = transform.position.x - 5;
                            target.y = transform.position.y;
                            pathList = PathfindingOld.Instance.FindPath(transform.position, target);
                        }
                        // if target x is smaller, back up in the +x direction
                        else if(target.x < transform.position.x){
                            target.x = transform.position.x + 5;
                            target.y = transform.position.y;
                            pathList = PathfindingOld.Instance.FindPath(transform.position, target);
                        }
                    }
                    else{
                        // if target y is larger, back up in the -y direction
                        if(target.y > transform.position.y){
                            target.y = transform.position.y - 5;
                            target.x = transform.position.x;
                            pathList = PathfindingOld.Instance.FindPath(transform.position, target);
                        }
                        // if target y is smaller, back up in the +y direction
                        else if(target.y < transform.position.y){
                            target.y = transform.position.y + 5;
                            target.x = transform.position.x;
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
                else{
                    pathListIndex = -1;
                }
                timePassed = 0f;
                randomTimeFrame1 = UnityEngine.Random.Range(3f, 5f);
            } 
        }
        void FixedUpdate()
        {
            
            if(playerTarget && !defeated){

                // Vector3 mouseWorldPosition = GetMouseWorldPosition();
                // target = mouseWorldPosition;
                
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
                
                // healthBar.UpdateHealthBar(totalhealth-damage_taken, totalhealth);
                // if(totalhealth <= damage_taken)
                // {
                //     _spriterenderer.sprite = destroyed;
                //     if (!playedSound)
                //     {
                //         _audioSource.PlayOneShot(_audio);
                //         playedSound = true;
                //     }
                   
                //     speed = 0;
                //     defeated = true;
                    
                //     gameObject.layer = 0;
                //     rb.angularVelocity = 0f;
                //     foreach (Transform child in transform)

                if (traveling){
                    if (pathListIndex > 0)
                    {
                        // Debug.Log("PathList index: " + pathListIndex);
                        // Debug.Log("index is larger than 0: " + (pathListIndex > 0));
                        Vector3 nextcoord = pathList[pathListIndex];
                        nextcoord.x += originalPosition.x;
                        nextcoord.y += originalPosition.y;
                        // nextcoord.x = (int) nextcoord.x;
                        // nextcoord.y = (int) nextcoord.y;
                        // if same location, increment the pathListIndex and continue
                        // Debug.Log("Current Coord X: " + transform.position.x + " CurrentCoord Y: "+ transform.position.y);
                        // Debug.Log("NextCoord X: " + nextcoord.x + " NextCoord Y: " + nextcoord.y);
                        // pathListIndex = 1;
                        while ((int) nextcoord.x == (int) transform.position.x && (int) nextcoord.y == (int) transform.position.y && pathListIndex < pathList.Count&& pathList.Count > 1){
                            
                            // debug.log("Path same, updating nextcoord");
                            
                            pathListIndex++;
                            nextcoord = pathList[pathListIndex];
                            nextcoord.x += originalPosition.x;
                            nextcoord.y += originalPosition.y;
                            // nextcoord.x = (int) nextcoord.x;
                            // nextcoord.y = (int) nextcoord.y;
                            // debug.log("NextCoord X: " + nextcoord.x + " NextCoord Y: " + nextcoord.y);
                        }
                        
                        Vector3 direction = (nextcoord - transform.position).normalized;
                        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg ;
                        // debug.log("angle: " + angle);
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

                        

                        // debug.log("Made it here.");

                        distance = Vector3.Distance(transform.position, target);
                        // tooClose overrides maxDistanceFromEnemy, basically since the tank wants to get away from the enemy.
                        if(!tooClose){
                            // debug.log("TooClose not used!");
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
                                        // debug.log("Moving...");
                                        // debug.log("Speed: " + speed);
                                        currentVelocity = rb.velocity;
                                        rb.velocity = (currentVelocity + moveDirection * speed) * drag * Time.deltaTime;
                                        break;
                                }
                                //rb.MovePosition((Vector2)transform.position + (speed * Time.deltaTime * moveDirection));
                            }
                            else{
                                rb.velocity = Vector3.zero;
                                pathListIndex = -1;
                                traveling = false;
                                shouldmove = 0;
                            }
                        }
                        else{
                            if(distance >= 1){
                                // debug.log("Distance greater");
                                switch(action)
                                {
                                    case actions_list.GETTING_HIT:
                                        Freezeframes();
                                        break;
                                    case actions_list.HITSTUN:
                                        Hitstun();
                                        break;
                                    default:
                                        // debug.log("Moving...");
                                        // debug.log("Speed: " + speed);
                                        currentVelocity = rb.velocity;
                                        rb.velocity = (currentVelocity + moveDirection * speed) * drag * Time.deltaTime;
                                        break;
                                }
                                //rb.MovePosition((Vector2)transform.position + (speed * Time.deltaTime * moveDirection));
                            }
                            else{
                                rb.velocity = Vector3.zero;
                                pathListIndex = -1;
                                traveling = false;
                                shouldmove = 0;
                                tooClose = false;
                            }
                        }
                        
                    }
                }
                else{
                    // Wait a bit before moving
                    int xcount = UnityEngine.Random.Range(1, 1000);
                    if(xcount > 960){
                        shouldmove++;
                    }
                    if(shouldmove == 3){
                        traveling = true;
                        // debug.log("Traveling set to True");
                    }
                }
                
                if (gettinghit){
                        rb.velocity = Vector3.zero;
                        // pathListIndex = -1;
                        traveling = false;
                        shouldmove = 0;
                        gettinghit = false;
                        damage_taken += 1;
                        
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
                            Destroy(gameObject, 3f);
                        }
                    } 
                    if(totalhealth <= damage_taken && temp == false)
                    {
                        tmpcontroller.onDestroyEnemy();
                        temp = true;

                    }   
            
                }
            }

        
    }


    
}



// using System;
// using System.Collections;
// using System.Collections.Generic;
// using TMPro;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.TextCore.Text;
// using UnityEngine.U2D;

// namespace Main{
//     public class TankController : CharacterBase
//     {
//         [SerializeField] FloatingHealthBar healthBar;
//         [SerializeField] TMPController tmpcontroller;
//         [SerializeField] private float maxDistanceFromEnemy;
//         [SerializeField] private float maxY;
//         private Rigidbody2D rb;
//         public Transform target;
//         private Vector2 moveDirection;
//         private float timeBtwShots;
//         public float startTimeBtwShots;
//         public GameObject enemyProjectile;
//         public Sprite turnLeft;
//         public Sprite turnRight;
//         public Sprite turnUp;
//         public Sprite turnDown;
//         public Sprite destroyed;
//         private bool defeated = false;
//         private bool temp = false;
//         private float distance;
        
//         // Start is called before the first frame update
//         void Start()
//         {
//             target = GameObject.FindGameObjectWithTag("Player").transform;
//             rb = GetComponent<Rigidbody2D>();
//             timeBtwShots = startTimeBtwShots;
//             //healthBar = GameObject.Find("TankHealthBar").GetComponentInChildren<FloatingHealthBar>();
//             tmpcontroller = FindAnyObjectByType<TMPController>();

//         }

//         // Update is called once per frame
//         void FixedUpdate()
//         {
            
//             if (target && !defeated)
//             {
//                 Vector3 direction = (target.position - transform.position).normalized;
//                 float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg ;
//                 moveDirection = direction;
                
//                 Sprite newSprite = null;
//                 if (angle >= -45 && angle < 45)
//                     newSprite = turnRight;
//                 else if (angle >= 45 && angle < 135)
//                     newSprite = turnUp; 
//                 else if (angle >= 135 && angle < 225)
//                     newSprite = turnLeft; 
//                 else
//                     newSprite = turnDown; 
//                 _spriterenderer.sprite = newSprite;
                
//                 if (direction.x > 0)
//                 {
//                     transform.localScale = new Vector3(1, -1, 1);
//                 }
//                 else
//                 {
//                     transform.localScale = new Vector3(1, 1, 1);
//                 }

//                 if (timeBtwShots <= 0)
//                 {
//                     Vector3 offset = new Vector3(-.1f, 0.5f, 0.0f);
//                     Instantiate(enemyProjectile, transform.position + offset, Quaternion.identity);
//                     timeBtwShots = startTimeBtwShots;
//                 }
//                 else
//                 {
//                     timeBtwShots -= Time.deltaTime;
//                 }

//             }
//             if (gettinghit){
//                 gettinghit = false;
//                 damage_taken += 1;
                
//                 healthBar.UpdateHealthBar(totalhealth-damage_taken, totalhealth);
//                 if(totalhealth <= damage_taken)
//                 {
//                     _spriterenderer.sprite = destroyed;
//                     speed = 0;
//                     defeated = true;
                    
//                     gameObject.layer = 0;
//                     rb.angularVelocity = 0f;
//                     foreach (Transform child in transform)
//                     {
//                         Destroy(child.gameObject);
//                     }
//                     Destroy(gameObject, 3f);
//                 }
//             } 
//             if(totalhealth <= damage_taken && temp == false)
//             {
//                 tmpcontroller.onDestroyEnemy();
//                 temp = true;

//             }   
            
//             distance = Vector3.Distance(transform.position, target.transform.position);
//             if(distance >= maxDistanceFromEnemy){
//                 switch(action)
//                 {
//                     case actions_list.GETTING_HIT:
//                         Freezeframes();
//                         break;
//                     case actions_list.HITSTUN:
//                         Hitstun();
//                         break;
//                     default:
//                         currentVelocity = rb.velocity;
//                         rb.velocity = (currentVelocity + moveDirection * speed) * drag * Time.deltaTime;
//                         break;
//                 }
//                 //rb.MovePosition((Vector2)transform.position + (speed * Time.deltaTime * moveDirection));
//             }
//         }


//     }
// }