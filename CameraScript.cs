using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{

	private static GameObject MainCamera;
	private static GameObject BattleCamera;
	private static GameObject BattleCamera2;


	// Use this for initialization
	void Start ()
	{
		MainCamera = gameObject;
		BattleCamera = GameObject.Find ("BattleCamera");
		BattleCamera2 = GameObject.Find ("BattleCamera2");
		BattleCamera.SetActive (false);
		BattleCamera2.SetActive (false);
	}

	public static void CameraChange (string onCamera, string offCamera)
	{
		switch (onCamera) {
		case "Main":
			MainCamera.SetActive (true);
			break;
		case "Battle":
			BattleCamera.SetActive (true);
			break;
		case "Battle2":
			BattleCamera2.SetActive (true);
			break;
		}
		switch (offCamera) {
		case "Main":
			MainCamera.SetActive (false);
			break;
		case "Battle":
			BattleCamera.SetActive (false);
			break;
		case "Battle2":
			BattleCamera2.SetActive (false);
			break;
		}

	}
	
	// Update is called once per frame
	void Update ()
	{

	}
}
