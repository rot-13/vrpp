using UnityEngine;
using System.Collections;

public class AddGlow : MonoBehaviour {

	public GameObject mesher;
	private Renderer renderer;
	private Shader shader1;
	private Shader shader2;

	// Use this for initialization
	void Start () {
		renderer = mesher.GetComponent<Renderer>();
		shader1 = Shader.Find("Standard");
		shader2 = Shader.Find("MK/MKGlow/Normal/DiffuseRim");
	}
	
	// Update is called once per frame
	void Update () {
		var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Transform cam = Camera.main.transform;
		RaycastHit hit = new RaycastHit ();
		if (Physics.Raycast (cam.position, cam.forward, out hit, 30) && hit.transform.name.StartsWith ("artik")) {
			renderer.sharedMaterial.shader = shader2;
		} else {
			renderer.sharedMaterial.shader = shader1;
		}
	}
}
