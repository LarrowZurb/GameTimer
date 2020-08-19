using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameTimer {

	public class MinuteUpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

#pragma warning disable 649
		[SerializeField] SettingsManager manager;
#pragma warning restore 649

		void IPointerDownHandler.OnPointerDown( PointerEventData _eventData ) {
			StartCoroutine( "HandleDown" );
		}

		void IPointerUpHandler.OnPointerUp( PointerEventData _eventData ) {
			StopCoroutine( "HandleDown" );
		}

		IEnumerator HandleDown() {
			while ( true ) {
				manager.OnMinuteUp();
				yield return new WaitForSeconds( 0.25f );
			}
		}
	}
}