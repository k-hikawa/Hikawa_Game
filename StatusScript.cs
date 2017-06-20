using UnityEngine;
using System.Collections;

public class StatusScript : MonoBehaviour
{
	public static float time;
	public static int gameOver;
	public static int level;
	public static int exp;
	public static int max_hp;
	public static int hp;
	public static int max_mp;
	public static int mp;
	public static int attack;
	public static int defence;
	public static int speed;
	public static int lack;
	public static string[] item = new string[0];
	public static string[] skil = new string[0];
	public static string[] magic = new string[0];
	public static int remagic;
	public static int story;
	public static int cave;

	public static void init ()
	{
		item = new string[0];
		skil = new string[0];
		magic = new string[0];
		if (PlayerPrefs.HasKey ("t")) {
			time = PlayerPrefs.GetFloat ("t");
			gameOver = PlayerPrefs.GetInt ("g");
			level = PlayerPrefs.GetInt ("lv");
			exp = PlayerPrefs.GetInt ("e");
			max_hp = PlayerPrefs.GetInt ("mh");
			hp = PlayerPrefs.GetInt ("h");
			max_mp = PlayerPrefs.GetInt ("mm");
			mp = PlayerPrefs.GetInt ("m");
			attack = PlayerPrefs.GetInt ("a");
			defence = PlayerPrefs.GetInt ("d");
			speed = PlayerPrefs.GetInt ("s");
			lack = PlayerPrefs.GetInt ("l");
			for (int i = 0; i < 10; i++) {
				if (PlayerPrefs.GetString ("item" + i) != "") {
					item = array_push (item, PlayerPrefs.GetString ("item" + i));
				}
				if (PlayerPrefs.GetString ("skil" + i) != "") {
					skil = array_push (skil, PlayerPrefs.GetString ("skil" + i));
				}
				if (PlayerPrefs.GetString ("magic" + i) != "") {
					magic = array_push (magic, PlayerPrefs.GetString ("magic" + i));
				}
			}
			remagic = PlayerPrefs.GetInt ("r");
			story = PlayerPrefs.GetInt ("story");
			cave = PlayerPrefs.GetInt ("cave");
		} else {
			time = 0;
			gameOver = 0;
			level = 1;
			exp = 0;
			max_hp = Random.Range (25, 28);
			hp = max_hp;
			max_mp = Random.Range (16, 18);
			mp = max_mp;
			attack = Random.Range (16, 18);
			defence = Random.Range (15, 17);
			speed = Random.Range (15, 17);
			lack = Random.Range (15, 17);
			item = array_push (item, "やくそう");
			skil = array_push (skil, "パンチ");
			remagic = 0;
			story = 0;
			cave = 0;
			/*
			//チートモード
			level = 10;
			exp = 0;
			max_hp = 20;
			hp = max_hp;
			max_mp = 20 * 5 * 5;
			mp = max_mp;
			attack = 20 * 3 * 5;
			defence = 20 * 3 * 5;
			speed = 20 * 3 * 5;
			lack = 20 * 3 * 5;
			skil = array_push (skil, "キック");
			skil = array_push (skil, "スライディング");
			magic = array_push (magic, "ファイア");
			magic = array_push (magic, "ヒール");
			magic = array_push (magic, "チャージ");
			magic = array_push (magic, "バリア");
			remagic = 0;*/

		}

	}

	// Use this for initialization
	void Awake ()
	{
		init ();

	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}


	// string型の配列の末尾に要素を追加する関数
	static string[] array_push (string[] array, string value)
	{
		array.CopyTo (array = new string[array.Length + 1], 0);
		array [array.Length - 1] = value;
		return array;
	}
}
