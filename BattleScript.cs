using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BattleScript : MonoBehaviour
{


	public static string[] return_message = new string[0];
	public static int[] wait_time = new int[0];
	public static int[] hp_change = new int[0];
	public static int[] mp_change = new int[0];
	public static int[] level_change = new int[0];
	public static int[] max_hp_change = new int[0];
	public static int[] max_mp_change = new int[0];
	public static string[] action_name = new string[0];
	public static string[] up_down = new string[0];
	int score;

	public static bool turn_start;

	public static bool player_die;
	public static bool enemy_die;
	public static bool success_run;

	string enemy;

	string enemy_name;
	private int enemy_level;
	private int enemy_hp;
	private int enemy_attack;
	private int enemy_defence;
	private int enemy_speed;
	private int enemy_lack;
	private int enemy_exp;
	private string enemy_drop;
	private string[] enemy_skil = new string[5];
	private int enemy_magic_gurd;


	bool pre_player;

	int dragon_charge;

	bool command_d;

	bool charge;
	bool recharge;
	int bal;
	bool blackBal;

	bool enemy_charge;

	// Use this for initialization
	void OnEnable ()
	{
		this.GetComponent<Canvas> ().enabled = true;
		enemy = EnemyScript.battleMonster;
		string[] enemys = enemy.Split (' ');
		enemy_name = enemys [0];
		enemy_level = int.Parse (enemys [1].Substring (3));
		enemy_status ();
		charge = false;
		recharge = false;
		bal = 0;
		blackBal = false;
		enemy_charge = false;
		dragon_charge = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// ターンアニメーション中とバトルコマンドが選ばれていない時は処理しない
		if (!turn_start && BattleMenuScript.chose_command != "") {
			// 素早さに関係のない特殊なコマンドの処理
			switch (BattleMenuScript.chose_command) {
			case "防御":
				send_action ("身を守った！", "", 40);
				command_d = true;
				break;
			case "逃走":
				send_action ("逃げ出した!", "", 40);
				if (run_compare (StatusScript.speed, StatusScript.level, enemy_speed, enemy_level)) {
					if (enemy_name == "ドラゴンキング") {
						send_action ("しかし、回り込まれてしまった...", "", 40);
					} else {
						send_action ("うまく逃げ切れた！", "run", 60);
						success_run = true;
					}
				} else {
					send_action ("しかし、回り込まれてしまった...", "", 40);
				}
				break;
			case "ウサミミ":
				send_action ("ウサミミを使って逃げ出した!", "", 40);
				if (enemy_name == "ドラゴンキング") {
					send_action ("しかし、回り込まれてしまった...", "", 40);
				} else {
					send_action ("うまく逃げ切れた！", "run", 60);
					success_run = true;
				}
				break;
			case "やくそう":
				send_action ("やくそうを使った！", "", 40);
				StatusScript.hp += 50;
				score = 50;
				if (StatusScript.hp > StatusScript.max_hp) {
					StatusScript.hp = StatusScript.max_hp;
				}
				send_action ("体力が50回復した！", "heal", 40);
				break;
			case "せいすい":
				send_action ("せいすいを使った！", "", 40);
				StatusScript.mp += 50;
				score = 50;
				if (StatusScript.mp > StatusScript.max_mp) {
					StatusScript.mp = StatusScript.max_mp;
				}
				send_action ("魔力が50回復した！", "heal", 40);
				break;
			case "上やくそう":
				send_action ("上やくそうを使った！", "", 40);
				StatusScript.hp += 150;
				score = 150;
				if (StatusScript.hp > StatusScript.max_hp) {
					StatusScript.hp = StatusScript.max_hp;
				}
				send_action ("体力が150回復した！", "heal", 40);
				break;
			case "回復薬":
				send_action ("回復薬を使った！", "", 40);
				StatusScript.hp += 300;
				score = 300;
				if (StatusScript.hp > StatusScript.max_hp) {
					StatusScript.hp = StatusScript.max_hp;
				}
				send_action ("体力が300回復した！", "heal", 40);
				break;
			case "魔法薬":
				send_action ("魔法薬を使った！", "", 40);
				StatusScript.mp += 200;
				score = 200;
				if (StatusScript.mp > StatusScript.max_mp) {
					StatusScript.mp = StatusScript.max_mp;
				}
				send_action ("魔力が200回復した！", "heal", 40);
				break;
			}
			if (!success_run) {
				pre_player = speed_compare ();
				// 素早さ比較で先攻を決める
				if (pre_player) {
					player_pattern ();
					send_action ("", "wait", 20);
					// どちらも死んでないなら
					if (!player_die && !enemy_die) {
						enemy_pattern ();
						if (!player_die && !enemy_die) {
							if (enemy_name == "ドラゴンキング" && dragon_charge != 1 && dragon_charge != 2 && Random.Range (0, 2) == 0) {
								send_action ("", "wait", 20);
								enemy_pattern ();
							}
						}
					}
				} else {
					enemy_pattern ();
					if (!player_die && !enemy_die) {
						if (enemy_name == "ドラゴンキング" && dragon_charge != 1 && dragon_charge != 2 && Random.Range (0, 2) == 0) {
							send_action ("", "wait", 20);
							enemy_pattern ();
						}
					}
					send_action ("", "wait", 20);
					if (!player_die && !enemy_die) {
						player_pattern ();
					}
				}
				if (!player_die && !enemy_die) {
					if (blackBal) {
						blackBal = false;
						send_action ("ブラックバリアの効果が切れた", "balBlack_break", 40);
					}
					if (bal > 0) {
						bal--;
						if (bal == 0) {
							send_action ("バリアの効果が切れた", "bal_break", 40);
						}
					}
				}
			}

			turn_start = true;
		}
	}
		


	// プレイヤーの行動
	void player_pattern ()
	{
		switch (BattleMenuScript.chose_command) {
		case "パンチ":
			send_action ("パンチで攻撃した！", "panch", 40);


			score = damage (Mathf.FloorToInt (StatusScript.attack * 1.5f), StatusScript.lack, enemy_defence, enemy_lack);

			if (charge) {
				score *= 3;
			}
			enemy_hp -= score;
			if (score != 0) {
				send_action (enemy_name + "に" + score + "のダメージ！", "damage", 40);
			} else {
				send_action ("しかし、攻撃は外れてしまった...", "miss", 40);
			}
			if (charge) {
				charge = false;
				send_action ("チャージの効果が切れた", "charge_end", 40);
			}
			break;
		case "キック":
			send_action ("キックで攻撃した！", "kick", 40);

			score = damage (Mathf.FloorToInt (StatusScript.attack * 1.8f), Mathf.FloorToInt (StatusScript.lack / 2), enemy_defence, enemy_lack);

			if (charge) {
				score *= 3;
			}
			enemy_hp -= score;
			if (score != 0) {
				send_action (enemy_name + "に" + score + "のダメージ！", "damage", 40);
			} else {
				send_action ("しかし、攻撃は外れてしまった...", "miss", 40);
			}
			if (charge) {
				charge = false;
				send_action ("チャージの効果が切れた", "charge_end", 40);
			}
			break;

		case "スライディング":
			send_action ("スライディングで攻撃した！", "slide", 40);

			score = damage (Mathf.FloorToInt (StatusScript.attack * 1.2f), Mathf.FloorToInt (StatusScript.lack), Mathf.FloorToInt (enemy_defence / 2), enemy_lack);

			if (charge) {
				score *= 3;
			}
			enemy_hp -= score;
			if (score != 0) {
				send_action (enemy_name + "に" + score + "のダメージ！", "damage", 40);
			} else {
				send_action ("しかし、攻撃は外れてしまった...", "miss", 40);
			}
			if (charge) {
				charge = false;
				send_action ("チャージの効果が切れた", "charge_end", 40);
			}
			break;

		case "ファイア":
			send_action ("ファイアを唱えた！", "magic", 40);
			if (StatusScript.mp >= 6) {
				StatusScript.mp -= 6;

				if (recharge) {
					score = Random.Range (25, 45) * 3 - enemy_magic_gurd;
				} else {
					score = Random.Range (25, 45) - enemy_magic_gurd;
				}
				if (score <= 0) {
					score = 1;
				}
				enemy_hp -= score;
				send_action (enemy_name + "に" + score + "のダメージ！", "fire", 60);
				if (recharge) {
					recharge = false;
					send_action ("魔の波動が切れた", "recharge_end", 40);
				}
			} else {
				
				send_action ("しかし、魔力が足りなかった...", "", 40);
			}
			break;
		case "デスファイア":
			
			if (StatusScript.mp >= 20) {
				send_action ("デスファイアを唱えた！", "remagic", 60);
				StatusScript.mp -= 20;

				if (recharge) {
					score = Random.Range (120, 161) * 3 - enemy_magic_gurd;
				} else {
					score = Random.Range (120, 161) - enemy_magic_gurd;
				}

				if (score <= 0) {
					score = 1;
				}
				enemy_hp -= score;
				send_action (enemy_name + "に" + score + "のダメージ！", "d_fire", 60);
				if (recharge) {
					recharge = false;
					send_action ("魔の波動が切れた", "recharge_end", 40);
				}
			} else {
				send_action ("デスファイアを唱えた！", "magic", 40);
				send_action ("しかし、魔力が足りなかった...", "", 40);
			}
			break;
		case "ヒール":
			send_action ("ヒールを唱えた！", "magic", 40);
			if (StatusScript.mp >= 13) {
				StatusScript.mp -= 13;

				if (recharge) {
					score = Random.Range (85, 116) * 3;
				} else {
					score = Random.Range (85, 116);
				}

				StatusScript.hp += score;
				if (StatusScript.hp > StatusScript.max_hp) {
					StatusScript.hp = StatusScript.max_hp;
				}
				send_action ("体力が" + score + "回復した！", "heal", 40);
				if (recharge) {
					recharge = false;
					send_action ("魔の波動が切れた", "recharge_end", 40);
				}
			} else {
				
				send_action ("しかし、魔力が足りなかった...", "", 40);
			}
			break;
		case "吸魂":
			
			if (StatusScript.mp >= 28) {
				send_action ("吸魂した！", "remagic", 60);
				StatusScript.mp -= 28;
				if (recharge) {
					score = Random.Range (55, 76) * 3;
				} else {
					score = Random.Range (55, 76);
				}

				if (score <= 0) {
					score = 1;
				}
				enemy_hp -= score;
				send_action (enemy_name + "に" + score + "のダメージ！", "e_damage", 40);
				score = Mathf.FloorToInt (score * Random.Range (1.8f, 2.0f));
				StatusScript.hp += score;
				if (StatusScript.hp > StatusScript.max_hp) {
					StatusScript.hp = StatusScript.max_hp;
				}
				send_action ("体力が" + score + "回復した！", "heal", 40);
				if (recharge) {
					recharge = false;
					send_action ("魔の波動が切れた", "recharge_end", 40);
				}
			} else {
				send_action ("吸魂した！", "magic", 40);
				send_action ("しかし、魔力が足りなかった...", "", 40);
			}
			break;
		case "チャージ":

			if (StatusScript.mp >= 15) {
				StatusScript.mp -= 15;
				send_action ("チャージした！", "charge", 40);
				if (charge) {
					send_action ("しかし、何も起こらなかった", "", 40);
				}
				charge = true;
			} else {
				send_action ("チャージした！", "magic", 40);
				send_action ("しかし、魔力が足りなかった...", "", 40);
			}
			break;
		case "魔の波動":

			if (StatusScript.mp >= 18) {
				StatusScript.mp -= 18;
				send_action ("魔の波動をためた！", "recharge", 60);
				if (recharge) {
					send_action ("しかし、何も起こらなかった", "", 40);
				}
				recharge = true;
			} else {
				send_action ("魔の波動をためた！", "magic", 40);
				send_action ("しかし、魔力が足りなかった...", "", 40);
			}
			break;
		case "バリア":
			send_action ("バリアを唱えた！", "magic", 40);
			if (StatusScript.mp >= 10) {
				StatusScript.mp -= 10;
				if (bal == 0) {
					bal = 3;
					send_action ("バリアが展開された！", "bal", 40);
				} else {
					send_action ("しかし、何も起こらなかった...", "magic", 40);
				}
			} else {
				send_action ("しかし、魔力が足りなかった...", "", 40);
			}
			break;
		case "ブラックバリア":
			
			if (StatusScript.mp >= 11) {
				send_action ("ブラックバリアを唱えた！", "remagic", 60);
				StatusScript.mp -= 11;
				if (pre_player) {
					send_action ("ブラックバリアが展開された！", "blackBal", 40);
					blackBal = true;
				} else {
					send_action ("しかし、何も起こらなかった...", "magic", 40);
				}
			} else {
				send_action ("ブラックバリアを唱えた！", "magic", 40);
				send_action ("しかし、魔力が足りなかった...", "", 40);
			}
			break;
		
		}
		die_check ();
	}

	// 敵の攻撃
	void enemy_pattern ()
	{
		
		int rand = Random.Range (0, 100);
		int rand_i = 0;
		if (rand < 30) {//30
			rand_i = 0;
		} else if (rand < 55) {//25
			rand_i = 1;
		} else if (rand < 75) {//20
			rand_i = 2;
		} else if (rand < 80) {//15
			rand_i = 3;
		} else if (rand < 90) {//10
			rand_i = 4;
		}

		if (enemy_name == "ドラゴンキング" && enemy_hp <= 400 && dragon_charge != 3) {
			if (dragon_charge == 0) {
				send_action ("ドラゴンキングは力をため出した！", "", 40);
				dragon_charge = 1;
			} else if (dragon_charge == 1) {
				send_action ("ドラゴンキングは力をためている！", "", 40);
				dragon_charge = 2;
			} else if (dragon_charge == 2) {
				dragon_charge = 3;
				send_action ("ドラゴンキングは力を解放した！", "", 40);
				send_action ("ドラゴンキングの\nドラゴンズメテオストライク！！！", "ssm", 80);
				score = damage (enemy_attack * 4, enemy_lack, StatusScript.defence, 0);
				if (bal > 0) {
					score = Mathf.FloorToInt (score / 2);
				}
				if (score == 0) {
					send_action ("攻撃をかわした！", "e_miss", 40);
				} else {
					StatusScript.hp -= score;
					if (StatusScript.hp <= 0) {
						StatusScript.hp = 0;
					}
					send_action (score + "のダメージを受けた！", "p_damage", 40);
				}
			}
		} else {

			switch (enemy_skil [rand_i]) {
			case "攻撃":
				send_action (enemy_name + "の攻撃！", "attack", 40);

				score = damage (Mathf.FloorToInt (enemy_attack * 0.9f), enemy_lack, StatusScript.defence, StatusScript.lack);

				if (enemy_charge) {
					score *= 3;
				}
				if (bal > 0) {
					score = Mathf.FloorToInt (score / 2);
				}
				if (score == 0) {
					send_action ("攻撃をかわした！", "e_miss", 40);
				} else {
					StatusScript.hp -= score;
					if (StatusScript.hp <= 0) {
						StatusScript.hp = 0;
					}
					send_action (score + "のダメージを受けた！", "p_damage", 40);
				}
				if (enemy_charge) {
					enemy_charge = false;
					send_action (enemy_name + "のチャージの効果が切れた", "e_charge_end", 40);
				}
				break;
			case "ファイア":
				send_action (enemy_name + "はファイアを唱えた！", "e_magic", 40);
				if (blackBal) {
				
					send_action ("ブラックバリアで跳ね返った！", "", 40);
					score = Random.Range (20, 36) - enemy_magic_gurd;
					enemy_hp -= score;
					send_action (enemy_name + "に" + score + "のダメージ！", "fire", 80);
				} else {
					score = Random.Range (20, 36);
					if (bal > 0) {
						score = Mathf.FloorToInt (score / 2);
					}
					StatusScript.hp -= score;
					if (StatusScript.hp <= 0) {
						StatusScript.hp = 0;
					}
					send_action (score + "のダメージを受けた！", "e_fire", 60);
				}
				break;
			case "呪い":
				send_action (enemy_name + "は呪いをかけた！", "e_magic", 40);
				if (blackBal) {
					send_action ("ブラックバリアで跳ね返った！", "", 40);
					send_action ("呪いによって" + enemy_name + "の残りの体力が半分になった...", "", 40);
					enemy_hp = Mathf.FloorToInt (enemy_hp / 2);
				} else {

					if (Random.Range (-100, StatusScript.lack) < 0) {
						StatusScript.hp = Mathf.FloorToInt (StatusScript.hp / 2);
						if (StatusScript.hp <= 0) {
							StatusScript.hp = 0;
						}
						send_action ("呪いによって残りの体力が半分になった...", "", 40);
					} else {
						send_action ("しかし、何も起こらなかった", "", 40);
					}
				}
				break;
			case "怨念":
				send_action (enemy_name + "は怨念をかけた！", "e_magic", 40);
				if (blackBal) {
					send_action ("ブラックバリアで跳ね返った！", "", 40);
					send_action ("しかし、何も起こらなかった...", "", 40);
				} else {
					if (Random.Range (-100, StatusScript.lack) < 0) {
						StatusScript.mp = Mathf.FloorToInt (StatusScript.mp / 2);
						if (StatusScript.mp <= 0) {
							StatusScript.mp = 0;
						}
						send_action ("怨念によって残りの魔力が半分になった...", "", 40);
					} else {
						send_action ("しかし、何も起こらなかった", "", 40);
					}
				}
				break;
			case "吸血":
				send_action (enemy_name + "は吸血した！", "attack", 40);
				score = damage (Mathf.FloorToInt (enemy_attack * 0.9f), enemy_lack, StatusScript.defence, StatusScript.lack);
				if (bal > 0) {
					score = Mathf.FloorToInt (score / 2);
				}
				StatusScript.hp -= score;
				if (StatusScript.hp <= 0) {
					StatusScript.hp = 0;
				}
				enemy_hp += score;
				send_action (score + "のダメージを受けた！", "p_damage", 40);
				send_action (enemy_name + "の体力が" + score + "回復した！", "e_heal", 40);
				break;
			case "逃走":
				send_action (enemy_name + "は逃げ出した！", "", 40);
				if (run_compare (enemy_speed, enemy_level, StatusScript.speed, StatusScript.level)) {
					enemy_die = true;
				} else {
					send_action ("しかし、" + enemy_name + "は逃げ切れなかった！", "", 40);
				}
				break;
			case "チャージ":
				send_action (enemy_name + "はチャージした！", "e_charge", 40);
				if (enemy_charge) {
					send_action ("しかし、何も起こらなかった", "", 40);
				}
				enemy_charge = true;
				break;
			case "キック":
				send_action (enemy_name + "はキックで攻撃した！", "attack3", 40);

				score = damage (Mathf.FloorToInt (enemy_attack * 1.3f), Mathf.FloorToInt (enemy_lack / 2), StatusScript.defence, StatusScript.lack);

				if (enemy_charge) {
					score *= 3;
				}

				if (score == 0) {
					send_action ("攻撃をかわした！", "e_miss", 40);
				} else {

					if (bal > 0) {
						score = Mathf.FloorToInt (score / 2);
					}

					StatusScript.hp -= score;
					if (StatusScript.hp <= 0) {
						StatusScript.hp = 0;
					}

					send_action (score + "のダメージを受けた！", "p_damage", 40);
				}
				if (enemy_charge) {
					enemy_charge = false;
					send_action (enemy_name + "のチャージの効果が切れた", "e_charge_end", 40);
				}
				break;
			case "吠える":
				send_action (enemy_name + "は恐ろしい声で絶叫した！", "scream", 60);
				score = Random.Range (30, 51);
				StatusScript.mp -= score;
				if (StatusScript.mp <= 0) {
					StatusScript.mp = 0;
				}
				send_action ("魔力を" + score + "失った...", "", 40);
				break;
			case "角":
				send_action (enemy_name + "は角で攻撃した！", "attack2", 60);
				score = damage (Mathf.FloorToInt (enemy_attack * 1.3f), enemy_lack, StatusScript.defence, StatusScript.lack);
				if (score == 0) {
					send_action ("攻撃をかわした！", "e_miss", 40);
				} else {
					if (bal > 0) {
						score = Mathf.FloorToInt (score / 2);
					}
					StatusScript.hp -= score;
					if (StatusScript.hp <= 0) {
						StatusScript.hp = 0;
					}
			
					send_action (score + "のダメージを受けた！", "p_damage", 40);
				}
				break;
			case "炎":
				send_action (enemy_name + "は燃え盛る炎を吐いた！", "attack3", 60);
				score = Random.Range (60, 81);
				if (bal > 0) {
					score = Mathf.FloorToInt (score / 2);
				}
				StatusScript.hp -= score;
				if (StatusScript.hp <= 0) {
					StatusScript.hp = 0;
				}
				send_action (score + "のダメージを受けた！", "e_fire2", 60);
				break;
			case "ドラゴンブレス":
				send_action (enemy_name + "のドラゴンブレス！", "attack3", 60);
				score = Random.Range (80, 101);
				if (bal > 0) {
					score = Mathf.FloorToInt (score / 2);
				}
				StatusScript.hp -= score;
				if (StatusScript.hp <= 0) {
					StatusScript.hp = 0;
				}
				send_action (score + "のダメージを受けた！", "e_fire2", 60);
				break;
			case "威圧":
				send_action (enemy_name + "は龍の威圧を放った！", "dragon", 60);
				bool breaking = false;
				if (bal > 0) {
					bal = 0;
					send_action ("バリアの効果が切れた", "bal_break", 40);
					breaking = true;
				}
				if (charge) {
					charge = false;
					send_action ("チャージの効果が切れた", "charge_end", 40);
					breaking = true;
				}
				if (recharge) {
					recharge = false;
					send_action ("魔の波動の効果が切れた", "recharge_end", 40);
					breaking = true;
				}
				if (blackBal) {
					blackBal = false;
					send_action ("ブラックバリアの効果が切れた", "balBlack_break", 40);
					breaking = true;
				}
				if (!breaking) {
					send_action ("しかし、何も起こらなかった...", "", 40);
				}
				break;
			}
		}
		die_check ();
	}

	//体力の有無のチェックを行う関数
	void die_check ()
	{
		if (StatusScript.hp <= 0) {
			send_action ("体力が無くなってしまった...", "gameover", 160);
			player_die = true;
		} else if (enemy_hp <= 0) {
			send_action (enemy_name + "を倒した！", "die", 60);
			send_action (enemy_exp + "の経験値を得た！", "", 40);
			get_exp (StatusScript.level);
			if (StatusScript.item.Length < 10 && Random.Range (-300, 1000 + StatusScript.lack) > 600) {
				send_action (enemy_name + "は" + enemy_drop + "を落とした！", "fanfare", 40);
				StatusScript.item = array_push (StatusScript.item, enemy_drop);
			}
			enemy_die = true;
		}
	}

	void get_exp (int tempLevel)
	{
		bool level_up = false;
		StatusScript.exp += enemy_exp;
		// 次のレベルになるためのexp lv.1->40 lv.2->120 lv.3->180
		while ((StatusScript.level + 1) * 30 * StatusScript.level <= StatusScript.exp) {
			level_up = true;
			StatusScript.level++;
			send_action ("レベル" + StatusScript.level + "に上がった！", "fanfare", 60);

			int h_rand = Random.Range (6, 10);
			int m_rand = Random.Range (3, 5);
			int a_rand = Random.Range (3, 5);
			int d_rand = Random.Range (2, 4);
			int s_rand = Random.Range (2, 4);
			int l_rand = Random.Range (2, 4);

			StatusScript.max_hp += h_rand;
			StatusScript.max_mp += m_rand;
			StatusScript.attack += a_rand;
			StatusScript.defence += d_rand;
			StatusScript.speed += s_rand;
			StatusScript.lack += l_rand;
			StatusScript.hp = StatusScript.max_hp;
			StatusScript.mp = StatusScript.max_mp;


			send_action ("体力+" + h_rand + " 魔力+" + m_rand + " 攻撃+" + a_rand + " 防御+" + d_rand + " 敏捷+" + s_rand + " 運+" + l_rand, "", 60);
			send_action ("ステータスポイントを3獲得した！", "", 40);


			switch (StatusScript.level) {
			case 3:
				StatusScript.magic = array_push (StatusScript.magic, "ファイア");
				send_action ("ファイアの魔法を習得した！", "fanfare", 40);
				break;
			case 5:
				StatusScript.skil = array_push (StatusScript.skil, "キック");
				send_action ("キックの攻撃技を習得した！", "fanfare", 40);
				break;
			case 7:
				StatusScript.magic = array_push (StatusScript.magic, "ヒール");
				send_action ("ヒールの魔法を習得した！", "fanfare", 40);
				break;
			case 12:
				StatusScript.magic = array_push (StatusScript.magic, "チャージ");
				send_action ("チャージの魔法を習得した！", "fanfare", 40);
				break;
			case 15:
				StatusScript.skil = array_push (StatusScript.skil, "スライディング");
				send_action ("スライディングの攻撃技を習得した！", "fanfare", 40);
				break;
			case 19:
				StatusScript.magic = array_push (StatusScript.magic, "バリア");
				send_action ("バリアの魔法を習得した！", "fanfare", 40);
				break;
			}

		}
		if (level_up) {
			send_action ("", "", (tempLevel - StatusScript.level) * 3);
		}
	}

	// ダメージ計算(攻撃側攻撃力, 攻撃側運, 防御側防御力, 防御側運）
	int damage (int atk, int lac, int def, int d_lac)
	{
		bool miss = false;	
		bool critical = false;
		int damage;

		// 回避判定
		if (Random.Range (0, 100 + d_lac - lac) >= 95) {
			miss = true;
		}

		// 防御時
		if (command_d) {
			def = def * 3;
			command_d = false;
		} else {
			// クリティカル判定
			if (Random.Range (0, 1000) < lac) {
				critical = true;
			}
		}

		//ダメ計
		damage = Mathf.FloorToInt (atk * Random.Range (0.9f, 1.0f) * 0.9f - def * 0.85f);

		if (damage <= 0) {
			damage = 1;
		}

		if (critical) {
			damage *= 2; 
			send_action ("クリティカルヒット！", "", 20);
		}

		if (miss && !critical) {
			damage = 0;
		}


		return damage;

	}


	// プレイヤーの方が早いならtrueを返す
	bool speed_compare ()
	{

		int p_s = StatusScript.speed + Mathf.FloorToInt (Random.Range (0, StatusScript.lack) / 10);
		int e_s = enemy_speed + Mathf.FloorToInt (Random.Range (0, enemy_lack) / 10);

		if (p_s < e_s) {
			return false;
		} else if (p_s > e_s) {
			return true;
		} else {
			if (Random.Range (0, 2) == 0) {
				return true;
			} else {
				return false;
			}
		}
	}

	// 逃げられるかどうかの判定
	bool run_compare (int s, int l, int s1, int l1)
	{
		int p_s = s;
		int e_s = s1;
		int level_difference = l - l1;



		if (level_difference >= 0) {
			if (Random.Range (0, p_s * level_difference) >= Random.Range (0, e_s)) {
				return true;
			} else {
				return false;
			}
		} else {
			int speed_difference = p_s - e_s;
			if (speed_difference >= 0) {
				if (Random.Range (speed_difference * level_difference, p_s) >= Random.Range (0, e_s)) {
					return true;
				} else {
					return false;
				}
			} else {
				if (Random.Range (-1 * speed_difference * level_difference, p_s) >= Random.Range (0, e_s)) {
					return true;
				} else {
					return false;
				}
			}

		}
	}


	// 敵キャラステータス挿入関数
	void enemy_status ()
	{
		switch (enemy_name) {
		case "スライム":
			enemy_hp = 15;
			enemy_attack = 17;
			enemy_defence = 5;
			enemy_speed = 3;
			enemy_lack = 2;
			enemy_exp = 60;
			enemy_drop = "やくそう";
			enemy_skil [0] = "攻撃";
			enemy_skil [1] = "攻撃";
			enemy_skil [2] = "攻撃";
			enemy_skil [3] = "攻撃";
			enemy_skil [4] = "攻撃";
			enemy_magic_gurd = 0;
			break;
		case "野ウサギ":
			enemy_hp = 16;
			enemy_attack = 19;
			enemy_defence = 8;
			enemy_speed = 11;
			enemy_lack = 14;
			enemy_exp = 63;
			enemy_drop = "ウサミミ";
			enemy_skil [0] = "攻撃";
			enemy_skil [1] = "攻撃";
			enemy_skil [2] = "攻撃";
			enemy_skil [3] = "攻撃";
			enemy_skil [4] = "逃走";
			enemy_magic_gurd = -25;
			break;
		
		case "きりかぶおばけ":
			enemy_hp = 19;
			enemy_attack = 14;
			enemy_defence = 6;
			enemy_speed = 19;
			enemy_lack = 30;
			enemy_exp = 68;
			enemy_drop = "せいすい";
			enemy_skil [0] = "攻撃";
			enemy_skil [1] = "呪い";
			enemy_skil [2] = "怨念";
			enemy_skil [3] = "呪い";
			enemy_skil [4] = "怨念";
			enemy_magic_gurd = -10;
			break;
		case "ゴールドラビット":
			enemy_hp = 28;
			enemy_attack = 26;
			enemy_defence = 11;
			enemy_speed = 85;
			enemy_lack = 80;
			enemy_exp = 200;
			enemy_drop = "ウサミミ";
			enemy_skil [0] = "攻撃";
			enemy_skil [1] = "逃走";
			enemy_skil [2] = "逃走";
			enemy_skil [3] = "逃走";
			enemy_skil [4] = "逃走";
			enemy_magic_gurd = -5;
			break;
		case "コウモリ":
			enemy_hp = 20;
			enemy_attack = 23;
			enemy_defence = 10;
			enemy_speed = 18;
			enemy_lack = 10;
			enemy_exp = 68;
			enemy_drop = "せいすい";
			enemy_skil [0] = "攻撃";
			enemy_skil [1] = "攻撃";
			enemy_skil [2] = "吸血";
			enemy_skil [3] = "吸血";
			enemy_skil [4] = "吸血";
			enemy_magic_gurd = -10;
			break;
		case "ヴァンパイア":
			enemy_hp = 25;
			enemy_attack = 26;
			enemy_defence = 16;
			enemy_speed = 27;
			enemy_lack = 14;
			enemy_exp = 83;
			enemy_drop = "せいすい";
			enemy_skil [0] = "吸血";
			enemy_skil [1] = "吸血";
			enemy_skil [2] = "吸血";
			enemy_skil [3] = "怨念";
			enemy_skil [4] = "呪い";
			enemy_magic_gurd = -5;
			break;
		case "チビすけ":
			enemy_hp = 24;
			enemy_attack = 23;
			enemy_defence = 14;
			enemy_speed = 9;
			enemy_lack = 11;
			enemy_exp = 66;
			enemy_drop = "せいすい";
			enemy_skil [0] = "攻撃";
			enemy_skil [1] = "攻撃";
			enemy_skil [2] = "攻撃";
			enemy_skil [3] = "呪い";
			enemy_skil [4] = "呪い";
			enemy_magic_gurd = -10;
			break;
		case "ファイアスライム":
			enemy_hp = 25;
			enemy_attack = 18;
			enemy_defence = 7;
			enemy_speed = 10;
			enemy_lack = 9;
			enemy_exp = 82;
			enemy_drop = "上やくそう";
			enemy_skil [0] = "ファイア";
			enemy_skil [1] = "攻撃";
			enemy_skil [2] = "ファイア";
			enemy_skil [3] = "ファイア";
			enemy_skil [4] = "ファイア";
			enemy_magic_gurd = 30;
			break;
		case "スーパースライム":
			enemy_hp = 42;
			enemy_attack = 21;
			enemy_defence = 7;
			enemy_speed = 12;
			enemy_lack = 13;
			enemy_exp = 78;
			enemy_drop = "上やくそう";
			enemy_skil [0] = "攻撃";
			enemy_skil [1] = "攻撃";
			enemy_skil [2] = "攻撃";
			enemy_skil [3] = "攻撃";
			enemy_skil [4] = "攻撃";
			enemy_magic_gurd = 0;
			break;
		case "骸骨兵":
			enemy_hp = 48;
			enemy_attack = 30;
			enemy_defence = 40;
			enemy_speed = 8;
			enemy_lack = 8;
			enemy_exp = 95;
			enemy_drop = "魔法薬";
			enemy_skil [0] = "攻撃";
			enemy_skil [1] = "攻撃";
			enemy_skil [2] = "攻撃";
			enemy_skil [3] = "怨念";
			enemy_skil [4] = "呪い";
			enemy_magic_gurd = -30;
			break;
		case "ミントラビット":
			enemy_hp = 43;
			enemy_attack = 28;
			enemy_defence = 14;
			enemy_speed = 24;
			enemy_lack = 18;
			enemy_exp = 89;
			enemy_drop = "ウサミミ";
			enemy_skil [0] = "攻撃";
			enemy_skil [1] = "攻撃";
			enemy_skil [2] = "攻撃";
			enemy_skil [3] = "攻撃";
			enemy_skil [4] = "逃走";
			enemy_magic_gurd = -5;
			break;
		case "ファイター":
			enemy_hp = 51;
			enemy_attack = 36;
			enemy_defence = 21;
			enemy_speed = 12;
			enemy_lack = 15;
			enemy_exp = 133;
			enemy_drop = "回復薬";
			enemy_skil [0] = "攻撃";
			enemy_skil [1] = "チャージ";
			enemy_skil [2] = "キック";
			enemy_skil [3] = "キック";
			enemy_skil [4] = "チャージ";
			enemy_magic_gurd = 10;
			break;
		case "ベビードラゴン":
			enemy_hp = 28;
			enemy_attack = 25;
			enemy_defence = 17;
			enemy_speed = 12;
			enemy_lack = 26;
			enemy_exp = 98;
			enemy_drop = "上やくそう";
			enemy_skil [0] = "攻撃";
			enemy_skil [1] = "攻撃";
			enemy_skil [2] = "角";
			enemy_skil [3] = "角";
			enemy_skil [4] = "吠える";
			enemy_magic_gurd = 10;
			break;
		case "森のドラゴン":
			enemy_hp = 56;
			enemy_attack = 33;
			enemy_defence = 20;
			enemy_speed = 11;
			enemy_lack = 18;
			enemy_exp = 141;
			enemy_drop = "回復薬";
			enemy_skil [0] = "攻撃";
			enemy_skil [1] = "角";
			enemy_skil [2] = "角";
			enemy_skil [3] = "吠える";
			enemy_skil [4] = "吠える";
			enemy_magic_gurd = 15;
			break;
		case "フレイムドラゴン":
			enemy_hp = 56;
			enemy_attack = 30;
			enemy_defence = 25;
			enemy_speed = 15;
			enemy_lack = 15;
			enemy_exp = 163;
			enemy_drop = "回復薬";
			enemy_skil [0] = "攻撃";
			enemy_skil [1] = "炎";
			enemy_skil [2] = "炎";
			enemy_skil [3] = "角";
			enemy_skil [4] = "吠える";
			enemy_magic_gurd = 40;
			break;
		case "ドラゴンキング":
			enemy_hp = 150;
			enemy_attack = 30;
			enemy_defence = 28;
			enemy_speed = 17;
			enemy_lack = 32;
			enemy_exp = 1000;
			enemy_drop = "回復薬";
			enemy_skil [0] = "角";
			enemy_skil [1] = "角";
			enemy_skil [2] = "威圧";
			enemy_skil [3] = "ドラゴンブレス";
			enemy_skil [4] = "吠える";
			enemy_magic_gurd = 15;
			break;
		}
		// レベルによって能力を上げる 
		enemy_hp += Mathf.FloorToInt (enemy_level * enemy_hp / 5);
		enemy_attack += Mathf.FloorToInt (enemy_level * enemy_attack / 8);
		enemy_defence += Mathf.FloorToInt (enemy_level * enemy_defence / 8);
		enemy_speed += Mathf.FloorToInt (enemy_level * enemy_speed / 8);
		enemy_lack += Mathf.FloorToInt (enemy_level * enemy_lack / 8);
		enemy_exp = Mathf.FloorToInt (enemy_exp * (enemy_level + 4) / 5);
	}

	// アクションが起こるたびに配列の末尾に一つずつ入れる関数
	void send_action (string solo_message, string solo_action_name, int solo_wait_time)
	{
		if (solo_action_name == "p_damage" && StatusScript.max_hp / 3 <= score) {
			solo_action_name = "p_damage2";
		}
		return_message = array_push (return_message, solo_message);
		wait_time = int_array_push (wait_time, solo_wait_time);
		hp_change = int_array_push (hp_change, StatusScript.hp);
		mp_change = int_array_push (mp_change, StatusScript.mp);
		max_hp_change = int_array_push (max_hp_change, StatusScript.max_hp);
		max_mp_change = int_array_push (max_mp_change, StatusScript.max_mp);
		level_change = int_array_push (level_change, StatusScript.level);
		action_name = array_push (action_name, solo_action_name);
		up_down = array_push (up_down, string.Format ("{0}", score));
	}


	// string型の配列の末尾に要素を追加する関数
	string[] array_push (string[] array, string value)
	{
		array.CopyTo (array = new string[array.Length + 1], 0);
		array [array.Length - 1] = value;
		return array;
	}

	// int型の配列の末尾に要素を追加する関数
	int[] int_array_push (int[] array, int value)
	{
		array.CopyTo (array = new int[array.Length + 1], 0);
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
