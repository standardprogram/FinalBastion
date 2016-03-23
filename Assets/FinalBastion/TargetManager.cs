using UnityEngine;
using System.Collections;

public class TargetManager : MonoBehaviour {

	float lastTime = 0;

	GameObject prefab, manager;
	// Use this for initialization
	void Start () {
		prefab = Resources.Load ("Target") as GameObject;
		manager = GameObject.Find ("TargetManager");
	}
	
	// Update is called once per frame
	void Update () {
//		Debug.Log (Time.time-lastTime);
		if (Time.time - lastTime > 3) {
			CreateTarget();
			lastTime = Time.time;
		}
	}


	private void CreateTarget() {

		float x = Random.Range (-4, 4);
		float z = Random.Range (5, 8);
		Vector3 position = new Vector3 (x, 0, z);
		float angle = Mathf.Atan (x / z) * Mathf.Rad2Deg;
		Debug.Log ("target:" + angle);

		Quaternion rotation = Quaternion.Euler (0, angle, 0);

		GameObject target = GameObject.Instantiate (prefab, position, rotation) as GameObject;

		//target.transform.rotation = Quaternion.Euler (0, 0, 90);
		target.transform.parent = manager.transform;
		target.SetActive (true);
	}
}
