using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameTimer {

	public class SettingsManager : MonoBehaviour {


		[SerializeField] TMP_Text minutes_text;
		[SerializeField] TMP_Text seconds_text;
		[SerializeField] Toggle toggle_countDown;
		[SerializeField] Toggle toggle_roundEnd;
		[SerializeField] Slider slider_countDown;
		[SerializeField] Slider slider_roundEnd;


		private int minutes;
		private int seconds;


		private void OnEnable() {
			LoadPlayerPrefs();

			float gameTime = GameManager.instance.CurrentGame.DefaultGameTime;
			minutes = (int)(gameTime / 60);
			seconds = (int)(gameTime - (minutes * 60));

			SetCurrentTimeAsText();
		}


		private void OnDisable() {
			SavePlayerPrefs();
			
		}


		public void SetCurrentTimeAsText() {
			minutes_text.text = $"{minutes:D2}";
			seconds_text.text = $"{seconds:D2}";
		}


		private void LoadPlayerPrefs() {
			GameManager.instance.SetGameType( PlayerPrefs.GetString( "gameType" ) );

			GameManager.instance.CurrentGame.DefaultGameTime = PlayerPrefs.GetFloat( "gameTime", GameManager.instance.CurrentGame.DefaultGameTime );
			GameManager.instance.CurrentGame.DefaultTurnTime = PlayerPrefs.GetFloat( "turnTime", GameManager.instance.CurrentGame.DefaultTurnTime );

			SoundManager.instance.CountdownEnabled = PlayerPrefs.GetString( "countDown_enabled", "true" ).Equals( "true" );
			SoundManager.instance.CountdownVolume = PlayerPrefs.GetFloat( "countDown_volume", 1f );
			SoundManager.instance.RoundEndEnabled = PlayerPrefs.GetString( "roundEnd_enabled", "true" ).Equals( "true" );
			SoundManager.instance.RoundEndVolume = PlayerPrefs.GetFloat( "roundEnd_volume", 1f );
		}

		public void SavePlayerPrefs() {
			PlayerPrefs.SetString( "gameType", GameManager.instance.CurrentGameType.GameTypeName );

			PlayerPrefs.SetFloat( "gameTime", GameManager.instance.CurrentGame.DefaultGameTime );
			PlayerPrefs.SetFloat( "turnTime", GameManager.instance.CurrentGame.DefaultTurnTime );

			PlayerPrefs.SetString( "countDown_enabled", SoundManager.instance.CountdownEnabled.ToString().ToLower() );
			PlayerPrefs.SetFloat( "countDown_volume", SoundManager.instance.CountdownVolume );
			PlayerPrefs.SetString( "roundEnd_enabled", SoundManager.instance.RoundEndEnabled.ToString().ToLower() );
			PlayerPrefs.SetFloat( "roundEnd_volume", SoundManager.instance.RoundEndVolume );
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