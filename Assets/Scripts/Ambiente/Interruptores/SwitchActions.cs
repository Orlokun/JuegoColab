﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchActions {

    GameObject Chasca = GameObject.Find("Chasca");
    int groupId;
    public SwitchActions(GroupOfSwitchs group)
    {
        this.groupId = group.groupId;
        foreach(Switch switchi in group.GetSwitchs())
        {
            switchi.SetJobDone();
        }
    }

	public void DoSomething()
    {
        switch(groupId)
        {
            case 0:
                Chasca.GetComponent<SpriteRenderer>().enabled = true;
                Chasca.GetComponent<BoxCollider2D>().enabled = true;
                break;
            case 1:
                //asdada
                break;
            default:
                break;
        }
    }
}
