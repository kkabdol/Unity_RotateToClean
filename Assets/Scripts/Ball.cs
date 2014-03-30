using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ball : MonoBehaviour
{
	public AudioClip throwAwaySound;

	[HideInInspector]
	public Puller
		puller = null;

	private const int ballStackLimit = 2;
	private List<GameObject> ballStack = new List<GameObject> ();
	private WorldController worldController;

	void Awake ()
	{
		worldController = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<WorldController> ();
	}

	void Update ()
	{
		if (worldController.isWorldActive == true) {
			UpdateThrowAway ();
		}
	}

	void UpdateThrowAway ()
	{
		if (ballStack.Count >= ballStackLimit) {
			AudioSource.PlayClipAtPoint (throwAwaySound, transform.position);

			for (int i=0, imax=ballStack.Count; i<imax; ++i) {
				ballStack [i].GetComponent<Ball> ().ThrowAway ();
			}
			ThrowAway ();
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (IsSameKindBall (other) &&
			ballStack.Find (x => x == other.gameObject) == null) {
			ballStack.Add (other.gameObject);
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (IsSameKindBall (other)) {
			ballStack.Remove (other.gameObject);
		}
	}

	bool IsSameKindBall (Collider other)
	{
		return (other.isTrigger == false &&
			other.tag.Equals (Tags.ball) &&
			other.renderer.material.color == renderer.material.color);
	}

	public void ThrowAway ()
	{
		ballStack.Clear ();
		gameObject.transform.position = new Vector3 (1000f, 1000f, 1000f);
		gameObject.SetActive (false);
	}

	public void Renew (Color color, Transform target)
	{
		renderer.material.color = color;
		transform.parent = target.parent;
		transform.position = target.position;
		transform.rotation = target.rotation;
		gameObject.SetActive (true);
	}

	public void SetPuller (Puller newPuller)
	{
		puller = newPuller;

		if (newPuller != null) {
			transform.parent = newPuller.transform.parent;
		} else {
			transform.parent = null;
		}
	}
}
