using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    [Range(1,100)]
    private float speed = 50;

    [Range(1, 25)]
    private float zoomSpeed = 15;

    private float speedMultiplier = 2f;

    [SerializeField]
    private Transform rotateAroundTarget;

	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            speed *= speedMultiplier;
            zoomSpeed *= speedMultiplier;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            speed /= speedMultiplier;
            zoomSpeed /= speedMultiplier;
        }

        // Zoom
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.forward * zoomSpeed * Time.deltaTime);
            //transform.RotateAround(rotateAroundTarget.position, Vector3.left, speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.back * zoomSpeed * Time.deltaTime);
            //transform.RotateAround(rotateAroundTarget.position, Vector3.right, speed * Time.deltaTime);
        }

        // Rotate
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.RotateAround(rotateAroundTarget.position, Vector3.up, speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.RotateAround(rotateAroundTarget.position, Vector3.down, speed * Time.deltaTime);
        }
    }

    public void ChangeTarget(Transform whatTarget)
    {
        rotateAroundTarget = whatTarget;
    }
}
