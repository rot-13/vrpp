using UnityEngine;
using System.Collections;

public class AddGlow : MonoBehaviour {

	private Renderer renderer;
	private Shader shader1;
	private Shader shader2;

	// Use this for initialization
	void Start () {
		renderer = GetComponent<Renderer>();
		shader1 = Shader.Find("Standard");
		shader2 = Shader.Find("MK/MKGlow/Normal/DiffuseRim");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Jump")) {
			if (renderer.sharedMaterial.shader == shader1)
				renderer.sharedMaterial.shader = shader2;
			else
				renderer.sharedMaterial.shader = shader1;
		}
	}

	void Glow () {
	}
}
