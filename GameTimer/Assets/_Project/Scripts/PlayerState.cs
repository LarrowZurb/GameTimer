using UnityEngine;

namespace GameTimer {

	struct PlayerState {

		public bool interactable;
		public string buttonText;
		public Color buttonColor;
		public bool timerRunning;

		public PlayerState( bool _interactable, string _text, Color _color, bool isRunning ) {
			interactable = _interactable;
			buttonText = _text;
			buttonColor = _color;
			timerRunning = isRunning;
		}

		public void SetCurrentState( bool _interactable, string _text, Color _color, bool isRunning ) {
			interactable = _interactable;
			buttonText = _text;
			buttonColor = _color;
			timerRunning = isRunning;
		}



	}
}