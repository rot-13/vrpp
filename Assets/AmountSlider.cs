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
    private ArrayList previousLadder;
    private ArrayList previousLadderTexts;
    private ArrayList previousLadderPos;
    private ArrayList previousLadderTextsPos;

	private Color spineColor;
    private Vector3 spawnedPosition;
	private Vector3 spawnedForward;
	private float targetAngle;
	private float currentAngle;
	private int chosenIdx;
	private float startedLookingAtAmountTime;

    private bool animating;
    private float animationTimer;

    private float removalTimer;

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
            animate(Mathf.Min(animationTimer / 2f, 1f));
            if (animationTimer > 2f)
            {
                animating = false;
            }
            else
            {
                animationTimer += Time.deltaTime;
            }
        }

        if (previousLadder != null)
        {
            removalTimer += Time.deltaTime;
            animateRemoval(Mathf.Min(removalTimer / 2f, 1f));
            if (removalTimer > 2f)
            {
                foreach (GameObject t in previousLadder)
                {
                    Destroy(t);
                }
                foreach (Object t in previousLadderTexts)
                {
                    Destroy(t);
                }
                previousLadder = null;
                previousLadderPos = null;
                previousLadderTexts = null;
                previousLadderTextsPos = null;
            }
        }
	}

	public void SpawnLadder() {
        if (existingLadder != null)
        {
            removeLadder();
        }
		startedLookingAtAmountTime = -1;
		chosenIdx = -1;
		targetAngle = FIRST_ANGLE;
		currentAngle = FIRST_ANGLE;
		spawnedPosition = transform.position;
		spawnedForward = transform.forward;

        existingLadder = new ArrayList();
		existingLadderTexts = new ArrayList ();

        float bottomAngle = -Mathf.PI / 2f;

		for (int i = 0; i < NUM_SPINES; i++) {
            SliderData sliderData = GetSliderData(bottomAngle, DISTANCE_M);
            GameObject spinePart = (GameObject) Instantiate (spine, sliderData.position, sliderData.rotation * Quaternion.Euler(90, 0, 0));
			spineColor = spinePart.GetComponent<Renderer> ().material.color;
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
                existingSlider = (GameObject) Instantiate(slider, sliderData.position, sliderData.rotation * Quaternion.Euler(90, 0, 0));
			}
		}

        animating = true;
        animationTimer = 0f;
	}

	Color getColorForIdx(int idx) {
		return ((GameObject)existingLadder [idx]).transform.GetComponent<Renderer> ().material.color;
	}

	void UpdateAngle() {
		if (chosenIdx == -1) {
			return;
		}
		float epsilon = 1f * (Mathf.PI / 180f);
		if (Mathf.Abs (targetAngle - currentAngle) < epsilon) {
			currentAngle = targetAngle;
			startedLookingAtAmountTime += Time.deltaTime;

		} else {
			currentAngle += (targetAngle - currentAngle) * 0.05f;
		}

		SliderData sliderData = GetSliderData (currentAngle, DISTANCE_M);
		existingSlider.transform.position = sliderData.position;
        existingSlider.transform.rotation = sliderData.rotation * Quaternion.Euler(90, 0, 0);
	}

	void UpdateSliderColors() {
		for (int i = 0; i < NUM_SPINES; i++) {
			Material mat = ((GameObject)existingLadder [i]).transform.GetComponent<Renderer> ().material;
			if (i == chosenIdx) {
				continue;
			}
			mat.color = spineColor;
		}
		
		animateGlow ();
	}

	void SlideByLook() {
		
		Transform cam = Camera.main.transform;
		RaycastHit hit = new RaycastHit ();
		if (Physics.Raycast (cam.position, cam.forward, out hit, 10)) {
			if (hit.transform.name.StartsWith ("spine")) {
				var idx = existingLadder.IndexOf (hit.transform.gameObject);
				if (idx != chosenIdx) {
					chosenIdx = idx;
					startedLookingAtAmountTime = 0;
				}

				if (idx >= 0) {
					targetAngle = FIRST_ANGLE + idx * ANGLE_RAD;
				}
			}
		} else {
			chosenIdx = -1;
			startedLookingAtAmountTime = -1;
		}

		UpdateSliderColors ();
	}

	SliderData GetSliderData(float angle, float distance) {
		Vector3 forward = spawnedForward;
		forward.y = 0;
		forward.Normalize ();

		forward = Quaternion.Euler(0, 20, 0) * forward;

		forward *= Mathf.Cos (angle);
		forward.y = Mathf.Sin (angle);
		forward.Normalize ();

		Vector3 position = forward * distance + spawnedPosition;
		Quaternion rotation = Quaternion.LookRotation (position - spawnedPosition);

		return new SliderData (position, rotation);
	}


	void removeLadder() {
        if (existingLadder != null)
        {
            if (previousLadder != null)
            {
                foreach (Object t in previousLadder)
                {
                    Destroy(t);
                }
                foreach (Object t in previousLadderTexts)
                {
                    Destroy(t);
                }
            }
            removalTimer = 0f;
            previousLadder = existingLadder;
            previousLadderPos = new ArrayList();
            previousLadderTexts = existingLadderTexts;
            previousLadderTextsPos = new ArrayList();
            for (int i = 0; i < NUM_SPINES; ++i)
            {
                previousLadderPos.Add(((GameObject)previousLadder[i]).transform.position);
                previousLadderTextsPos.Add(((Canvas)previousLadderTexts[i]).transform.position);
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

	void animateGlow() {
		if (chosenIdx == -1 || startedLookingAtAmountTime == -1) {
			return;
		}

		float timeDiff = startedLookingAtAmountTime;
		float maxTimeSecs = 1.5f;
		if (timeDiff >= maxTimeSecs) {
			// TODO that's it, we've selected the amount
			print ("Chose amount!");
			removeLadder ();
		} else {
			float emission = Mathf.PingPong (timeDiff / maxTimeSecs, 1.0f);
			print ("Emission=" + emission);

			Color glowColor = Color.yellow;
			Color finalColor = Color.Lerp(spineColor, glowColor, emission);

			((GameObject)existingLadder [chosenIdx]).transform.GetComponent<Renderer> ().material.color = finalColor;
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
            SliderData sd = GetSliderData(angle, DISTANCE_M);
            spine.transform.position = sd.position;
            spine.transform.rotation = sd.rotation * Quaternion.Euler(90, 0, 0);

			Canvas canvas = (Canvas) existingLadderTexts [i];
			SliderData canvasSd = GetSliderData (angle, DISTANCE_M - 0.1f);
			canvas.transform.position = canvasSd.position;
			canvas.transform.rotation = canvasSd.rotation;


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

    void animateRemoval(float t)
    {
        float animationDuration = 0.3f;
        for (int i = 0; i < NUM_SPINES; ++i)
        {
            GameObject spine = (GameObject)previousLadder[i];
            Canvas text = (Canvas)previousLadderTexts[i];
            float startT = (NUM_SPINES - i - 1) * ((1f - animationDuration) / NUM_SPINES);
            float spineT = partition(t, startT, startT + animationDuration);
            float offset = lerp(Mathf.Pow(spineT, 10), 0, 100);
            spine.transform.position = ((Vector3)previousLadderPos[i]) + new Vector3(0, offset, 0);
            text.transform.position = ((Vector3)previousLadderTextsPos[i]) + new Vector3(0, offset, 0);
        }
    }

}
