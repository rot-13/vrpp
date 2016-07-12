using UnityEngine;
using System.Collections;

public class AmountLadder2 : MonoBehaviour {

	public GameObject spine;
	public GameObject slide;

//	private boolean spawned;

	private float SCALE = 1.5f;
	private float DISTANCE_M = 4f;
	private float ANGLE_RAD = 3f * (Mathf.PI / 180f);
	private int NUM_SPINES = 20;

    private ArrayList existingLadder;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.B)) {
			SpawnLadder ();
		}

        if (existingLadder != null)
        {
            // TODO: raycast
        }
	}

	void SpawnLadder() {
        if (existingLadder != null)
        {
            removeLadder();
        }
        existingLadder = new ArrayList();
		float firstAngle = -(1.5f + 4f*3f) * (Mathf.PI / 180f);

		for (int i = 0; i < NUM_SPINES; i++) {
			Vector3 forward = transform.forward;
			forward.y = 0;
			forward.Normalize ();

            forward = Quaternion.Euler(0, 20, 0) * forward;

			forward *= Mathf.Cos (firstAngle + i * ANGLE_RAD);
			forward.y = Mathf.Sin (firstAngle + i * ANGLE_RAD);
			forward.Normalize ();

			Vector3 position = forward * DISTANCE_M + transform.position;
            Object spinePart = Instantiate (spine, position, Quaternion.LookRotation(position - transform.position) * Quaternion.Euler(90, 0, 0));
            existingLadder.Add(spinePart);

			if (i == 0) {
                Object slidePart = Instantiate(slide, position, Quaternion.LookRotation(position - transform.position) * Quaternion.Euler(90, 0, 0));
                existingLadder.Add(slidePart);
			}
		}
	}

	void removeLadder() {
        if (existingLadder != null)
        {
            foreach(Object t in existingLadder)
            {
                Destroy(t);
            }
            existingLadder = null;
        }
	}

}
