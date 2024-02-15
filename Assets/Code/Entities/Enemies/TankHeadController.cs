using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankHeadController :  CharacterBase
{
    private Rigidbody2D rb;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            rb.MoveRotation(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) ;
            
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
}
