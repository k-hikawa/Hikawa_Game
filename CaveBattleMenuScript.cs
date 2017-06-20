using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CaveBattleMenuScript : MonoBehaviour
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
	private MeshRenderer bal;
	private MeshRenderer blackBal;



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

		myText = GetComponentInChildren<Text> ();
		enemyText = GameObject.Find ("EnemyText").GetComponentInChildren<Text> ();
		statusText = GameObject.Find ("PlayerStatusText").GetComponentInChildren<Text> ();

		menu2 = GameObject.Find ("BattleTextBack2");
		menu2.SetActive (false);

		if (!CaveEventScript.chuboss) {
			enemy = CaveEnemyScript.battleMonster;
			string[] enemys = enemy.Split (' ');
			enemy_name = enemys [0];
			enemy_description = enemys [1];
		} else {
			enemy = GameObject.Find ("enemychan レベル1").name;
			string[] enemys = enemy.Split (' ');
			enemy_name = enemys [0];
			enemy_description = "レベル" + StatusScript.level;
		}
			
			
		CaveCameraScript.CameraChange ("Battle", "Main");
		if (enemy_name != "enemychan") {
			MainAudioSource = GameObject.Find ("BattleCamera").GetComponent<AudioSource> ();
		} else {
			MainAudioSource = GameObject.Find ("enemychan レベル1").GetComponent<AudioSource> ();
		}
		MainAudioSource.Play ();

		enemyDamage = GameObject.Find ("EnemyDamage").GetComponentInChildren<Text> ();
		enemyHeal = GameObject.Find ("EnemyHeal").GetComponentInChildren<Text> ();
		playerHeal = GameObject.Find ("PlayerHeal").GetComponentInChildren<Text> ();
		playerDamage = GameObject.Find ("PlayerDamage").GetComponentInChildren<Text> ();


		if (!CaveEventScript.chuboss) {
			eAnimator = GameObject.Find (enemy).GetComponent<Animator> ();
		} else {
			eAnimator = GameObject.Find ("enemychan レベル1").GetComponent<Animator> ();
		}
		unitychan = GameObject.Find ("unitychan");
		anim = unitychan.GetComponent<Animator> ();

			
		p_fire = GameObject.Find ("EnemySide").transform.FindChild ("Fire").gameObject;
		e_fire2 = GameObject.Find ("PlayerSide").transform.FindChild ("Fire2").gameObject;
		e_fire = GameObject.Find ("PlayerSide").transform.FindChild ("Fire").gameObject;
		d_fire = GameObject.Find ("EnemySide").transform.FindChild ("DeathFire").gameObject;
		bal = GameObject.Find ("PlayerSide").transform.FindChild ("Bal").gameObject.GetComponent <MeshRenderer> ();
		blackBal = GameObject.Find ("PlayerSide").transform.FindChild ("BlackBal").gameObject.GetComponent <MeshRenderer> ();
			
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
		wait = 60;
	}

	// Update is called once per frame
	void Update ()
	{
		// waitがある時はなくなるまで待つ
		if (wait > 0) {
			wait--;
				
		} else {
			if (wait == 0) {
				stanby = true;
				wait--;
			}
		}

		// ターン開始したら
		if (CaveBattleScript.turn_start) {
			if (CaveBattleScript.wait_time [0] < 0) {
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
					CaveBattleScript.wait_time [0]++;
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
					myText.text = "ステータスポイントを振り分けてください。 残り" + -CaveBattleScript.wait_time [0] + "ポイント\n▶体力" + StatusScript.max_hp + " 魔力" + StatusScript.max_mp + " 攻撃" + StatusScript.attack + " 防御" + StatusScript.defence + " 敏捷" + StatusScript.speed + " 運" + StatusScript.lack;
					break;
				case 1:
					myText.text = "ステータスポイントを振り分けてください。 残り" + -CaveBattleScript.wait_time [0] + "ポイント\n 体力" + StatusScript.max_hp + "▶魔力" + StatusScript.max_mp + " 攻撃" + StatusScript.attack + " 防御" + StatusScript.defence + " 敏捷" + StatusScript.speed + " 運" + StatusScript.lack;
					break;
				case 2:
					myText.text = "ステータスポイントを振り分けてください。 残り" + -CaveBattleScript.wait_time [0] + "ポイント\n 体力" + StatusScript.max_hp + " 魔力" + StatusScript.max_mp + "▶攻撃" + StatusScript.attack + " 防御" + StatusScript.defence + " 敏捷" + StatusScript.speed + " 運" + StatusScript.lack;
					break;
				case 3:
					myText.text = "ステータスポイントを振り分けてください。 残り" + -CaveBattleScript.wait_time [0] + "ポイント\n 体力" + StatusScript.max_hp + " 魔力" + StatusScript.max_mp + " 攻撃" + StatusScript.attack + "▶防御" + StatusScript.defence + " 敏捷" + StatusScript.speed + " 運" + StatusScript.lack;
					break;
				case 4:
					myText.text = "ステータスポイントを振り分けてください。 残り" + -CaveBattleScript.wait_time [0] + "ポイント\n 体力" + StatusScript.max_hp + " 魔力" + StatusScript.max_mp + " 攻撃" + StatusScript.attack + " 防御" + StatusScript.defence + "▶敏捷" + StatusScript.speed + " 運" + StatusScript.lack;
					break;
				case 5:
					myText.text = "ステータスポイントを振り分けてください。 残り" + -CaveBattleScript.wait_time [0] + "ポイント\n 体力" + StatusScript.max_hp + " 魔力" + StatusScript.max_mp + " 攻撃" + StatusScript.attack + " 防御" + StatusScript.defence + " 敏捷" + StatusScript.speed + "▶運" + StatusScript.lack;
					break;
				}

				// ステータスの更新
				hp_slider.maxValue = CaveBattleScript.max_hp_change [0];
				mp_slider.maxValue = CaveBattleScript.max_mp_change [0];
				hp_slider.value = CaveBattleScript.hp_change [0];
				mp_slider.value = CaveBattleScript.mp_change [0];

				statusText.text = "レベル" + CaveBattleScript.level_change [0] + "\n体力：";
				if (CaveBattleScript.max_hp_change [0] / 5 >= CaveBattleScript.hp_change [0]) {
					statusText.text += "<color=red>" + CaveBattleScript.hp_change [0] + "</color>";
					hp_sliderC.color = new Color (1f, 0f, 0f);
				} else if (CaveBattleScript.max_hp_change [0] / 2 >= CaveBattleScript.hp_change [0]) {
					statusText.text += "<color=yellow>" + CaveBattleScript.hp_change [0] + "</color>";
					hp_sliderC.color = new Color (1f, 1f, 0f);
				} else {
					statusText.text += CaveBattleScript.hp_change [0];
					hp_sliderC.color = new Color (0f, 1f, 0f);
				}
				statusText.text += "/" + CaveBattleScript.max_hp_change [0] + "\n\n魔力：";
				if (CaveBattleScript.max_mp_change [0] / 5 >= CaveBattleScript.mp_change [0]) {
					statusText.text += "<color=red>" + CaveBattleScript.mp_change [0] + "</color>";
					mp_sliderC.color = new Color (1f, 0f, 0f);
				} else if (CaveBattleScript.max_mp_change [0] / 2 >= CaveBattleScript.mp_change [0]) {
					statusText.text += "<color=yellow>" + CaveBattleScript.mp_change [0] + "</color>";
					mp_sliderC.color = new Color (1f, 1f, 0f);
				} else {
					statusText.text += CaveBattleScript.mp_change [0];
					mp_sliderC.color = new Color (0f, 1f, 0f);
				}
				statusText.text += "/" + CaveBattleScript.max_mp_change [0];

			} else {
				CaveBattleScript.wait_time [0]--;
				myText.text = CaveBattleScript.return_message [0];
				// ステータスの更新
				hp_slider.maxValue = CaveBattleScript.max_hp_change [0];
				mp_slider.maxValue = CaveBattleScript.max_mp_change [0];
				hp_slider.value = CaveBattleScript.hp_change [0];
				mp_slider.value = CaveBattleScript.mp_change [0];

				statusText.text = "レベル" + CaveBattleScript.level_change [0] + "\n体力：";
				if (CaveBattleScript.max_hp_change [0] / 5 >= CaveBattleScript.hp_change [0]) {
					statusText.text += "<color=red>" + CaveBattleScript.hp_change [0] + "</color>";
					hp_sliderC.color = new Color (1f, 0f, 0f);
				} else if (CaveBattleScript.max_hp_change [0] / 2 >= CaveBattleScript.hp_change [0]) {
					statusText.text += "<color=yellow>" + CaveBattleScript.hp_change [0] + "</color>";
					hp_sliderC.color = new Color (1f, 1f, 0f);
				} else {
					statusText.text += CaveBattleScript.hp_change [0];
					hp_sliderC.color = new Color (0f, 1f, 0f);
				}
				statusText.text += "/" + CaveBattleScript.max_hp_change [0] + "\n\n魔力：";

				if (CaveBattleScript.max_mp_change [0] / 5 >= CaveBattleScript.mp_change [0]) {
					statusText.text += "<color=red>" + CaveBattleScript.mp_change [0] + "</color>";
					mp_sliderC.color = new Color (1f, 0f, 0f);
				} else if (CaveBattleScript.max_mp_change [0] / 2 >= CaveBattleScript.mp_change [0]) {
					statusText.text += "<color=yellow>" + CaveBattleScript.mp_change [0] + "</color>";
					mp_sliderC.color = new Color (1f, 1f, 0f);
				} else {
					statusText.text += CaveBattleScript.mp_change [0];
					mp_sliderC.color = new Color (0f, 1f, 0f);
				}
				statusText.text += "/" + CaveBattleScript.max_mp_change [0];
			}
			if (CaveBattleScript.action_name [0] != "") {

				// アニメーションを行う内容
				switch (CaveBattleScript.action_name [0]) {
				case "run":
						//unitychan_back ();
					if (CaveBattleScript.wait_time [0] == 59) {
						runSource.PlayOneShot (runSource.clip);
					}
					break;

				case "panch":
					if (CaveBattleScript.wait_time [0] == 39) {
						//unitychan_forward ();
						anim.SetBool ("panch", true);
					} else if (CaveBattleScript.wait_time [0] == 0) {
						anim.SetBool ("panch", false);
						attackSource.PlayOneShot (attackSource.clip);
					}

					break;

				case "e_panch":
					if (CaveBattleScript.wait_time [0] == 39) {
						//unitychan_forward ();
						eAnimator.SetBool ("panch", true);
					} else if (CaveBattleScript.wait_time [0] == 0) {
						eAnimator.SetBool ("panch", false);
						attackSource.PlayOneShot (attackSource.clip);
					}

					break;


				case "kick":
					if (CaveBattleScript.wait_time [0] == 39) {
						//unitychan_forward ();
						anim.SetBool ("kick", true);
					} else if (CaveBattleScript.wait_time [0] == 0) {
						anim.SetBool ("kick", false);
						attackSource.PlayOneShot (attackSource.clip);
					}
					break;
				case "e_kick":
					if (CaveBattleScript.wait_time [0] == 39) {
						//unitychan_forward ();
						eAnimator.SetBool ("kick", true);
					} else if (CaveBattleScript.wait_time [0] == 0) {
						eAnimator.SetBool ("kick", false);
						attackSource.PlayOneShot (attackSource.clip);
					}
					break;

				case "slide":
					if (CaveBattleScript.wait_time [0] == 39) {

						anim.SetBool ("slide", true);
					} else if (CaveBattleScript.wait_time [0] == 0) {
						anim.SetBool ("slide", false);
						attackSource.PlayOneShot (attackSource.clip);
					}
					break;

				case "miss":
						//unitychan_back ();
					break;


				case "magic":
					if (CaveBattleScript.wait_time [0] == 39) {
						magicSource.PlayOneShot (magicSource.clip);
						anim.SetBool ("magic", true);

					} else if (CaveBattleScript.wait_time [0] == 0) {
						anim.SetBool ("magic", false);
					}
					break;

				case "remagic":
					if (CaveBattleScript.wait_time [0] == 59) {
						remagicSource.PlayOneShot (remagicSource.clip);
						anim.SetBool ("magic", true);
						CavePlayerScript.enemychan_magic.SetActive (true);
						CavePlayerScript.enemychananim.SetBool ("magic", true);

					} else if (CaveBattleScript.wait_time [0] == 0) {
						anim.SetBool ("magic", false);
						CavePlayerScript.enemychan_magic.SetActive (false);
						CavePlayerScript.enemychananim.SetBool ("magic", false);
					}
					break;

				case "e_magic":
					if (CaveBattleScript.wait_time [0] == 39) {
						magicSource.PlayOneShot (magicSource.clip);
						if (!CaveEventScript.chuboss) {
							eAnimator.SetBool ("attack", true);
						} else {
							eAnimator.SetBool ("magic", true);
						}

					} else if (CaveBattleScript.wait_time [0] == 0) {
						if (!CaveEventScript.chuboss) {
							eAnimator.SetBool ("attack", false);
						} else {
							eAnimator.SetBool ("magic", false);
						}
					}
					break;

				case "fire":
					if (CaveBattleScript.wait_time [0] == 59) {
						p_fire.SetActive (true);
						eAnimator.SetBool ("damage", true);
						GameObject.Find ("EnemyDamageMark").GetComponent <Canvas> ().enabled = true;
						enemyDamage.text = CaveBattleScript.up_down [0];
					} else if (CaveBattleScript.wait_time [0] == 0) {
						eAnimator.SetBool ("damage", false);
						p_fire.SetActive (false);
						GameObject.Find ("EnemyDamageMark").GetComponent <Canvas> ().enabled = false;
					}
					break;

				case "e_fire":
					if (CaveBattleScript.wait_time [0] == 59) {
						e_fire.SetActive (true);
						anim.SetBool ("damage", true);
						GameObject.Find ("PlayerDamageMark").GetComponent <Canvas> ().enabled = true;
						playerDamage.text = CaveBattleScript.up_down [0];
					} else if (CaveBattleScript.wait_time [0] == 0) {
						anim.SetBool ("damage", false);
						e_fire.SetActive (false);
						GameObject.Find ("PlayerDamageMark").GetComponent <Canvas> ().enabled = false;
					}
					break;

				case "e_fire2":
					if (CaveBattleScript.wait_time [0] == 59) {
						e_fire2.SetActive (true);
						anim.SetBool ("damage", true);
						GameObject.Find ("PlayerDamageMark").GetComponent <Canvas> ().enabled = true;
						playerDamage.text = CaveBattleScript.up_down [0];
					} else if (CaveBattleScript.wait_time [0] == 0) {
						e_fire2.SetActive (false);
						anim.SetBool ("damage", false);
						GameObject.Find ("PlayerDamageMark").GetComponent <Canvas> ().enabled = false;
					}
					break;

				case "d_fire":
					if (CaveBattleScript.wait_time [0] == 59) {
						d_fire.SetActive (true);
						eAnimator.SetBool ("damage", true);
						GameObject.Find ("EnemyDamageMark").GetComponent <Canvas> ().enabled = true;
						enemyDamage.text = CaveBattleScript.up_down [0];
					} else if (CaveBattleScript.wait_time [0] == 0) {
						eAnimator.SetBool ("damage", false);
						d_fire.SetActive (false);
						GameObject.Find ("EnemyDamageMark").GetComponent <Canvas> ().enabled = false;
					}
					break;

				// 敵が回復した
				case "e_heal":
					if (CaveBattleScript.wait_time [0] == 39) {
						healSource.PlayOneShot (healSource.clip);
						GameObject.Find ("EnemyHealMark").GetComponent <Canvas> ().enabled = true;
						enemyHeal.text = CaveBattleScript.up_down [0];
					} else if (CaveBattleScript.wait_time [0] == 0) {
						GameObject.Find ("EnemyHealMark").GetComponent <Canvas> ().enabled = false;

					}
					break;

				case "heal":
					if (CaveBattleScript.wait_time [0] == 39) {
						healSource.PlayOneShot (healSource.clip);

						GameObject.Find ("PlayerSide").transform.FindChild ("HealParticle").gameObject.GetComponent<ParticleSystem> ().Play ();
						GameObject.Find ("PlayerHealMark").GetComponent <Canvas> ().enabled = true;
						playerHeal.text = CaveBattleScript.up_down [0];
					} else if (CaveBattleScript.wait_time [0] == 0) {
						GameObject.Find ("PlayerHealMark").GetComponent <Canvas> ().enabled = false;

					}
					break;

				case "bal":
					if (CaveBattleScript.wait_time [0] == 39) {
						bal.enabled = true;
					}
					break;

				case "bal_break":
					if (CaveBattleScript.wait_time [0] == 39) {
						bal.enabled = false;
					}
					break;

				case "blackBal":
					if (CaveBattleScript.wait_time [0] == 39) {
						blackBal.enabled = true;
					}
					break;

				case "balBlack_break":
					if (CaveBattleScript.wait_time [0] == 39) {
						blackBal.enabled = false;
					}
					break;

				case "charge":
					if (CaveBattleScript.wait_time [0] == 39) {
						magicSource.PlayOneShot (magicSource.clip);
						anim.SetBool ("magic", true);

					} else if (CaveBattleScript.wait_time [0] == 0) {
						anim.SetBool ("magic", false);
						if (enemy_name == "ドラゴンキング") {
							GameObject.Find ("PlayerSide2").transform.FindChild ("ChargeParticle").gameObject.GetComponent<ParticleSystem> ().Play ();
						} else {
							GameObject.Find ("PlayerSide").transform.FindChild ("ChargeParticle").gameObject.GetComponent<ParticleSystem> ().Play ();
						}
					}
					break;
				case "charge_end":
					if (CaveBattleScript.wait_time [0] == 39) {
						if (enemy_name == "ドラゴンキング") {
							GameObject.Find ("PlayerSide2").transform.FindChild ("ChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
						} else {
							GameObject.Find ("PlayerSide").transform.FindChild ("ChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
						}
					}
					break;
				case "e_charge":
					if (CaveBattleScript.wait_time [0] == 39) {
						magicSource.PlayOneShot (magicSource.clip);
						if (CaveEventScript.chuboss) {
							eAnimator.SetBool ("magic", true);
						} else {
							eAnimator.SetBool ("attack2", true);
						}
					} else if (CaveBattleScript.wait_time [0] == 0) {
						if (CaveEventScript.chuboss) {
							eAnimator.SetBool ("magic", false);
						} else {
							eAnimator.SetBool ("attack2", false);
						}
						GameObject.Find ("EnemySide").transform.FindChild ("ChargeParticle").gameObject.GetComponent<ParticleSystem> ().Play ();
					}
					break;

				case "e_charge_end":
					if (CaveBattleScript.wait_time [0] == 39) {
						GameObject.Find ("EnemySide").transform.FindChild ("ChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
					}
					break;

				case "recharge":
					if (CaveBattleScript.wait_time [0] == 59) {
						remagicSource.PlayOneShot (remagicSource.clip);
						anim.SetBool ("magic", true);
						CavePlayerScript.enemychan_magic.SetActive (true);
						CavePlayerScript.enemychananim.SetBool ("magic", true);
					} else if (CaveBattleScript.wait_time [0] == 0) {
						anim.SetBool ("magic", false);

						CavePlayerScript.enemychan_magic.SetActive (false);
						CavePlayerScript.enemychananim.SetBool ("magic", false);
							

						GameObject.Find ("PlayerSide").transform.FindChild ("ReChargeParticle").gameObject.GetComponent<ParticleSystem> ().Play ();

					}

					break;
				case "recharge_end":
					if (CaveBattleScript.wait_time [0] == 39) {
						
						GameObject.Find ("PlayerSide").transform.FindChild ("ReChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();

					}
					break;

				// 敵の攻撃
				case "attack":

				case "attack3":

					if (CaveBattleScript.wait_time [0] == 39) {
						//eAnimator.SetBool ("move", true);
						//GameObject.Find (enemy).transform.position += GameObject.Find (enemy).transform.TransformDirection (Vector3.forward) * 0.1f;
						eAnimator.SetBool (CaveBattleScript.action_name [0], true);
						// 敵の攻撃アニメーション
					} else {
						eAnimator.SetBool (CaveBattleScript.action_name [0], false);
						/*if (BattleScript.wait_time [0] == 0) {
							attackSource.PlayOneShot (attackSource.clip);
						}*/
					}
					break;

				case "attack2":
					if (CaveBattleScript.wait_time [0] == 59) {
						eAnimator.SetBool (CaveBattleScript.action_name [0], true);

						// 敵の攻撃アニメーション
					} else if (CaveBattleScript.wait_time [0] == 0) {
						eAnimator.SetBool (CaveBattleScript.action_name [0], false);
					}

					break;
				case "scream":
					if (CaveBattleScript.wait_time [0] == 39) {
						eAnimator.SetBool ("attack3", true);
						screamSource.PlayOneShot (screamSource.clip);
						// 敵の攻撃アニメーション
					} else if (CaveBattleScript.wait_time [0] == 0) {
						eAnimator.SetBool ("attack3", false);
					}


					break;


				// プレイヤーがダメージを受けた
				case "p_damage":
						// 敵が元の位置に移動
					if (CaveBattleScript.wait_time [0] == 39) {
						anim.SetBool ("damage", true);
						//GameObject.Find (enemy).transform.position -= GameObject.Find (enemy).transform.TransformDirection (Vector3.forward) * 0.08f;
						GameObject.Find ("PlayerDamageMark").GetComponent <Canvas> ().enabled = true;
						playerDamage.text = CaveBattleScript.up_down [0];
						attackSource.PlayOneShot (attackSource.clip);
						// 敵が元の位置に到着
					} else if (CaveBattleScript.wait_time [0] == 0) {
						//eAnimator.SetBool ("move", false);
						anim.SetBool ("damage", false);
						GameObject.Find ("PlayerDamageMark").GetComponent <Canvas> ().enabled = false;
					}

					break;

				// プレイヤーが大ダメージを受けた
				case "p_damage2":
						// 敵が元の位置に移動
					if (CaveBattleScript.wait_time [0] == 39) {
						anim.SetBool ("damage2", true);
						GameObject.Find ("PlayerDamageMark").GetComponent <Canvas> ().enabled = true;
						playerDamage.text = CaveBattleScript.up_down [0];
						criticalSource.PlayOneShot (criticalSource.clip);
						// 敵が元の位置に到着
					} else if (CaveBattleScript.wait_time [0] == 0) {

						anim.SetBool ("damage2", false);
						GameObject.Find ("PlayerDamageMark").GetComponent <Canvas> ().enabled = false;
					}

					break;

				case "e_miss":
						// 敵が元の位置に移動
					if (CaveBattleScript.wait_time [0] >= 39) {
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
					if (CaveBattleScript.wait_time [0] == 39) {
						eAnimator.SetBool (CaveBattleScript.action_name [0], true);
						GameObject.Find ("EnemyDamageMark").GetComponent <Canvas> ().enabled = true;
						enemyDamage.text = CaveBattleScript.up_down [0];
					} else if (CaveBattleScript.wait_time [0] == 0) {
						GameObject.Find ("EnemyDamageMark").GetComponent <Canvas> ().enabled = false;
					}
					break;



				// unitychanが動かない版
				case "e_damage":
						// 敵のダメージアニメーション
					if (CaveBattleScript.wait_time [0] == 39) {
						eAnimator.SetBool ("damage", true);
						GameObject.Find ("EnemyDamageMark").GetComponent <Canvas> ().enabled = true;
						enemyDamage.text = CaveBattleScript.up_down [0];
					} else if (CaveBattleScript.wait_time [0] == 1) {
						eAnimator.SetBool ("damage", false);
						GameObject.Find ("EnemyDamageMark").GetComponent <Canvas> ().enabled = false;
					}
					break;

				// 敵が死んだ
				case "die":
					MainAudioSource.Stop ();
					if (CaveBattleScript.wait_time [0] == 59) {
						if (!CaveEventScript.chuboss) {
							eAnimator.SetBool (CaveBattleScript.action_name [0], true);
						} else {
							eAnimator.SetBool ("gameover", true);
						}
						winSource.PlayOneShot (winSource.clip);
						anim.SetBool ("win", true);
					} else if (CaveBattleScript.wait_time [0] == 0) {
						anim.SetBool ("win", false);
					}
					break;


				case "fanfare":
					if (CaveBattleScript.wait_time [0] == 39) {
						fanfareSource.PlayOneShot (fanfareSource.clip);
						anim.SetBool ("win", true);
					} else {
						anim.SetBool ("win", false);
					}
					break;
				case "gameover":
					if (CaveBattleScript.wait_time [0] == 159) {
						MainAudioSource.Stop ();
						gameoverSource.PlayOneShot (gameoverSource.clip);
						anim.SetBool ("gameover", true);
						StatusScript.gameOver++;
					}
					break;

				}
			}

			// メッセージを表示して一定時間経過したら
			if (CaveBattleScript.wait_time [0] == 0) {
				//GameObject.Find (enemy).transform.position = new Vector3 (-188.9944f, 0, 64.44905f);
				CaveBattleScript.return_message = array_delete (CaveBattleScript.return_message, 0);
				CaveBattleScript.wait_time = int_array_delete (CaveBattleScript.wait_time, 0);
				CaveBattleScript.hp_change = int_array_delete (CaveBattleScript.hp_change, 0);
				CaveBattleScript.mp_change = int_array_delete (CaveBattleScript.mp_change, 0);
				CaveBattleScript.level_change = int_array_delete (CaveBattleScript.level_change, 0);
				CaveBattleScript.max_hp_change = int_array_delete (CaveBattleScript.max_hp_change, 0);
				CaveBattleScript.max_mp_change = int_array_delete (CaveBattleScript.max_mp_change, 0);
				CaveBattleScript.up_down = array_delete (CaveBattleScript.up_down, 0);
				if (CaveBattleScript.action_name [0] != "") {
					eAnimator.SetBool (CaveBattleScript.action_name [0], false);
				}
				CaveBattleScript.action_name = array_delete (CaveBattleScript.action_name, 0);

				// すべてのアクションが終了したら
				if (CaveBattleScript.return_message.Length == 0) {
					CaveBattleScript.turn_start = false;
					chose_command = "";
					commandSelect = 0;
					active = 0;

					// プレイヤーが死んだら
					if (CaveBattleScript.player_die) {
						anim.SetBool ("gameover", false);

						menu2.SetActive (true);

						bal.enabled = false;
						blackBal.enabled = false;
						stanby = false;
						CaveBattleScript.player_die = false;
						StatusScript.hp = StatusScript.max_hp;
						StatusScript.mp = StatusScript.max_mp;
							

						GameObject.Find ("unitychan").transform.position = new Vector3 (-5.5f, -0.628f, 75.89f);
						GameObject.Find ("unitychan").transform.eulerAngles = new Vector3 (0, 180, 0);

							
						GameObject.Find ("EnemySide").transform.FindChild ("ChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
						GameObject.Find ("PlayerSide").transform.FindChild ("ChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
						GameObject.Find ("PlayerSide").transform.FindChild ("ReChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
						CaveCameraScript.CameraChange ("Main", "Battle");
							
						if (enemy_name == "enemychan") {
							CaveEventScript.chuboss = false;
							StatusScript.story = 1;
							GameObject.Find ("enemychan レベル1").transform.position = new Vector3 (-57.86483f, -0.2216457f, -90.522f);
							GameObject.Find ("enemychan レベル1").transform.eulerAngles = new Vector3 (0, 0, 0);

						} else {
							
							GameObject.Find (CaveEnemyScript.battleMonster).transform.position = CaveEnemyScript.enemyPosition;
							GameObject.Find (CaveEnemyScript.battleMonster).transform.eulerAngles = new Vector3 (0, 90.663f, 0);
						}
						GameObject.Find ("BattleMenu").GetComponent<Canvas> ().enabled = false;
						GameObject.Find ("BattleText").GetComponent<CaveBattleMenuScript> ().enabled = false;
						GameObject.Find ("BattleMenu").GetComponent<CaveBattleScript> ().enabled = false;
						PlayerScript.changeMode (0);

						// 逃げることができたら
					} else if (CaveBattleScript.success_run) {
						menu2.SetActive (true);

						bal.enabled = false;
						blackBal.enabled = false;
						MainAudioSource.Stop ();
						CaveEnemyScript.run_rag = true;
						stanby = false;
						CaveBattleScript.success_run = false;

						GameObject.Find ("unitychan").transform.position = CaveEnemyScript.playerPosition;
						GameObject.Find ("unitychan").transform.eulerAngles = CaveEnemyScript.playerRotation;
						

						GameObject.Find ("EnemySide").transform.FindChild ("ChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
						GameObject.Find ("PlayerSide").transform.FindChild ("ChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
						GameObject.Find ("PlayerSide").transform.FindChild ("ReChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
						CaveCameraScript.CameraChange ("Main", "Battle");
							
						GameObject.Find (CaveEnemyScript.battleMonster).transform.position = CaveEnemyScript.enemyPosition;
						GameObject.Find ("BattleMenu").GetComponent<Canvas> ().enabled = false;
						GameObject.Find ("BattleText").GetComponent<CaveBattleMenuScript> ().enabled = false;
						GameObject.Find ("BattleMenu").GetComponent<CaveBattleScript> ().enabled = false;
						PlayerScript.changeMode (0);

						// 敵が死んだら
					} else if (CaveBattleScript.enemy_die) {
						menu2.SetActive (true);

						bal.enabled = false;
						blackBal.enabled = false;
						MainAudioSource.Stop ();
						CaveBattleScript.enemy_die = false;
						stanby = false;


						if (enemy_name != "enemychan") {
							GameObject.Find ("unitychan").transform.position = CaveEnemyScript.playerPosition;
							GameObject.Find ("unitychan").transform.eulerAngles = CaveEnemyScript.playerRotation;
							Destroy (GameObject.Find (CaveEnemyScript.battleMonster));
						} else {
							eAnimator.SetBool ("gameover", false);
							StatusScript.story++;
							GameObject.Find ("unitychan").transform.position = new Vector3 (-63.02083f, -0.8799973f, -13.48889f);
							GameObject.Find ("unitychan").transform.eulerAngles = new Vector3 (0, 180, 0);
							//GameObject.Find ("enemychan レベル1").transform.position = new Vector3 (-57.86483f, -0.2216457f, -90.522f);
							//GameObject.Find ("unitychan").transform.LookAt (GameObject.Find ("enemychan レベル1").transform.position);
							Destroy (GameObject.Find (CaveEnemyScript.battleMonster));
							//GameObject.Find ("enemychan レベル1").transform.LookAt (unitychan.transform.position);
						}

						GameObject.Find ("EnemySide").transform.FindChild ("ChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
						GameObject.Find ("PlayerSide").transform.FindChild ("ChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
						GameObject.Find ("PlayerSide").transform.FindChild ("ReChargeParticle").gameObject.GetComponent<ParticleSystem> ().Stop ();
						CaveCameraScript.CameraChange ("Main", "Battle");


						GameObject.Find ("BattleMenu").GetComponent<Canvas> ().enabled = false;
						GameObject.Find ("BattleText").GetComponent<CaveBattleMenuScript> ().enabled = false;
						GameObject.Find ("BattleMenu").GetComponent<CaveBattleScript> ().enabled = false;
						PlayerScript.changeMode (0);
					} else {
						turn_num++;
						stanby = true;
					}
				}
			}
		}




		if (stanby) {
			
			GameObject.Find (enemy).transform.position = new Vector3 (-84.48f, -0.45f, 76.04f);
			unitychan.transform.position = new Vector3 (-82.337f, -0.531f, 74.65f);
				
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
