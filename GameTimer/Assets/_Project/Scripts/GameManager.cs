using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Diagnostics;

public class GameManager : MonoBehaviour {
	public static GameManager instance;

	[Header( "UI Components" )]
	[SerializeField] TMP_Text clock;
	[SerializeField] GameObject settingPanel;
	[SerializeField] Button pauseButton;
	[SerializeField] Image pauseButtonImage;
	[SerializeField] Button resetButton;
	[SerializeField] Button settingsButton;

	[Header( "Button Images" )]
	[SerializeField] Sprite pauseSprite;
	[SerializeField] Sprite playSprite;

	[Header( "Player Buttons" )]
	[SerializeField] Button playerLeftButton;
	[SerializeField] TMP_Text playerLeftText;
	[SerializeField] Image playerLeftImage;

	[SerializeField] Button playerRightButton;
	[SerializeField] TMP_Text playerRightText;
	[SerializeField] Image playerRightImage;

	[Header("Sounds")]
	[SerializeField] AudioClip roundEnding;
	[SerializeField] AudioClip roundFinished;
	[SerializeField] AudioSource audioSource;


	Button currentPlayerTurn;

	float defaultTime = 30;
	float currentTime;
	float roundTime;
	Stopwatch timer = new Stopwatch();

	bool isPaused = false;
	bool countdownPlayed = false;
	bool endingPlayed = false;

	//public float CurrentTime { get; set; }
	//public float RoundTime { get => roundTime; set { roundTime = value; currentTime = value; clock.text = GetCurrentTimeAsText(); } }

	public GameManager() {
		if ( instance == null ) {
			instance = this;
		} else {
			Destroy( this );
		}
	}


	// Start is called before the first frame update
	void Start() {
		roundTime = defaultTime;
		Reset();
	}

	// Update is called once per frame
	void Update() {
		if ( timer.IsRunning ) {
			currentTime = roundTime - (float)timer.Elapsed.TotalSeconds;
			if (currentTime <= 10 && !countdownPlayed ) {
				audioSource.clip = roundEnding;
				audioSource.Play();
				countdownPlayed = true;
			}
			if(currentTime <= 2 && !endingPlayed ) {
				if ( audioSource.isPlaying ) {
					audioSource.Stop();
				}
				audioSource.clip = roundFinished;
				audioSource.Play();
				endingPlayed = true;
			}
			if ( currentTime <= 0 ) {
				onPlayerButton( currentPlayerTurn );
			}
			clock.text = GetCurrentTimeAsText();
		}
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
			currentPlayerTurn.GetComponentInChildren<TMP_Text>().text = "Waiting";

			if ( currentPlayerTurn == playerLeftButton ) {
				currentPlayerTurn = playerRightButton;
			} else {
				currentPlayerTurn = playerLeftButton;
			}

			currentPlayerTurn.interactable = true;
			currentPlayerTurn.image.color = Color.green;
			currentPlayerTurn.GetComponentInChildren<TMP_Text>().text = "End\nTurn";
			clock.gameObject.transform.Rotate( 0, 0, 180f );

			timer.Restart();
		} else {

			if ( _playerButton == playerLeftButton ) {
				playerRightButton.interactable = false;
				playerRightButton.image.color = new Color( 1, 0, 0, 0.5f );
				playerRightButton.GetComponentInChildren<TMP_Text>().text = "Waiting";
				clock.gameObject.transform.eulerAngles = new Vector3( 0, 0, -90 );
			} else {
				playerLeftButton.interactable = false;
				playerLeftButton.image.color = new Color( 1, 0, 0, 0.5f );
				playerLeftButton.GetComponentInChildren<TMP_Text>().text = "Waiting";
				clock.gameObject.transform.eulerAngles = new Vector3( 0, 0, 90 );
			}
			currentPlayerTurn = _playerButton;
			currentPlayerTurn.interactable = true;
			currentPlayerTurn.image.color = Color.green;
			currentPlayerTurn.GetComponentInChildren<TMP_Text>().text = "End\nTurn";

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
}
