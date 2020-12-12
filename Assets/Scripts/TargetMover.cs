using UnityEngine;
using System.Collections;

public class TargetMover : MonoBehaviour {

	// define the possible states through an enumeration
	public enum motionDirections {Spin, Horizontal, Vertical};
	public enum additionalDirections { None, Spin, Horizontal, Vertical };

	// store the state
	public motionDirections motionState = motionDirections.Horizontal;
	public additionalDirections additionalMotionState = additionalDirections.None;

	// motion parameters
	public float spinSpeed = 180.0f;
	public float horizontalMotionMagnitude = 0.1f;
	public float verticalMotionMagnitude = 0.1f;

    private void Start()
    {
		float xORz = Random.value;
		if (xORz >= 0.5f)
			transform.Rotate(Vector3.up * 90);
	}

    // Update is called once per frame
    void Update () {

		// do the appropriate motion based on the motionState
		switch(motionState) {
			case motionDirections.Spin:
				if (additionalMotionState == additionalDirections.Spin)
					additionalMotionState = additionalDirections.None;
				// rotate around the up axix of the gameObject
				gameObject.transform.Rotate (Vector3.up * spinSpeed * Time.deltaTime);
				break;
			
			case motionDirections.Vertical:
				if (additionalMotionState == additionalDirections.Vertical)
					additionalMotionState = additionalDirections.None;
				// move up and down over time
				gameObject.transform.Translate(Vector3.up * Mathf.Cos(Time.timeSinceLevelLoad) * verticalMotionMagnitude);
				break;

            case motionDirections.Horizontal:
				if (additionalMotionState == additionalDirections.Horizontal)
					additionalMotionState = additionalDirections.None;
				// move left and right over time
				gameObject.transform.Translate(Vector3.right * Mathf.Cos(Time.timeSinceLevelLoad) * horizontalMotionMagnitude);
                break;
		}
		switch (additionalMotionState)
		{
			case additionalDirections.Spin:
				// rotate around the up axix of the gameObject
				gameObject.transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime);
				break;

			case additionalDirections.Vertical:
				// move up and down over time
				gameObject.transform.Translate(Vector3.up * Mathf.Cos(Time.timeSinceLevelLoad) * verticalMotionMagnitude);
				break;

			case additionalDirections.Horizontal:
				// move up and down over time
				gameObject.transform.Translate(Vector3.right * Mathf.Cos(Time.timeSinceLevelLoad) * horizontalMotionMagnitude);
				break;
		}
	}
}
