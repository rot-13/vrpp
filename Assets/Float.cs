using UnityEngine;
using System.Collections;

public class Float : MonoBehaviour {
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
		tempPos.x = transform.position.x;
		tempPos.z = transform.position.z;
		tempPos.y = tempVal + amplitude * Mathf.Sin (speed * Time.time);
		transform.position = tempPos;
	}
}
