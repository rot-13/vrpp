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
			mesher.GetComponent<Renderer> ().sharedMaterial = tempMat;
		}
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
