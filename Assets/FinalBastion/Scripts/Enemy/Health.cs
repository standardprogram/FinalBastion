using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public float maxHealth = 100.0f;
	public float health= 100.0f;
	public float regenerateSpeed = 0.0f;
	public bool invincible = false;
	public bool dead = false;

	
	public SignalSender damageSignals;
	public SignalSender dieSignals;
	

	private float colliderRadiusHeuristic = 1.0f;


	void Awake () {
		enabled = false;

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

		
//		Debug.Log("Health = "+health);
		// Enable so the Update function will be called
		// if regeneration is enabled
		if (regenerateSpeed > 0)
			enabled = true;
		

		
		// Die if no health left
		if (health <= 0)
		{
			//GameScore.RegisterDeath (gameObject);
			
			health = 0;
			dead = true;
			dieSignals.SendSignals (this);
			enabled = false;

			SendMessageUpwards("EnemyDead", SendMessageOptions.DontRequireReceiver);
		}
	}


}
