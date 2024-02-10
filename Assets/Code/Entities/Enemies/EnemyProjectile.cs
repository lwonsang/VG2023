using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyProjectile : MonoBehaviour
{
    private Transform targetPlayer;
    private Vector2 shotLocation;

    [SerializeField] private float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player").transform; 
        shotLocation = new Vector2(targetPlayer.position.x, targetPlayer.position.y);

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = (shotLocation - (Vector2)transform.position).normalized;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        transform.position = Vector2.MoveTowards(transform.position, shotLocation, speed * Time.deltaTime);

        if (transform.position.x == shotLocation.x && transform.position.y == shotLocation.y)
        {
            DestroyProjectile();
        }
    }


    void DestroyProjectile()
    {
        if (gameObject)
        {    
            // Do something  
            Destroy(gameObject);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other){
        //reload scene
            
        if(other.gameObject.CompareTag("Player")){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            
        }
    }
}
