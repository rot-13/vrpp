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
		tempVal = transform.parent.transform.position.y;
		tempPos = transform.position;
		tempPos.y = tempVal + amplitude * Mathf.Sin (speed * Time.time);
		transform.position = tempPos;
	}

	void LateUpdate() {
		var rotTemp = transform.rotation;
		rotTemp.x = rotTemp.z = 0;
		transform.rotation = rotTemp;
	}
}
