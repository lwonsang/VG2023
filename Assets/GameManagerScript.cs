using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI myText;
    // Start is called before the first frame update
    private bool gameOverFromEscape = false;

    void Start()
    {
        myText.text = "RESUME";
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape)){
            print("ESCAPE PRESSED");
            gameOverFromEscape = true;
            gameOver();
            
            Time.timeScale = 0;
        }
        
    }
    public void gameOver(){
        Time.timeScale = 0;
        if (gameOverFromEscape)
        {
            myText.text = "RESUME";
        }
        else
        {
            myText.text = "RESTART";
        }
        // InputSystem.DisableDevice(Keyboard.current);
        gameOverUI.SetActive(true);
        gameOverFromEscape = false;
    }

    public void restart(){
        if (myText.text == "RESTART")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1;
        }
        else
        {
            gameOverUI.SetActive(false);
            Time.timeScale = 1;
        }
        
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
