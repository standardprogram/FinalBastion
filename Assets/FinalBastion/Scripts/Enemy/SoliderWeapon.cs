using UnityEngine;
using System.Collections;

public class SoliderWeapon : MonoBehaviour {

	public void OnDead() {
		Destroy(gameObject);
	}
}
