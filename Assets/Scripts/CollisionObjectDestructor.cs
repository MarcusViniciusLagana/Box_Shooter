using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class CollisionObjectDestructor : MonoBehaviour {

	void OnCollisionEnter(Collision newCollision)	{
		Destroy(gameObject);
	}
}
