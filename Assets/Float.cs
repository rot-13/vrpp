using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Float : NetworkBehaviour {
	public float amplitude;          //Set in Inspector 
	public float speed;                  //Set in Inspector 
	public Material tempMat;
	public GameObject mesher;
	private float tempVal;
	private Vector3 tempPos;

	void Start () 
	{
		if (Network.isClient) {
			mesher.GetComponent<Renderer>().sharedMaterial = tempMat;
		}
		if (!isLocalPlayer) {
			return;
		}
		tempVal = transform.position.y;
	}
	void Update ()
	{   
		if (isLocalPlayer) {
			return;
		}
		tempVal = transform.parent.transform.position.y;
		tempPos = transform.position;
		tempPos.y = tempVal + amplitude * Mathf.Sin (speed * Time.time);
		transform.position = tempPos;
	}

	void LateUpdate() {
		if (!isLocalPlayer) {
			return;
		}
		var rotTemp = transform.rotation;
		rotTemp.x = rotTemp.z = 0;
		transform.rotation = rotTemp;
	}
}
