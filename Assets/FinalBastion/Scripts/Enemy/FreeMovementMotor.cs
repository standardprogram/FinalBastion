using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Rigidbody))]

public class MovementMotor : MonoBehaviour {
	[HideInInspector]
	public Vector3 movementDirection;
	[HideInInspector]
	public Vector3 movementTarget;
	[HideInInspector]	
	public Vector3 facingDirection;

    //MoveController:
    public float walkingSpeed = 5.0f;
    public float walkingSnappyness = 50.0f;
    public float turningSmoothing = 0.3f;


    void FidxUpdate()
    {
        Vector3 targetVelocity = movementDirection * walkingSpeed;
		Vector3 deltaVelocity = targetVelocity - rigibody.velocity;

		if (rigibody.useGravity)
            deltaVelocity.y = 0;
		rigibody.AddForce(deltaVelocity * walkingSnappyness, ForceMode.Acceleration);

        Vector3 faceDir = facingDirection;
        if (faceDir == Vector3.zero)
            faceDir = movementDirection;

        if (faceDir == Vector3.zero) {
			rigibody.angularVelocity = Vector3.zero;
        }
        else
        {
            float rotationAngle = AngleAroundAxis(transform.forward, faceDir, Vector3.up);
			rigibody.angularVelocity = (Vector3.up * rotationAngle * turningSmoothing);
        }

    }

    static float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
    {
        dirA = dirA - Vector3.Project(dirA, axis);
        dirB = dirB - Vector3.Project(dirB, axis);

        float angle = Vector3.Angle(dirA, dirB);

        return angle * (Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) < 0? -1:1);
    }


	Rigidbody rigibody;
	// Use this for initialization
	void Start () {
		rigibody = this.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
