﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BendTree : MonoBehaviour
{

    #region Start

    void Start()
    {
        PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
        collider.enabled = false;
    }

    #endregion

    #region Common

    public void Fall()
    {
        PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
        collider.enabled = true;

        SceneAnimator sceneAnimator = GameObject.FindObjectOfType<SceneAnimator>();
        sceneAnimator.SetBool("RockBottom", true, this.gameObject);
    }

    #endregion

}