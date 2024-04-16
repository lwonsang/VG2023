using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI myText;

    [SerializeField] AudioSource menuAudioSource;
    [SerializeField] AudioSource backgroundAudioSource;
    [SerializeField] AudioClip deathAudio;
    [SerializeField] AudioClip menuMusic;

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
            gameOverFromEscape = true;
            gameOver();
            
            Time.timeScale = 0;

            
        }
        
    }
    public void gameOver(){
        Time.timeScale = 0;

        //stop background music
        backgroundAudioSource.Pause();
        
        if (gameOverFromEscape)
        {
            myText.text = "RESUME";
            //play regular menu music
            menuAudioSource.PlayOneShot(menuMusic);
        }
        else
        {
            myText.text = "RESTART";
            //play death/gameover music
            menuAudioSource.PlayOneShot(deathAudio);
        }
        // InputSystem.DisableDevice(Keyboard.current);
        gameOverUI.SetActive(true);
        gameOverFromEscape = false;
    }

    public void restart(){
        //stop any audio
        menuAudioSource.Stop();
        if (myText.text == "RESTART")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1;
        }
        else
        {
            gameOverUI.SetActive(false);
            Time.timeScale = 1;

            //prob resume audio here
            backgroundAudioSource.Play();
            backgroundAudioSource.loop = true;
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
