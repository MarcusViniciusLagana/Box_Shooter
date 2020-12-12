using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour	{

	// Player impact on game
	public float reducedTimeAmount = -0.5f;

	// Reference to AudioClip to play
	public AudioClip hurtSFX;

	// when collided with another gameObject
	void OnCollisionEnter (Collision newCollision)	{
		// exit if there is a game manager and the game is over
		if (GameManager.gm) {
			if (GameManager.gm.gameIsOver)
				return;
		}

		// only do stuff if hit by a Negative Box Projectile
		if (newCollision.gameObject.tag == "TargetProjectile")	{

			if (hurtSFX)	{
				// dynamically create a new gameObject with an AudioSource
				// this automatically destroys itself once the audio is done
				AudioSource.PlayClipAtPoint(hurtSFX, transform.position, 0.5f);
			}

			// if game manager exists, make time adjustments
			if (GameManager.gm) {
				GameManager.gm.targetHit (0, reducedTimeAmount);
			}
		}
	}
}
