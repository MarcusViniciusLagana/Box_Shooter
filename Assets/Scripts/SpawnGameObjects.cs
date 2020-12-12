using UnityEngine;
using System.Collections;
using System.Linq;

public class SpawnGameObjects : MonoBehaviour
{
	// public variables
	public float secondsBetweenSpawning = 0.1f;
	public float xMinRange = -25.0f;
	public float xMaxRange = 25.0f;
	public float yMinRange = 8.0f;
	public float yMaxRange = 25.0f;
	public float zMinRange = -25.0f;
	public float zMaxRange = 25.0f;
	public int maxNumberTargets = 30;
	public float ceiling = 25.0f;
	public GameObject[] spawnObjects; // what prefabs to spawn
	public float[] chanceToSpawn; // what the chance of each prefab to spawn prefabs to spawn

	private float nextSpawnTime;
	private Transform floor;
	private Vector3 pos, neg;

	// Use this for initialization
	void Start ()
	{
		// determine when to spawn the next object
		nextSpawnTime = Time.time+secondsBetweenSpawning;
		// determine the limits for the spawn
		floor = GameObject.FindWithTag("Floor").transform;
		pos.x = floor.position.x + 4f * floor.localScale.x + 4;
		neg.x = floor.position.x - 4f * floor.localScale.x - 4;
		pos.z = floor.position.z + 4f * floor.localScale.z + 4;
		neg.z = floor.position.z - 4f * floor.localScale.z - 4;
		pos.y = floor.position.y + ceiling;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// exit if there is a game manager and the game is over
		if (GameManager.gm) {
			if (GameManager.gm.gameIsOver)
				return;
		}

		// if time to spawn a new game object
		if (Time.time  >= nextSpawnTime && transform.childCount <= maxNumberTargets - 1) {
			// Spawn the game object through function below
			MakeThingToSpawn ();

			// determine the next time to spawn the object
			nextSpawnTime = Time.time+secondsBetweenSpawning;
		}
	}

	void MakeThingToSpawn ()
	{
		Vector3 spawnPosition = floor.position;
		spawnPosition.y = pos.y;
		//Vector3 spawnPosition;
		float randomNumber = 1f;
		float cumulatedChance = 0;
		int objectToSpawn;

		// get a random position between the specified ranges
		while (spawnPosition.x <= pos.x && spawnPosition.x >= neg.x && spawnPosition.z <= pos.z && spawnPosition.z >= neg.z && spawnPosition.y <= pos.y)	{
			spawnPosition.x = Random.Range(xMinRange, xMaxRange);
			spawnPosition.y = Random.Range(yMinRange, yMaxRange);
			spawnPosition.z = Random.Range(zMinRange, zMaxRange);
		}

		// determine which object to spawn
		while(randomNumber==1f)
			randomNumber = Random.value;

		if (chanceToSpawn.Length != spawnObjects.Length || chanceToSpawn.Sum() != 1)
			objectToSpawn = Mathf.FloorToInt(randomNumber * spawnObjects.Length);
		else {
			for (objectToSpawn = 0; objectToSpawn < spawnObjects.Length; objectToSpawn++)	{
				cumulatedChance += chanceToSpawn[objectToSpawn];
				if (randomNumber < cumulatedChance) {
					break;
				}
			}
		}
		// actually spawn the game object
		GameObject spawnedObject = Instantiate (spawnObjects [objectToSpawn], spawnPosition, transform.rotation) as GameObject;

		// make the parent the spawner so hierarchy doesn't get super messy
		spawnedObject.transform.parent = gameObject.transform;
	}
}
