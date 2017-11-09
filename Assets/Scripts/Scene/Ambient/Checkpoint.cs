﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    #region Attributes

    public bool activated;

    #endregion

    #region Start & Update

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Events

    void OnTriggerEnter2D(Collider2D other)
    {
        if (activated)
        {
            return;
        }

        if (GameObjectIsPlayer(other.gameObject))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.respawnPosition = transform.position;
            activated = true;
        }

    }

    #endregion

    #region Utils

    protected bool GameObjectIsPlayer(GameObject other)
    {

        if (other.tag == "Player")
        {
            PlayerController playerController = other.GetComponent<PlayerController>();

            if (playerController.localPlayer)
            {
                return true;
            }
        }

        return false;
    }

    #endregion


}
