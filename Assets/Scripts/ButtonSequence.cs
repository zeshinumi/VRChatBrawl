using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSequence : MonoBehaviour
{
	public const int C_NONE = 0;
	public const int C_QUICK = 1;
	public const int C_HEAVY = 2;
		
	public List<InputKey> inputList = new List<InputKey>();
	public string combo;
	public string lastSentCombo;
	int comboMax = 4;
	Character chr;
	public Intent intent;
	Animator anim;
	List<KeyCode> code;
	bool[] isPressed;
	bool isAttacking;
	bool bt_Q;
	float lastButtonPress;

    // Start is called before the first frame update
    void Start()
    {
			Animator anim = transform.GetComponent<Animator>();
			chr = transform.GetComponent<Character>();
			intent = chr.mob.intent;
			code = FileHandler.GetFileHandler().LoadControls();
			isPressed = new bool[code.Count];
			for(int i = 0; i < code.Count; i++) {
				isPressed[i] = false;
			}
		
			combo = "";
    }

		private void FixedUpdate() {
			for(int i = 0; i < code.Count; i++) {
				DetectChange(i);
			}
			if(inputList.Count >= 11) {
				inputList.RemoveAt(0);
			}
			
			if(combo != lastSentCombo) {
				intent.SetCombo(combo);
				lastSentCombo = combo;
			}

			if(!IsStillAttacking())
				combo = "";

		}

		private void DetectChange(int slot) {
			if(isPressed[slot] != Input.GetKeyDown(code[slot])) {
				isPressed[slot] = Input.GetKeyDown(code[slot]);
				if(isPressed[slot]) {
					inputList.Add(new InputKey(code[slot], Time.time));
					string nextC = KeyToCombo(intent.KeyToIntent(code[slot]));
					if(combo.Length < comboMax && !combo.Contains("b")) {
						combo += KeyToCombo(intent.KeyToIntent(code[slot]));
					}
				}
			}
		}

		private string KeyToCombo(int intent) {
			if(intent == Intent.ATTACK_QUICK) {
				return "a";
			}else if(intent == Intent.ATTACK_HEAVY) {
				return "b";
			} else {
				return "";
			}
		}

		public bool IsDoubleTap(KeyCode key) {
			int lastIndex = inputList.FindLastIndex(i => i.key == key);
			return lastIndex != -1 && lastIndex > 0 && 
				Time.time < inputList[lastIndex].lastPressed + .25f &&
				inputList[lastIndex - 1].key == key &&
				inputList[lastIndex].lastPressed < inputList[lastIndex-1].lastPressed + .25f;
		}

		private bool IsStillAttacking() {
			KeyCode quickKey = intent.IntentToKey(Intent.ATTACK_QUICK);
			KeyCode heavyKey = intent.IntentToKey(Intent.ATTACK_HEAVY);
			int lastIndex = inputList.FindLastIndex(i => i.key == quickKey || i.key == heavyKey);
			return lastIndex != -1 &&
				Time.time < inputList[lastIndex].lastPressed + .5f;
		}

		public bool IsDoingAttack() {				
				KeyCode quickKey = intent.IntentToKey(Intent.ATTACK_QUICK);
				KeyCode heavyKey = intent.IntentToKey(Intent.ATTACK_HEAVY);
				int lastIndex = inputList.FindLastIndex(i => i.key == quickKey || i.key == heavyKey);
				return lastIndex != -1 &&
					Time.time < inputList[lastIndex].lastPressed + .25f;
			}

		public void ResetCombo() {
			combo = "";
		}


}
