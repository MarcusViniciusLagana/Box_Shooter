using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockerHealth : MonoBehaviour
{
    public int health = 10;
    // explosion when hit?
    public GameObject explosionPrefab;
	public AudioClip hit;

	private Color actualColor = new Color(1, 1, 1, 1);
	private Color subColor = new Color(17f/255f, 17f/255f, 17f/255f, 0);

	void OnCollisionEnter(Collision newCollision)	{
		// exit if there is a game manager and the game is over
		if (GameManager.gm)	{
			if (GameManager.gm.gameIsOver)
				return;
		}

		// only do stuff if hit by a projectile
		if (newCollision.gameObject.tag == "Projectile" || newCollision.gameObject.tag == "TargetProjectile")	{
			health -= 1;
			if (health <= 0)	{
				if (explosionPrefab)	{
					GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
				}
				// destroy self
				Destroy(gameObject);
			}
			else	{
				actualColor -= subColor;
				this.gameObject.GetComponent<Renderer>().material.SetColor("_Color",actualColor);
				if (hit)
					AudioSource.PlayClipAtPoint(hit, transform.position, 2);
			}
		}
	}
}
