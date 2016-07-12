using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CharacterControl : NetworkBehaviour {
	public Transform cameraPivot;
	public CharacterController character;
	private Vector3 initialPosition;
	private Vector3 currentPosition = Vector3.zero;
	public float speed = 5f;
	private Vector2 rotationSpeed = new Vector2 (75, 50);
	private float currentRotation = 75f;
	private float rotationLimitX = 150f;
	private bool changingView = false;

	void Update ()
	{
        if (!isLocalPlayer)
        {
            return;
        }
		if (Mathf.Abs (Input.GetAxis ("Horizontal")) < 0.1f)
			currentRotation = rotationSpeed.x;
		else
			currentRotation += 1f;
		if (currentRotation > rotationLimitX) currentRotation = rotationLimitX;
		character.SimpleMove(character.transform.forward * speed * Input.GetAxis("Vertical"));
		transform.Rotate(0, currentRotation * Time.deltaTime * Input.GetAxis ("Horizontal"), 0, Space.World);
		if (Input.GetMouseButtonDown(0)) {
			changingView = true;
			initialPosition = Input.mousePosition;
		}
		if (Input.GetMouseButtonUp(0))
			changingView = false;
		if (changingView) {
			currentPosition.x = Mathf.Clamp ((Input.mousePosition.x - initialPosition.x) / 100, -3, 3);
			currentPosition.y = Mathf.Clamp ((Input.mousePosition.y - initialPosition.y) / 100, -2, 2);
			currentPosition.x *= rotationSpeed.x;
			currentPosition.y *= rotationSpeed.y;
			currentPosition *= Time.deltaTime;
			transform.Rotate (0, currentPosition.x, 0, Space.World);
			cameraPivot.Rotate (-currentPosition.y, 0, 0);
		}
		if (Mathf.Abs( Input.GetAxis("Vertical")) > 0.25f )
			cameraPivot.rotation = Quaternion.Slerp (cameraPivot.rotation, transform.rotation, Time.deltaTime);
	}
}