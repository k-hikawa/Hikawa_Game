using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class StoryScript : MonoBehaviour
{
	//static AudioSource AudioSource;
	Text storyText;
	private static GameObject storyUI;
	private static string[] mes = new string[0];
	AudioSource returnSource;
	AudioSource cautionSound;
	private GameObject Mark;

	public static void story_script (int story_num)
	{
		switch (story_num) {
		case 0:
			mes = array_push (mes, "ここは...？");
			mes = array_push (mes, "頭が痛い...");
			mes = array_push (mes, "何かを忘れている...");
			mes = array_push (mes, "思い出せない...");
			mes = array_push (mes, "記憶を取り戻さないと...");
			mes = array_push (mes, "ん？");
			mes = array_push (mes, "何だ？あいつは？");
			break;

		

		

		
		}
		PlayerScript.mode = 3;
		storyUI.GetComponent<Canvas> ().enabled = true;
		//AudioSource.volume = 0.1f;
	}

	// Use this for initialization
	void Start ()
	{
		//AudioSource = GameObject.Find ("MainCamera").GetComponent<AudioSource> ();
		storyUI = this.gameObject;
		storyText = GameObject.Find ("StoryText").GetComponentInChildren<Text> ();
		returnSource = storyUI.GetComponent <AudioSource> ();
		AudioSource[] sounds = GameObject.Find ("SoundBox").GetComponents <AudioSource> ();
		cautionSound = sounds [5];
		Mark = GameObject.Find ("Mark");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if ((PlayerScript.mode == 3 || PlayerScript.mode == 4)) {
			if (!storyEventCheck ()) {
				message ();
			} else {
				storyEvent ();
			}
		} else if (StatusScript.story == 0 && PlayerScript.mode == 2) {
			storyUI.GetComponent<Canvas> ().enabled = false;
			mes = array_delete (mes, 0);
			StatusScript.story++;
		}
		if (StatusScript.story >= 1) {
			this.GetComponent <StoryScript> ().enabled = false;
		}



	}

	// エンターで進むメッセージを表示させる
	void message ()
	{
		storyText.text = mes [0];
		if (Input.GetKeyUp (KeyCode.Return)) {
			returnSource.PlayOneShot (returnSource.clip);
			mes = array_delete (mes, 0);
		}

		if (mes.Length == 0) {
			//AudioSource.volume = 0.65f;
			storyUI.GetComponent<Canvas> ().enabled = false;
			StatusScript.story++;
			PlayerScript.mode = 0;
		}
	}

	bool storyEventCheck ()
	{
		if (StatusScript.story == 0) {
			if (mes.Length == 1) {
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}
	}

	void storyEvent ()
	{
		if (StatusScript.story == 0) {
			if (mes.Length == 1) {
				storyText.text = mes [0];
				GameObject slime = GameObject.Find ("スライム レベル1");
				GameObject player = GameObject.Find ("unitychan");
				slime.transform.LookAt (player.transform.position);
				if (PlayerScript.mode != 4) {
					cautionSound.PlayOneShot (cautionSound.clip);
					Mark.GetComponent<Canvas> ().enabled = true;
				}
				slime.transform.position += slime.transform.TransformDirection (Vector3.forward) * 0.17f;
				PlayerScript.mode = 4;
			}
		}
	}

	// string型の配列の末尾に要素を追加する関数
	static string[] array_push (string[] array, string value)
	{
		array.CopyTo (array = new string[array.Length + 1], 0);
		array [array.Length - 1] = value;
		return array;
	}

	// string型の配列のnum番目の要素を削除する関数
	string[] array_delete (string[] array, int num)
	{
		List<string> tempList = new List<string> (array);
		tempList.RemoveAt (num);
		return tempList.ToArray ();
	}
}
