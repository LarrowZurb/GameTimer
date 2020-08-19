using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Diagnostics;

namespace GameTimer {

	public class Player {

		private PlayerState previousPlayerState = new PlayerState( true, "Start", Color.yellow, false );

		private Button playerButton;
		private TMP_Text playerText;
		private Image playerImage;

		private int totalTurnsTimeSeconds;
		private int currentTurnTimeSeconds;
		private Stopwatch turnTimer = new Stopwatch();

		private bool isPaused = false;

		public bool IsPaused { get => isPaused; }
		public int CurrentTurnTimeSeconds { get => (int)turnTimer.Elapsed.TotalSeconds; }
		public int TotalTurnsTimeSeconds { get => totalTurnsTimeSeconds; }


		public Player( GameObject _prefab ) {
			GameObject _controls = GameObject.Instantiate( _prefab );
			_controls.transform.SetParent( GameManager.instance.GameTimerUI.transform );
			PlayerControls _playerControls = _controls.GetComponent<PlayerControls>();
			playerButton = _playerControls.playerButton;
			playerText = _playerControls.playerText;
			playerImage = _playerControls.playerImage;
		}


		public void StartTurn() {

			playerButton.interactable = true;
			playerText.text = "End\nTrun";
			playerImage.color = Color.green;
			turnTimer.Reset();
			turnTimer.Start();
		}


		public void AwaitTurn() {

			playerButton.interactable = true;
			playerText.text = "Start";
			playerImage.color = Color.yellow;
		}


		public void PauseTurn( bool _pause ) {

			if ( isPaused && !_pause ) {
				playerButton.interactable = previousPlayerState.interactable;
				playerText.text = previousPlayerState.buttonText;
				playerImage.color = previousPlayerState.buttonColor;
				if ( previousPlayerState.timerRunning ) {
					turnTimer.Start();
				}
			} else if ( !isPaused && _pause ) {
				previousPlayerState.interactable = false;
				previousPlayerState.buttonText = playerText.text;
				previousPlayerState.buttonColor = playerImage.color;
				previousPlayerState.timerRunning = turnTimer.IsRunning;

				if ( turnTimer.IsRunning ) {
					turnTimer.Stop();
				}

				playerButton.interactable = false;
				playerText.text = "Paused";
				Color _color = playerImage.color;
				_color.a = 0.5f;
				playerImage.color = _color;
			}

			isPaused = !isPaused;

		}
		
		
		public void ResetTurn() {

			totalTurnsTimeSeconds = 0;
			if ( turnTimer.IsRunning ) {
				turnTimer.Stop();
			}
			AwaitTurn();
		}

		
		public void EndTurn() {

			playerButton.interactable = false;
			playerText.text = "Waiting";
			playerImage.color = new Color( 1f, 0f, 0f, 0.5f );
			totalTurnsTimeSeconds += (int)turnTimer.Elapsed.TotalSeconds;
			turnTimer.Reset();
		}


		public void OnPlayerButton() {

			if ( turnTimer.IsRunning ) {
				EndTurn();
			} else {
				StartTurn();
			}
		}



	}
}