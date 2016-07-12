using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerBehavior : NetworkBehaviour {

	// Use this for initialization
	void Start () {
        var cam = GetComponent<Camera>();
        cam.enabled = isLocalPlayer;
	}
	
	// Update is called once per frame
	void Update () {
	}
}
