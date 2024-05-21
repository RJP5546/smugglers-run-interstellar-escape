using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.IO;

public class ArduinoReader : MonoBehaviour
{
    void OnMessageArrived(string msg)
    {
        Debug.Log(msg);
    }


    /*
    SerialPort data_stream = new SerialPort("COM5", 9600); //arduino is connected to com5 at a rate of 9600
    public string recievedDataString;
    int TestVal1;
    int TestVal2;
    int TestVal3;

    private void Start()
    {
        data_stream.Open(); //start the serial data stream
        data_stream.ReadTimeout = 1;
        InvokeRepeating("Serial_Data_Reading", 2f, 0.01f);
    }

    private void Update()
    {
        int recvInput = Serial_Data_Reading();
        Debug.Log(recvInput);
    }

    int Serial_Data_Reading()
    {
        recievedDataString = data_stream.ReadLine();
        string[] datas = recievedDataString.Split(","); //splits data between ,
        
        if (datas[0] != "" && datas[1] != "" && datas[2] != "")
        {
            TestVal1 = Mathf.RoundToInt(float.Parse(datas[0]));
            return TestVal1;
        }
        else { return TestVal1; }
        
    }
    */
}
