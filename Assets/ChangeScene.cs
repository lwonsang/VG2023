using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ChangeScene : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI resetScoreText;
    // public AudioSource audioSource;
    // public AudioClip buttonClick;
    // Start is called before the first frame update
    void Start()
    {
        //likely the problem
        // audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public void PlayButtonClick() {
    //     print("PLAYING SOUND");
    //     audioSource.PlayOneShot(buttonClick);
    // }

    public void showMainMenu(){
        SceneManager.LoadScene("MainMenu");
    }
    
    public void showInstructions(){
        SceneManager.LoadScene("Instructions");
    }
    public void startGame(){
        SceneManager.LoadScene("Player_and_Enemy");
    }


    public void showCredits(){
        SceneManager.LoadScene("Credits");
    }
    
    public void resetScore(){
        PlayerPrefs.DeleteKey("highScore");
        resetScoreText.text = "Score Reset!";
    }
}
