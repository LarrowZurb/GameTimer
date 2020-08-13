using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Diagnostics;

namespace GameTimer {

	public class GameManager : MonoBehaviour {
		public static GameManager instance;

#pragma warning disable 649
		[Header( "UI Components" )]
		[SerializeField] private TMP_Text clock;
		[SerializeField] private GameObject settingPanel;
		[SerializeField] private Button pauseButton;
		[SerializeField] private Image pauseButtonImage;
		[SerializeField] private Button resetButton;
		[SerializeField] private Button settingsButton;

		[Header( "Button Images" )]
		[SerializeField] private Sprite pauseSprite;
		[SerializeField] private Sprite playSprite;


		[Header( "Player Buttons" )]
		[SerializeField] private Button playerLeftButton;
		[SerializeField] private TMP_Text playerLeftText;
		[SerializeField] private Image playerLeftImage;

		[SerializeField] private Button playerRightButton;
		[SerializeField] private TMP_Text playerRightText;
		[SerializeField] private Image playerRightImage;

		[Header( "Sounds" )]
		[SerializeField] private AudioClip countdown_clip;
		[SerializeField] private AudioClip roundEnd_clip;
		[SerializeField] private AudioSource audioSource;
#pragma warning restore 649

		private Button currentPlayerTurn;
		private float defaultTime = 30;
		private float currentTime;
		private float roundTime;
		private Stopwatch timer = new Stopwatch();
		private bool isPaused = false;

		private bool countdownPlayed = false;
		private bool playCountdown = true;
		private float countdownVolume = 1f;

		private bool endingPlayed = false;
		private bool playRoundEnd = true;
		private float roundEndVolume = 1f;


		public void Awake() {
			if ( instance == null ) {
				instance = this;
			} else {
				Destroy( this );
			}
		}


		// Start is called before the first frame update
		private void Start() {
			roundTime = PlayerPrefs.GetFloat( "roundTime", defaultTime );

			playCountdown = PlayerPrefs.GetString( "countDown_isOn", "true" ).ToLower().Equals( "true" );
			countdownVolume = PlayerPrefs.GetFloat( "countDown_volume", 1f );

			playRoundEnd = PlayerPrefs.GetString( "roundEnd_isOn", "true" ).ToLower().Equals( "true" );
			roundEndVolume = PlayerPrefs.GetFloat( "roundEnd_volume", 1f );

			Reset();
		}

		// Update is called once per frame
		private void Update() {
			if ( timer.IsRunning ) {
				currentTime = roundTime - (float)timer.Elapsed.TotalSeconds;
				if ( currentTime <= 10 ) {
					PlayCountdown();
				}
				if ( currentTime <= 2 ) {
					PlayRoundEnd();
				}
				if ( currentTime <= 0 ) {
					onPlayerButton( currentPlayerTurn );
				}
				clock.text = GetCurrentTimeAsText();
			}
		}

		private void PlayCountdown() {
			if ( !playCountdown || countdownPlayed )
				return;

			if ( audioSource.isPlaying ) {
				audioSource.Stop();
			}

			audioSource.clip = countdown_clip;
			audioSource.volume = countdownVolume;
			audioSource.Play();

			countdownPlayed = true;
		}

		private void PlayRoundEnd() {
			if ( !playRoundEnd || endingPlayed )
				return;

			if ( audioSource.isPlaying ) {
				audioSource.Stop();
			}

			audioSource.clip = roundEnd_clip;
			audioSource.volume = roundEndVolume;
			audioSource.Play();

			endingPlayed = true;
		}

		public float GetRoundTime() {
			return roundTime;
		}

		public void SetRoundTime( float _time ) {
			roundTime = _time;
			Reset();
		}

		public void onPlayerButton( Button _playerButton ) {

			if ( timer.IsRunning ) {
				currentPlayerTurn.interactable = false;
				currentPlayerTurn.image.color = new Color( 1, 0, 0, 0.5f );
				//currentPlayerTurn.GetComponentInChildren<TMP_Text>().text = "Waiting";

				if ( currentPlayerTurn == playerLeftButton ) {
					playerLeftText.text = "Waiting";
					playerRightText.text = "End\nTurn";
					currentPlayerTurn = playerRightButton;
				} else {
					playerRightText.text = "Waiting";
					playerLeftText.text = "End\nTurn";
					currentPlayerTurn = playerLeftButton;
				}

				currentPlayerTurn.interactable = true;
				currentPlayerTurn.image.color = Color.green;
				//currentPlayerTurn.GetComponentInChildren<TMP_Text>().text = "End\nTurn";
				clock.gameObject.transform.Rotate( 0, 0, 180f );

				timer.Restart();
			} else {

				if ( _playerButton == playerLeftButton ) {
					playerRightButton.interactable = false;
					playerRightButton.image.color = new Color( 1, 0, 0, 0.5f );
					playerRightText.text = "Waiting";
					playerLeftText.text = "End\nTurn";
					clock.gameObject.transform.eulerAngles = new Vector3( 0, 0, -90 );
				} else {
					playerLeftButton.interactable = false;
					playerLeftButton.image.color = new Color( 1, 0, 0, 0.5f );
					playerLeftText.text = "Waiting";
					playerRightText.text = "End\nTurn";
					clock.gameObject.transform.eulerAngles = new Vector3( 0, 0, 90 );
				}
				currentPlayerTurn = _playerButton;
				currentPlayerTurn.interactable = true;
				currentPlayerTurn.image.color = Color.green;
				//currentPlayerTurn.GetComponentInChildren<TMP_Text>().text = "End\nTurn";

				timer.Reset();
				timer.Start();

				settingsButton.interactable = false;
				pauseButton.interactable = true;
			}

			countdownPlayed = false;
			endingPlayed = false;
			audioSource.Stop();

		}

		public void OnPauseButton() {
			if ( !isPaused ) {
				timer.Stop();
				pauseButtonImage.sprite = playSprite;
			} else {
				timer.Start();
				pauseButtonImage.sprite = pauseSprite;
			}

			isPaused = !isPaused;
			resetButton.interactable = isPaused;
			currentPlayerTurn.interactable = !isPaused;

		}

		public void Reset() {
			timer.Stop();
			timer.Reset();

			playerLeftText.text = "Start";
			playerLeftImage.color = Color.yellow;
			playerLeftButton.interactable = true;

			playerRightText.text = "Start";
			playerRightImage.color = Color.yellow;
			playerRightButton.interactable = true;

			currentPlayerTurn = null;
			isPaused = false;
			settingsButton.interactable = true;
			resetButton.interactable = false;
			pauseButton.interactable = false;
			pauseButtonImage.sprite = pauseSprite;

			countdownPlayed = false;
			endingPlayed = false;

			currentTime = roundTime;
			clock.text = GetCurrentTimeAsText();
		}

		public string GetCurrentTimeAsText() {
			int _minutes = (int)(currentTime / 60);
			int _seconds = (int)(currentTime - (_minutes * 60));

			return $"{_minutes:D2}:{_seconds:D2}";
		}

		public void ToggleSettings() {
			if ( settingPanel.activeSelf ) {
				settingPanel.SetActive( false );
			} else {
				settingPanel.SetActive( true );
			}
		}

		public void SetCountdown( bool _isOn, float _volume ) {
			playCountdown = _isOn;
			countdownVolume = _volume;
		}

		public void SetRoundEnd( bool _isOn, float _volume ) {
			playRoundEnd = _isOn;
			roundEndVolume = _volume;
		}
	}

}