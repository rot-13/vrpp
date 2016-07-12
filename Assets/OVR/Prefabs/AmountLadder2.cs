using UnityEngine;
using System.Collections;

public class AmountLadder2 : MonoBehaviour {

	public Transform spine;
	public Transform slide;

	private bool spawned;

	private float SCALE = 1.5f;
	private float DISTANCE_M = 4f;
	private float ANGLE_RAD = 3f * (Mathf.PI / 180f);
	private int NUM_SPINES = 20;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.B)) {
			spawned = true;
			SpawnLadder ();
		}
		if (spawned) {
			SlideByLook ();
		}
	}

	// TODO animate
	void SpawnLadder() {
		float firstAngle = -(1.5f + 8f*3f) * (Mathf.PI / 180f);

		for (int i = 0; i < NUM_SPINES; i++) {
			Vector3 forward = transform.forward;
			forward.y = 0;
			forward.Normalize ();

			forward *= Mathf.Cos (firstAngle + i * ANGLE_RAD);
			forward.y = Mathf.Sin (firstAngle + i * ANGLE_RAD);
			forward.Normalize ();

			Vector3 position = forward * DISTANCE_M + transform.position;
			Instantiate (spine, position, Quaternion.LookRotation(position - transform.position));

			if (i == 0) {
				Instantiate(slide, position, Quaternion.LookRotation(position - transform.position));
			}
		}
	}

	void SlideByLook() {
		var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Transform cam = Camera.main.transform;
		RaycastHit hit = new RaycastHit ();
		GameObject s = GameObject.Find ("slider(Clone)");
		if (Physics.Raycast (cam.position, cam.forward, out hit, 10)) {
			if (hit.transform.name.StartsWith("spine")) {
				Debug.DrawLine (ray.origin, hit.point);
				print ("Name=" + hit.transform.name + " childCount=" + hit.transform.childCount);
				s.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z);
				s.transform.rotation = hit.transform.rotation;
			}

		}
	}

	void removeLadder() {
		// TODO
		spawned = false;
	}

}
