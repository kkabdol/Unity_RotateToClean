using UnityEngine;
using System.Collections;

public class GameUI : MonoBehaviour
{
	void OnGUI ()
	{
		const int width = 50;
		const int height = 50;

		if (GUI.Button (new Rect (Screen.width - width, 0, width, height), "Reset"))
			Application.LoadLevel (0);
		
	}
}
