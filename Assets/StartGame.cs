using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class StartGame : MonoBehaviour {

    public bool isHost;
    public string address;

	// Use this for initialization
	void Start () {
        var networkManager = GetComponent<NetworkManager>();

        if (isHost)
        {
            networkManager.StartHost();
        }
        else
        {
            networkManager.networkAddress = address;
            networkManager.StartClient();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
