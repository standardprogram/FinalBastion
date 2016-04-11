using UnityEngine;
using System.Collections;


public class F3DTurret : MonoBehaviour
{
    public Transform hub;           // Turret hub 
    public Transform barrel;        // Turret barrel

    RaycastHit hitInfo;             // Raycast structure
    bool isFiring;                  // Is turret currently in firing state
	bool startFire;


    float hubAngle, barrelAngle;    // Current hub and barrel angles

    // Project vector on plane
    Vector3 ProjectVectorOnPlane(Vector3 planeNormal, Vector3 vector)
    {
        return vector - (Vector3.Dot(vector, planeNormal) * planeNormal);
    }

    // Get signed vector angle
    float SignedVectorAngle(Vector3 referenceVector, Vector3 otherVector, Vector3 normal)
    {
        Vector3 perpVector;
        float angle;
       
        perpVector = Vector3.Cross(normal, referenceVector);
        angle = Vector3.Angle(referenceVector, otherVector);
        angle *= Mathf.Sign(Vector3.Dot(perpVector, otherVector));

        return angle;
    }

    // Turret tracking
    void Track()
    {
		if(hub != null && barrel != null)
        {
            // Construct a ray pointing from screen mouse position into world space
			Ray cameraRay = Mojing.SDK.getMainCamera ().ViewportPointToRay (new Vector3 (0.5f, 0.5f, 1));//Camera.main.ScreenPointToRay(Input.mousePosition);

            // Raycast
            if (Physics.Raycast(cameraRay, out hitInfo, 500f))
            {
				GameObject obj = hitInfo.collider.gameObject;
				if (isFiring && obj != null && obj.tag == "TARGET") {
					Debug.Log("!!!!!!!!!!!!!!!");
					float damage = F3DFXController.instance.GetDamage();
					Debug.Log("damage= "+damage);
					Health health = obj.GetComponent<Health>();
					health.OnDamage (damage, cameraRay.direction);
				}

                // Calculate heading vector and rotation quaternion
                Vector3 headingVector = ProjectVectorOnPlane(hub.up, 
				                               hitInfo.point - hub.position);


				Quaternion newHubRotation = Quaternion.LookRotation(headingVector, hub.up);
                // Check current heading angle
				hubAngle = SignedVectorAngle(hub.up, headingVector, Vector3.up);
			/*
                // Limit heading angle if required
                if (hubAngle <= -60)
                    newHubRotation = Quaternion.LookRotation(Quaternion.Euler(0, -60, 0) * transform.forward, hub.up);
                else if (hubAngle >= 60)
                    newHubRotation = Quaternion.LookRotation(Quaternion.Euler(0, 60, 0) * transform.forward, hub.up);
*/

				//Debug.Log("Hub Rot:"+transform.rotation.eulerAngles);
				//Debug.Log("New Rot:"+newHubRotation.eulerAngles);
				//Debug.Log("angle:"+hubAngle);
		
				// Apply heading rotation
				hub.rotation = Quaternion.Slerp(hub.rotation, newHubRotation, Time.deltaTime * 5f);

                // Calculate elevation vector and rotation quaternion
                Vector3 elevationVector = ProjectVectorOnPlane(hub.right, hitInfo.point - barrel.position);
                Quaternion newBarrelRotation = Quaternion.LookRotation(elevationVector);

                // Check current elevation angle
                barrelAngle = SignedVectorAngle(hub.forward, elevationVector, hub.right);
              
                // Limit elevation angle if required
                if (barrelAngle < -45)
                    newBarrelRotation = Quaternion.LookRotation(Quaternion.AngleAxis(-30f, hub.right) * hub.forward);   
                else if (barrelAngle > 15)
                    newBarrelRotation = Quaternion.LookRotation(Quaternion.AngleAxis(15f, hub.right) * hub.forward);  

                // Apply elevation rotation
                barrel.rotation = Quaternion.Slerp(barrel.rotation, newBarrelRotation, Time.deltaTime * 5f);

			}
        }
    }
	
	public void StartFire() {
		Debug.Log ("MachineGun start fire!");
		startFire = true;

	}
	
	
	public void StopFire() {
		Debug.Log ("MachineGun stop fire!");
		startFire = false;

	}

    void Update()
    {
        // Track turret
        Track();

        // Fire turret
		if (!isFiring && startFire)
        {
			isFiring = true;
            F3DFXController.instance.Fire();
        }

        // Stop firing
		if(isFiring && !startFire)
        {
			isFiring = false;
            F3DFXController.instance.Stop();
        }
    }
}
