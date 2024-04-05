using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape)){
            gameOver();
            Time.timeScale = 0;
        }
    }
    public void gameOver(){
        // InputSystem.DisableDevice(Keyboard.current);
        gameOverUI.SetActive(true);
    }

    

    public void restart(){
        //TODO: decide what scene to build and with what character
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void mainMenu(){
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
    public void quit(){
        Application.Quit();
        Time.timeScale = 1;
    }
}
