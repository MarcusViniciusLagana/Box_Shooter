using UnityEngine;
using System.Collections;
using System.Numerics;

public class TargetShooter : MonoBehaviour {

	// Reference to projectile prefab to shoot
	public GameObject projectile;
	public float power = 10.0f;
	public float shootAfterSeconds = 1f; // how long before shooting the player

	// Reference to AudioClip to play
	public AudioClip shootSFX;

	public int maxNumberProjectiles = 30;
	private GameObject projectilesContainer;

	private float nextShootTime;
	private Transform aim;

	// Use this for initialization
	void Start()	{
		// set the shootTime to be the current time + shootAfterSeconds seconds
		nextShootTime = Time.time + shootAfterSeconds;
		// Set the aim to be the Child tranform component of the Target-Negative object
		aim = GetComponentsInChildren<Transform>()[1];
		projectilesContainer = GameObject.FindWithTag("ProjectilesContainer");
	}

	// Update is called once per frame
	void Update () {
		// continually check to see if past the shoot time
		if (Time.time >= nextShootTime)	{
			// exit if there is a game manager and the game is over
			if (GameManager.gm)	{
				if (GameManager.gm.gameIsOver)
					return;
			}
			// if projectile is specified
			if (projectile && projectilesContainer.transform.childCount <= maxNumberProjectiles)	{
				// Aim and shoot at the player
				aim.LookAt(GameObject.FindWithTag("Player").transform);
				GameObject newProjectile = Instantiate(projectile, aim.position + 2*aim.forward, aim.rotation) as GameObject;
				newProjectile.transform.parent = projectilesContainer.transform;

				// if the projectile does not have a rigidbody component, add one
				if (!newProjectile.GetComponent<Rigidbody>()) 	{
					newProjectile.AddComponent<Rigidbody>();
				}
				// Apply force to the newProjectile's Rigidbody component if it has one
				newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.forward * power, ForceMode.VelocityChange);
				
				// play sound effect if set
				if (shootSFX)	{
					if (newProjectile.GetComponent<AudioSource> ()) { // the projectile has an AudioSource component
						// play the sound clip through the AudioSource component on the gameobject.
						// note: The audio will travel with the gameobject.
						newProjectile.GetComponent<AudioSource> ().PlayOneShot (shootSFX);
					} else {
						// dynamically create a new gameObject with an AudioSource
						// this automatically destroys itself once the audio is done
						AudioSource.PlayClipAtPoint (shootSFX, newProjectile.transform.position);
					}
				}
				nextShootTime = Time.time + shootAfterSeconds;
			}
		}
	}
}
