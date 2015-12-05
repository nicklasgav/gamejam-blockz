using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class startgame : MonoBehaviour {

	// Use this for initialization
	[SerializeField] private Button MyButton = null; // assign in the editor
	
	void Start() { 
		MyButton.onClick.AddListener(() => { 
			Application.LoadLevel("Level 1");
		});
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
