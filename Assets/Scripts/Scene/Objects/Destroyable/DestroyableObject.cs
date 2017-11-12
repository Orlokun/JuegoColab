﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{

    #region Attributes

    public float destroyDelayTime;

    #endregion

    #region Start

    protected virtual void Start()
    {
        destroyDelayTime = .04f;
    }

    #endregion

    #region Common

    public virtual void DestroyMe(bool destroyedFromLocal)
    {

        if (destroyedFromLocal)
        {
            SendDestroyDataToServer();
        }

        Destroy(this.gameObject, destroyDelayTime);
    }

    #endregion

    #region Messaging

    protected void SendDestroyDataToServer()
    {
        if (Client.instance)
        {
            Client.instance.SendMessageToServer("ObjectDestroyed/" + name + "/");
        }

    }

    #endregion

}