﻿using System.Collections;
using UnityEngine;

public class GearSystem : MonoBehaviour
{

    #region Attributes

    public struct Gear { public GameObject item; public bool placed; };

    public PlannerSwitch switchObj;
    public Sprite activatedSprite;
    public Gear[] requiredGears;

    public int activationTime;

    private float activationDistance = 1f;
    private bool activated;

    #endregion

    #region Start

    private void Start()
    {
        HideInactiveGears();
    }

    #endregion

    #region Common

    // Call from outside
    public void PlaceGear(GameObject gearGO)
    {

        if (activated)
        {
            return;
        }

        int pos = GearPosition(gearGO);

        if (pos != -1)
        {
            Gear gear = requiredGears[pos];

            PlaceGear(gear);

            if (AllGearsPlaced())
            {
                activated = true;
                StartCoroutine(Actioned());
            }

        }

    }

    // Call only from within
    protected void PlaceGear(Gear gear)
    {

        if (gear.item.GetComponent<SpriteRenderer>())
        {
            gear.placed = true;
            gear.item.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            Debug.LogError(gear + " does not have a SpriteRenderer");
        }

    }

    #endregion

    #region Utils

    // Hide every gear that was not "placed" from the editor
    protected void HideInactiveGears()
    {
        if (requiredGears == null)
        {
            Debug.Log(name + " has no required gears to work ?");
            return;
        }

        for (int i = 0; i < requiredGears.Length; i++)
        {
            if (!requiredGears[i].placed)
            {
                SpriteRenderer spriteRenderer = requiredGears[i].item.GetComponent<SpriteRenderer>();

                if (spriteRenderer)
                {
                    spriteRenderer.enabled = false;
                }
                else
                {
                    Debug.LogError(requiredGears[i] + " does not have a SpriteRenderer");
                }

            }
        }
    }

    protected int GearPosition(GameObject gear)
    {

        for (int i = 0; i < requiredGears.Length; i++)
        {
            if (requiredGears[i].item.Equals(gear))
            {
                return i;
            }
        }

        return -1;

    }

    protected bool AllGearsPlaced()
    {

        for (int i = 0; i < requiredGears.Length; i++)
        {
            if (!requiredGears[i].placed)
            {
                return false;
            }
        }

        return true;
    }

    #endregion

    #region Coroutines

    protected IEnumerator Actioned()
    {
        yield return new WaitForSeconds(activationTime);
        new GearSystemActions().DoSomething(this);
    }

    #endregion

}