using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intent
{
	public const int MOVE_LEFT = -1;
	public const int MOVE_LEFT_DOUBLETAP = -2;
	public const int DONT_USE = 0;
	public const int MOVE_RIGHT = 1;
	public const int MOVE_RIGHT_DOUBLETAP = 2;
	public const int MOVE_UP = -3;
	public const int MOVE_DOWN = 3;
	public const int MOVE_JUMP = 4;
	public const int ATTACK_QUICK = 5;
	public const int ATTACK_HEAVY = 6;
	public const int ATTACK_SPECIAL = 7;
	public const int BLOCK = 8;
	public const int INTERACT = 9;

	public const int ATTACK_SPECIAL_1 = 10;
	public const int ATTACK_SPECIAL_2 = 11;
	public const int ATTACK_SPECIAL_3 = 12;

	public enum iCombo {
		None = 0,
		a = 1,
		b = 2,
		ab = 3,
		aa = 4,
		aab = 5,
		aaa = 6,
		aaab = 7,
		aaaa = 8
	}

	public const string CONTROLS = "Controls";

	public int moveLeftRight;
	public int moveUpDown;
	public int moveJump;
	public bool atkQuick;
	public bool atkHeavy;
	public bool doBlock;
	public int curCombo;
	public bool isAttacking;
	public int useSpecial;

	private List<KeyCode> keys;
	FileHandler files;
	
	public Intent(
		int pMoveLeftRight = DONT_USE,
		int pMoveUpDown = DONT_USE,
		int pMoveJump = DONT_USE,
		bool pAtkQuick = false,
		bool pAtkHeavy = false
		) {
		files = FileHandler.GetFileHandler();
		moveLeftRight = pMoveLeftRight;
		moveUpDown = pMoveUpDown;
		moveJump = pMoveJump;
		atkQuick = pAtkQuick;
		atkHeavy = pAtkHeavy;
		keys = files.LoadControls();
	}

	public void Validate() {
		if(doBlock) {
			atkQuick = false;
			atkHeavy = false;
		}
	}

	public void Clear() {
		moveLeftRight = DONT_USE;
		moveUpDown = DONT_USE;
		moveJump = DONT_USE;
		atkQuick = false;
		atkHeavy = false;
	}

	public void ResetKeys() {
		keys = files.ResetKeys();
	}

	public bool SetNewKey(KeyCode oldKeyCode, KeyCode newKey) {
		int oldIntent = KeyToIntent(oldKeyCode);
		int index = keys.FindIndex(i => i == oldKeyCode);
		int dupIndex = keys.FindIndex(index, i => i == newKey);

		if(oldIntent == MOVE_LEFT || oldIntent == MOVE_RIGHT) {
			if(dupIndex == -1) {
				keys[index] = newKey;
				keys[index + 1] = newKey;
				files.SaveControls(keys);
				return true;
			} else
				return false;
		} else	if(index >= 0 && dupIndex == -1) {
			keys[index] = newKey;
			files.SaveControls(keys);
			return true;
		} else {
			return false;
		}
	}

	public int KeyToIntent(KeyCode key) {
		return keys.FindIndex(i => i == key) - 3;
	}

	public KeyCode IntentToKey(int intent) {
		return keys[intent + 3];
	}

	public void SetCombo(string combo) {
		int newCombo = 0;
		if(combo != "") {
			newCombo = (int)System.Enum.Parse(typeof(iCombo), combo);
			if(newCombo == 8)
				newCombo = 6;
		}
		if(newCombo > curCombo)
			curCombo = newCombo;

	}

	public void ResetCombo() {
		curCombo = 0;
	}


}
