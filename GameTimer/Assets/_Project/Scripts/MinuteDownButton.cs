using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameTimer {

	public class MinuteDownButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

#pragma warning disable 649
		[SerializeField] settingsManager manager;
#pragma warning restore 649

		void IPointerDownHandler.OnPointerDown( PointerEventData eventData ) {
			StartCoroutine( "HandleDown" );
		}

		void IPointerUpHandler.OnPointerUp( PointerEventData eventData ) {
			StopCoroutine( "HandleDown" );
		}

		IEnumerator HandleDown() {
			while ( true ) {
				manager.OnMinuteDown();
				yield return new WaitForSeconds( 0.25f );
			}
		}
	}
}