﻿using UnityEngine;
using System.Collections;

public class RealFloat : MonoBehaviour {
	public float amplitude;          //Set in Inspector 
	public float speed;                  //Set in Inspector 
	private float tempVal;
	private Vector3 tempPos;

	void Start () 
	{
		tempVal = transform.position.y;
	}

	void Update ()
	{
//		tempVal = transform.parent.transform.position.y;
//		tempPos.y = tempVal + amplitude * Mathf.Sin (speed * Time.time);
//		transform.position = tempPos;
	}
}
