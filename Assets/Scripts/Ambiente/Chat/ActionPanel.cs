﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPanel : MonoBehaviour {

    public void ChatButton()
    {
        List<Room> rooms = Server.instance.rooms;
        foreach (Room room in rooms)
        {
            room.CreateTextChat();
        }
    }

    public void ServerButton()
    {
        ServerNetworkDiscovery script = Server.instance.gameObject.GetComponent<ServerNetworkDiscovery>();
        script.ServerInitialize();
    }

    public void ResetServer()
    {
        ServerNetworkDiscovery script = Server.instance.gameObject.GetComponent<ServerNetworkDiscovery>();
        script.ResetServer();
    }

    public void MaxPlayerRoomButton()
    {
        Text inputText = GameObject.Find("InputPlayerText").GetComponent<Text>();
        int number = Int32.Parse(inputText.text);
        Server.instance.maxJugadores = number;
    }

    public void SceneToLoadButton()
    {
        Text inputText = GameObject.Find("InputSceneText").GetComponent<Text>();
        Server.instance.sceneToLoad = "Escena" + inputText.text;
    }
}
