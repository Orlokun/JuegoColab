﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attach : MonoBehaviour {

	// Use this for initialization
	void Start () {

		Transform roca = GameObject.FindGameObjectWithTag ("rocaCaida").GetComponent <Transform>();
		gameObject.transform.position = roca.position;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}