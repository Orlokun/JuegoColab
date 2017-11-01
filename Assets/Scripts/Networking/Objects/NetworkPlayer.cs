﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class NetworkPlayer
{
    public int connectionId;
    public Room room;
    public bool connected;
    public int charId;
    public float positionX;
    public float positionY;
    public bool isGrounded;
    public float speedX;
    public int directionX;
    public int directionY;
    public bool attacking;
    public bool power;
    public bool controlOverEnemies;
    public string[] inventory = new string[8];
    public string ipAddress;

    public NetworkPlayer(int connectionId, int charId, Room room, string address)
    {
        this.ipAddress = address;
        this.connectionId = connectionId;
        this.room = room;
        this.charId = charId;

        controlOverEnemies = false;
        isGrounded = false;
        attacking = false;
        connected = true;
        power = false;

        positionX = 0;
        positionY = 0;
        speedX = 0;
        directionX = 1;
        directionY = 1;

    }

    public void InventoryUpdate(string message)
    {
        char[] separator = new char[1];
        separator[0] = '/';
        string[] msg = message.Split(separator);
        int index = Int32.Parse(msg[2]);

        if (msg[1] == "Add")
        {
            AddItemToInventory(index, msg[3]);
        }
        else
        {
            RemoveItemFromInventory(index);
        }
    }

    private void AddItemToInventory(int index, string spriteName)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (i == index)
            {
                inventory[i] = spriteName;
                return;
            }
        }
    }

    private void RemoveItemFromInventory(int index)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (i == index)
            {
                inventory[i] = null;
                return;
            }
        }
    }

    public string GetReconnectData()
    {
        return "PlayerChangePosition/" +
           charId + "/" +
           positionX + "/" +
           positionY + "/" +
           directionX + "/" +
           directionY + "/" +
           speedX + "/" +
           isGrounded;
    }

}
