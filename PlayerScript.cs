using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{

	public static GameObject enemychan_magic;
	public static Animator enemychananim;
	public static GameObject enemychan_magic2;
	public static Animator enemychananim2;

	AudioSource menuSound;
	private GameObject Menu;
	public static int mode;
	//0=フィールド, 1=メニュー, 2=戦闘, 3=ストーリー, 4=ストーリーイベント

	public static int getMode ()
	{
		return mode;
	}

	public static void changeMode (int change)
	{
		mode = change;
	}


	// Use this for initialization
	void Start ()
	{
		StatusScript.cave = 0;
		enemychan_magic = GameObject.Find ("enemychan");
		enemychananim = enemychan_magic.GetComponent <Animator> ();
		enemychan_magic.SetActive (false);
		enemychan_magic2 = GameObject.Find ("enemychan2");
		enemychananim2 = enemychan_magic2.GetComponent <Animator> ();
		enemychan_magic2.SetActive (false);

		if (CaveExitScript.caveExit) {
			transform.position = new Vector3 (481.44f, 1.09f, 313.3017f);
			transform.eulerAngles = new Vector3 (0, 18.712f, 0);
			CaveExitScript.caveExit = false;
		} else if (CaveEntranceScript.caveEntrance) {
			GameObject.Find ("unitychan").transform.position = new Vector3 (451.15f, 1.01f, 274.05f);
			transform.eulerAngles = new Vector3 (0, 175.497f, 0);
			CaveEntranceScript.caveEntrance = false;
		} else if (PlayerPrefs.HasKey ("x")) {
			if (PlayerPrefs.GetInt ("cave") == 0) {
				transform.position = new Vector3 (PlayerPrefs.GetFloat ("x"), PlayerPrefs.GetFloat ("y"), PlayerPrefs.GetFloat ("z"));
				transform.eulerAngles = new Vector3 (PlayerPrefs.GetFloat ("rx"), PlayerPrefs.GetFloat ("ry"), PlayerPrefs.GetFloat ("rz"));
			} else {
				SceneManager.LoadScene ("Cave");
			}
		}



		AudioSource[] sounds = GameObject.Find ("SoundBox").GetComponents <AudioSource> ();
		menuSound = sounds [1];

	
		Menu = GameObject.Find ("Menu");
		Menu.GetComponent<Canvas> ().enabled = false;
		mode = 0;

	}


		

	
	// Update is called once per frame
	void Update ()
	{
		
		// フィールドモード時にスペースキーでメニューを開く
		if (StatusScript.story != 0 && mode == 0 && Input.GetKeyDown (KeyCode.Space)) {
			menuSound.PlayOneShot (menuSound.clip);
			//GetComponent<UnityChanControlScriptWithRgidBody> ().enabled = false;
			Menu.GetComponent<Canvas> ().enabled = true;

			mode = 1;
		}

		StatusScript.time += Time.deltaTime;
	}

	void OnCollisionEnter (Collision other)
	{
		if (StatusScript.story == 0) {
			switch (other.gameObject.name) {
			case "Terrain":
				StoryScript.story_script (0);
				break;
			}
		}
	}



}
