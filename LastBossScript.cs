using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LastBossScript : MonoBehaviour
{
	Text storyText;
	private static GameObject storyUI;
	private static string[] mes = new string[0];
	AudioSource returnSource;
	AudioSource fanfareSource;
	AudioSource MainSource;



	// Use this for initialization
	void Start ()
	{
		storyUI = GameObject.Find ("Story");
		storyText = GameObject.Find ("StoryText").GetComponentInChildren<Text> ();
		returnSource = storyUI.GetComponent <AudioSource> ();
		AudioSource[] sounds = GameObject.Find ("SoundBox").GetComponents <AudioSource> (); 
		fanfareSource = sounds [11];
		MainSource = GameObject.Find ("MainCamera").GetComponent <AudioSource> ();
		if (StatusScript.story >= 6) {
			this.GetComponent <BoxCollider> ().isTrigger = true;
		}

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (StatusScript.story == 5) {
			message ();
			if (mes.Length == 0) {
				StatusScript.story = 6;
			}
		}

		if (StatusScript.story == 7) {
			PlayerScript.mode = 3;
			mes_into ();
			storyUI.GetComponent<Canvas> ().enabled = true;
			StatusScript.story = 8;
		}
		if (StatusScript.story == 8) {
			message ();
			if (mes.Length == 0) {
				PlayerPrefs.SetFloat ("x", 23.51092f);
				PlayerPrefs.SetFloat ("y", 0f);
				PlayerPrefs.SetFloat ("z", 46.97041f);
				PlayerPrefs.SetFloat ("rx", 0);
				PlayerPrefs.SetFloat ("ry", 96.98601f);
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
				PlayerPrefs.SetInt ("story", 6);
				PlayerPrefs.SetInt ("cave", StatusScript.cave);
				PlayerPrefs.Save ();

				SceneManager.LoadScene ("Ending");
			}
		}
	}

	void OnCollisionEnter (Collision other)
	{
		if (other.gameObject.name == "unitychan") {
			if (StatusScript.story == 4) {
				StoryScript.story_script (4);
				PlayerScript.mode = 3;
				storyUI.GetComponent<Canvas> ().enabled = true;
				mes_into ();
				StatusScript.story = 5;

				this.GetComponent <BoxCollider> ().isTrigger = true;
				PlayerPrefs.SetFloat ("x", GameObject.Find ("unitychan").transform.position.x);
				PlayerPrefs.SetFloat ("y", GameObject.Find ("unitychan").transform.position.y);
				PlayerPrefs.SetFloat ("z", GameObject.Find ("unitychan").transform.position.z);
				PlayerPrefs.SetFloat ("rx", GameObject.Find ("unitychan").transform.eulerAngles.x);
				PlayerPrefs.SetFloat ("ry", GameObject.Find ("unitychan").transform.eulerAngles.y);
				PlayerPrefs.SetFloat ("rz", GameObject.Find ("unitychan").transform.eulerAngles.z);
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
				PlayerPrefs.SetInt ("cave", StatusScript.cave);
				PlayerPrefs.Save ();
			}

		}
	}

	void mes_into ()
	{
		switch (StatusScript.story) {
		case 4:
			mes = array_push (mes, "こ、これがドラゴンキング！？");
			mes = array_push (mes, "なんて大きさなの！？");
			mes = array_push (mes, "...");
			mes = array_push (mes, "...");
			mes = array_push (mes, "「...大丈夫、力を貸してあげる...」");
			mes = array_push (mes, "...！！！enemychan...!!");
			mes = array_push (mes, "そうだ！今の私なら大丈夫！");
			mes = array_push (mes, "これが最後の戦い！いくよっ！！");
			break;
		case 7:
			MainSource.Stop ();
			fanfareSource.PlayOneShot (fanfareSource.clip);
			mes = array_push (mes, "ついに...");
			mes = array_push (mes, "ついにドラゴンキングを倒した！");
			mes = array_push (mes, "おめでとう！ゲームクリア！！");
			mes = array_push (mes, "やったね！");
			break;
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
			//StatusScript.story++;
			PlayerScript.mode = 0;
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
