using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour {
	
	// Objects to drag in
	public MovementMotor motor;
	public Transform character;

		
	void Awake () {		
		motor.movementDirection = Vector2.zero;
		motor.facingDirection = Vector2.zero;

		// Ensure we have character set
		// Default to using the transform this component is on
		if (!character)
			character = transform;

	}
	
	void Start () {
	
	
	}

	public void OnHurt() {

	}

	void Update () {
		// HANDLE CHARACTER MOVEMENT DIRECTION

		#if UNITY_ANDROID || UNITY_WP8 || UNITY_WP_8_1 || UNITY_BLACKBERRY || UNITY_TIZEN
		motor.movementDirection = Vector3.zero;

		// On mobiles, use the thumb stick and convert it into screen movement space
		motor.facingDirection = Vector3.zero;


		#else
		//motor.movementDirection = Input.GetAxis ("Horizontal") * screenMovementRight + Input.GetAxis ("Vertical") * screenMovementForward;
		#endif
		
		//Debug.Log("H:"+Input.GetAxis ("Horizontal"));
		//Debug.Log("3rd:"+Input.GetAxis ("3rdAxis"));
		//Debug.Log("J3rd:"+Input.GetAxis ("Joystick 3rdAxis"));
		// Make sure the direction vector doesn't exceed a length of 1
		// so the character can't move faster diagonally than horizontally or vertically
		if (motor.movementDirection.sqrMagnitude > 1)
			motor.movementDirection.Normalize();
	
	
	}

}
