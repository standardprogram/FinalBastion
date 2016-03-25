using UnityEngine;
using System.Collections;

public class MachineGun : MonoBehaviour {

	private bool isFire;
	private ParticleSystem bullet, gunfire;
	private AudioSource audioFire;

	private int speed;

	// Use this for initialization
	void Start () {
		isFire = false;
		speed = 10;

		bullet = GameObject.Find ("Bullet").GetComponent<ParticleSystem>();
		gunfire = GameObject.Find ("GunFire").GetComponent<ParticleSystem> ();

		audioFire = this.gameObject.GetComponent<AudioSource> ();
	}


	private float lastBulletTime = 0;

	// Update is called once per frame
	void Update () {

		if (isFire && Time.realtimeSinceStartup - lastBulletTime > 0.1f) {
			Ray ray = Mojing.SDK.getMainCamera ().ViewportPointToRay (new Vector3 (0.5f, 0.5f, 1));
		
			RaycastHit hitInfo;
		
			if (Physics.Raycast (ray, out hitInfo)) {
				GameObject obj = hitInfo.collider.gameObject;
				if (obj != null && obj.tag == "TARGET") {
					obj.GetComponent<Target> ().OnShot ();
				}
			
			}

			lastBulletTime = Time.realtimeSinceStartup;
		}
	}

	public void StartFire() {
		Debug.Log ("MachineGun start fire!");
		isFire = true;

		if (!audioFire.isPlaying) {
			audioFire.Play();
		}
		
		if (! bullet.isPlaying) {
			bullet.Play ();
		}
		
		if (! gunfire.isPlaying) {
			gunfire.Play();
		}

	}


	public void StopFire() {
		Debug.Log ("MachineGun stop fire!");
		isFire = false;
	
		if (audioFire.isPlaying) {
			audioFire.Stop();
		}

		bullet.Stop ();
		
		gunfire.Stop ();
	
	}

}
