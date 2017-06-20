using UnityEngine;
using System.Collections;

public class EnemychanEncountScript : MonoBehaviour
{

	int front_time;
	bool event_start;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (PlayerScript.mode == 3 && StatusScript.story == 2) {
			//バトル開始
		}
	}

	void OnCollisionEnter (Collision other)
	{
		if (other.gameObject.name == "unitychan") {
			StoryScript.story_script (1);
		}
	}
}
