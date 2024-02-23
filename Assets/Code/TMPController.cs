using System.Collections;
using System.Collections.Generic;
using System.Security;
using TMPro;
using UnityEngine;

public class TMPController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI myText;
    int n = 0;

    public void onDestroyEnemy(){
        print("on destroy called");
        n++;
        print("n = " + n);
        myText.text = "Enemies killed: " + n;
    }

}
