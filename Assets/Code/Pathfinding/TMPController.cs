using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security;
using TMPro;
using UnityEngine;

public class TMPController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI myText;
    public CharacterBase currentPlayer;
    [SerializeField] EvoController evocontroller;
    int n = 0;
    int xp = 0;
    int level = 1;

    public void onDestroyEnemy(){
        // print("on destroy called");
        n++;
        // PlayerPrefs.SetInt("Score", n); //set high score
        // print("n = " + n);
        myText.text = "Score: " + n;
        xp++;

        if(xp == 3){
            level++;
            xp = 0;
            currentPlayer.LevelUp();
        }

        
        evocontroller.UpdateEvoBar(xp, level);
    }

    public int getScore(){
        return n;
    }

    // void Start() {
    //     n = PlayerPrefs.GetInt("Score");
    // }

}
