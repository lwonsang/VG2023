using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class setScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI myText;
    [SerializeField] TMPController tmpcontroller;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        myText.text = "Score: " + tmpcontroller.getScore();
    }

}
