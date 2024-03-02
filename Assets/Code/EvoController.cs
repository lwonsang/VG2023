using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EvoController : MonoBehaviour
{

    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI myText;

    public void UpdateEvoBar(float currentValue, float Level){
        slider.value = currentValue / 3;
        myText.text = "Level: "+ Level.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
         myText.text = "Level: 0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
