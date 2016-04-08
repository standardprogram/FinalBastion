using UnityEngine;
using System.Collections;

public class MuzzleFlashAnimate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale = Vector3.one * Random.Range(0.5f,1.5f);
		Vector3 angles = transform.localEulerAngles;
		angles.z = Random.Range(0.0f,90.0f);
	}
}
