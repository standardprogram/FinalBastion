using UnityEngine;
using System.Collections;
using MojingSample.CrossPlatformInput;

public class UserInputManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


		if (CrossPlatformInputManager.GetButton("OK") /*|| CrossPlatformInputManager.GetButtonUp("Fire1")*/)
		{
			Debug.Log("OK-----GetButtonDown");

			Ray ray = Mojing.SDK.getMainCamera().ViewportPointToRay(new Vector3(0.5f, 0.5f, 1));

			RaycastHit hitInfo;

			if(Physics.Raycast(ray, out hitInfo)) {
				GameObject obj = hitInfo.collider.gameObject;
				if(obj != null && obj.tag == "TARGET") {

					obj.GetComponent<Target>().OnShot();
				}

			}

		}
		
		if (CrossPlatformInputManager.GetButtonUp("C"))
		{
			#if IOS_DEVICE
			MojingSDK.Unity_StopTracker();
			#endif
			Application.Quit();
		}
		
		if (CrossPlatformInputManager.GetButtonUp("MENU"))
		{
			Debug.Log("MENU-----GetButtonDown");
		}


	}
}
