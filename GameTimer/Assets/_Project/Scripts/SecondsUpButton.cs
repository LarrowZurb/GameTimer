﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SecondsUpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

	[SerializeField] settingsManager manager;

	void IPointerDownHandler.OnPointerDown( PointerEventData eventData ) {
		StartCoroutine("HandleDown");
	}

	void IPointerUpHandler.OnPointerUp( PointerEventData eventData ) {
		StopCoroutine( "HandleDown" );
	}

	IEnumerator HandleDown() {
		while ( true ) {
			manager.OnSecondsUp();
			yield return new WaitForSeconds( 0.25f );
		}
	}
}