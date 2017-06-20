using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatusTextScript : MonoBehaviour
{
	Text myText;

	// Use this for initialization
	void Start ()
	{
		myText = GetComponentInChildren<Text> ();

	}
	
	// Update is called once per frame
	void Update ()
	{
		int next = (StatusScript.level + 1) * 30 * StatusScript.level - StatusScript.exp;
		myText.text = "レベル" + StatusScript.level + "\n経験値：" + StatusScript.exp + "\n次のレベル：" + next + "\n体力：" + StatusScript.hp + "/" + StatusScript.max_hp + "\n魔力：" + StatusScript.mp + "/" + StatusScript.max_mp + "\n攻撃：" + StatusScript.attack + "\n防御：" + StatusScript.defence + "\n敏捷：" + StatusScript.speed + "\n運\u3000：" + StatusScript.lack;
	}
}
