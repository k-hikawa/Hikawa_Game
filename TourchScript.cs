﻿using UnityEngine;
using System.Collections;

public class TourchScript : MonoBehaviour
{

	Transform unitychan;
	GameObject fire;

	// Use this for initialization
	void Start ()
	{
		unitychan = GameObject.Find ("unitychan").transform;
		fire = transform.FindChild ("Torchfire").gameObject;
		fire.SetActive (false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Vector3.Distance (unitychan.position, this.transform.position) < 10) {
			fire.SetActive (true);
		} else {
			fire.SetActive (false);	
		}
	}
}
