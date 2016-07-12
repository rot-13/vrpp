﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AmountSlider : MonoBehaviour {

	public GameObject spine;
	public GameObject slider;
	public Canvas amountCanvas;

	private float DISTANCE_M = 4f;
	private float ANGLE_RAD = 3f * (Mathf.PI / 180f);
	private int NUM_SPINES = 20;
	private float FIRST_ANGLE = -(1.5f + 4f*3f) * (Mathf.PI / 180f);

    private ArrayList existingLadder;
	private ArrayList existingLadderTexts;

	private Vector3 spawnedPosition;
	private Vector3 spawnedForward;
	private float targetAngle;
	private float currentAngle;

	private class SliderData {
		public Vector3 position;
		public Quaternion rotation;
		public SliderData(Vector3 position, Quaternion rotation) {
			this.position = position;
			this.rotation = rotation;
		}
	}

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
			SlideByLook ();
//			UpdateAngle ();
        }
	}

	void SpawnLadder() {
        if (existingLadder != null)
        {
            removeLadder();
        }
		targetAngle = FIRST_ANGLE;
		currentAngle = FIRST_ANGLE;
		spawnedPosition = transform.position;
		spawnedForward = transform.forward;

        existingLadder = new ArrayList();
		existingLadderTexts = new ArrayList ();

		for (int i = 0; i < NUM_SPINES; i++) {
			float angle = FIRST_ANGLE + i * ANGLE_RAD;
			SliderData sliderData = GetSliderData (FIRST_ANGLE + i * ANGLE_RAD);
			Object spinePart = Instantiate (spine, sliderData.position, sliderData.rotation * Quaternion.Euler(90, 0, 0));
			existingLadder.Add(spinePart);


			Vector3 forward = transform.forward;
			forward.y = 0;
			forward.Normalize ();
			forward = Quaternion.Euler(0, 20, 0) * forward;
			forward *= Mathf.Cos (angle);
			forward.y = Mathf.Sin (angle);
			forward.Normalize ();
			Vector3 canvasPosition = forward * (DISTANCE_M - 0.1f) + spawnedPosition;
			Quaternion rotation = Quaternion.LookRotation (canvasPosition - spawnedPosition);

			Object canvas = Instantiate (amountCanvas, canvasPosition, rotation);
			Text amountText = ((Canvas)canvas).GetComponentInChildren<Text> ();
			amountText.text = "$" + (i+1)*5;
			existingLadderTexts.Add (canvas);


			if (i == 0) {
				Object slidePart = Instantiate(slider, sliderData.position, sliderData.rotation * Quaternion.Euler(90, 0, 0));
				existingLadder.Add(slidePart);
			}
		}
	}

	void UpdateAngle() {
		float epsilon = 1f * (Mathf.PI / 180f);
		if (Mathf.Abs (targetAngle - currentAngle) < epsilon) {
			currentAngle = targetAngle;
		} else {
			currentAngle += (targetAngle - currentAngle) * 0.1f;
		}
		SliderData sliderData = GetSliderData (currentAngle);
		GameObject s = GameObject.Find ("slider(Clone)");
		s.transform.position = sliderData.position;
		s.transform.rotation = sliderData.rotation;
	}

	void SlideByLook() {
		Transform cam = Camera.main.transform;
		RaycastHit hit = new RaycastHit ();
		GameObject s = GameObject.Find ("slider(Clone)");
		if (Physics.Raycast (cam.position, cam.forward, out hit, 10)) {
			if (hit.transform.name.StartsWith("spine")) {
				s.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z);
				s.transform.rotation = hit.transform.rotation;
//				targetAngle = GetSliderAngleForSpine (hit.transform);


			}
		}
	}

	float GetSliderAngleForSpine(Transform hitTransform) {
		Vector3 v = hitTransform.position - spawnedPosition;
		v.Normalize ();

		Vector3 forward = spawnedForward;
		forward.y = 0;
		forward.Normalize ();

		float angle = Mathf.Acos (Vector3.Dot (forward, v));

		print ("Dot = " + Vector3.Dot (forward, v));
		return angle;
	}


	SliderData GetSliderData(float angle) {
		Vector3 forward = spawnedForward;
		forward.y = 0;
		forward.Normalize ();

		forward = Quaternion.Euler(0, 20, 0) * forward;

		forward *= Mathf.Cos (angle);
		forward.y = Mathf.Sin (angle);
		forward.Normalize ();

		Vector3 position = forward * DISTANCE_M + spawnedPosition;
		Quaternion rotation = Quaternion.LookRotation (position - spawnedPosition);

		return new SliderData (position, rotation);
	}


	void removeLadder() {
		// TODO animate
        if (existingLadder != null)
        {
            foreach(Object t in existingLadder)
            {
                Destroy(t);
            }
			foreach (Object t in existingLadderTexts) {
				Destroy (t);
			}
            existingLadder = null;
			existingLadderTexts = null;
        }
	}

}
