using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {

	float lastTime = 0;

	GameObject prefab, manager;
	// Use this for initialization
	void Start () {
		prefab = Resources.Load ("Prefabs/Enemy_Solider") as GameObject;
		manager = GameObject.Find ("EnemyManager");

		CreateTarget ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void EnemyDead() {
		CreateTarget ();
	}

	private void CreateTarget() {

		float x = Random.Range (-40, 40);
		float z = Random.Range (150, 180);
		Vector3 position = new Vector3 (x, 10, z);


		Quaternion rotation = Quaternion.Euler (0, 180, 0);

		GameObject target = GameObject.Instantiate (prefab, position, rotation) as GameObject;

		//target.transform.rotation = Quaternion.Euler (0, 0, 90);
		target.transform.parent = manager.transform;
		target.SetActive (true);
	}
}
