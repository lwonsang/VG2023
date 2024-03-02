using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MainFloatingHealthBar : MonoBehaviour
{

    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI myText;

    public void UpdateHealthBar(float currentValue, float maxValue){
        slider.value = currentValue / maxValue;
        myText.text = currentValue + "/" + maxValue;
    }

    // Start is called before the first frame update
    void Start()
    {       
        myText.text = "35/35";
    }

    // Update is called once per frame
//     void Update()
//     {
//     }
}
