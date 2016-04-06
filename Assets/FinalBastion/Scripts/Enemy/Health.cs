using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public float maxHealth = 100.0f;
	public float health= 100.0f;
	public float regenerateSpeed = 0.0f;
	public bool invincible = false;
	public bool dead = false;
	
	public GameObject damagePrefab;
	public Transform damageEffectTransform;
	public float damageEffectMultiplier = 1.0f;
	public bool damageEffectCentered = true;
	
	
	public SignalSender damageSignals;
	public SignalSender dieSignals;
	
	private float lastDamageTime = 0f;
	private ParticleEmitter damageEffect ;
	private float damageEffectCenterYOffset;
	
	private float colliderRadiusHeuristic = 1.0f;


	void Awake () {
		enabled = false;
		if (damagePrefab) {
			if (damageEffectTransform == null)
				damageEffectTransform = transform;
			GameObject effect = Spawner.Spawn (damagePrefab, Vector3.zero, Quaternion.identity);
			effect.transform.parent = damageEffectTransform;
			effect.transform.localPosition = Vector3.zero;
			damageEffect = effect.particleEmitter;
			Vector2 tempSize = new Vector2(collider.bounds.extents.x,collider.bounds.extents.z);
			colliderRadiusHeuristic = tempSize.magnitude * 0.5f;
			damageEffectCenterYOffset = collider.bounds.extents.y;
			
		}
		
	}

	
	public void OnDamage (float amount, Vector3 fromDirection) {
		// Take no damage if invincible, dead, or if the damage is zero
		if(invincible)
			return;
		if (dead)
			return;
		if (amount <= 0)
			return;
		
		health -= amount;
		damageSignals.SendSignals (this);
		lastDamageTime = Time.time;
		
		Debug.Log("Health = "+health);
		// Enable so the Update function will be called
		// if regeneration is enabled
		if (regenerateSpeed > 0)
			enabled = true;
		
		// Show damage effect if there is one
		if (damageEffect) {
			damageEffect.transform.rotation = Quaternion.LookRotation (fromDirection, Vector3.up);
			if(!damageEffectCentered) {
				Vector3 dir  = fromDirection;
				dir.y = 0.0f;
				damageEffect.transform.position = (transform.position + Vector3.up * damageEffectCenterYOffset) + colliderRadiusHeuristic * dir;
			}
			// @NOTE: due to popular demand (ethan, storm) we decided
			// to make the amount damage independent ...
			//var particleAmount = Random.Range (damageEffect.minEmission, damageEffect.maxEmission + 1);
			//particleAmount = particleAmount * amount * damageEffectMultiplier;
			damageEffect.Emit();// (particleAmount);
			
		}
		
		// Die if no health left
		if (health <= 0)
		{
			//GameScore.RegisterDeath (gameObject);
			
			health = 0;
			dead = true;
			dieSignals.SendSignals (this);
			enabled = false;
			
		}
	}

	
	void OnEnable () {
		Regenerate ();
	}
	
	// Regenerate health
	
	IEnumerator Regenerate () {
		if (regenerateSpeed > 0.0f) {
			while (enabled) {
				if (Time.time > lastDamageTime + 3) {
					health += regenerateSpeed;

					if (health >= maxHealth) {
						health = maxHealth;
						enabled = false;
					}
				}
				yield return new WaitForSeconds (1.0f);
			}
		}
	}
}
