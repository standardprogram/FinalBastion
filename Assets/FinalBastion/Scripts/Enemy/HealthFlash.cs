using UnityEngine;
using System.Collections;

public class HealthFlash : MonoBehaviour {
	HealthFlash playerHealth;
	Material healthMaterial;

	private float healthBlink = 1.0f;
	private float oneOverMaxHealth = 0.5f;

	// Use this for initialization
	void Start () {
		oneOverMaxHealth = 1.0f / playerHealth.oneOverMaxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		float relativeHealth = playerHealth.healthBlink * oneOverMaxHealth;

		healthMaterial.SetFloat ("_SelfIllumination", relativeHealth*2.0f * healthBlink);

		if (relativeHealth < 0.45f)
			healthBlink = Mathf.PingPong (Time.time * 6.0f, 2.0f);
		else 
			healthBlink = 1.0f;
	}
}

