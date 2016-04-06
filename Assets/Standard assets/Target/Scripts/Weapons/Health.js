#pragma strict

public var maxHealth : float = 100.0;
public var health : float = 100.0;
public var regenerateSpeed : float = 0.0;
public var invincible : boolean = false;
public var dead : boolean = false;

public var damagePrefab : GameObject;
public var damageEffectTransform : Transform;
public var damageEffectMultiplier : float = 1.0;
public var damageEffectCentered : boolean = true;


public var damageSignals : SignalSender;
public var dieSignals : SignalSender;

private var lastDamageTime : float = 0;
private var damageEffect : ParticleEmitter;
private var damageEffectCenterYOffset : float;

private var colliderRadiusHeuristic : float = 1.0;


function Awake () {
	enabled = false;
	if (damagePrefab) {
		if (damageEffectTransform == null)
			damageEffectTransform = transform;
		var effect : GameObject = Spawner.Spawn (damagePrefab, Vector3.zero, Quaternion.identity);
		effect.transform.parent = damageEffectTransform;
		effect.transform.localPosition = Vector3.zero;
		damageEffect = effect.particleEmitter;
		var tempSize : Vector2 = Vector2(collider.bounds.extents.x,collider.bounds.extents.z);
		colliderRadiusHeuristic = tempSize.magnitude * 0.5;
		damageEffectCenterYOffset = collider.bounds.extents.y;

	}

}


function OnDamage (amount : float, fromDirection : Vector3) {
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
			var dir : Vector3 = fromDirection;
			dir.y = 0.0;
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

function OnEnable () {
	Regenerate ();
}

// Regenerate health

function Regenerate () {
	if (regenerateSpeed > 0.0f) {
		while (enabled) {
			if (Time.time > lastDamageTime + 3) {
				health += regenerateSpeed;

				yield;

				if (health >= maxHealth) {
					health = maxHealth;
					enabled = false;
				}
			}
			yield WaitForSeconds (1.0f);
		}
	}
}
