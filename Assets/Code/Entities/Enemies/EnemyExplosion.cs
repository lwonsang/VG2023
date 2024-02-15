using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private Transform targetPlayer;
    private Vector2 explosionLocation;

    [SerializeField] private float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player").transform; 
        explosionLocation = new Vector2(targetPlayer.position.x, targetPlayer.position.y);

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = (explosionLocation - (Vector2)transform.position).normalized;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        transform.position = Vector2.MoveTowards(transform.position, explosionLocation, speed * Time.deltaTime);

        if (transform.position.x == explosionLocation.x && transform.position.y == explosionLocation.y)
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
    
    // void OnTriggerEnter2D(Collider2D other){
    //     
    // }
}
