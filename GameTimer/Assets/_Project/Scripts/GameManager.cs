using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Collections;

namespace GameTimer {

	public class GameManager : MonoBehaviour {

		public static GameManager instance;

		#region Serialised Fields
		[SerializeField] private List<GameType> gameTypes;
		[SerializeField] public GameObject playerControls;
		[SerializeField] public GameObject playerControlsHolder;

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
		#endregion

		#region Private Fields
		private bool isPaused = false;
		private GameType currentGameType;
		private Game currentGame;
		private Dictionary<string, GameType> gameTypesDictionary = new Dictionary<string, GameType>();
		private Coroutine coroutine;
		#endregion

		#region Properties
		public GameType CurrentGameType { get => currentGameType; }
		public Game CurrentGame { get => currentGame; }
		public GameObject GameTimerUI { get => gameTimerUI; }
		#endregion

		#region Unity Methods

		public void Awake() {
			if ( instance == null ) {
				instance = this;
				SetUpDictionary();
				LoadPlayerPrefs();
			} else {
				Destroy( this );
			}
		}


		private void Start() {
			ResetGame();
		}

		#endregion


		private void SetUpDictionary() {
			foreach ( GameType _gameType in gameTypes ) {
				if ( currentGameType == null ) {
					currentGameType = _gameType;
				}
				gameTypesDictionary.Add( _gameType.GameTypeName, _gameType );
			}
			gameTypes.Clear();
			gameTypes = null;
		}


		public void LoadPlayerPrefs() {
			currentGameType =  gameTypesDictionary[ PlayerPrefs.GetString( "gameType", currentGameType.GameTypeName ) ];
			currentGame = new Game( currentGameType );

			currentGame.DefaultGameTime = PlayerPrefs.GetFloat( "gameTime", currentGame.DefaultGameTime );
			currentGame.DefaultTurnTime = PlayerPrefs.GetFloat( "turnTime", currentGame.DefaultTurnTime );

			SoundManager.instance.CountdownEnabled = PlayerPrefs.GetString( "countDown_enabled", "true" ).Equals( "true" );
			SoundManager.instance.CountdownVolume = PlayerPrefs.GetFloat( "countDown_volume", 1f );
			SoundManager.instance.RoundEndEnabled = PlayerPrefs.GetString( "roundEnd_enabled", "true" ).Equals( "true" );
			SoundManager.instance.RoundEndVolume = PlayerPrefs.GetFloat( "roundEnd_volume", 1f );
		}


		public void SavePlayerPrefs() {
			PlayerPrefs.SetString( "gameType", currentGameType.GameTypeName );

			PlayerPrefs.SetFloat( "gameTime", currentGame.DefaultGameTime );
			PlayerPrefs.SetFloat( "turnTime", currentGame.DefaultTurnTime );

			PlayerPrefs.SetString( "countDown_enabled", SoundManager.instance.CountdownEnabled.ToString().ToLower() );
			PlayerPrefs.SetFloat( "countDown_volume", SoundManager.instance.CountdownVolume );
			PlayerPrefs.SetString( "roundEnd_enabled", SoundManager.instance.RoundEndEnabled.ToString().ToLower() );
			PlayerPrefs.SetFloat( "roundEnd_volume", SoundManager.instance.RoundEndVolume );
		}


		public void OnPauseButton() {
			if ( !isPaused ) {
				currentGame.PauseGame( true );
				pauseButtonImage.sprite = playSprite;
				SoundManager.instance.ResetSoundsPlayed();
			} else {
				currentGame.PauseGame( false );
				pauseButtonImage.sprite = pauseSprite;
			}

			isPaused = !isPaused;
			resetButton.interactable = isPaused;

		}


		public void OnGameStarted() {
			pauseButton.interactable = true;
			settingsButton.interactable = false;
			resetButton.interactable = false;
		}


		public void ResetGame() {
			currentGame.EndGame();
			isPaused = false;
			settingsButton.interactable = true;
			resetButton.interactable = false;
			pauseButton.interactable = false;
			pauseButtonImage.sprite = pauseSprite;
			if ( coroutine != null ) {
				StopCoroutine( coroutine );
			}

			SetCurrentTimeAsText( currentGame.DefaultTurnTime );
		}


		public void SetCurrentTimeAsText( float _time ) {
			int _minutes = (int)(_time / 60);
			int _seconds = (int)(_time - (_minutes * 60));

			clock.text = $"{_minutes:D2}:{_seconds:D2}";
		}


		public void ToggleSettings() {
			if ( settingPanel.activeSelf ) {
				settingPanel.SetActive( false );
			} else {
				settingPanel.SetActive( true );
				if(coroutine != null ) {
					StopCoroutine( coroutine );
				}
			}
		}


		public void MonitorPlayerTurnTime() {
			Player _player = CurrentGame.CurrentPlayerTurn;
			if ( _player.PlayerIndex > 0) {
				clock.transform.eulerAngles = new Vector3( 0, 0, 90f );
			} else {
				clock.transform.eulerAngles = new Vector3( 0, 0, -90f );
			}
			coroutine = StartCoroutine( "SetCurrentPlayerTurnTime" );
		}


		private IEnumerator SetCurrentPlayerTurnTime() {
			Player _player = CurrentGame.CurrentPlayerTurn;
			while ( CurrentGame.CurrentPlayerTurn == _player ) {
				float _defaultTime = currentGame.DefaultTurnTime;
				float _elapsedTime = _player.CurrentTurnTimeSeconds;
				float _timeLeft = _defaultTime - _elapsedTime;
				SetCurrentTimeAsText( _timeLeft );
				if ( !isPaused ) {
					if ( _timeLeft <= 10 ) {
						SoundManager.instance.PlayCountdown();
					}

					if ( _timeLeft <= 2 ) {
						SoundManager.instance.PlayTurnEnd();
					}

					if ( _timeLeft <= 0 ) {
						CurrentGame.CurrentPlayerTurn.EndTurn();
					}
				}
				yield return null;
			}

			SoundManager.instance.ResetSoundsPlayed();
			
		}

	}
}