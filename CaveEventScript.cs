using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CaveEventScript : MonoBehaviour
{


	//static AudioSource AudioSource;
	Text storyText;
	private static GameObject storyUI;
	private static string[] mes = new string[0];
	AudioSource returnSource;
	AudioSource cautionSound;
	private GameObject Mark;

	private GameObject unitychan;
	private GameObject enemychan;
	Animator UAnimator;
	//public static string enemyName;
	public static Vector3 playerPosition;
	public static Vector3 playerRotation;
	public static Vector3 enemyPosition;
	public static bool chuboss;
	bool rea;



	void OnCollisionEnter (Collision other)
	{
		
		if (other.gameObject.name == "unitychan" && StatusScript.story == 1) {
			story_script (1);
		}
	}

	// Use this for initialization
	void Start ()
	{
		//AudioSource = GameObject.Find ("MainCamera").GetComponent<AudioSource> ();
		storyUI = GameObject.Find ("Story").gameObject;
		storyText = GameObject.Find ("StoryText").GetComponentInChildren<Text> ();
		returnSource = storyUI.GetComponent <AudioSource> ();
		AudioSource[] sounds = GameObject.Find ("SoundBox").GetComponents <AudioSource> ();
		cautionSound = sounds [5];
		Mark = GameObject.Find ("Mark");
		unitychan = GameObject.Find ("unitychan");
		enemychan = GameObject.Find ("enemychan レベル1");

		UAnimator = unitychan.GetComponent<Animator> ();
		if (StatusScript.story >= 3) {
			Destroy (enemychan);
			Destroy (this.gameObject);
		}

	}

	// Update is called once per frame
	void Update ()
	{
		if (mes.Length != 0 || rea) {
			message ();
			if (mes.Length == 0) {
				storyEvent ();
			}
		} else if (StatusScript.story == 2) {
			story_script (2);
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
			if (StatusScript.story == 2) {
				StatusScript.remagic = 1;
				Destroy (enemychan);
				GameObject.Find ("Exit").GetComponent <MeshRenderer> ().enabled = true;
				StatusScript.story++;
				story_script (3);
			} else if (StatusScript.story == 3) {
				StatusScript.story++;
				PlayerScript.changeMode (0);
				Destroy (this.gameObject);
			}
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
		} else if (StatusScript.story == 1) {
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
		if (StatusScript.story == 1) {


			PlayerScript.changeMode (2);
			UAnimator.SetBool ("move", false);

			// キャラクターたちをバトルフィールドに移動させる
			playerPosition = unitychan.transform.position;
			playerRotation = unitychan.transform.eulerAngles;
			enemyPosition = this.transform.position;


			enemychan.transform.position = new Vector3 (-84.48f, -0.45f, 76.04f);
			unitychan.transform.position = new Vector3 (-82.337f, -0.531f, 74.65f);
			enemychan.transform.LookAt (unitychan.transform.position);
			unitychan.transform.LookAt (enemychan.gameObject.transform.position);

			/*BattleCamera2.enabled = true;
					BattleCamera.enabled = false;*/
			CaveCameraScript.CameraChange ("Battle", "Main");
			// 対戦モンスターの名前を取得し、バトルスクリプトを呼び出す
			chuboss = true;
			GameObject.Find ("BattleText").GetComponent<CaveBattleMenuScript> ().enabled = true;
			GameObject.Find ("BattleMenu").GetComponent<CaveBattleScript> ().enabled = true;
		} else if (StatusScript.story == 2) {
			chuboss = false;

		}
	}

		





	void story_script (int story_num)
	{
		switch (story_num) {


		case 1:
			mes = array_push (mes, "あなたは誰？");
			mes = array_push (mes, "何か思い出しそう...");
			mes = array_push (mes, "うぅ、頭が痛い...");
			mes = array_push (mes, "Enemychan「私はあなた自身...」");
			mes = array_push (mes, "違う、こんなの私じゃ...");
			mes = array_push (mes, "enemychan「私はあなたの影の部分」");
			mes = array_push (mes, "enemychan「この世界はあなたの心の中」");
			mes = array_push (mes, "enemychan「あなたは私をここに封印することで完璧になろうとした」");
			mes = array_push (mes, "enemychan「けど光あるものは影がある」");
			mes = array_push (mes, "enemychan「影のないものには光もない」");
			mes = array_push (mes, "enemychan「さあ、弱い心...自分の影を受け入れるのよ」");
			mes = array_push (mes, "enemycChan「そして、自分自身に打ち勝つの！」");

			break;

		case 2:
			mes = array_push (mes, "思い出した！");
			mes = array_push (mes, "enemychan「よかった...」");
			mes = array_push (mes, "ごめんね心配かけて");
			mes = array_push (mes, "enemychan「ううん、わかればいいの」");
			mes = array_push (mes, "さあ！一緒に元の世界に帰ろう！");
			mes = array_push (mes, "enemychan「うん、出口はここだよ！」");
			mes = array_push (mes, "ありがとう！私は影の私を受け入れる！そしてもっと強くなる！");
			break;

		case 3:
			mes = array_push (mes, "enemychanと一つになった！");
			mes = array_push (mes, "enemychanの力を借りて使うことのできる強力な魔法、「裏魔法」が使用できるようになりました。");
			mes = array_push (mes, "裏魔法は戦闘中、魔法の選択画面でスペースキーを押している間、選択することができます。");
			//mes = array_push (mes, "また、体力がなくなった時の復活地点がここになりました。");
			mes = array_push (mes, "この世界は私の心の中");
			mes = array_push (mes, "この世界のモンスター達は私の心のバイ菌のようなもの");
			mes = array_push (mes, "元の世界に帰るにはモンスター達の親玉であるドラゴンキングを倒さなければいけない");
			mes = array_push (mes, "ドラゴンキングはとても強い、");
			mes = array_push (mes, "でも大丈夫、私なら絶対に勝てる！");

			break;


		}
		PlayerScript.mode = 3;
		storyUI.GetComponent<Canvas> ().enabled = true;
		//AudioSource.volume = 0.1f;
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
