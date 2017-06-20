using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class BattleMenuScript : MonoBehaviour
{
	Text myText;
	Text enemyText;
	Text statusText;
	Animator eAnimator;

	Animator anim;



	private GameObject unitychan;

	private GameObject e_fire;
	private GameObject p_fire;
	private GameObject e_fire2;
	private GameObject d_fire;
	private GameObject menu2;
	private GameObject ene;

	private MeshRenderer bal;
	private MeshRenderer blackBal;
	private GameObject explosion;

	// 前進速度
	public float forwardSpeed = 7.0f;
	// 後退速度
	public float backwardSpeed = 7.0f;
	// キャラクターコントローラ（カプセルコライダ）の移動量
	private Vector3 velocity;

	private AnimatorStateInfo currentBaseState;

	private string enemy;
	private string enemy_name;
	private string enemy_description;
	private int wait;
	private int active;
	private int skil_active;
	private bool stanby;
	private int commandSelect;
	private string commandMessage;
	private int turnStart_hp;
	private int turnStart_mp;

	Text enemyDamage;
	Text enemyHeal;
	Text playerHeal;
	Text playerDamage;


	public static string chose_command;

	private int turn_num;

	AudioSource MainAudioSource;

	AudioSource systemSource;
	AudioSource returnSource;
	AudioSource magicSource;
	AudioSource healSource;
	AudioSource attackSource;
	AudioSource chargeSource;
	AudioSource fanfareSource;
	AudioSource criticalSource;
	AudioSource runSource;
	AudioSource winSource;
	AudioSource gameoverSource;
	AudioSource remagicSource;
	AudioSource screamSource;
	AudioSource starSource;
	AudioSource dragonSource;

	Slider hp_slider;
	Slider mp_slider;
	Image hp_sliderC;
	Image mp_sliderC;

	// Use this for initialization
	void OnEnable ()
	{
		
		AudioSource[] sounds = GameObject.Find ("SoundBox").GetComponents <AudioSource> ();
		systemSource = sounds [0];
		returnSource = sounds [2];
		magicSource = sounds [3];
		healSource = sounds [4];
		attackSource = sounds [6];
		chargeSource = sounds [7];
		fanfareSource = sounds [8];
		criticalSource = sounds [9];
		runSource = sounds [10];
		winSource = sounds [11];
		gameoverSource = sounds [12];
		screamSource = sounds [13];
		remagicSource = sounds [14];
		starSource = sounds [15];
		dragonSource = sounds [16];

		myText = GetComponentInChildren<Text> ();
		enemyText = GameObject.Find ("EnemyText").GetComponentInChildren<Text> ();
		statusText = GameObject.Find ("PlayerStatusText").GetComponentInChildren<Text> ();

		menu2 = GameObject.Find ("BattleTextBack2");
		menu2.SetActive (false);

		enemy = EnemyScript.battleMonster;
		string[] enemys = enemy.Split (' ');
		enemy_name = enemys [0];
		enemy_description = enemys [1];

		if (enemy_name == "ドラゴンキング") {
			CameraScript.CameraChange ("Battle2", "Main");
			MainAudioSource = GameObject.Find ("BattleCamera2").GetComponent<AudioSource> ();
		} else {
			CameraScript.CameraChange ("Battle", "Main");
			MainAudioSource = GameObject.Find ("BattleCamera").GetComponent<AudioSource> ();
		}
		MainAudioSource.Play ();

		enemyDamage = GameObject.Find ("EnemyDamage").GetComponentInChildren<Text> ();
		enemyHeal = GameObject.Find ("EnemyHeal").GetComponentInChildren<Text> ();
		playerHeal = GameObject.Find ("PlayerHeal").GetComponentInChildren<Text> ();
		playerDamage = GameObject.Find ("PlayerDamage").GetComponentInChildren<Text> ();


		eAnimator = GameObject.Find (enemy).GetComponent<Animator> ();
		unitychan = GameObject.Find ("unitychan");





		anim = unitychan.GetComponent<Animator> ();

		if (enemy_name == "ドラゴンキング") {
			p_fire = GameObject.Find ("EnemySide2").transform.FindChild ("Fire").gameObject;
			e_fire = GameObject.Find ("PlayerSide2").transform.FindChild ("Fire").gameObject;
			e_fire2 = GameObject.Find ("PlayerSide2").transform.FindChild ("Fire2").gameObject;
			d_fire = GameObject.Find ("EnemySide2").transform.FindChild ("DeathFire").gameObject;
			bal = GameObject.Find ("PlayerSide2").transform.FindChild ("Bal").gameObject.GetComponent <MeshRenderer> ();
			blackBal = GameObject.Find ("PlayerSide2").transform.FindChild ("BlackBal").gameObject.GetComponent <MeshRenderer> ();
			explosion = GameObject.Find ("Explosion");
			explosion.SetActive (false);
		} else {
			p_fire = GameObject.Find ("EnemySide").transform.FindChild ("Fire").gameObject;
			e_fire2 = GameObject.Find ("PlayerSide").transform.FindChild ("Fire2").gameObject;
			e_fire = GameObject.Find ("PlayerSide").transform.FindChild ("Fire").gameObject;
			d_fire = GameObject.Find ("EnemySide").transform.FindChild ("DeathFire").gameObject;
			bal = GameObject.Find ("PlayerSide").transform.FindChild ("Bal").gameObject.GetComponent <MeshRenderer> ();
			blackBal = GameObject.Find ("PlayerSide").transform.FindChild ("BlackBal").gameObject.GetComponent <MeshRenderer> ();
		}




		//hp、mpバーの初期設定
		hp_slider = GameObject.Find ("HpSlider").GetComponent<Slider> ();
		mp_slider = GameObject.Find ("MpSlider").GetComponent <Slider> ();
		hp_slider.maxValue = StatusScript.max_hp;
		mp_slider.maxValue = StatusScript.max_mp;
		hp_slider.value = StatusScript.hp;
		mp_slider.value = StatusScript.mp;
		hp_sliderC = GameObject.Find ("HpSlider").transform.FindChild ("Fill Area").transform.FindChild ("Fill").GetComponent <Image> ();
		mp_sliderC = GameObject.Find ("MpSlider").transform.FindChild ("Fill Area").transform.FindChild ("Fill").gameObject.GetComponent <Image> ();


		myText.text = enemy_name + " があらわれた！";
		enemyText.text = enemy_name + " " + enemy_description;

		// 残りHPによって色を変える
		statusText.text = "レベル" + StatusScript.level + "\n体力：";
		if (StatusScript.max_hp / 10 >= StatusScript.hp) {
			statusText.text += "<color=red>" + StatusScript.hp + "</color>";
			hp_sliderC.color = new Color (1f, 0f, 0f);
		} else if (StatusScript.max_hp / 2 >= StatusScript.hp) {
			statusText.text += "<color=yellow>" + StatusScript.hp + "</color>";
			hp_sliderC.color = new Color (1f, 1f, 0f);
		} else {
			statusText.text += StatusScript.hp;
			hp_sliderC.color = new Color (0f, 1f, 0f);
		}
		statusText.text += "/" + StatusScript.max_hp + "\n\n魔力：";

		if (StatusScript.max_mp / 5 >= StatusScript.mp) {
			statusText.text += "<color=red>" + StatusScript.mp + "</color>";
			mp_sliderC.color = new Color (1f, 0f, 0f);
		} else if (StatusScript.max_mp / 2 >= StatusScript.mp) {
			statusText.text += "<color=yellow>" + StatusScript.mp + "</color>";
			mp_sliderC.color = new Color (1f, 1f, 0f);
		} else {
			statusText.text += StatusScript.mp;
			mp_sliderC.color = new Color (0f, 1f, 0f);
		}
		statusText.text += "/" + StatusScript.max_mp;



		chose_command = "";
		turn_num = 1;
		commandSelect = 0;
		active = 0;
		skil_active = 0;
		if (enemy_name == "ドラゴンキング") {
			wait = 120;
		} else {
			wait = 60;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		// waitがある時はなくなるまで待つ
		if (wait > 0) {
			wait--;
			if (enemy_name == "ドラゴンキング") {
				if (unitychan.transform.localScale.x < 21) {
					unitychan.transform.localScale = new Vector3 (unitychan.transform.localScale.x + wait / 80, unitychan.transform.localScale.y + wait / 80, unitychan.transform.localScale.z + wait / 80);
				}
				if (wait == 119) {
					remagicSource.PlayOneShot (remagicSource.clip);
					PlayerScript.enemychan_magic2.SetActive (true);
					PlayerScript.enemychananim2.SetBool ("magic", true);
					anim.SetBool ("magic", true);

				} else if (wait == 1) {
					anim.SetBool ("magic", false);
					PlayerScript.enemychan_magic2.SetActive (false);
					PlayerScript.enemychananim2.SetBool ("magic", false);
				}
			}
		} else {
			if (wait == 0) {
				stanby = true;
				wait--;
			}
		}
			
		// ターン開始したら
		if (BattleScript.turn_start) {
			if (BattleScript.wait_time [0] < 0) {
				if (Input.GetKeyDown (KeyCode.RightArrow)) {
					systemSource.PlayOneShot (systemSource.clip);
					if (skil_active == 5) {
						skil_active = 0;
					} else {
						skil_active++;
					}
				} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
					systemSource.PlayOneShot (systemSource.clip);
					if (skil_active == 0) {
						skil_active = 5;
					} else {
						skil_active--;
					}
				} else if (Input.GetKeyUp (KeyCode.Return)) {
					returnSource.PlayOneShot (returnSource.clip);
					BattleScript.wait_time [0]++;
					switch (skil_active) {
					case 0:
						StatusScript.max_hp++;
						StatusScript.hp = StatusScript.max_hp;
						break;
					case 1:
						StatusScript.max_mp++;
						StatusScript.mp = StatusScript.max_mp;
						break;
					case 2:
						StatusScript.attack++;
						break;
					case 3:
						StatusScript.defence++;
						break;
					case 4:
						StatusScript.speed++;
						break;
					case 5:
						StatusScript.lack++;
						break;
					}

				}
				switch (skil_active) {
				case 0:
					myText.text = "ステータスポイントを振り分けてください。 残り" + -BattleScript.wait_time [0] + "ポイント\n▶体力" + StatusScript.max_hp + " 魔力" + StatusScript.max_mp + " 攻撃" + StatusScript.attack + " 防御" + StatusScript.defence + " 敏捷" + StatusScript.speed + " 運" + StatusScript.lack;
					break;
				case 1:
					myText.text = "ステータスポイントを振り分けてください。 残り" + -BattleScript.wait_time [0] + "ポイント\n 体力" + StatusScript.max_hp + "▶魔力" + StatusScript.max_mp + " 攻撃" + StatusScript.attack + " 防御" + StatusScript.defence + " 敏捷" + StatusScript.speed + " 運" + StatusScript.lack;
					break;
				case 2:
					myText.text = "ステータスポイントを振り分けてください。 残り" + -BattleScript.wait_time [0] + "ポイント\n 体力" + StatusScript.max_hp + " 魔力" + StatusScript.max_mp + "▶攻撃" + StatusScript.attack + " 防御" + StatusScript.defence + " 敏捷" + StatusScript.speed + " 運" + StatusScript.lack;
					break;
				case 3:
					myText.text = "ステータスポイントを振り分けてください。 残り" + -BattleScript.wait_time [0] + "ポイント\n 体力" + StatusScript.max_hp + " 魔力" + StatusScript.max_mp + " 攻撃" + StatusScript.attack + "▶防御" + StatusScript.defence + " 敏捷" + StatusScript.speed + " 運" + StatusScript.lack;
					break;
				case 4:
					myText.text = "ステータスポイントを振り分けてください。 残り" + -BattleScript.wait_time [0] + "ポイント\n 体力" + StatusScript.max_hp + " 魔力" + StatusScript.max_mp + " 攻撃" + StatusScript.attack + " 防御" + StatusScript.defence + "▶敏捷" + StatusScript.speed + " 運" + StatusScript.lack;
					break;
				case 5:
					myText.text = "ステータスポイントを振り分けてください。 残り" + -BattleScript.wait_time [0] + "ポイント\n 体力" + StatusScript.max_hp + " 魔力" + StatusScript.max_mp + " 攻撃" + StatusScript.attack + " 防御" + StatusScript.defence + " 敏捷" + StatusScript.speed + "▶運" + StatusScript.lack;
					break;
				}


				// ステータスの更新
				hp_slider.maxValue = BattleScript.max_hp_change [0];
				mp_slider.maxValue = BattleScript.max_mp_change [0];
				hp_slider.value = BattleScript.hp_change [0];
				mp_slider.value = BattleScript.mp_change [0];

				statusText.text = "レベル" + BattleScript.level_change [0] + "\n体力：";
				if (BattleScript.max_hp_change [0] / 5 >= BattleScript.hp_change [0]) {
					statusText.text += "<color=red>" + BattleScript.hp_change [0] + "</color>";
					hp_sliderC.color = new Color (1f, 0f, 0f);
				} else if (BattleScript.max_hp_change [0] / 2 >= BattleScript.hp_change [0]) {
					statusText.text += "<color=yellow>" + BattleScript.hp_change [0] + "</color>";
					hp_sliderC.color = new Color (1f, 1f, 0f);
				} else {
					statusText.text += BattleScript.hp_change [0];
					hp_sliderC.color = new Color (0f, 1f, 0f);
				}
				statusText.text += "/" + BattleScript.max_hp_change [0] + "\n\n魔力：";
				if (BattleScript.max_mp_change [0] / 5 >= BattleScript.mp_change [0]) {
					statusText.text += "<color=red>" + BattleScript.mp_change [0] + "</color>";
					mp_sliderC.color = new Color (1f, 0f, 0f);
				} else if (BattleScript.max_mp_change [0] / 2 >= BattleScript.mp_change [0]) {
					statusText.text += "<color=yellow>" + BattleScript.mp_change [0] + "</color>";
					mp_sliderC.color = new Color (1f, 1f, 0f);
				} else {
					statusText.text += BattleScript.mp_change [0];
					mp_sliderC.color = new Color (0f, 1f, 0f);
				}
				statusText.text += "/" + BattleScript.max_mp_change [0];


			} else {
				BattleScript.wait_time [0]--;
				myText.text = BattleScript.return_message [0];
				// ステータスの更新
				hp_slider.maxValue = BattleScript.max_hp_change [0];
				mp_slider.maxValue = BattleScript.max_mp_change [0];
				hp_slider.value = BattleScript.hp_change [0];
				mp_slider.value = BattleScript.mp_change [0];

				statusText.text = "レベル" + BattleScript.level_change [0] + "\n体力：";
				if (BattleScript.max_hp_change [0] / 5 >= BattleScript.hp_change [0]) {
					statusText.text += "<color=red>" + BattleScript.hp_change [0] + "</color>";
					hp_sliderC.color = new Color (1f, 0f, 0f);
				} else if (BattleScript.max_hp_change [0] / 2 >= BattleScript.hp_change [0]) {
					statusText.text += "<color=yellow>" + BattleScript.hp_change [0] + "</color>";
					hp_sliderC.color = new Color (1f, 1f, 0f);
				} else {
					statusText.text += BattleScript.hp_change [0];
					hp_sliderC.color = new Color (0f, 1f, 0f);
				}
				statusText.text += "/" + BattleScript.max_hp_change [0] + "\n\n魔力：";

				if (BattleScript.max_mp_change [0] / 5 >= BattleScript.mp_change [0]) {
					statusText.text += "<color=red>" + BattleScript.mp_change [0] + "</color>";
					mp_sliderC.color = new Color (1f, 0f, 0f);
				} else if (BattleScript.max_mp_change [0] / 2 >= BattleScript.mp_change [0]) {
					statusText.text += "<color=yellow>" + BattleScript.mp_change [0] + "</color>";
					mp_sliderC.color = new Color (1f, 1f, 0f);
				} else {
					statusText.text += BattleScript.mp_change [0];
					mp_sliderC.color = new Color (0f, 1f, 0f);
				}
				statusText.text += "/" + BattleScript.max_mp_change [0];

			}
			if (BattleScript.action_name [0] != "") {

				// アニメーションを行う内容
				switch (BattleScript.action_name [0]) {
				case "run":
					//unitychan_back ();
					if (BattleScript.wait_time [0] == 59) {
						runSource.PlayOneShot (runSource.clip);
					}
					break;

				case "panch":
					if (BattleScript.wait_time [0] == 39) {
						//unitychan_forward ();
						anim.SetBool ("panch", true);
					} else if (BattleScript.wait_time [0] == 0) {
						anim.SetBool ("panch", false);
						attackSource.PlayOneShot (attackSource.clip);
					}

					break;
				case "kick":
					if (BattleScript.wait_time [0] == 39) {
						//unitychan_forward ();
						anim.SetBool ("kick", true);
					} else if (BattleScript.wait_time [0] == 0) {
						anim.SetBool ("kick", false);
						attackSource.PlayOneShot (attackSource.clip);
					}
					break;

				case "slide":
					if (BattleScript.wait_time [0] == 39) {
						anim.SetBool ("slide", true);
					} else if (BattleScript.wait_time [0] == 0) {
						anim.SetBool ("slide", false);
						attackSource.PlayOneShot (attackSource.clip);
					}
					break;

				case "miss":
					//unitychan_back ();
					break;


				case "magic":
					if (BattleScript.wait_time [0] == 39) {
						magicSource.PlayOneShot (magicSource.clip);
						anim.SetBool ("magic", true);

					} else if (BattleScript.wait_time [0] == 0) {
						anim.SetBool ("magic", false);
					}
					break;

				case "remagic":
					if (BattleScript.wait_time [0] == 59) {
						remagicSource.PlayOneShot (remagicSource.clip);
						if (enemy_name != "ドラゴンキング") {
							PlayerScript.enemychan_magic.SetActive (true);
							PlayerScript.enemychananim.SetBool ("magic", true);
						} else {
							PlayerScript.enemychan_magic2.SetActive (true);
							PlayerScript.enemychananim2.SetBool ("magic", true);
						}
						anim.SetBool ("magic", true);

					} else if (BattleScript.wait_time [0] == 0) {
						anim.SetBool ("magic", false);
						if (enemy_name != "ドラゴンキング") {
							PlayerScript.enemychan_magic.SetActive (false);
							PlayerScript.enemychananim.SetBool ("magic", false);
						} else {
							PlayerScript.enemychan_magic2.SetActive (false);
							PlayerScript.enemychananim2.SetBool ("magic", false);
						}
					}
					break;

				case "e_magic":
					if (BattleScript.wait_time [0] == 39) {
						magicSource.PlayOneShot (magicSource.clip);
						eAnimator.SetBool ("attack", true);

					} else if (BattleScript.wait_time [0] == 0) {
						eAnimator.SetBool ("attack", false);
					}
					break;

				case "fire":
					if (BattleScript.wait_time [0] == 59) {
						p_fire.SetActive (true);
						eAnimator.SetBool ("damage", true);
						GameObject.Find ("EnemyDamageMark").GetComponent <Canvas> ().enabled = true;
						enemyDamage.text = BattleScript.up_down [0];
					} else if (BattleScript.wait_time [0] == 0) {
						eAnimator.SetBool ("damage", false);
						p_fire.SetActive (false);
						GameObject.Find ("EnemyDamageMark").GetComponent <Canvas> ().enabled = false;
					}
					break;

				case "e_fire":
					if (BattleScript.wait_time [0] == 59) {
						e_fire.SetActive (true);
						anim.SetBool ("damage", true);
						GameObject.Find ("PlayerDamageMark").GetComponent <Canvas> ().enabled = true;
						playerDamage.text = BattleScript.up_down [0];
					} else if (BattleScript.wait_time [0] == 0) {
						e_fire.SetActive (false);
						anim.SetBool ("damage", false);
						GameObject.Find ("PlayerDamageMark").GetComponent <Canvas> ().enabled = false;
					}
					break;

				case "e_fire2":
					if (BattleScript.wait_time [0] == 59) {
						e_fire2.SetActive (true);
						anim.SetBool ("damage", true);
						GameObject.Find ("PlayerDamageMark").GetComponent <Canvas> ().enabled = true;
						playerDamage.text = BattleScript.up_down [0];
					} else if (BattleScript.wait_time [0] == 0) {
						e_fire2.SetActive (false);
						anim.SetBool ("damage", false);
						GameObject.Find ("PlayerDamageMark").GetComponent <Canvas> ().enabled = false;
					}
					break;

				case "d_fire":
					if (BattleScript.wait_time [0] == 59) {
						d_fire.SetActive (true);
						eAnimator.SetBool ("damage", true);
						GameObject.Find ("EnemyDamageMark").GetComponent <Canvas> ().enabled = true;
						enemyDamage.text = BattleScript.up_down [0];
					} else if (BattleScript.wait_time [0] == 0) {
						eAnimator.SetBool ("damage", false);
						d_fire.SetActive (false);
						GameObject.Find ("EnemyDamageMark").GetComponent <Canvas> ().enabled = false;
					}
					break;
				
				// 敵が回復した
				case "e_heal":
					if (BattleScript.wait_time [0] == 39) {
						healSource.PlayOneShot (healSource.clip);
						GameObject.Find ("EnemyHealMark").GetComponent <Canvas> ().enabled = true;
						enemyHeal.text = BattleScript.up_down [0];
					} else if (BattleScript.wait_time [0] == 0) {
						GameObject.Find ("EnemyHealMark").GetComponent <Canvas> ().enabled = false;

					}
					break;

				case "heal":
					if (BattleScript.wait_time [0] == 39) {
						healSource.PlayOneShot (healSource.clip);
						if (enemy_name == "ドラゴンキング") {
							GameObject.Find ("PlayerSide2").transform.FindChild ("HealParticle").gameObject.GetComponent<ParticleSystem> ().Play ();
						} else {
							GameObject.Find ("PlayerSide").transform.FindChild ("HealParticle").gameObject.GetComponent<ParticleSystem> ().Play ();
						}
						GameObject.Find ("PlayerHealMark").GetComponent <Canvas> ().enabled = true;
						playerHeal.text = BattleScript.up_down [0];
					} else if (BattleScript.wait_time [0] == 0) {
						GameObject.Find ("PlayerHealMark").GetComponent <Canvas> ().enabled = false;

					}
					break;

				case "bal":
					if (BattleScript.wait_time [0] == 39) {
						bal.enabled = true;
					}
					break;
				
				case "bal_break":
					if (BattleScript.wait_time [0] == 39) {
						bal.enabled = false;
					}
					break;

				case "blackBal":
					if (BattleScript.wait_time [0] == 39) {
						blackBal.enabled = true;
					}
					break;

				case "balBlack_break":
					if (BattleScript.wait_time [0] == 39) {
						blackBal.enabled = false;
					}
					break;

				case "charge":
					if (BattleScript.wait_time [0] == 39) {
						magicSource.PlayOneShot (magicSource.clip);
						anim.SetBool ("magic", true);

					} else if (BattleScript.wait_time [0] == 0) {
						anim.SetBool ("magic", false);
						if (enemy_name == "ドラゴンキング") {
							GameObject.Find ("PlayerSide2").transform.FindChild ("ChargeParticle").gameObject.GetComponent<ParticleSystem> ().Play ();
						} else {
							GameObject.Find ("PlayerSide").transform.FindChild ("ChargeParticle").gameObject.GetComponent<ParticleSystem> ().Play ();
						}
					}
					break;
				case "charge_end":
					if (BattleScript.wait_time [0] == 39) {
						if (enemy_name == "ドラゴンキング") {
							GameObject.Find ("PlayerSide2").transform.FindChild ("ChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
						} else {
							GameObject.Find ("PlayerSide").transform.FindChild ("ChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
						}
					}
					break;
				case "e_charge":
					if (BattleScript.wait_time [0] == 39) {
						magicSource.PlayOneShot (magicSource.clip);
						eAnimator.SetBool ("attack2", true);
					} else if (BattleScript.wait_time [0] == 0) {
						eAnimator.SetBool ("attack2", false);
						GameObject.Find ("EnemySide").transform.FindChild ("ChargeParticle").gameObject.GetComponent<ParticleSystem> ().Play ();
					}
					break;

				case "e_charge_end":
					if (BattleScript.wait_time [0] == 39) {
						GameObject.Find ("EnemySide").transform.FindChild ("ChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
					}
					break;

				case "recharge":
					if (BattleScript.wait_time [0] == 59) {
						remagicSource.PlayOneShot (remagicSource.clip);
						anim.SetBool ("magic", true);
						if (enemy_name != "ドラゴンキング") {
							PlayerScript.enemychan_magic.SetActive (true);
							PlayerScript.enemychananim.SetBool ("magic", true);
						} else {
							PlayerScript.enemychan_magic2.SetActive (true);
							PlayerScript.enemychananim2.SetBool ("magic", true);
						}
					} else if (BattleScript.wait_time [0] == 0) {
						anim.SetBool ("magic", false);
						if (enemy_name != "ドラゴンキング") {
							PlayerScript.enemychan_magic.SetActive (false);
							PlayerScript.enemychananim.SetBool ("magic", false);
							GameObject.Find ("PlayerSide").transform.FindChild ("ReChargeParticle").gameObject.GetComponent<ParticleSystem> ().Play ();
						} else {
							PlayerScript.enemychan_magic2.SetActive (false);
							PlayerScript.enemychananim2.SetBool ("magic", false);
							GameObject.Find ("PlayerSide2").transform.FindChild ("ReChargeParticle").gameObject.GetComponent<ParticleSystem> ().Play ();
						}


					}

					break;
				case "recharge_end":
					if (BattleScript.wait_time [0] == 39) {
						if (enemy_name == "ドラゴンキング") {
							GameObject.Find ("PlayerSide2").transform.FindChild ("ReChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
						} else {
							GameObject.Find ("PlayerSide").transform.FindChild ("ReChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
						}
					}
					break;

				// 敵の攻撃
				case "attack":

				
					
					if (BattleScript.wait_time [0] == 39) {
						//eAnimator.SetBool ("move", true);
						//GameObject.Find (enemy).transform.position += GameObject.Find (enemy).transform.TransformDirection (Vector3.forward) * 0.1f;
						eAnimator.SetBool (BattleScript.action_name [0], true);
						// 敵の攻撃アニメーション
					} else {
						eAnimator.SetBool (BattleScript.action_name [0], false);
						/*if (BattleScript.wait_time [0] == 0) {
							attackSource.PlayOneShot (attackSource.clip);
						}*/
					}
					break;
				case "attack3":

					if (BattleScript.wait_time [0] == 59) {
						//eAnimator.SetBool ("move", true);
						//GameObject.Find (enemy).transform.position += GameObject.Find (enemy).transform.TransformDirection (Vector3.forward) * 0.1f;
						eAnimator.SetBool (BattleScript.action_name [0], true);
						// 敵の攻撃アニメーション
					} else {
						eAnimator.SetBool (BattleScript.action_name [0], false);
						/*if (BattleScript.wait_time [0] == 0) {
							attackSource.PlayOneShot (attackSource.clip);
						}*/
					}
					break;

				case "dragon":
					if (BattleScript.wait_time [0] == 59) {
						dragonSource.PlayOneShot (dragonSource.clip);
						//eAnimator.SetBool ("move", true);
						//GameObject.Find (enemy).transform.position += GameObject.Find (enemy).transform.TransformDirection (Vector3.forward) * 0.1f;
						eAnimator.SetBool ("attack3", true);
						// 敵の攻撃アニメーション
					} else {
						eAnimator.SetBool ("attack3", false);
						/*if (BattleScript.wait_time [0] == 0) {
							attackSource.PlayOneShot (attackSource.clip);
						}*/
					}
					break;

				case "attack2":
					if (BattleScript.wait_time [0] == 59) {
						eAnimator.SetBool (BattleScript.action_name [0], true);

						// 敵の攻撃アニメーション
					} else if (BattleScript.wait_time [0] == 0) {
						eAnimator.SetBool (BattleScript.action_name [0], false);
					}

					break;
				case "scream":
					if (BattleScript.wait_time [0] == 59) {
						eAnimator.SetBool ("attack3", true);
						screamSource.PlayOneShot (screamSource.clip);
						// 敵の攻撃アニメーション
					} else if (BattleScript.wait_time [0] == 0) {
						eAnimator.SetBool ("attack3", false);
					}
				
				
					
					break;
				

				// プレイヤーがダメージを受けた
				case "p_damage":
					// 敵が元の位置に移動
					if (BattleScript.wait_time [0] == 39) {
						anim.SetBool ("damage", true);
						//GameObject.Find (enemy).transform.position -= GameObject.Find (enemy).transform.TransformDirection (Vector3.forward) * 0.08f;
						GameObject.Find ("PlayerDamageMark").GetComponent <Canvas> ().enabled = true;
						playerDamage.text = BattleScript.up_down [0];
						attackSource.PlayOneShot (attackSource.clip);
						// 敵が元の位置に到着
					} else if (BattleScript.wait_time [0] == 0) {
						//eAnimator.SetBool ("move", false);
						anim.SetBool ("damage", false);
						GameObject.Find ("PlayerDamageMark").GetComponent <Canvas> ().enabled = false;
					}

					break;

				// プレイヤーが大ダメージを受けた
				case "p_damage2":
					// 敵が元の位置に移動
					if (BattleScript.wait_time [0] == 39) {
						anim.SetBool ("damage2", true);
						GameObject.Find ("PlayerDamageMark").GetComponent <Canvas> ().enabled = true;
						playerDamage.text = BattleScript.up_down [0];
						criticalSource.PlayOneShot (criticalSource.clip);
						// 敵が元の位置に到着
					} else if (BattleScript.wait_time [0] == 0) {
						
						anim.SetBool ("damage2", false);
						GameObject.Find ("PlayerDamageMark").GetComponent <Canvas> ().enabled = false;
					}

					break;

				case "e_miss":
					// 敵が元の位置に移動
					if (BattleScript.wait_time [0] >= 39) {
						//anim.SetBool ("damage", true);
						//GameObject.Find (enemy).transform.position -= GameObject.Find (enemy).transform.TransformDirection (Vector3.forward) * 0.08f;

						// 敵が元の位置に到着
					} else {
						//eAnimator.SetBool ("move", false);

					}
					break;

				


				// 敵がダメージを受けた
				case "damage":
				case "damage2":
					//unitychan_back ();
					// 敵のダメージアニメーション
					if (BattleScript.wait_time [0] == 39) {
						eAnimator.SetBool (BattleScript.action_name [0], true);
						GameObject.Find ("EnemyDamageMark").GetComponent <Canvas> ().enabled = true;
						enemyDamage.text = BattleScript.up_down [0];
					} else if (BattleScript.wait_time [0] == 0) {
						GameObject.Find ("EnemyDamageMark").GetComponent <Canvas> ().enabled = false;
					}
					break;

				

				// unitychanが動かない版
				case "e_damage":
					// 敵のダメージアニメーション
					if (BattleScript.wait_time [0] == 39) {
						eAnimator.SetBool ("damage", true);
						GameObject.Find ("EnemyDamageMark").GetComponent <Canvas> ().enabled = true;
						enemyDamage.text = BattleScript.up_down [0];
					} else if (BattleScript.wait_time [0] == 1) {
						eAnimator.SetBool ("damage", false);
						GameObject.Find ("EnemyDamageMark").GetComponent <Canvas> ().enabled = false;
					}
					break;

				// 敵が死んだ
				case "die":
					MainAudioSource.Stop ();
					if (BattleScript.wait_time [0] == 59) {
						eAnimator.SetBool (BattleScript.action_name [0], true);
						winSource.PlayOneShot (winSource.clip);
						anim.SetBool ("win", true);
					} else if (BattleScript.wait_time [0] == 0) {
						anim.SetBool ("win", false);
					}
					break;

				
				case "fanfare":
					if (BattleScript.wait_time [0] == 39) {
						fanfareSource.PlayOneShot (fanfareSource.clip);
						anim.SetBool ("win", true);
					} else {
						anim.SetBool ("win", false);
					}
					break;
				case "gameover":
					if (BattleScript.wait_time [0] == 159) {
						MainAudioSource.Stop ();
						gameoverSource.PlayOneShot (gameoverSource.clip);
						anim.SetBool ("gameover", true);
						StatusScript.gameOver++;
					}
					break;

				case "ssm":
					if (BattleScript.wait_time [0] == 79) {
						starSource.PlayOneShot (starSource.clip);
						eAnimator.SetBool ("run", true);
						explosion.SetActive (true);
						e_fire2.SetActive (true);
						playerDamage.text = BattleScript.up_down [0];
					} else if (BattleScript.wait_time [0] == 0) {
						e_fire2.SetActive (false);
						eAnimator.SetBool ("run", false);
					}
					break;
				
				}
			}

			// メッセージを表示して一定時間経過したら
			if (BattleScript.wait_time [0] == 0) {
				//GameObject.Find (enemy).transform.position = new Vector3 (-188.9944f, 0, 64.44905f);
				BattleScript.return_message = array_delete (BattleScript.return_message, 0);
				BattleScript.wait_time = int_array_delete (BattleScript.wait_time, 0);
				BattleScript.hp_change = int_array_delete (BattleScript.hp_change, 0);
				BattleScript.mp_change = int_array_delete (BattleScript.mp_change, 0);
				BattleScript.level_change = int_array_delete (BattleScript.level_change, 0);
				BattleScript.max_hp_change = int_array_delete (BattleScript.max_hp_change, 0);
				BattleScript.max_mp_change = int_array_delete (BattleScript.max_mp_change, 0);
				BattleScript.up_down = array_delete (BattleScript.up_down, 0);
				if (BattleScript.action_name [0] != "") {
					eAnimator.SetBool (BattleScript.action_name [0], false);
				}
				BattleScript.action_name = array_delete (BattleScript.action_name, 0);

				// すべてのアクションが終了したら
				if (BattleScript.return_message.Length == 0) {
					BattleScript.turn_start = false;
					chose_command = "";
					commandSelect = 0;
					active = 0;

					// プレイヤーが死んだら
					if (BattleScript.player_die) {
						anim.SetBool ("gameover", false);
						unitychan.transform.localScale = new Vector3 (1, 1, 1);
						menu2.SetActive (true);

						bal.enabled = false;
						blackBal.enabled = false;
						stanby = false;
						BattleScript.player_die = false;
						StatusScript.hp = StatusScript.max_hp;
						StatusScript.mp = StatusScript.max_mp;
						if (PlayerPrefs.HasKey ("x")) {
							if (StatusScript.remagic == 1) {
								GameObject.Find ("unitychan").transform.position = new Vector3 (481.44f, 1.09f, 313.3017f);
								GameObject.Find ("unitychan").transform.eulerAngles = new Vector3 (0, 18.712f, 0);
							} else {
								GameObject.Find ("unitychan").transform.position = new Vector3 (22.7f, 0f, 46.1f);
								GameObject.Find ("unitychan").transform.eulerAngles = new Vector3 (0, 91.143f, 0);
							}

							//セーブしてなかったら
						} else {
							GameObject.Find ("unitychan").transform.position = new Vector3 (25.799f, 1.192f, 42.30209f);
							GameObject.Find ("unitychan").transform.eulerAngles = new Vector3 (0f, 91.14301f, 0f);
						}
						if (enemy_name == "ドラゴンキング") {
							StatusScript.story = 6;
							CameraScript.CameraChange ("Main", "Battle2");
							GameObject.Find ("PlayerSide2").transform.FindChild ("ChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
							GameObject.Find ("PlayerSide2").transform.FindChild ("ReChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
						} else {
							GameObject.Find ("EnemySide").transform.FindChild ("ChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
							GameObject.Find ("PlayerSide").transform.FindChild ("ChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
							GameObject.Find ("PlayerSide").transform.FindChild ("ReChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
							CameraScript.CameraChange ("Main", "Battle");
						}

						GameObject.Find (EnemyScript.battleMonster).transform.position = EnemyScript.enemyPosition;
						GameObject.Find (EnemyScript.battleMonster).transform.eulerAngles = new Vector3 (0, 90.663f, 0);
						GameObject.Find ("BattleMenu").GetComponent<Canvas> ().enabled = false;
						GameObject.Find ("BattleText").GetComponent<BattleMenuScript> ().enabled = false;
						GameObject.Find ("BattleField").GetComponent<Terrain> ().enabled = false;
						GameObject.Find ("BattleField2").GetComponent<Terrain> ().enabled = false;
						GameObject.Find ("BattleMenu").GetComponent<BattleScript> ().enabled = false;
						PlayerScript.changeMode (0);

						// 逃げることができたら
					} else if (BattleScript.success_run) {
						menu2.SetActive (true);
						unitychan.transform.localScale = new Vector3 (1, 1, 1);
						bal.enabled = false;
						blackBal.enabled = false;
						MainAudioSource.Stop ();
						EnemyScript.run_rag = true;
						stanby = false;
						BattleScript.success_run = false;

						if (enemy_name == "ドラゴンキング") {
							GameObject.Find ("unitychan").transform.position = new Vector3 (481.44f, 1.09f, 313.3017f);
							GameObject.Find ("unitychan").transform.eulerAngles = new Vector3 (0, 18.712f, 0);
							CameraScript.CameraChange ("Main", "Battle2");
							GameObject.Find ("PlayerSide2").transform.FindChild ("ChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
							GameObject.Find ("PlayerSide2").transform.FindChild ("ReChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
						} else {
							GameObject.Find ("unitychan").transform.position = EnemyScript.playerPosition;
							GameObject.Find ("unitychan").transform.eulerAngles = EnemyScript.playerRotation;
							GameObject.Find ("EnemySide").transform.FindChild ("ChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
							GameObject.Find ("PlayerSide").transform.FindChild ("ChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
							GameObject.Find ("PlayerSide").transform.FindChild ("ReChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
							CameraScript.CameraChange ("Main", "Battle");
						}

						GameObject.Find (EnemyScript.battleMonster).transform.position = EnemyScript.enemyPosition;
						GameObject.Find ("BattleMenu").GetComponent<Canvas> ().enabled = false;
						GameObject.Find ("BattleText").GetComponent<BattleMenuScript> ().enabled = false;
						GameObject.Find ("BattleField").GetComponent<Terrain> ().enabled = false;
						GameObject.Find ("BattleField2").GetComponent<Terrain> ().enabled = false;
						GameObject.Find ("BattleMenu").GetComponent<BattleScript> ().enabled = false;
						PlayerScript.changeMode (0);

						// 敵が死んだら
					} else if (BattleScript.enemy_die) {
						menu2.SetActive (true);
						unitychan.transform.localScale = new Vector3 (1, 1, 1);
						bal.enabled = false;
						blackBal.enabled = false;
						MainAudioSource.Stop ();
						BattleScript.enemy_die = false;
						stanby = false;
						GameObject.Find ("unitychan").transform.position = EnemyScript.playerPosition;
						GameObject.Find ("unitychan").transform.eulerAngles = EnemyScript.playerRotation;
						if (enemy_name == "ドラゴンキング") {
							StatusScript.story = 7;
							CameraScript.CameraChange ("Main", "Battle2");
							GameObject.Find ("PlayerSide2").transform.FindChild ("ChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
							GameObject.Find ("PlayerSide2").transform.FindChild ("ReChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
						} else {
							GameObject.Find ("EnemySide").transform.FindChild ("ChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
							GameObject.Find ("PlayerSide").transform.FindChild ("ChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
							GameObject.Find ("PlayerSide").transform.FindChild ("ReChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
							CameraScript.CameraChange ("Main", "Battle");
						}

						Destroy (GameObject.Find (EnemyScript.battleMonster));

						GameObject.Find ("BattleMenu").GetComponent<Canvas> ().enabled = false;
						GameObject.Find ("BattleText").GetComponent<BattleMenuScript> ().enabled = false;
						GameObject.Find ("BattleField").GetComponent<Terrain> ().enabled = false;
						GameObject.Find ("BattleField2").GetComponent<Terrain> ().enabled = false;
						GameObject.Find ("BattleMenu").GetComponent<BattleScript> ().enabled = false;
						PlayerScript.changeMode (0);
					} else {
						turn_num++;
						stanby = true;
					}
				}
			}
		}




		if (stanby) {
			if (enemy_name != "ドラゴンキング") {
				GameObject.Find (enemy).transform.position = new Vector3 (-315.5f, 0, 62.9f);
				unitychan.transform.position = new Vector3 (-309.5f, 0f, 58.7f);

			} else {
				GameObject.Find (enemy).transform.position = new Vector3 (325.9f, 0, 1169.9f);
				unitychan.transform.position = new Vector3 (344.6f, 0f, 1004f);
			}
			if (commandSelect == 0) {
				if (Input.GetKeyDown (KeyCode.RightArrow)) {
					systemSource.PlayOneShot (systemSource.clip);
					if (active == 4) {
						active = 0;
					} else {
						active++;
					}
				}
				if (Input.GetKeyDown (KeyCode.LeftArrow)) {
					systemSource.PlayOneShot (systemSource.clip);
					if (active == 0) {
						active = 4;
					} else {
						active--;
					}
				}
			
				switch (active) {
				case 0:
					myText.text = "ターン" + turn_num + "\n ▶攻撃  魔法  アイテム  防御  逃走";
					myText.text += "\n攻撃技を使用して攻撃します。";
					break;
				case 1:
					myText.text = "ターン" + turn_num + "\n  攻撃 ▶魔法  アイテム  防御  逃走";
					myText.text += "\n魔力を消費して魔法を使います。";
					break;
				case 2:
					myText.text = "ターン" + turn_num + "\n  攻撃  魔法 ▶アイテム  防御  逃走";
					myText.text += "\nアイテムを使います。";
					break;
				case 3:
					myText.text = "ターン" + turn_num + "\n  攻撃  魔法  アイテム ▶防御  逃走";
					myText.text += "\nこのターンに受けるダメージを軽減します。";
					break;
				case 4:
					myText.text = "ターン" + turn_num + "\n  攻撃  魔法  アイテム  防御 ▶逃走";
					myText.text += "\n戦闘からの離脱を試みます。";
					break;
			
				}
						
				if (Input.GetKeyUp (KeyCode.Return)) {
					returnSource.PlayOneShot (returnSource.clip);
					commandSelect = active + 1;
					active = 0;
				}

				// 攻撃を選択した時
			} else if (commandSelect == 1) {
				if (Input.GetKeyDown (KeyCode.RightArrow)) {
					systemSource.PlayOneShot (systemSource.clip);
					if (active == StatusScript.skil.Length) {
						active = 0;
					} else {
						active++;
					}
				}
				if (Input.GetKeyDown (KeyCode.LeftArrow)) {
					systemSource.PlayOneShot (systemSource.clip);
					if (active == 0) {
						active = StatusScript.skil.Length;
					} else {
						active--;
					}
				}
				myText.text = "";
				//myText.text = "ターン" + turn_num + "\n ";
				for (int i = 0; i < StatusScript.skil.Length; i++) {
					if (active == i) {
						myText.text += "▶";
						commandMessage = MenuScript.skil_message (StatusScript.skil [i]);
					}
					myText.text += StatusScript.skil [i] + " ";
				}
				if (active == StatusScript.skil.Length) {
					myText.text += "▶";
					commandMessage = "";
				}
				myText.text += "戻る";
				myText.text += "\n" + commandMessage;
				if (Input.GetKeyUp (KeyCode.Return)) {
					returnSource.PlayOneShot (returnSource.clip);
					if (active == StatusScript.skil.Length) {
						commandSelect = 0;
						active = 0;
					} else {
						chose_command = StatusScript.skil [active];
					}
				} 
					

				// 魔法を選択した時
			} else if (commandSelect == 2) {
				
				if (Input.GetKeyDown (KeyCode.RightArrow)) {
					systemSource.PlayOneShot (systemSource.clip);
					if (active == StatusScript.magic.Length) {
						active = 0;
					} else {
						active++;
					}
				}
				if (Input.GetKeyDown (KeyCode.LeftArrow)) {
					systemSource.PlayOneShot (systemSource.clip);
					if (active == 0) {
						active = StatusScript.magic.Length;
					} else {
						active--;
					}
				}
				myText.text = "";
				//myText.text = "ターン" + turn_num + "\n ";

				// 裏魔法習得時スペースキー押しながらで
				if (StatusScript.remagic == 1 && Input.GetKey (KeyCode.Space)) {
					menu2.SetActive (true);
					myText.text = "<color=#FFFFFF>";
					for (int i = 0; i < StatusScript.magic.Length; i++) {
						if (active == i) {
							myText.text += "▶";
							commandMessage = MenuScript.remagic_message (i);
						}
						myText.text += MenuScript.remagic_name (i) + " ";
					}
					if (active == StatusScript.magic.Length) {
						myText.text += "▶";
						commandMessage = "";
					}
					myText.text += "戻る";
					myText.text += "\n" + commandMessage;
					if (Input.GetKeyUp (KeyCode.Return)) {
						returnSource.PlayOneShot (returnSource.clip);
						menu2.SetActive (false);
						if (active == StatusScript.magic.Length) {
							commandSelect = 0;
							active = 1;
						} else {
							chose_command = MenuScript.remagic_name (active);
						}
					}
					myText.text += "</color>";
				} else {
					menu2.SetActive (false);
					for (int i = 0; i < StatusScript.magic.Length; i++) {
						if (active == i) {
							myText.text += "▶";
							commandMessage = MenuScript.magic_message (StatusScript.magic [i]);
						}
						myText.text += StatusScript.magic [i] + " ";
					}
					if (active == StatusScript.magic.Length) {
						myText.text += "▶";
						commandMessage = "";
					}
					myText.text += "戻る";
					myText.text += "\n" + commandMessage;
					if (Input.GetKeyUp (KeyCode.Return)) {
						returnSource.PlayOneShot (returnSource.clip);
						if (active == StatusScript.magic.Length) {
							commandSelect = 0;
							active = 1;
						} else {
							chose_command = StatusScript.magic [active];
						}
					}
				}

				// アイテムを選択した時
			} else if (commandSelect == 3) {
				if (Input.GetKeyDown (KeyCode.RightArrow)) {
					systemSource.PlayOneShot (systemSource.clip);
					if (active == StatusScript.item.Length) {
						active = 0;
					} else {
						active++;
					}
				}
				if (Input.GetKeyDown (KeyCode.LeftArrow)) {
					systemSource.PlayOneShot (systemSource.clip);
					if (active == 0) {
						active = StatusScript.item.Length;
					} else {
						active--;
					}
				}
				if (Input.GetKeyDown (KeyCode.UpArrow)) {
					systemSource.PlayOneShot (systemSource.clip);
					if (active - 5 < 0) {
						active = 0;
					} else {
						active -= 5;
					}
				}
				if (Input.GetKeyDown (KeyCode.DownArrow)) {
					systemSource.PlayOneShot (systemSource.clip);
					if (active + 5 > StatusScript.item.Length) {
						active = StatusScript.item.Length;
					} else {
						active += 5;
					}
				}
				myText.text = "";
				//myText.text = "ターン" + turn_num + "\n ";
				for (int i = 0; i < StatusScript.item.Length; i++) {
					if (active == i) {
						myText.text += "▶";
						commandMessage = MenuScript.item_message (StatusScript.item [i]);
					}
					myText.text += StatusScript.item [i] + " ";
				}
				if (active == StatusScript.item.Length) {
					myText.text += "▶";
					commandMessage = "";
				}
				myText.text += "戻る";
				myText.text += "\n" + commandMessage;
				if (Input.GetKeyUp (KeyCode.Return)) {
					returnSource.PlayOneShot (returnSource.clip);
					if (active == StatusScript.item.Length) {
						commandSelect = 0;
						active = 2;
					} else {
						chose_command = StatusScript.item [active];
						StatusScript.item = array_delete (StatusScript.item, active);
					}

				}

				//防御を選択した時
			} else if (commandSelect == 4) {
				chose_command = "防御";

				// 逃走を選択した時
			} else if (commandSelect == 5) {
				chose_command = "逃走";
			}
			
			if (chose_command != "") {
				turnStart_hp = StatusScript.hp;
				turnStart_mp = StatusScript.mp;
				stanby = false;
			}

		}
	}

	void unitychan_forward ()
	{
		float h = 0f;		// 入力デバイスの水平軸をhで定義
		float v = 0.5f;				// 入力デバイスの垂直軸をvで定義
		anim.SetFloat ("Speed", v);							// Animator側で設定している"Speed"パラメタにvを渡す
		anim.SetFloat ("Direction", h); 						// Animator側で設定している"Direction"パラメタにhを渡す
		anim.speed = 1.5f;								// Animatorのモーション再生速度に animSpeedを設定する
		currentBaseState = anim.GetCurrentAnimatorStateInfo (0);	// 参照用のステート変数にBase Layer (0)の現在のステートを設定する


		// 以下、キャラクターの移動処理
		velocity = new Vector3 (0, 0, v);		// 上下のキー入力からZ軸方向の移動量を取得
		// キャラクターのローカル空間での方向に変換
		velocity = unitychan.transform.TransformDirection (velocity);
		//以下のvの閾値は、Mecanim側のトランジションと一緒に調整する
		if (v > 0.1) {
			velocity *= forwardSpeed;		// 移動速度を掛ける
		} else if (v < -0.1) {
			velocity *= backwardSpeed;	// 移動速度を掛ける
		}
		// 上下のキー入力でキャラクターを移動させる
		unitychan.transform.localPosition += velocity * Time.fixedDeltaTime;
	}

	void unitychan_back ()
	{
		float h = 0f;		// 入力デバイスの水平軸をhで定義
		float v = -0.8f;				// 入力デバイスの垂直軸をvで定義
		anim.SetFloat ("Speed", v);							// Animator側で設定している"Speed"パラメタにvを渡す
		anim.SetFloat ("Direction", h); 						// Animator側で設定している"Direction"パラメタにhを渡す
		anim.speed = 2.5f;								// Animatorのモーション再生速度に animSpeedを設定する
		currentBaseState = anim.GetCurrentAnimatorStateInfo (0);	// 参照用のステート変数にBase Layer (0)の現在のステートを設定する


		// 以下、キャラクターの移動処理
		velocity = new Vector3 (0, 0, v);		// 上下のキー入力からZ軸方向の移動量を取得
		// キャラクターのローカル空間での方向に変換
		velocity = unitychan.transform.TransformDirection (velocity);
		//以下のvの閾値は、Mecanim側のトランジションと一緒に調整する
		if (v > 0.1) {
			velocity *= forwardSpeed;		// 移動速度を掛ける
		} else if (v < -0.1) {
			velocity *= backwardSpeed;	// 移動速度を掛ける
		}
		// 上下のキー入力でキャラクターを移動させる
		unitychan.transform.localPosition += velocity * Time.fixedDeltaTime;
	}


	// string型の配列のnum番目の要素を削除する関数
	string[] array_delete (string[] array, int num)
	{
		List<string> tempList = new List<string> (array);
		tempList.RemoveAt (num);
		return tempList.ToArray ();
	}

	//int型の配列のnum番目の要素を削除する関数
	int[] int_array_delete (int[] array, int num)
	{
		List<int> tempList = new List<int> (array);
		tempList.RemoveAt (num);
		return tempList.ToArray ();
	}
}

