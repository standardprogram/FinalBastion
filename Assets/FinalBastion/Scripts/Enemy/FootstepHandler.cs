using UnityEngine;
using System.Collections;

public class FootstepHandler : MonoBehaviour {
	enum FootType {
		Player,
		Mech,
		Spider
	}


	AudioSource audioSource;
	FootType footType;

	private PhysicMaterial physicMaterial;

	void OnCollisionEnter(Collision collisionInfo)
	{
		physicMaterial = collisionInfo.collider.sharedMaterial;
	}

	void OnFootstep() {
		if (!audioSource.enabled) {
			return;
		}

		AudioClip sound;

		switch (footType) {
		case FootType.Player:
			sound = MaterialImpactManager.GetPlayerFootstepSound(physicMaterial);
			break;
		case FootType.Mech:
			sound = MaterialImpactManager.GetMechFootstepSound(physicMaterial);
			break;
		case FootType.Spider:
			sound = MaterialImpactManager.GetSpiderFootstepSound(physicMaterial);
			break;
		}

		audioSource.pitch = Random.Range (0.98, 1.02);
		audioSource.PlayOneShot (sound, Random.Range (0.8, 1.2));
	}

}

/*
function OnFootstep () {
	if (!audioSource.enabled)
	{
		return;
	}
	
	var sound : AudioClip;
	switch (footType) {
	case FootType.Player:
		sound = MaterialImpactManager.GetPlayerFootstepSound (physicMaterial);
		break;
	case FootType.Mech:
		sound = MaterialImpactManager.GetMechFootstepSound (physicMaterial);
		break;
	case FootType.Spider:
		sound = MaterialImpactManager.GetSpiderFootstepSound (physicMaterial);
		break;
	}	
	audioSource.pitch = Random.Range (0.98, 1.02);
	audioSource.PlayOneShot (sound, Random.Range (0.8, 1.2));
}
*/
