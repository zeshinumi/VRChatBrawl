using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
	private Intent intent;
	private bool collectKey;
	private Transform keyCollect;
	private KeyCode keycodeToChange;
  // Start is called before the first frame update
  void Start()
  {
		intent = new Intent();
		SetKeyLabels();
			
	}

	private void SetKeyLabels() {
		Transform buttons = transform.Find("ChangeButtons");
		buttons.Find("Up").GetComponentInChildren<Text>().text = intent.IntentToKey(Intent.MOVE_UP).ToString();
		buttons.Find("Down").GetComponentInChildren<Text>().text = intent.IntentToKey(Intent.MOVE_DOWN).ToString();
		buttons.Find("Left").GetComponentInChildren<Text>().text = intent.IntentToKey(Intent.MOVE_LEFT).ToString();
		buttons.Find("Right").GetComponentInChildren<Text>().text = intent.IntentToKey(Intent.MOVE_RIGHT).ToString();
		buttons.Find("Jump").GetComponentInChildren<Text>().text = intent.IntentToKey(Intent.MOVE_JUMP).ToString();
		buttons.Find("QuickAtk").GetComponentInChildren<Text>().text = intent.IntentToKey(Intent.ATTACK_QUICK).ToString();
		buttons.Find("HeavyAtk").GetComponentInChildren<Text>().text = intent.IntentToKey(Intent.ATTACK_HEAVY).ToString();
		buttons.Find("Special").GetComponentInChildren<Text>().text = intent.IntentToKey(Intent.ATTACK_SPECIAL).ToString();
		buttons.Find("Interact").GetComponentInChildren<Text>().text = intent.IntentToKey(Intent.INTERACT).ToString();
	}

	public void CollectKey(Transform curKey) {
		keyCollect = curKey;
		keycodeToChange = (KeyCode)System.Enum.Parse(typeof(KeyCode), curKey.GetComponentInChildren<Text>().text);
		collectKey = true;
	}

	public void ResetAllKeys() {
		intent.ResetKeys();
		SetKeyLabels();
	}

	private void Update() {
		/*if(collectKey && Input.anyKeyDown) {
			foreach(KeyCode kcode in Enum.GetValues(typeof(KeyCode))) {
				if(Input.GetKey(kcode))
					keyCollect.GetComponentInChildren<Text>().text = kcode.ToString();
			}
		}*/
	}

	public void OnGUI() {
		Event e = Event.current;
		if(collectKey && e.isKey) {
			if(intent.SetNewKey(keycodeToChange, e.keyCode)) {
				keyCollect.GetComponentInChildren<Text>().text = e.keyCode.ToString();
				collectKey = false;
			}
		}
	}
}
