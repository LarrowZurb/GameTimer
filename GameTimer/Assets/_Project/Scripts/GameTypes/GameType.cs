using System.Collections.Generic;
using UnityEngine;

namespace GameTimer {

	[CreateAssetMenu( fileName = "NewGameType", menuName = "GameTimer/New Game Type" )]
	public class GameType : ScriptableObject {

		#region Serialized Fields
		[SerializeField] private string gameTypeName;
		[Range( 1f, 4f )]
		[SerializeField] private int numPlayers = 2;
		[SerializeField] private int numRounds = 0;
		[SerializeField] private int gameLengthSeconds = 0;
		[SerializeField] private int turnLengthSeconds = 0;
		#endregion

		#region Properties
		public int NumPlayers { get => numPlayers; }
		public int NumRounds { get => numRounds; }
		public int GameLength { get => gameLengthSeconds; }
		public int TurnLength { get => turnLengthSeconds; }
		public string GameTypeName { get => gameTypeName; }
		public List<Player> Players { set => players = value; }
		#endregion

		#region Private Fields
		private List<Player> players;
		private int currentPlayerIndex = -1;
		#endregion


		public Player GetNextPlayer() {
			currentPlayerIndex++;
			if ( currentPlayerIndex >= players.Count ) {
				currentPlayerIndex = 0;
			}
			return players[currentPlayerIndex];
		}

		public void SetCurrentPlayerIndex( Player _player ) {
			int _index = players.FindIndex( _x => _x.Equals( _player ) );
			if ( _index > -1 ) {
				currentPlayerIndex = _index;
			}
		}


		public int GetPlayerIndex( Player _player ) {
			int _index = players.FindIndex( _x => _x.Equals( _player ) );
			return _index;
		}

	}
}