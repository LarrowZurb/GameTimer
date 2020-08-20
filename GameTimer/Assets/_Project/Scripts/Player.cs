using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Collections;

namespace GameTimer {

	public class Player {

		#region Private Fields
		private Button playerButton;
		private TMP_Text playerText;
		private Image playerImage;
		private int totalTurnsTimeSeconds;
		//private int currentTurnTimeSeconds;
		private Stopwatch turnTimer = new Stopwatch();
		private bool isPaused = false;
		private Game currentGame;
		private int playerIndex = -1;
		#endregion

		#region Properties
		public bool IsPaused { get => isPaused; }
		public int CurrentTurnTimeSeconds { get => (int)turnTimer.Elapsed.TotalSeconds; }
		public int TotalTurnsTimeSeconds { get => totalTurnsTimeSeconds; }
		public int PlayerIndex { get => playerIndex; }
		#endregion


		public Player( GameObject _prefab, Game _currentGame, int _playerIndex ) {
			currentGame = _currentGame;
			playerIndex = _playerIndex;
			GameObject _controls = GameObject.Instantiate( _prefab, GameManager.instance.playerControlsHolder.transform );
			if(_playerIndex == 1 ) {
				_controls.transform.position = new Vector3(Screen.width,Screen.height/2,0);
				_controls.transform.eulerAngles = new Vector3( 0, 0, 180f );
			}
			PlayerControls _playerControls = _controls.GetComponent<PlayerControls>();
			playerButton = _playerControls.playerButton;
			playerText = _playerControls.playerText;
			playerImage = _playerControls.playerImage;
		}


		public void StartTurn() {

			playerButton.interactable = true;
			playerText.text = "End\nTurn";
			playerImage.color = Color.green;
			playerButton.onClick.RemoveAllListeners();
			playerButton.onClick.AddListener( () => EndTurn() );
			turnTimer.Reset();
			turnTimer.Start();
			GameManager.instance.MonitorPlayerTurnTime();
		}


		public void AwaitStart() {

			playerButton.interactable = true;
			playerText.text = "Start";
			playerImage.color = Color.yellow;
			playerButton.onClick.RemoveAllListeners();
			playerButton.onClick.AddListener( () => currentGame.StartGame( this ) );
		}


		public void AwaitTurn() {
			playerButton.interactable = false;
			playerText.text = "Waiting";
			playerImage.color = new Color( 1f, 0f, 0f, 0.5f );
		}


		public void PauseTurn( bool _pause ) {

			if ( _pause ) {
				playerText.text = "Paused";
				turnTimer.Stop();
				playerButton.interactable = false;
			} else {
				if(currentGame.CurrentPlayerTurn == this ) {
					playerText.text = "End\nTurn";
					playerButton.interactable = true;
					turnTimer.Start();
				} else {
					playerText.text = "Waiting";
				}
			}
		}


		public void ResetTurn() {

			totalTurnsTimeSeconds = 0;
			if ( turnTimer.IsRunning ) {
				turnTimer.Stop();
			}
			AwaitStart();
		}


		public void EndTurn() {

			AwaitTurn();
			totalTurnsTimeSeconds += (int)turnTimer.Elapsed.TotalSeconds;
			turnTimer.Reset();
			currentGame.StartNextPlayerTurn();
		}


	}
}