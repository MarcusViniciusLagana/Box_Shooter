using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetShooterMover : MonoBehaviour
{
    public float spinSpeed = 180.0f;
    public float horizontalMotionVelocity = 0.1f;
    public float verticalMotionVelocity = 0.1f;
    public float transitionTime = 1f;

    private float nextTransition;
    private enum motionDirections { Spin, Horizontal, Vertical };
    private motionDirections motionState = motionDirections.Horizontal;
    private Quaternion initial;

    private void Start()    {
        nextTransition = transitionTime + Time.time;
        initial = transform.rotation;
    }

    // Update is called once per frame
    void Update()    {
        switch (motionState)   {
            case motionDirections.Spin:
                // rotate around the up axix of the gameObject
                gameObject.transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime);
                break;
            case motionDirections.Vertical:
                // move up and down over time
                gameObject.transform.Translate(Vector3.up * Time.deltaTime * -verticalMotionVelocity);
                break;

            case motionDirections.Horizontal:
                // move left and right over time
                gameObject.transform.Translate(Vector3.right * Time.deltaTime * horizontalMotionVelocity);
                break;
        }
        if (Time.time > nextTransition)    {
            nextTransition = transitionTime + Time.time;
            switch (motionState)
            {
                case motionDirections.Spin:
                    motionState = motionDirections.Vertical;
                    break;
                case motionDirections.Vertical:
                    transform.SetPositionAndRotation(transform.position,initial);
                    motionState = motionDirections.Horizontal;
                    horizontalMotionVelocity = -horizontalMotionVelocity;
                    break;
                case motionDirections.Horizontal:
                    motionState = motionDirections.Spin;
                    break;
            }
        }
    }
}
