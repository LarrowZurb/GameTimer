using System.Diagnostics;
using System.Collections.Generic;

namespace GameTimer {

	public class Game {

		private GameType currentGameType;
		private List<Player> players;
		private Player currentPlayerTurn;

		private float defaultGameTime;
		private float defaultTurnTime;
		private float currentGameTime;

		private Stopwatch gameTimer;

		public float DefaultGameTime { get => defaultGameTime; set => defaultGameTime = value; }
		public float DefaultTurnTime { get => defaultTurnTime; set => defaultTurnTime = value; }
		public float CurrentGameTime { get => currentGameTime; }
		public float CurrentTurnTime { get => currentPlayerTurn.CurrentTurnTimeSeconds; }
		public Player CurrentPlayerTurn { get => currentPlayerTurn; }

		public Game( GameType _gameType ) {
			currentGameType = _gameType;

			for ( int _i = 0; _i < currentGameType.NumPlayers; _i++ ) {
				players[_i] = new Player( GameManager.instance.playerControls );
				players[_i].AwaitTurn();
			}

			defaultGameTime = currentGameType.GameLength;
			defaultTurnTime = currentGameType.TurnLength;
			if ( defaultGameTime > 0 ) {
				currentGameTime = 0;
				gameTimer = new Stopwatch();
			}
		}


		public void StartGame( Player _player ) {
			currentPlayerTurn = _player;

			foreach ( Player _p in players ) {
				if ( _p == currentPlayerTurn ) {
					_p.StartTurn();
				} else {
					_p.AwaitTurn();
				}
			}

			if ( gameTimer != null ) {
				gameTimer.Start();
			}

		}


		public void PauseGame(bool _pause) {
			foreach ( Player _player in players ) {
				_player.PauseTurn( _pause );
			}

			if ( gameTimer != null ) {
				if ( _pause ) {
					gameTimer.Stop();
				} else {
					gameTimer.Start();
				}
			}

		}


		public void EndGame() {
			foreach ( Player _player in players ) {
				_player.ResetTurn();
			}

			if ( gameTimer != null ) {
				gameTimer.Reset();
			}

		}


		public bool GameTimeFinished() {
			if ( gameTimer != null && gameTimer.Elapsed.TotalSeconds >= defaultGameTime ) {
				EndGame();
				return true;
			}

			return false;
		}



	}
}