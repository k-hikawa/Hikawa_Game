using UnityEngine;
using System.Collections;

//using UnityEngine.SceneManagement;

public class EnemyScript : MonoBehaviour
{
	
	//public static string enemyName;
	public static Vector3 playerPosition;
	public static Vector3 playerRotation;
	public static Vector3 enemyPosition;

	public static bool run_rag;
	int run_rag_count;

	public Transform player;
	private GameObject unitychan;
	/*public static Camera MainCamera;
	public static Camera BattleCamera;
	public static Camera BattleCamera2;*/
	private GameObject Mark;
	private GameObject Mark2;
	private bool caution;
	private Vector3 initPoint;
	bool moveTarget;
	int moveDistance;
	Animator myAnimator;
	public static string battleMonster;
	private string this_name;
	private int this_level;

	AudioSource cautionSound;


	// Use this for initialization
	void Start ()
	{
		myAnimator = GetComponent<Animator> ();
		/*MainCamera = GameObject.Find ("MainCamera").GetComponent <Camera> ();
		BattleCamera = GameObject.Find ("BattleCamera").GetComponent <Camera> ();
		BattleCamera2 = GameObject.Find ("BattleCamera2").GetComponent <Camera> ();*/
		unitychan = GameObject.Find ("unitychan");
		Mark = GameObject.Find ("Mark");
		Mark2 = GameObject.Find ("Mark2");
		string[] enemys = this.gameObject.name.Split (' ');
		this_name = enemys [0];
		this_level = int.Parse (enemys [1].Substring (3));

		initPoint = this.transform.position;
		player = unitychan.transform;

		AudioSource[] sounds = GameObject.Find ("SoundBox").GetComponents <AudioSource> ();
		cautionSound = sounds [5];
		moveDistance = -180;
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (run_rag) {
			if (run_rag_count == 0) {
				run_rag_count = 60;
			}
			run_rag_count--;
			if (run_rag_count == 0) {
				run_rag = false;
			}
		} else {

			if (PlayerScript.mode == 0 && this_name != "ドラゴンキング") {

				// プレイヤーが近づいたら
				if (Vector3.Distance (player.position, this.transform.position) < 10) {
					
					transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (player.position - transform.position), 0.2f);
					moveTarget = false;
					moveDistance = 0;
					myAnimator.SetBool ("move", true);
					transform.position += transform.TransformDirection (Vector3.forward) * 0.17f;
					if (!caution) {
						cautionSound.PlayOneShot (cautionSound.clip);
						caution = true;
					}
					if (StatusScript.level >= this_level) {
						Mark.GetComponent<Canvas> ().enabled = true;
					} else {
						Mark2.GetComponent<Canvas> ().enabled = true;
					}

				} else {
					if (caution) {
						caution = false;
						Mark.GetComponent<Canvas> ().enabled = false;
						Mark2.GetComponent<Canvas> ().enabled = false;
					}
					// 通常時はランダム徘徊
					randomMove ();

				}
			}
		}

	}


	void OnCollisionEnter (Collision other)
	{
		if ((PlayerScript.mode == 0 || PlayerScript.mode == 4) && !run_rag) {
			if (other.gameObject.name == "unitychan") {
				if (this_name != "ドラゴンキング") {
					caution = false;
					Mark.GetComponent<Canvas> ().enabled = false;
					Mark2.GetComponent<Canvas> ().enabled = false;

					PlayerScript.changeMode (2);


					myAnimator.SetBool ("move", false);
					// バトルフィールドを見えるようにする
					GameObject.Find ("BattleField").GetComponent<Terrain> ().enabled = true;

					// キャラクターたちをバトルフィールドに移動させる
					playerPosition = unitychan.transform.position;
					playerRotation = unitychan.transform.eulerAngles;
					enemyPosition = this.transform.position;

					unitychan.transform.position = new Vector3 (-309.5f, 0f, 58.7f);

					this.transform.position = new Vector3 (-315.5f, 0, 62.9f);
					transform.LookAt (player);
					unitychan.transform.LookAt (this.gameObject.transform);

					/*BattleCamera2.enabled = true;
					BattleCamera.enabled = false;*/
					CameraScript.CameraChange ("Battle", "Main");
					// 対戦モンスターの名前を取得し、バトルスクリプトを呼び出す
					battleMonster = this.gameObject.name;
					GameObject.Find ("BattleText").GetComponent<BattleMenuScript> ().enabled = true;
					GameObject.Find ("BattleMenu").GetComponent<BattleScript> ().enabled = true;


				} else {
					
					caution = false;
					Mark.GetComponent<Canvas> ().enabled = false;
					Mark2.GetComponent<Canvas> ().enabled = false;

					PlayerScript.changeMode (2);
					/*
					BattleCamera2.enabled = t;
					BattleCamera.enabled = false;
					MainCamera.enabled = false;*/

					// バトルフィールドを見えるようにする
					GameObject.Find ("BattleField2").GetComponent<Terrain> ().enabled = true;
					// キャラクターたちをバトルフィールドに移動させる
					playerPosition = unitychan.transform.position;
					playerRotation = unitychan.transform.eulerAngles;
					enemyPosition = this.transform.position;

					unitychan.transform.position = new Vector3 (344.6f, 0f, 1004f);
					this.transform.position = new Vector3 (325.9f, 0f, 1169.9f);

					transform.LookAt (player);
					unitychan.transform.LookAt (this.gameObject.transform);

					CameraScript.CameraChange ("Battle2", "Main");

					// 対戦モンスターの名前を取得し、バトルスクリプトを呼び出す
					battleMonster = this.gameObject.name;
					GameObject.Find ("BattleText").GetComponent<BattleMenuScript> ().enabled = true;
					GameObject.Find ("BattleMenu").GetComponent<BattleScript> ().enabled = true;

				}
					
			}
		}
	}

	// 敵の徘徊アルゴリズム関数
	void randomMove ()
	{
		if (moveTarget) {
			// 初期位置から離れると初期位置の方角に戻る
			if (Vector3.Distance (initPoint, this.transform.position) > 50) {
				transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (initPoint - transform.position), 0.5f);
			} else {
				float direction = Random.Range (0f, 360f);
				//transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (Vector3 (0f, direction, 0f) - transform.position), 0.1f);
				this.transform.Rotate (new Vector3 (0f, direction, 0f));
			}
			moveTarget = false;
			moveDistance = Random.Range (180, 280);
		} else {
			if (moveDistance > 0 && moveDistance <= 150) {
				myAnimator.SetBool ("move", true);
				transform.position += transform.TransformDirection (Vector3.forward) * 0.05f;
				moveDistance--;
			} else {
				
				myAnimator.SetBool ("move", false);
				
				moveDistance--;
				if (moveDistance < Random.Range (-180, -90)) {
					moveTarget = true;
				}

			}

		}
		
	}

}