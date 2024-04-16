using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class setScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currRunText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    [SerializeField] TMPController tmpcontroller;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currRunText.text = "Score: " + tmpcontroller.getScore();
        if (tmpcontroller.getScore() > PlayerPrefs.GetInt("highScore")){
            PlayerPrefs.SetInt("highScore", tmpcontroller.getScore());
        }
    
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("highScore");
    }

}
