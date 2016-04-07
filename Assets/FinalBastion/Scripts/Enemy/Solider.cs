using UnityEngine;
using System.Collections;

public class Solider : MonoBehaviour {
	private Quaternion stand, laydown;
	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		if (stand != null && laydown != null)
			transform.rotation = Quaternion.Slerp (stand, laydown, Time.time);

	}

	private IEnumerator destroy() {
		yield return new WaitForSeconds(2);
		Destroy (gameObject);
		//Debug.Log (transform.rotation);
	}

	public void OnDead() {
		stand = transform.rotation;
		laydown = transform.rotation * Quaternion.Euler (-80, 0, 0);
		StartCoroutine (destroy());
	}
}
