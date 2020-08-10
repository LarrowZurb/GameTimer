using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class settingsManager : MonoBehaviour
{
	[SerializeField] TMP_Text minutes_text;
	[SerializeField] TMP_Text seconds_text;

	private int minutes;
	private int seconds;

	private void OnEnable() {
		float roundTime = GameManager.instance.GetRoundTime();
		minutes = (int)(roundTime / 60);
		seconds = (int)(roundTime - (minutes * 60));

		SetCurrentTimeAsText();
	}

	private void OnDisable() {
		GameManager.instance.SetRoundTime( minutes * 60 + seconds );
	}

	public void SetCurrentTimeAsText() {
		minutes_text.text = $"{minutes:D2}";
		seconds_text.text = $"{seconds:D2}";
	}

	public void OnMinuteUp() {
		if (minutes == 60 ) {
			minutes = 0;
		}else{
			minutes += 1;
		}
		SetCurrentTimeAsText();
	}

	public void OnMinuteDown() {
		if ( minutes == 0 ) {
			minutes = 60;
		} else {
			minutes -= 1;
		}
		SetCurrentTimeAsText();
	}

	public void OnSecondsUp() {
		if ( seconds == 59 ) {
			seconds = 0;
		} else {
			seconds += 1;
		}
		SetCurrentTimeAsText();
	}

	public void OnSecondsDown() {
		if ( seconds == 0 ) {
			seconds = 59;
		} else {
			seconds -= 1;
		}
		SetCurrentTimeAsText();
	}

}
