using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoffeeProgressBar : MonoBehaviour
{
    public Slider slider;
 
    // Update is called once per frame
    void Update()
    {

    }

    public void changeSliderValue(float value)
    {
        slider.value = value;
    }

    public void turnOn() 
    { 
        gameObject.SetActive(true);
    }
    public void turnOff() 
    {
        gameObject.SetActive(false);
    }


}
