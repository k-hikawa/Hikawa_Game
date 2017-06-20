using UnityEngine;
using System.Collections;

public class CavePlayerScript : MonoBehaviour
{

	AudioSource menuSound;
	public static GameObject enemychan_magic;
	public static Animator enemychananim;

	private static GameObject HelpCamera;
	public static bool help;
	Renderer celling;

	private GameObject Menu;
	//0=フィールド, 1=メニュー, 2=戦闘, 3=ストーリー, 4=ストーリーイベント


	// Use this for initialization
	void Start ()
	{
		StatusScript.cave = 1;
		enemychan_magic = GameObject.Find ("enemychan");
		enemychananim = enemychan_magic.GetComponent <Animator> ();
		enemychan_magic.SetActive (false);

		if (CaveScript.caveIn) {
			CaveScript.caveIn = false;
			transform.position = new Vector3 (-5.5f, -0.628f, 75.89f);
			transform.eulerAngles = new Vector3 (0, 180, 0);
		} else if (PlayerPrefs.HasKey ("x")) {
			transform.position = new Vector3 (PlayerPrefs.GetFloat ("x"), PlayerPrefs.GetFloat ("y"), PlayerPrefs.GetFloat ("z"));
			transform.eulerAngles = new Vector3 (PlayerPrefs.GetFloat ("rx"), PlayerPrefs.GetFloat ("ry"), PlayerPrefs.GetFloat ("rz"));

		}

		AudioSource[] sounds = GameObject.Find ("SoundBox").GetComponents <AudioSource> ();
		menuSound = sounds [1];


		Menu = GameObject.Find ("Menu");
		Menu.GetComponent<Canvas> ().enabled = false;
		PlayerScript.mode = 0;

		HelpCamera = GameObject.Find ("HelpCamera");
		HelpCamera.SetActive (false);

		celling = GameObject.Find ("Celling").GetComponent <Renderer> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		// フィールドモード時にスペースキーでメニューを開く
		if (StatusScript.story != 0 && PlayerScript.mode == 0 && Input.GetKeyDown (KeyCode.Space)) {
			menuSound.PlayOneShot (menuSound.clip);
			Menu.GetComponent<Canvas> ().enabled = true;
			PlayerScript.mode = 1;
		}

		if (help) {
			// ヘルプカメラ中にエンターで戻す
			if (PlayerScript.mode != 0 || Input.GetKeyDown (KeyCode.Return)) {
				help = false;
				HelpCamera.SetActive (false);
				celling.enabled = true;
			}

			// エンターでヘルプカメラに切り替える
		} else if (PlayerScript.mode == 0 && Input.GetKeyDown (KeyCode.Return)) {
			HelpCamera.SetActive (true);
			help = true;
			celling.enabled = false;
		}
		StatusScript.time += Time.deltaTime;
	}


}
