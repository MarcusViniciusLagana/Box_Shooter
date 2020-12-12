using UnityEngine;
using System.Collections;

public class TargetBehavior : MonoBehaviour
{

	// target impact on game
	public int minScoreAmount = 0;
	public int maxScoreAmount = 0;
	public float timeAmount = 0.0f;

	// explosion when hit?
	public GameObject explosionPrefab;

	// when collided with another gameObject
	void OnCollisionEnter (Collision newCollision)
	{
		int scoreAmount = Random.Range(minScoreAmount, maxScoreAmount + 1);
		
		// exit if there is a game manager and the game is over
		if (GameManager.gm) {
			if (GameManager.gm.gameIsOver)
				return;
		}

		// only do stuff if hit by a projectile
		if (newCollision.gameObject.tag == "Projectile" || (newCollision.gameObject.tag == "TargetProjectile")){
			// if hit by player and game manager exists, make adjustments based on target properties
			if (GameManager.gm && newCollision.gameObject.tag == "Projectile")
				GameManager.gm.targetHit(scoreAmount, timeAmount);
			if (explosionPrefab)	{
				// Instantiate an explosion effect at the gameObjects position and rotation
				GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
				if (transform.tag == "Target")	{
					explosion.transform.LookAt(GameObject.FindWithTag("Player").transform);
					if (scoreAmount >= 0)	{
						explosion.GetComponentInChildren<TextMesh>().text = "+" + scoreAmount.ToString();
						if (scoreAmount==0)
							explosion.GetComponentInChildren<TextMesh>().color = new Color(1, 1, 1, 1);
						else
							explosion.GetComponentInChildren<TextMesh>().color = new Color(0, 1, 223f / 255, 1);
					}
					else	{
						explosion.GetComponentInChildren<TextMesh>().text = scoreAmount.ToString();
						explosion.GetComponentInChildren<TextMesh>().color = new Color(1, 223f / 255, 0, 1);
					}
				}
				// if hit by another target, do not play any sound
				if (newCollision.gameObject.tag == "TargetProjectile")
				{
					explosion.GetComponent<ParticleSystem>().GetComponent<AudioSource>().Stop();
				}
			}
				
			// destroy the projectile -> this function is now being executed by CollisionObjectDestructor script on projectile objects.
			//Destroy (newCollision.gameObject);
				
			// destroy self
			Destroy (gameObject);
		}
	}
}
