using UnityEngine;
using System.Collections;

public class AddGlow : MonoBehaviour {

	public GameObject mesher;
	private Renderer renderer;
	private Shader shader1;
	private Shader shader2;
	public GameObject paypal;
	public GameObject paypal1;
	public GameObject paypal2;
	public GameObject script;
	public int counter = 0;
	private bool opened = false;

	// Use this for initialization
	void Start () {
		paypal.GetComponent<Renderer>().enabled = false;
		paypal1.GetComponent<Renderer>().enabled = false;
		paypal2.GetComponent<Renderer>().enabled = false;
		renderer = mesher.GetComponent<Renderer>();
		shader1 = Shader.Find("Standard");
		shader2 = Shader.Find("MK/MKGlow/Normal/DiffuseRim");
	}
	
	// Update is called once per frame
	void Update () {
		if (!Network.isClient) {
			paypal.transform.LookAt (Camera.main.transform, Vector3.up);
		}

		var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Transform cam = Camera.main.transform;
		RaycastHit hit = new RaycastHit ();
		if (Physics.Raycast (cam.position, cam.forward, out hit, 30) && hit.transform.name.StartsWith ("artik")) {
			paypal.GetComponent<Renderer>().enabled = true;
			paypal1.GetComponent<Renderer>().enabled = true;
			paypal2.GetComponent<Renderer>().enabled = true;
			renderer.sharedMaterial.shader = shader2;
			counter += 1;
			if (!opened && counter > 500) {
				opened = true;
				script.GetComponent<AmountSlider> ().SpawnLadder();
			}
		} else {
			counter = 0;
			renderer.sharedMaterial.shader = shader1;
			paypal.GetComponent<Renderer>().enabled = false;
			paypal1.GetComponent<Renderer>().enabled = false;
			paypal2.GetComponent<Renderer>().enabled = false;
		}
	}
}
