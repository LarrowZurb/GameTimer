using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace GameTimer {

	public class GameManager : MonoBehaviour {
		public static GameManager instance;

		[SerializeField] private List<GameType> gameTypes;
		[SerializeField] public GameObject playerControls;

		[Header( "UI Components" )]
		[SerializeField] private TMP_Text clock;
		[SerializeField] private Button pauseButton;
		[SerializeField] private Image pauseButtonImage;
		[SerializeField] private Button resetButton;
		[SerializeField] private Button settingsButton;
		[SerializeField] private GameObject settingPanel;
		[SerializeField] private GameObject gameTimerUI;

		[Header( "Button Images" )]
		[SerializeField] private Sprite pauseSprite;
		[SerializeField] private Sprite playSprite;


		private bool isPaused = false;

		private GameType currentGameType;
		private Game currentGame;

		public GameType CurrentGameType { get => currentGameType; }
		public Game CurrentGame { get => currentGame; }
		public GameObject GameTimerUI { get => gameTimerUI; }

		public void Awake() {
			if ( instance == null ) {
				instance = this;
				SetGameType( gameTypes[0].GameTypeName );
				currentGame = new Game( currentGameType );
			} else {
				Destroy( this );
			}
		}


		public void SetGameType( string _gameTypeName ) {
			for ( int _i = 0; _i < gameTypes.Count; _i++ ) {
				if ( gameTypes[_i].GameTypeName == _gameTypeName ) {
					currentGameType = gameTypes[_i];
					return;
				}
			}
		}


		// Start is called before the first frame update
		private void Start() {
			ResetGame();
		}



		// Update is called once per frame
		private void Update() {
			//if ( timer.IsRunning ) {
			//	currentTime = roundTime - (float)timer.Elapsed.TotalSeconds;
			//	if ( currentTime <= 10 ) {
			//		PlayCountdown();
			//	}
			//	if ( currentTime <= 2 ) {
			//		PlayRoundEnd();
			//	}
			//	if ( currentTime <= 0 ) {
			//		onPlayerButton( currentPlayerTurn );
			//	}
			//	clock.text = GetCurrentTimeAsText();
			//}
		}


		public void OnPauseButton() {
			if ( !isPaused ) {
				currentGame.PauseGame(true);
				pauseButtonImage.sprite = playSprite;
			} else {
				currentGame.PauseGame( false );
				pauseButtonImage.sprite = pauseSprite;
			}

			isPaused = !isPaused;
			resetButton.interactable = isPaused;

		}


		public void ResetGame() {
			currentGame.EndGame();
			isPaused = false;
			settingsButton.interactable = true;
			resetButton.interactable = false;
			pauseButton.interactable = false;
			pauseButtonImage.sprite = pauseSprite;

			clock.text = GetCurrentTimeAsText(currentGame.DefaultTurnTime);
		}


		public string GetCurrentTimeAsText(float _time) {
			int _minutes = (int)(_time / 60);
			int _seconds = (int)(_time - (_minutes * 60));

			return $"{_minutes:D2}:{_seconds:D2}";
		}


		public void ToggleSettings() {
			if ( settingPanel.activeSelf ) {
				settingPanel.SetActive( false );
			} else {
				settingPanel.SetActive( true );
			}
		}


		public void OnPlayerButton() {
			currentGame.CurrentPlayerTurn.OnPlayerButton();
		}

	}

}