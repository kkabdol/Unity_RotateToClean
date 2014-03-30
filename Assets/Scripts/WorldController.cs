using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldController : MonoBehaviour
{
	public bool isWorldActive = true;
	public GameObject ballPrefab;
	public BallLayer[] layers;
	public float[] layerDistances;
	public float layerThickness = 0.45f;
	public AudioClip clip;

	[HideInInspector]
	public bool
		isClicked = false;

	private const int ballCountInLayer = 6;
	private List<GameObject> ballPool;
	private Color[] colors = new Color[]{
		Color.green,
		Color.yellow,
		Color.blue,
		Color.red,
	};
	private int clickedLayer = 0;
	private Vector3 oldMouseWorldPos;

	void Awake ()
	{
		int size = layerDistances.Length * ballCountInLayer;
		ballPool = new List<GameObject> (size);
		for (int i=0, imax = size; i<imax; ++i) {
			GameObject newBall = Instantiate (ballPrefab) as GameObject;
			newBall.GetComponent<Ball> ().ThrowAway ();
			ballPool.Add (newBall);
		}
	}

	void Update ()
	{
		UpdateLayerRotating ();
		UpdateWorldActive ();
	}

	void UpdateWorldActive ()
	{
		if (isClicked == true) {
			isWorldActive = false;
			return;
		}

		for (int i=0, imax = layers.Length; i<imax; ++i) {
			if (layers [i].isRotating == true) {
				isWorldActive = false;
				return;
			}
		}

		if (isWorldActive == false) {
			isWorldActive = true;
		}
	}

	void UpdateLayerRotating ()
	{
		if (isClicked == true) {
			Vector3 pos = GetMouseWorldPosition ();
			RotateLayer (layers [clickedLayer].transform, oldMouseWorldPos, pos);
			
			oldMouseWorldPos = pos;
		}
	}

	void RotateLayer (Transform layer, Vector3 from, Vector3 to)
	{
		float angle = Vector3.Angle (from, to);
		angle = Vector3.Cross (from, to).z < 0 ? -angle : angle;

		layer.Rotate (new Vector3 (0f, 0f, angle));
	}

	public void GenerateBall (Transform target)
	{
		GameObject ball = ballPool.Find (x => x.activeSelf == false);
		if (ball != null) {
			Color randomColor = colors [Random.Range (0, colors.Length)];
			ball.GetComponent<Ball> ().Renew (randomColor, target);
		}
	}


	void OnMouseDown ()
	{
		AudioSource.PlayClipAtPoint (clip, Vector3.zero);

		oldMouseWorldPos = GetMouseWorldPosition ();

		clickedLayer = CalculateClickedLayer (oldMouseWorldPos);
		if (clickedLayer >= 0 && clickedLayer < layers.Length) {
			isClicked = true;

		}
	}

	Vector3 GetMouseWorldPosition ()
	{
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = -Camera.main.transform.position.z;
		Vector3 pos = Camera.main.ScreenToWorldPoint (mousePos);
		return pos;
	}

	int CalculateClickedLayer (Vector3 mouseWorldPoint)
	{
		float mouseDistance = Vector3.Distance (new Vector3 (mouseWorldPoint.x, mouseWorldPoint.y, 0f), Vector3.zero);
		Debug.Log ("mouse screen : " + Input.mousePosition);
		Debug.Log ("mouse world : " + mouseWorldPoint);
		Debug.Log ("dist : " + mouseDistance);

		for (int i=0, imax = layerDistances.Length; i<imax; ++i) {
			if (Mathf.Abs (mouseDistance - layerDistances [i]) < layerThickness) {
				return i;
			}
		}

		return -1;
	}

	void OnMouseUp ()
	{
		AudioSource.PlayClipAtPoint (clip, Vector3.zero);
		isClicked = false;
		Debug.Log ("OnMouseUp");
	}
}
