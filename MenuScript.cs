using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuScript : MonoBehaviour
{

	Text myText;
	Text listText;
	Text messageText;
	int num;

	private bool item_active;
	private bool magic_active;
	private bool skil_active;
	int active;


	AudioSource systemSound;
	AudioSource returnSound;
	AudioSource magicSound;
	AudioSource healSound;

	int wait;
	int recover;
	int mp_recover;

	// Use this for initialization
	void Start ()
	{
		AudioSource[] sounds = GameObject.Find ("SoundBox").GetComponents <AudioSource> ();
		systemSound = sounds [0];
		returnSound = sounds [2];
		magicSound = sounds [3];
		healSound = sounds [4];

		myText = GetComponentInChildren<Text> ();
		listText = GameObject.Find ("ListText").GetComponentInChildren<Text> ();
		messageText = GameObject.Find ("MessageText").GetComponentInChildren<Text> ();
		num = 0;
	}

	// Update is called once per frame
	void Update ()
	{
		if (PlayerScript.getMode () == 1) {
			if (wait != 0) {
				wait--;
			} else if (recover != 0) {
				healSound.PlayOneShot (healSound.clip);
				messageText.text = "体力が" + recover + "回復した！";
				wait = 40;
				recover = 0;
			} else if (mp_recover != 0) {
				healSound.PlayOneShot (healSound.clip);
				messageText.text = "魔力が" + mp_recover + "回復した！";
				wait = 40;
				mp_recover = 0;
			} else {

				GameObject.Find ("ListText").GetComponent <Text> ().enabled = true;
				if (item_active) {
					item_list ();
				} else if (skil_active) {
					skil_list ();
				} else if (magic_active) {
					magic_list ();
				} else {
					if (Input.GetKeyDown (KeyCode.DownArrow)) {
						listText.text = "";
						systemSound.PlayOneShot (systemSound.clip);
						if (num == 6) {
							num = 0;
						} else {
							num++;
						}
					}
					if (Input.GetKeyDown (KeyCode.UpArrow)) {
						systemSound.PlayOneShot (systemSound.clip);
						listText.text = "";
						if (num == 0) {
							num = 6;
						} else {
							num--;
						}
					}
					switch (num) {
					case 0:
						myText.text = "▶アイテム\n攻撃技\n魔法\nセーブ\nデータ削除\n通信対戦\n閉じる";
						messageText.text = "所持しているアイテムを使用することができます。";
						GameObject.Find ("ListText").GetComponent <Text> ().enabled = true;
						listText.text = "";
						for (int i = 0; i < StatusScript.item.Length; i++) {
							listText.text += StatusScript.item [i] + "\n";
						}
						if (Input.GetKeyUp (KeyCode.Return)) {
							returnSound.PlayOneShot (returnSound.clip);
							myText.text = "アイテム\n攻撃技\n魔法\nセーブ\nデータ削除\n通信対戦\n閉じる";
							item_active = true;
						}
						break;
					case 1:
						myText.text = "アイテム\n▶攻撃技\n魔法\nセーブ\nデータ削除\n通信対戦\n閉じる";
						messageText.text = "習得している攻撃技を確認します。";
						GameObject.Find ("ListText").GetComponent <Text> ().enabled = true;
						listText.text = "";
						for (int i = 0; i < StatusScript.skil.Length; i++) {
							listText.text += StatusScript.skil [i] + "\n";
						}
						if (Input.GetKeyUp (KeyCode.Return)) {
							returnSound.PlayOneShot (returnSound.clip);
							myText.text = "アイテム\n攻撃技\n魔法\nセーブ\nデータ削除\n通信対戦\n閉じる";
							skil_active = true;
						}
						break;
					case 2:
						myText.text = "アイテム\n攻撃技\n▶魔法\nセーブ\nデータ削除\n通信対戦\n閉じる";
						messageText.text = "習得している魔法を使用することができます。";
						GameObject.Find ("ListText").GetComponent <Text> ().enabled = true;
						listText.text = "";
						for (int i = 0; i < StatusScript.magic.Length; i++) {
							listText.text += StatusScript.magic [i] + "\n";
						}
						if (Input.GetKeyUp (KeyCode.Return)) {
							returnSound.PlayOneShot (returnSound.clip);
							myText.text = "アイテム\n攻撃技\n魔法\nセーブ\nデータ削除\n通信対戦\n閉じる";
							magic_active = true;
						}
						break;
					case 3:
						myText.text = "アイテム\n攻撃技\n魔法\n▶セーブ\nデータ削除\n通信対戦\n閉じる";
						int second = Mathf.FloorToInt (StatusScript.time) % 60;
						int minute = (Mathf.FloorToInt (StatusScript.time) / 60) % 60;
						int hour = Mathf.FloorToInt (Mathf.FloorToInt (StatusScript.time) / 3600);
						string playTime = time_indent (second, minute, hour);
						messageText.text = "ここまでの冒険の記録します。\nプレイ時間：" + playTime + " ゲームオーバー" + StatusScript.gameOver + "回";
						if (Input.GetKeyUp (KeyCode.Return)) {
							returnSound.PlayOneShot (returnSound.clip);
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
							GameObject.Find ("ListText").GetComponent <Text> ().enabled = true;
							listText.text = "セーブしました";
						}

						break;
					case 4:
						
						myText.text = "アイテム\n攻撃技\n魔法\nセーブ\n▶データ削除\n通信対戦\n閉じる";
						messageText.text = "セーブデータを削除します。";
						if (Input.GetKeyUp (KeyCode.Return)) {
							PlayerPrefs.DeleteAll ();
							returnSound.PlayOneShot (returnSound.clip);
							GameObject.Find ("ListText").GetComponent <Text> ().enabled = true;
							listText.text = "セーブデータを削除しました。\n※セーブせずにゲームを終了した場合、初めからになります。";
						}
						break;
					case 5:
						
						myText.text = "アイテム\n攻撃技\n魔法\nセーブ\nデータ削除\n▶通信対戦\n閉じる";
						messageText.text = "通信対戦を行います。";
						if (Input.GetKeyUp (KeyCode.Return)) {
							returnSound.PlayOneShot (returnSound.clip);
							GameObject.Find ("ListText").GetComponent <Text> ().enabled = true;
							listText.text = "未実装です...";
						}
						break;
					case 6:
						listText.text = "";
						myText.text = "アイテム\n攻撃技\n魔法\nセーブ\nデータ削除\n通信対戦\n▶閉じる";
						messageText.text = "メニューを閉じてゲームを再開します。";
						GameObject.Find ("ListText").GetComponent <Text> ().enabled = false;
						if (Input.GetKeyUp (KeyCode.Return)) {
							returnSound.PlayOneShot (returnSound.clip);
							num = 0;
							GameObject.Find ("Menu").GetComponent<Canvas> ().enabled = false;
							GameObject.Find ("ListText").GetComponent <Text> ().enabled = false;
							PlayerScript.changeMode (0);
						}
						break;
					}
				}
			}

		}
	}

	void item_list ()
	{
		listText.text = "";
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			systemSound.PlayOneShot (systemSound.clip);
			if (active == StatusScript.item.Length) {
				active = 0;
			} else {
				active++;
			}
		}
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			systemSound.PlayOneShot (systemSound.clip);
			if (active == 0) {
				active = StatusScript.item.Length;
			} else {
				active--;
			}
		}
		for (int i = 0; i < StatusScript.item.Length; i++) {
			if (active == i) {
				listText.text += "▶";
				messageText.text = item_message (StatusScript.item [i]);

			}
			listText.text += StatusScript.item [i] + "\n";
		}

		if (active == StatusScript.item.Length) {
			listText.text += "▶";
			messageText.text = "メニューに戻ります。";
			if (Input.GetKeyUp (KeyCode.Return)) {
				returnSound.PlayOneShot (returnSound.clip);
				myText.text = "▶アイテム\n攻撃技\n魔法\nセーブ\nデータ削除\n通信対戦\n閉じる";
				active = 0;
				item_active = false;
			}
		} else {
			if (Input.GetKeyUp (KeyCode.Return)) {
				returnSound.PlayOneShot (returnSound.clip);
				StatusScript.item = item_consume (StatusScript.item, active);
			}
		}
		listText.text += "戻る";

	}

	void skil_list ()
	{
		listText.text = "";
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			systemSound.PlayOneShot (systemSound.clip);
			if (active == StatusScript.skil.Length) {
				active = 0;
			} else {
				active++;
			}
		}
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			systemSound.PlayOneShot (systemSound.clip);
			if (active == 0) {
				active = StatusScript.skil.Length;
			} else {
				active--;
			}
		}
		for (int i = 0; i < StatusScript.skil.Length; i++) {
			if (active == i) {
				listText.text += "▶";
				messageText.text = skil_message (StatusScript.skil [i]);
			}
			listText.text += StatusScript.skil [i] + "\n";
		}
		if (active == StatusScript.skil.Length) {
			listText.text += "▶";
			messageText.text = "メニューに戻ります。";
			if (Input.GetKeyUp (KeyCode.Return)) {
				returnSound.PlayOneShot (returnSound.clip);
				myText.text = "アイテム\n▶攻撃技\n魔法\nセーブ\nデータ削除\n通信対戦\n閉じる";
				active = 0;
				skil_active = false;
			}
		}
		listText.text += "戻る";
	}

	void magic_list ()
	{
		listText.text = "";
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			systemSound.PlayOneShot (systemSound.clip);
			if (active == StatusScript.magic.Length) {
				active = 0;
			} else {
				active++;
			}
		}
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			systemSound.PlayOneShot (systemSound.clip);
			if (active == 0) {
				active = StatusScript.magic.Length;
			} else {
				active--;
			}
		}
		for (int i = 0; i < StatusScript.magic.Length; i++) {
			if (active == i) {
				listText.text += "▶";
				messageText.text = magic_message (StatusScript.magic [i]);
			}
			listText.text += StatusScript.magic [i] + "\n";
		}
		if (active == StatusScript.magic.Length) {
			listText.text += "▶";
			messageText.text = "メニューに戻ります。";
			if (Input.GetKeyUp (KeyCode.Return)) {
				returnSound.PlayOneShot (returnSound.clip);
				myText.text = "アイテム\n攻撃技\n▶魔法\nセーブ\nデータ削除\n通信対戦\n閉じる";
				active = 0;
				magic_active = false;
			}
		} else {
			if (Input.GetKeyUp (KeyCode.Return)) {
				returnSound.PlayOneShot (returnSound.clip);
				magic_use (StatusScript.magic [active]);

			}
		}
		listText.text += "戻る";
	}



	public static string item_message (string item_name)
	{
		string message = "";
		switch (item_name) {
		case "やくそう":
			message = "体力を50回復する。";
			break;
		case "せいすい":
			message = "魔力を50回復する。";
			break;
		case "魔法薬":
			message = "魔力を200回復する。";
			break;
		case "上やくそう":
			message = "体力を150回復する。";
			break;
		case "回復薬":
			message = "体力を300回復する。";
			break;
		case "ウサミミ":
			message = "戦闘から逃走する。必ず逃げられる。";
			break;
		}
		return message;
	}

	// アイテムの消費
	string[] item_consume (string[] itemArray, int num)
	{
		bool use = false;
		switch (itemArray [num]) {
		case "やくそう":
			if (StatusScript.hp != StatusScript.max_hp) {
				messageText.text = "やくそうを使った！";
				wait = 40;
				recover = 50;
				StatusScript.hp += 50;
				if (StatusScript.hp > StatusScript.max_hp) {
					StatusScript.hp = StatusScript.max_hp;
				}
				use = true;
			}
			break;
		case "せいすい":
			if (StatusScript.mp != StatusScript.max_mp) {
				messageText.text = "せいすいを使った！";
				wait = 40;
				mp_recover = 50;
				StatusScript.mp += 50;
				if (StatusScript.mp > StatusScript.max_mp) {
					StatusScript.mp = StatusScript.max_mp;
				}
				use = true;
			}
			break;
		case "上やくそう":
			if (StatusScript.hp != StatusScript.max_hp) {
				messageText.text = "上やくそうを使った！";
				wait = 40;
				recover = 150;
				StatusScript.hp += 150;
				if (StatusScript.hp > StatusScript.max_hp) {
					StatusScript.hp = StatusScript.max_hp;
				}
				use = true;
			}
			break;
		case "回復薬":
			if (StatusScript.hp != StatusScript.max_hp) {
				messageText.text = "回復薬を使った！";
				wait = 40;
				recover = 300;
				StatusScript.hp += 300;
				if (StatusScript.hp > StatusScript.max_hp) {
					StatusScript.hp = StatusScript.max_hp;
				}
				use = true;
			}
			break;
		case "魔法薬":
			if (StatusScript.mp != StatusScript.max_mp) {
				messageText.text = "魔法薬を使った！";
				wait = 40;
				mp_recover = 200;
				StatusScript.mp += 200;
				if (StatusScript.mp > StatusScript.max_mp) {
					StatusScript.mp = StatusScript.max_mp;
				}
				use = true;
			}
			break;
		}

		if (use) {
			List<string> tempList = new List<string> (itemArray);
			tempList.RemoveAt (num);
			return tempList.ToArray ();
		} else {
			return itemArray;
		}
	}


	public static string skil_message (string skil_name)
	{
		string message = "";
		switch (skil_name) {
		case "パンチ":
			message = "敵にダメージを与える。";
			break;
		case "キック":
			message = "威力は高いが、外れやすい。";
			break;
		case "スライディング":
			message = "威力は低いが、敵の防御の影響を半減する。";
			break;
		}

		return message;
	}



	public static string magic_message (string magic_name)
	{
		string message = "";
		switch (magic_name) {
		case "ファイア":
			message = "消費魔力：6\n火属性のダメージを与える。";
			break;
		case "ヒール":
			message = "消費魔力：13\n体力を約100回復する。";
			break;
		case "チャージ":
			message = "消費魔力：15\n次に行う攻撃技の威力が3倍になる。";
			break;
		case "バリア":
			message = "消費魔力：10\n3ターンの間、受けるダメージを軽減する。";
			break;
		
		}
		return message;

	}

	public static string remagic_message (int magic_num)
	{
		string message = "";
		switch (magic_num) {

		case 0:
			message = "消費魔力：20\n火属性の大きなダメージを与える。";
			break;
		case 1:
			message = "消費魔力：28\n闇属性ダメージを与えその約2倍の体力を回復する";
			break;
		case 2:
			message = "消費魔力：18\n次に使う魔法の威力、回復量が3倍になる。";
			break;
		case 3:
			message = "消費魔力：11\nこのターンに受ける魔法ダメージを敵に与える。";
			break;
		}
		return message;

	}

	public static string remagic_name (int magic_num)
	{
		string magic_name = "";
		switch (magic_num) {

		case 0:
			magic_name = "デスファイア";
			break;
		case 1:
			magic_name = "吸魂";
			break;
		case 2:
			magic_name = "魔の波動";
			break;
		case 3:
			magic_name = "ブラックバリア";
			break;
		}
		return magic_name;

	}

	// 魔法を使った時の処理
	void magic_use (string magic_name)
	{
		switch (magic_name) {
		case "ヒール":
			if (StatusScript.hp != StatusScript.max_hp && StatusScript.mp >= 5) {
				int score = Random.Range (85, 116);
				StatusScript.hp += score;
				if (StatusScript.hp > StatusScript.max_hp) {
					StatusScript.hp = StatusScript.max_hp;
				}
				StatusScript.mp -= 13;
				messageText.text = "ヒールを唱えた！";
				wait = 40;
				magicSound.PlayOneShot (magicSound.clip);
				recover = score;


			}

			break;
		}
	}


	string time_indent (int second, int minute, int hour)
	{
		string time = "";
		if (hour < 10) {
			time = "0";
		}
		time += hour + ":";
		if (minute < 10) {
			time += "0";
		}
		time += minute + ":";
		if (second < 10) {
			time += "0";
		}
		time += second;
		return time;
	}
}