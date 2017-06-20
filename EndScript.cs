using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScript : MonoBehaviour
{

	private RectTransform pos;
	private int fps;
	Text myText;

	// Use this for initialization
	void Start ()
	{
		int second = Mathf.FloorToInt (StatusScript.time) % 60;
		int minute = (Mathf.FloorToInt (StatusScript.time) / 60) % 60;
		int hour = Mathf.FloorToInt (Mathf.FloorToInt (StatusScript.time) / 3600);
		string playTime = time_indent (second, minute, hour);

		pos = this.GetComponent<RectTransform> ();
		myText = this.GetComponentInChildren <Text> ();
		myText.text += "\nクリアレベル " + StatusScript.level + "\nクリアタイム" + playTime + "\nゲームオーバー " + StatusScript.gameOver + "回";

	}
	
	// Update is called once per frame
	void Update ()
	{
		Debug.Log (transform.localPosition.y);
		if (transform.localPosition.y < 1580) {
			transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y + 2.5f, transform.localPosition.z);
		} else {
			fps++;
			if (fps == 180) {
				myText.text += "\nThank you for playing!!\n<size=25>Press any key to return</size>";
			}
			if (fps >= 180 && Input.anyKeyDown) {
				SceneManager.LoadScene ("Main");
			}
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
