using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConvSpeedButtons : MonoBehaviour
{
    // Start is called before the first frame update


    public Text textField;

    void Start()
    {
        RefreshTextField();
    }

    void Update()
    {
        RefreshTextField();
    }
    public void RefreshTextField()
    {
        //GameObject gm = GameObject.Find("testConv");
        //IChangeSpeed changeSpeed = gm.GetComponent<IChangeSpeed>();
        //float speed = (float)Math.Round(changeSpeed.GetSpeed(), 1);
        textField.text = SpeedConv.getSpeedConv().ToString();
    }
    public void DicButtonClick()
    {

        //GameObject gm = GameObject.Find("testConv");
        //IChangeSpeed changeSpeed = gm.GetComponent<IChangeSpeed>();

        if (SpeedConv.getSpeedConv() <= 0.1)
            return;
        SpeedConv.DicSpeedConv(0.1f);
        RefreshTextField();

    }
    public void IncButtonClick()
    {

        //GameObject gm = GameObject.Find("testConv");
        //IChangeSpeed changeSpeed = gm.GetComponent<IChangeSpeed>();
        SpeedConv.IncSpeedConv(0.1f);
        RefreshTextField();

    }
}