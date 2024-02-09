using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main{
    public class LevelBounds : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D other){

            //Dont want to reset game for now, just bump into the bounds
            
            // if(other.gameObject.GetComponent<PlayerMovement>()){
            //     // print("Collision with player movement");
            //     SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            // }
        }
    }
}

