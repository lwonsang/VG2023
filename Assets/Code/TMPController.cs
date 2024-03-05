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
        // print("n = " + n);
        myText.text = "Enemies killed: " + n;
        xp++;

        if(xp == 3){
            level++;
            xp = 0;
            currentPlayer.LevelUp();
        }

        
        evocontroller.UpdateEvoBar(xp, level);
    }

}
