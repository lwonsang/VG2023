using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main{
    public class TankController : MonoBehaviour
    {

        void OnCollisionEnter2D(Collision2D other){
            //reload scene
            
            if(other.gameObject.GetComponentInChildren<PlayerMovement>()){
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}