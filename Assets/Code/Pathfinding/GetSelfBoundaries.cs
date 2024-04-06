using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSelfBoundaries : MonoBehaviour
{
    // Outlets
    Collider _collider;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(_collider.bounds);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
