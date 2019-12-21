using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputKey
{
	public float lastPressed;
	public KeyCode key;

	public InputKey() {
		lastPressed = 0;
		key = KeyCode.None;
	}

	public InputKey(KeyCode pKey, float pLastPressed) {
		lastPressed = pLastPressed;
		key = pKey;
	}
}


