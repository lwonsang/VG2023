using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankHeadController :  CharacterBase
{
    private Rigidbody2D rb;
    public Transform target;
    public Transform aimPivot;
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
            // Vector3 mousePosition = Input.mousePosition;
            // Vector3 mousePositionInWord = Camera.main.ScreenToWorldPoint(mousePosition);
            // Vector3 directionFromPlayerToMouse = mousePositionInWord - transform.position;
            
            // float radiansToMouse = Mathf.Atan2(directionFromPlayerToMouse.y, directionFromPlayerToMouse.x);
            // float angleToMouse = radiansToMouse * Mathf.Rad2Deg + 180f;

            // aimPivot.rotation = Quaternion.Euler(0,0, angleToMouse);



            Vector3 direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180f;
            aimPivot.rotation = Quaternion.Euler(0,0, angle);



            // rb.rotation = angle + 180;
            // if (direction.x > 0)
            // {
            //     transform.localScale = new Vector3(1, -1, 1);
            // }
            // else
            // {
            //     transform.localScale = new Vector3(1, 1, 1);
            // }
        }
    }
}
