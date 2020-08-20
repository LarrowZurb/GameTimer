using UnityEngine;

namespace GameTimer {

	class SoundManager : MonoBehaviour {

		public static SoundManager instance;

		#region Serialized Fields
		[Header( "Sounds" )]
		[SerializeField] private AudioSource audioSource;
		[SerializeField] private AudioClip countdown_clip;
		[SerializeField] private AudioClip roundEnd_clip;
		#endregion

		#region Private Fields
		private bool countdownPlayed = false;
		private bool playCountdown = true;
		private float countdownVolume = 1f;
		private bool endingPlayed = false;
		private bool playRoundEnd = true;
		private float roundEndVolume = 1f;
		#endregion

		#region Properties
		public bool CountdownEnabled { get => playCountdown; set => playCountdown = value; }
		public float CountdownVolume { get => countdownVolume; set => countdownVolume = value; }
		public bool RoundEndEnabled { get => playRoundEnd; set => playRoundEnd = value; }
		public float RoundEndVolume { get => roundEndVolume; set => roundEndVolume = value; }
		#endregion

		#region Unity Methods
		private void Awake() {
			if ( instance == null ) {
				instance = this;
			} else {
				Destroy( this );
			}
		}
		#endregion


		public void ResetSoundsPlayed() {
			countdownPlayed = false;
			endingPlayed = false;
			if ( audioSource.isPlaying ) {
				audioSource.Stop();
			}
		}


		public void PlayCountdown() {
			if ( !playCountdown || countdownPlayed )
				return;

			PlayClip( countdown_clip, CountdownVolume );

			countdownPlayed = true;
		}


		public void PlayTurnEnd() {
			if ( !playRoundEnd || endingPlayed )
				return;

			PlayClip( roundEnd_clip, roundEndVolume );

			endingPlayed = true;
		}


		private void PlayClip( AudioClip _clip, float _volume ) {

			if ( audioSource.isPlaying ) {
				audioSource.Stop();
			}

			audioSource.clip = _clip;
			audioSource.volume = _volume;
			audioSource.Play();
		}



	}
}
