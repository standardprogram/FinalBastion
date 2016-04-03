using UnityEngine;
using System.Collections;
using MojingSample.CrossPlatformInput;


public class WeaponInputManager : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
		if (CrossPlatformInputManager.GetButton("OK") /*|| CrossPlatformInputManager.GetButtonUp("Fire1")*/)
		{
			Debug.Log("OK-----GetButtonDown");
			this.BroadcastMessage("StartFire", null, SendMessageOptions.DontRequireReceiver);

		}
		if(CrossPlatformInputManager.GetButtonUp("OK")) {
			Debug.Log("OK-----GetButtonUp");
			this.BroadcastMessage("StopFire", null, SendMessageOptions.DontRequireReceiver);
		}

		
	


	}
}
