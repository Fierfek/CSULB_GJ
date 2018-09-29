using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Oil : MonoBehaviour
{
    private Slider slider;

    public float oil;

    public float oil_Max;

    private void Start()
    {
        slider = GameObject.Find("Oil Slider").GetComponent<Slider>();

        slider.maxValue = oil_Max;
        slider.value = oil;
    }

    void setOil(int value)
    {
        oil = value;
    }

    void modOil(int value)
    {
        oil -= value;
    }

    


}
