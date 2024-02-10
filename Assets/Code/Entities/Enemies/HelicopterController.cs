using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HelicopterController : CharacterBase
{
    
    private Rigidbody2D rb;
    public Transform target;
    private Vector2 moveDirection;
    [SerializeField] private float minY;
    private float timeBtwShots;
    public float startTimeBtwShots;
    public GameObject projectile;
        

    void OnCollisionEnter2D(Collision2D other){
        //reload scene
            
        if(other.gameObject.CompareTag("Player")){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        startTimeBtwShots = 2;
    }


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
            rb.rotation = angle;
            moveDirection = direction;
            
            if (direction.x < 0)
            {
                transform.localScale = new Vector3(1, -1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            
            if (timeBtwShots <= 0)
            {
                Vector3 offset = new Vector3(0.0f, -0.5f, 0.0f);
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
        Vector2 newposition = (Vector2)transform.position + (speed * Time.deltaTime * moveDirection);
        if (newposition.y < minY) //could set it so that the helicopter cannot go below a certain point
            newposition = new Vector2(newposition.x,transform.position.y);
        rb.MovePosition(newposition);
        
    }
}
