using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MainFloatingHealthBar : MonoBehaviour
{

    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI myText;

    // public GameObject player;

    public void UpdateHealthBar(float currentValue, float maxValue){
        slider.value = currentValue / maxValue;
        if(slider.value <= 0){
            myText.text = 0 + "/" + maxValue;
            // gameObject.SetActive(false);
        }
        else{
            myText.text = currentValue + "/" + maxValue;
        }
        

    }

    // Start is called before the first frame update
    void Start()
    {       
        myText.text = "50/50";
    }

    // Update is called once per frame
//     void Update()
//     {
//     }
}
