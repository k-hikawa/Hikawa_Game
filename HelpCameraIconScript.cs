using UnityEngine;
using System.Collections;

public class HelpCameraIconScript : MonoBehaviour
{

	GameObject camera;
	GameObject camera2;

	// Use this for initialization
	void Start ()
	{
		camera = GameObject.Find ("CameraIcon");
		camera2 = GameObject.Find ("CameraIcon2");
		camera2.SetActive (false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (PlayerScript.mode == 0) {
			if (CavePlayerScript.help) {
				
				camera.gameObject.SetActive (false);
				camera2.SetActive (true);
			} else {
				
				camera.gameObject.SetActive (true);
				camera2.SetActive (false);
			}
		} else {
			camera.gameObject.SetActive (false);
			camera2.SetActive (false);
		}
	}
}
