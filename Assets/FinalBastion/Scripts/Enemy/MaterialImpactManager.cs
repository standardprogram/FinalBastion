using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class MaterialImpact {
	PhysicMaterial physicMaterial;
	AudioClip[] playerFootstepSounds;
	AudioClip[] mechFootstepSounds;
	AudioClip[] spiderFootstepSounds;
	AudioClip[] bulletHitSounds;
}


public class MaterialImpactManager : MonoBehaviour {

	MaterialImpact[] materials;
	
	private static Dictionary<PhysicMaterial, MaterialImpact> dict;
	private static MaterialImpact defaultMat;
	
	void Awake () {
		defaultMat = materials[0];
		
		dict = new Dictionary<PhysicMaterial, MaterialImpact> ();
		for (int i = 0; i < materials.Length; i++) {
			dict.Add (materials[i].physicMaterial, materials[i]);
		}
	}
	
	static AudioClip GetPlayerFootstepSound (PhysicMaterial mat) {
		var imp : MaterialImpact = GetMaterialImpact (mat);
		return GetRandomSoundFromArray(imp.playerFootstepSounds);
	}
	
	static AudioClip GetMechFootstepSound (PhysicMaterial mat ) {
		var imp : MaterialImpact = GetMaterialImpact (mat);
		return GetRandomSoundFromArray(imp.mechFootstepSounds);
	}
	
	static AudioClip GetSpiderFootstepSound (PhysicMaterial mat) {
		var imp : MaterialImpact = GetMaterialImpact (mat);
		return GetRandomSoundFromArray(imp.spiderFootstepSounds);
	}
	
	static AudioClip GetBulletHitSound (PhysicMaterial mat) {
		var imp : MaterialImpact = GetMaterialImpact (mat);
		return GetRandomSoundFromArray(imp.bulletHitSounds);
	}
	
	static MaterialImpact GetMaterialImpact (PhysicMaterial mat) {
		if (mat && dict.ContainsKey (mat))
			return dict[mat];
		return defaultMat;
	}
	
	static AudioClip GetRandomSoundFromArray (AudioClip[] audioClipArray) {
		if (audioClipArray.Length > 0)
			return audioClipArray[Random.Range (0, audioClipArray.Length)];
		return null;
	}
}
