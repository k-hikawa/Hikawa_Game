using UnityEngine;
using System.Collections;

public class CaveCameraScript : MonoBehaviour
{
	public static void CameraChange (string onCamera, string offCamera)
	{
		switch (onCamera) {
		case "Main":
			MainCamera.SetActive (true);
			break;
		case "Battle":
			BattleCamera.SetActive (true);
			break;
		
		}
		switch (offCamera) {
		case "Main":
			MainCamera.SetActive (false);
			break;
		case "Battle":
			BattleCamera.SetActive (false);
			break;
		
		}

	}

	private static GameObject MainCamera;
	private static GameObject BattleCamera;

	// Use this for initialization
	void Start ()
	{
		MainCamera = gameObject;
		BattleCamera = GameObject.Find ("BattleCamera");
		BattleCamera.SetActive (false);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
