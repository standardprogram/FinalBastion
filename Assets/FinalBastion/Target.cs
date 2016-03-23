using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour {

	private AudioSource audio;
	private int state;
	
	private Quaternion q2 = Quaternion.Euler(0, 0, 0);
	private Quaternion origin, flat;

	// Use this for initialization
	void Start () {
		state = 0;
		origin = gameObject.transform.rotation;
		gameObject.transform.Rotate (90, 0, 0);
		flat = gameObject.transform.rotation;
		audio = this.gameObject.GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
//		Debug.Log ("State:"+state+"   Rotation:"+gameObject.transform.rotation.eulerAngles.x);
		switch (state) {
		case 0:
			if(!(gameObject.transform.rotation.eulerAngles.x < 3)) {
				gameObject.transform.rotation = Quaternion.Slerp (gameObject.transform.rotation, origin, Time.deltaTime * 4);
		
			}else{
				state = 1;
			}
			break;

		case 2:
			if(!(gameObject.transform.rotation.eulerAngles.x > 87)) {
				gameObject.transform.rotation = Quaternion.Slerp (gameObject.transform.rotation, flat, Time.deltaTime * 8);
			}
			else {
				Destroy(this.gameObject);
			}
			break;
		}
	}

	public void OnShot() {
		if (!audio.isPlaying) {
			audio.Play();
		}
		if (state == 1) {
			state = 2;
		}
	}



}
