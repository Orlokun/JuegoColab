﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpAndManaHUD {

    public float maxHP;
    public float maxMP;
    public float maxExp;
    public float currentHP;
    public float currentMP;
    public float currentExp;
    public float percentageHP;
    public float percentageMP;
    public float percentageExp;
    Room room;

    public HpAndManaHUD(Room room)
    {                         
        this.room = room;
        maxHP = 250;
        maxMP = 250;
        maxExp = 250;
        currentHP = maxHP;
        currentMP = maxMP;
        currentExp = 0;
        percentageHP = 1;
        percentageMP = 1;
        percentageExp = 0;
    }

    public void RecieveHpAndMpHUD(string changeRate)
    {
        ChangeHP(changeRate);
        ChangeMP(changeRate);
    }

    public void ChangeHP(string deltaHP)
    {
        float valueDeltaHP = float.Parse(deltaHP);
        currentHP += valueDeltaHP;

        if (currentHP >= maxHP)
        {
            currentHP = maxHP;
        }
        else if (currentHP <= 0)
        {
            currentHP = 0;
            room.SendMessageToAllPlayers("PlayersAreDead");
        }

        percentageHP = currentHP / maxHP;
        room.SendMessageToAllPlayers("DisplayChangeHPToClient/" + percentageHP);
    }

    public void ChangeMaxHP(string NewMaxHP)
    {
        float valueMaxHP = float.Parse(NewMaxHP);
        maxHP = valueMaxHP;
        ChangeHP(NewMaxHP);
    }

    public void ChangeMP(string deltaMP)
    {
        float valueDeltaMP = float.Parse(deltaMP);
        currentMP += valueDeltaMP;

        if (currentMP >= maxMP)
        {
            currentMP = maxMP;
        }
        else if (currentMP <= 0)
        {
            currentMP = 0;
        }

        percentageMP = currentMP / maxMP;
        room.SendMessageToAllPlayers("DisplayChangeMPToClient/" + percentageMP);
    }

    public void ChangeMaxMP(string NewMaxMP)
    {
        float valueMaxMP = float.Parse(NewMaxMP);
        maxMP = valueMaxMP;
        ChangeMP(NewMaxMP);
    }

    public void ChangeExp(string deltaExp)
    {
        float valueDeltaExp = float.Parse(deltaExp);
        currentExp += valueDeltaExp;

        if (currentExp >= maxExp)
        {
            currentExp = 0;
            // levelUp
        }

        percentageExp = currentExp / maxExp;
        room.SendMessageToAllPlayers("DisplayChangeExpToClient/" + percentageExp);
    }

    public void ChangeMaxExp(string NewMaxExp)
    {
        float valueMaxExp = float.Parse(NewMaxExp);
        maxExp = valueMaxExp;
        ChangeMP(NewMaxExp);
    }
}
