using UnityEngine;
using System.Collections;

public class BallLayer : MonoBehaviour
{
	public float rotateSpeed = 5.0f;

	[HideInInspector]
	public bool
		isRotating = false;

	private float limit = 0.1f;
	private WorldController worldController;

	void Awake ()
	{
		worldController = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<WorldController> ();
	}

	void Update ()
	{
		if (worldController.isClicked == false) {
			float repeat = Mathf.Repeat (transform.rotation.eulerAngles.z, 60f);
			if (repeat < 30f) {
				repeat *= -1;
			} else {
				repeat -= 30f;
			}
		
			if (repeat < -limit || repeat > limit) {
				float angle = repeat * Time.deltaTime * rotateSpeed;
				transform.Rotate (new Vector3 (0f, 0f, angle));
				isRotating = true;
			} else {
				isRotating = false;
			}
		}
	}
}
