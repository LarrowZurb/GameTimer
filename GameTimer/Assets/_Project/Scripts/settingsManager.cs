using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameTimer {

	public class SettingsManager : MonoBehaviour {

		#region Serialized Fields
		[SerializeField] TMP_Text minutes_text;
		[SerializeField] TMP_Text seconds_text;
		[SerializeField] Toggle toggle_countDown;
		[SerializeField] Toggle toggle_roundEnd;
		[SerializeField] Slider slider_countDown;
		[SerializeField] Slider slider_roundEnd;
		#endregion

		#region Private Fields
		private int minutes;
		private int seconds;
		#endregion

		#region Unity Methods
		private void OnEnable() {
			float gameTime = GameManager.instance.CurrentGame.DefaultTurnTime;
			minutes = (int)(gameTime / 60);
			seconds = (int)(gameTime - (minutes * 60));
			SetCurrentTimeAsText();

			toggle_countDown.isOn = SoundManager.instance.CountdownEnabled;
			slider_countDown.value = SoundManager.instance.CountdownVolume;
			toggle_roundEnd.isOn = SoundManager.instance.RoundEndEnabled;
			slider_roundEnd.value = SoundManager.instance.RoundEndVolume;
		}


		private void OnDisable() {
			GameManager.instance.CurrentGame.DefaultTurnTime = minutes * 60 + seconds;
			GameManager.instance.SetCurrentTimeAsText( minutes * 60 + seconds );

			SoundManager.instance.CountdownEnabled = toggle_countDown.isOn;
			SoundManager.instance.CountdownVolume = slider_countDown.value;
			SoundManager.instance.RoundEndEnabled = toggle_roundEnd.isOn;
			SoundManager.instance.RoundEndVolume = slider_roundEnd.value;

			GameManager.instance.SavePlayerPrefs();
		}
		#endregion


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