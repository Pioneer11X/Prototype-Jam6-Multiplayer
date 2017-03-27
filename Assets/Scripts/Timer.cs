using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    // Use this for initialization
    private Text timerText;
    public float myCoolTimer = 60;
	void Start () {
        timerText = GetComponent<Text>();
		
	}
	
	// Update is called once per frame
	void Update () {
        myCoolTimer -= Time.deltaTime;
        timerText.text = myCoolTimer.ToString("f0");
        print(myCoolTimer);
	}
}
