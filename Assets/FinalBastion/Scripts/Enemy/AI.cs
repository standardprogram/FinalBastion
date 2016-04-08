using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour {
	
	// Objects to drag in
	public MovementMotor motor;


	private Health soliderHealth;
	private bool isDead;

	private int state;	//0:normal, 1:hurt, 2:dead

	private float lastTime = 0;
	public SignalSender startFireSignals;
	public SignalSender stopFireSignals;

	void Start () {
		state = 0;

		soliderHealth = this.GetComponent<Health> ();
	}



	void Update () {
		//soliderHealth.OnDamage (0.1f, Vector3.zero);
		if (Time.time - lastTime > 2) {
			int n = Random.Range (1, 3);
			if (n == 2) {
				StartCoroutine(Fire ());
			}
			lastTime = Time.time;
		}

		switch (state) {
		case 0: //normal
			motor.movementDirection = (Vector3.zero - transform.position);
			motor.facingDirection = (Vector3.zero - transform.position);
			break;
		case 1:	//hurt
			Vector3 originDirection = (Vector3.zero - transform.position);
			motor.movementDirection = new Vector3(originDirection.x+dodgeOffset, originDirection.y, originDirection.z);
//			Debug.Log(originDirection + "------"+motor.movementDirection);
			motor.facingDirection = (Vector3.zero - transform.position);
			break;
		case 2:	//dead
			motor.movementDirection = Vector3.zero;
			return;
		
		}

		if (motor.movementDirection.sqrMagnitude > 1)
			motor.movementDirection.Normalize ();
	
	}

	private IEnumerator Fire() {
		startFireSignals.SendSignals (this);
		yield return new WaitForSeconds (1);

		stopFireSignals.SendSignals (this);
	}

	private IEnumerator Dodge() {
		float dodgeTime = Random.Range(1, 4);
		yield return new WaitForSeconds (dodgeTime);

		dodgeOffset = 0;
		if(state == 1)
			state = 0;
	}

	private IEnumerator DelayRemoveBody() {
		yield return new WaitForSeconds(2);
		Destroy (gameObject);
		//Debug.Log (transform.rotation);
	}
	

	private float dodgeOffset = 0;

	public void OnHurt() {
		//调整角度跑几秒
		if (state == 0) {
			state = 1;
			dodgeOffset = Random.Range (-500, 500);

			StartCoroutine(Dodge());
		}
	}

	public void OnDead() {
		state = 2;
		StartCoroutine (DelayRemoveBody());
	}

}
