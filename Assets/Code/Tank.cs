using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace s1 {
    public class Tank : MonoBehaviour
    {

        void OnCollisionEnter2D(Collision2D other){
            //reload scene
            if(other.gameObject.GetComponent<PlayerMovement>()){
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