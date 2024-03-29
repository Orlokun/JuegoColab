﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    #region Attributes

    public PlannerObstacle obstacleObj = null;

    public string openningTrigger; // The trigger that makes dissapear the object
    public string openedPrefab; // How it looks when its opened

    protected SceneAnimator animControl;
    protected Rigidbody2D rgbd;

    #endregion

    #region Start & Update


    // Use this for initialization
    protected virtual void Start()
    {
        rgbd = GetComponent<Rigidbody2D>();

    }

    #endregion

    #region Common

    public virtual void MoveMe(Vector2 force, bool movedFromLocal)
    {
        if (rgbd)
        {
            rgbd.AddForce(force);

            if (movedFromLocal)
            {
                SendMovableDataToServer(force);
            }

            if (!animControl)
            {
                Debug.Log("AnimatorControl not found in " + name);
                return;
            }

            StartCoroutine(animControl.StartAnimation("Moving", this.gameObject));
        }
    }

    protected void TransitionToOpened(GameObject trigger)
    {
        if (obstacleObj != null)
        {
            obstacleObj.blocked = false;
            obstacleObj.open = true;
        }

        if (openedPrefab != null)
        {
            Client.instance.SendMessageToServer("InstantiateObject/Prefabs/" + openedPrefab,false);
            Client.instance.SendMessageToServer("DestroyObject/" + name,false);
        }
    }

    #endregion

    #region Events

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger)
        {
            if (TriggerIsOpener(other.gameObject))
            {
                TransitionToOpened(other.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Prevents weird collisions with other game objects.
        if (!collision.gameObject || !collision.rigidbody)
        {
            return;
        }

        // Counter the force of every other game object.
        if (!GameObjectIsPunch(collision.gameObject))
        {
            Vector2 counter = -collision.rigidbody.velocity;
            rgbd.AddForce(counter);
        }
    }

    #endregion

    #region Utils

    protected bool TriggerIsOpener(GameObject trigger)
    {
        return trigger.name == openningTrigger;
    }

    protected bool GameObjectIsPunch(GameObject other)
    {
        return other.GetComponent<PunchController>();
    }

    #endregion

    #region Messaging

    protected void SendMovableDataToServer(Vector2 force)
    {

        if (Client.instance)
        {
            Client.instance.SendMessageToServer("ObjectMoved/" +
                    name + "/" +
                    force.x + "/" +
                    force.y,false);
        }

    }

    #endregion

}