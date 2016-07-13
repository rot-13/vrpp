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
	public bool showHere = false;
	public Camera came;
	public int counter = 0;
	private bool opened = false;

	// Use this for initialization
	void Start () {
		renderer = mesher.GetComponent<Renderer>();
		shader1 = Shader.Find("Standard");
		shader2 = Shader.Find("MK/MKGlow/Normal/DiffuseRim");
	}
	
	// Update is called once per frame
	void Update () {
		var ray = came.ScreenPointToRay (Input.mousePosition);
		Transform cam = came.transform;
		RaycastHit hit = new RaycastHit ();
		if (showHere && Physics.Raycast (cam.position, cam.forward, out hit, 30) && hit.transform.name.StartsWith ("artik")) {
			hit.transform.parent.FindChild("innersphere").GetComponent<Renderer>().enabled = true;
			hit.transform.parent.FindChild("innersphere").transform.LookAt (came.transform, Vector3.up);
			hit.transform.parent.FindChild("innersphere").FindChild("pay").GetComponent<Renderer>().enabled = true;
			hit.transform.parent.FindChild("innersphere").FindChild("pal").GetComponent<Renderer>().enabled = true;
			renderer.sharedMaterial.shader = shader2;
			counter += 1;
			if (counter > 250) {
				counter = 0;
				transform.parent.FindChild("OVRCameraRig").GetComponent<AmountSlider> ().SpawnLadder();
			}
		} else {
			counter = 0;
			renderer.sharedMaterial.shader = shader1;
			paypal.GetComponent<Renderer>().enabled = false;
			paypal1.GetComponent<Renderer>().enabled = false;
			paypal2.GetComponent<Renderer> ().enabled = false;
		}
	}
}
