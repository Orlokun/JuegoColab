﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class Listening : NetworkDiscovery
{
    Client scriptClient;
    // Use this for initialization
    void Start()
    {
        Initialize();
        scriptClient = GetComponent<Client>();
    }

    public void InitializeListening()
    {
        StartAsClient();
        GameObject.Find("ConnectText").GetComponent<Text>().text = "Conectando2...";
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        StopBroadcast();
        scriptClient.Connect(fromAddress);
        GameObject.Find("ConnectText").GetComponent<Text>().text = "Esperando...";

    }
}
