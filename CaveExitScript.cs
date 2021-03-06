﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CaveExitScript : MonoBehaviour
{
	private MeshRenderer exit;
	public static bool caveExit;
	// Use this for initialization
	void Start ()
	{
		exit = this.gameObject.GetComponent <MeshRenderer> ();
		if (StatusScript.story >= 3) {
			exit.enabled = true;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void OnCollisionEnter (Collision other)
	{
		if (other.gameObject.name == "unitychan") {
			
			PlayerPrefs.SetFloat ("x", 481.44f);
			PlayerPrefs.SetFloat ("y", 1.09f);
			PlayerPrefs.SetFloat ("z", 313.3017f);
			PlayerPrefs.SetFloat ("rx", 0f);
			PlayerPrefs.SetFloat ("ry", 18.712f);
			PlayerPrefs.SetFloat ("rz", 0f);
			PlayerPrefs.SetFloat ("t", StatusScript.time);
			PlayerPrefs.SetInt ("g", StatusScript.gameOver);
			PlayerPrefs.SetInt ("lv", StatusScript.level);
			PlayerPrefs.SetInt ("e", StatusScript.exp);
			PlayerPrefs.SetInt ("mh", StatusScript.max_hp);
			PlayerPrefs.SetInt ("h", StatusScript.hp);
			PlayerPrefs.SetInt ("mm", StatusScript.max_mp);
			PlayerPrefs.SetInt ("m", StatusScript.mp);
			PlayerPrefs.SetInt ("a", StatusScript.attack);
			PlayerPrefs.SetInt ("d", StatusScript.defence);
			PlayerPrefs.SetInt ("s", StatusScript.speed);
			PlayerPrefs.SetInt ("l", StatusScript.lack);
			for (int i = 0; i < 10; i++) {
				if (StatusScript.item.Length > i) {
					PlayerPrefs.SetString ("item" + i, StatusScript.item [i]);
				} else {
					PlayerPrefs.SetString ("item" + i, "");
				}
				if (StatusScript.skil.Length > i) {
					PlayerPrefs.SetString ("skil" + i, StatusScript.skil [i]);
				} else {
					PlayerPrefs.SetString ("skil" + i, "");
				}
				if (StatusScript.magic.Length > i) {
					PlayerPrefs.SetString ("magic" + i, StatusScript.magic [i]);
				} else {
					PlayerPrefs.SetString ("magic" + i, "");
				}
			}
			PlayerPrefs.SetInt ("r", StatusScript.remagic);
			PlayerPrefs.SetInt ("story", StatusScript.story);
			PlayerPrefs.SetInt ("cave", 0);
			PlayerPrefs.Save ();

			caveExit = true;
			SceneManager.LoadScene ("Main");

		}

	}
}
