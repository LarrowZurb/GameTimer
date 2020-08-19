using UnityEngine;

namespace GameTimer {

	[CreateAssetMenu( fileName = "NewGameType", menuName = "GameTimer/New Game Type" )]
	public class GameType : ScriptableObject {

		[SerializeField] private string gameTypeName;
		[Range( 1f, 4f )]
		[SerializeField] private int numPlayers = 2;
		[SerializeField] private int numRounds = 0;
		[SerializeField] private int gameLengthSeconds = 0;
		[SerializeField] private int turnLengthSeconds = 0;
		

		public int NumPlayers { get => numPlayers; }
		public int NumRounds { get => numRounds; }
		public int GameLength { get => gameLengthSeconds; }
		public int TurnLength { get => turnLengthSeconds; }
		public string GameTypeName { get => gameTypeName; }
	}
}