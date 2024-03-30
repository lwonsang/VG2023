using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void showMainMenu(){
        SceneManager.LoadScene("MainMenu");
    }
    
    public void showInstructions(){
        SceneManager.LoadScene("Instructions");
    }
    public void startGame(){
        SceneManager.LoadScene("Player_and_Enemy");
    }
}
