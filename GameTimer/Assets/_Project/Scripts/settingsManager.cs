using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameTimer {

	public class settingsManager : MonoBehaviour {

#pragma warning disable 649
		[SerializeField] TMP_Text minutes_text;
		[SerializeField] TMP_Text seconds_text;
		[SerializeField] Toggle toggle_countDown;
		[SerializeField] Toggle toggle_roundEnd;
		[SerializeField] Slider slider_countDown;
		[SerializeField] Slider slider_roundEnd;
#pragma warning restore 649

		private int minutes;
		private int seconds;

		private void OnEnable() {
			float roundTime = GameManager.instance.GetRoundTime();
			minutes = (int)(roundTime / 60);
			seconds = (int)(roundTime - (minutes * 60));

			toggle_countDown.isOn = PlayerPrefs.GetString( "countDown_isOn", "true" ).ToLower().Equals( "true" );
			slider_countDown.value = PlayerPrefs.GetFloat( "countDown_volume", 1f );

			toggle_roundEnd.isOn = PlayerPrefs.GetString( "roundEnd_isOn", "true" ).ToLower().Equals( "true" );
			slider_roundEnd.value = PlayerPrefs.GetFloat( "roundEnd_volume", 1f );

			SetCurrentTimeAsText();
		}

		private void OnDisable() {
			GameManager.instance.SetRoundTime( minutes * 60 + seconds );
			GameManager.instance.SetCountdown( toggle_countDown.isOn, slider_countDown.value );
			GameManager.instance.SetRoundEnd( toggle_roundEnd.isOn,slider_roundEnd.value );

			PlayerPrefs.SetFloat( "roundTime", minutes * 60 + seconds );
			PlayerPrefs.SetString( "countDown_isOn", toggle_countDown.isOn.ToString().ToLower() );
			PlayerPrefs.SetFloat( "countDown_volume", slider_countDown.value );
			PlayerPrefs.SetString( "roundEnd_isOn", toggle_roundEnd.isOn.ToString().ToLower() );
			PlayerPrefs.SetFloat( "roundEnd_volume", slider_roundEnd.value );
		}

		public void SetCurrentTimeAsText() {
			minutes_text.text = $"{minutes:D2}";
			seconds_text.text = $"{seconds:D2}";
		}

		public void OnMinuteUp() {
			if ( minutes == 60 ) {
				minutes = 0;
			} else {
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
}