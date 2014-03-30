using UnityEngine;
using System.Collections;

public class Puller : MonoBehaviour
{
	public float pullingPower = 0.1f;
	public int priority = 0;	// the lower is the higher;
	public bool isBallGenerator = false;

	private GameObject ballPulling = null;
	private WorldController worldController;

	void Awake ()
	{
		worldController = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<WorldController> ();
	}

	void Update ()
	{
		if (isBallGenerator == true &&
			IsPullingBall () == false) {
			worldController.GenerateBall (transform);
		}

		if (IsPullingBall () && ballPulling.activeSelf == false) {
			StopPullingBall ();
		}
	}

	void FixedUpdate ()
	{
		if (worldController.isWorldActive == true &&
			IsPullingBall ()) {

			if (ballPulling.activeSelf) {
				ballPulling.rigidbody.position = Vector3.Lerp (ballPulling.rigidbody.position, 
				                                               transform.position, 
				                                               pullingPower * Time.fixedTime);
			}
		}

	}

	void OnTriggerEnter (Collider other)
	{
		if (IsPullingBall () == false &&
			IsPullableBall (other)) {
			StartPullingBall (other);
		}
	}

	void OnTriggerStay (Collider other)
	{
		if (IsPullingBall () == false &&
			IsPullableBall (other)) {
			StartPullingBall (other);
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (IsPullingBall () == true &&
			IsPullableBall (other)) {
			StopPullingBall ();
		}
	}

	bool IsPullableBall (Collider ballCol)
	{
		if (ballCol.isTrigger == true) {
			return false;
		}

		if (ballCol.tag.Equals (Tags.ball) == false) {
			return false;
		}

		Puller puller = ballCol.gameObject.GetComponent<Ball> ().puller;
		if (puller == null) {
			return true;
		}

		if (puller.priority > this.priority) {
			return true;
		} 

		return false;
	}

	void StartPullingBall (Collider ballCol)
	{
		Ball ball = ballCol.gameObject.GetComponent<Ball> ();
		if (ball.puller != null) {
			ball.puller.StopPullingBall ();
		}

		ball.SetPuller (this);
		ballPulling = ball.gameObject;
	}

	void StopPullingBall ()
	{
		if (ballPulling) {
			Ball ball = ballPulling.GetComponent<Ball> ();
			ball.SetPuller (null);
			ballPulling = null;
		}
	}

	bool IsPullingBall ()
	{
		return (ballPulling != null);
	}
}
