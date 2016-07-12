using UnityEngine;
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
    private GameObject existingSlider;

	private Vector3 spawnedPosition;
	private Vector3 spawnedForward;
	private float targetAngle;
	private float currentAngle;

    private bool animating;
    private float animationTimer;

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
        animating = false;
	}
	
	// Update is called once per frame
	void Update ()
    { 
		if (Input.GetKeyDown(KeyCode.B)) {
			SpawnLadder ();
		}

        if (existingLadder != null && !animating)
        {
			SlideByLook ();
			UpdateAngle ();
        }

        if (animating)
        {
            animate(Mathf.Min(animationTimer / 2f, 1));
            if (animationTimer > 2f)
            {
                animating = false;
            }
            else
            {
                animationTimer += Time.deltaTime;
            }
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

        float bottomAngle = -Mathf.PI / 2f;

		for (int i = 0; i < NUM_SPINES; i++) {
            //SliderData sliderData = GetSliderData (FIRST_ANGLE + i * ANGLE_RAD);
            SliderData sliderData = GetSliderData(bottomAngle);
            GameObject spinePart = (GameObject) Instantiate (spine, sliderData.position, sliderData.rotation * Quaternion.Euler(90, 0, 0));
			existingLadder.Add(spinePart);


			Vector3 forward = transform.forward;
			forward.y = 0;
			forward.Normalize ();
			forward = Quaternion.Euler(0, 20, 0) * forward;
			forward *= Mathf.Cos (bottomAngle);
			forward.y = Mathf.Sin (bottomAngle);
			forward.Normalize ();
			Vector3 canvasPosition = forward * (DISTANCE_M - 0.1f) + spawnedPosition;
			Quaternion rotation = Quaternion.LookRotation (canvasPosition - spawnedPosition);

			Object canvas = Instantiate (amountCanvas, canvasPosition, rotation);
			Text amountText = ((Canvas)canvas).GetComponentInChildren<Text> ();
			amountText.text = "$" + (i+1)*5;
			existingLadderTexts.Add (canvas);


			if (i == 0) {
                existingSlider = (GameObject) Instantiate(slide, sliderData.position, sliderData.rotation * Quaternion.Euler(90, 0, 0));
			}
		}

        animating = true;
        animationTimer = 0f;
	}

	void UpdateAngle() {
		float epsilon = 1f * (Mathf.PI / 180f);
		if (Mathf.Abs (targetAngle - currentAngle) < epsilon) {
			currentAngle = targetAngle;
		} else {
			currentAngle += (targetAngle - currentAngle) * 0.05f;
		}
		SliderData sliderData = GetSliderData (currentAngle);
		existingSlider.transform.position = sliderData.position;
        existingSlider.transform.rotation = sliderData.rotation * Quaternion.Euler(90, 0, 0);
	}

	void SlideByLook() {
		Transform cam = Camera.main.transform;
		RaycastHit hit = new RaycastHit ();
		if (Physics.Raycast (cam.position, cam.forward, out hit, 10)) {
			if (hit.transform.name.StartsWith("spine")) {
                var idx = existingLadder.IndexOf(hit.transform.gameObject);
                if (idx >= 0)
                {
                    targetAngle = FIRST_ANGLE + idx * ANGLE_RAD;
                }
			}
		}
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

        if (existingSlider != null)
        {
            Destroy(existingSlider);
            existingSlider = null;
        }
	}

    void animate(float t)
    {
        if (existingLadder == null)
        {
            return;
        }

        float bottomAngle = -Mathf.PI / 2f;
        float animationDuration = 0.3f;

        for (int i = 0; i < NUM_SPINES; ++i)
        {
            GameObject spine = (GameObject) existingLadder[i];
            float startT = (NUM_SPINES - i - 1) * ((1f - animationDuration) / NUM_SPINES);
            float spineT = partition(t, startT, startT + animationDuration);
            float angle = lerp(spineT, bottomAngle, FIRST_ANGLE + i * ANGLE_RAD);
            SliderData sd = GetSliderData(angle);
            spine.transform.position = sd.position;
            spine.transform.rotation = sd.rotation * Quaternion.Euler(90, 0, 0);

            if (i == 0)
            {
                existingSlider.transform.position = sd.position;
                existingSlider.transform.rotation = sd.rotation * Quaternion.Euler(90, 0, 0);
            }
        }
    }

    float partition(float t, float startT, float endT)
    {
        t = Mathf.Max(startT, Mathf.Min(t, endT));
        return (t - startT) / (endT - startT);
    }

    float lerp(float t, float min, float max)
    {
        return (1f - t) * min + t * max;
    }

}
