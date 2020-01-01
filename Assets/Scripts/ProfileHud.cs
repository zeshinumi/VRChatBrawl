using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileHud : MonoBehaviour
{
	//[SerializeField]

	public GameObject[] characters;

	public const int MAIN_CHR = 0;
	public const int SEC_CHR = 1;
	public const int THR_CHR = 2;

	private LineRenderer HP;
	private LineRenderer SP;
	private SpriteRenderer[] pics;
	private Character chr;
	private GameObject activeChr;
	private bool loadedAlready;

	// Start is called before the first frame update
	public void Start() {
		if(!loadedAlready) {
			characters = new GameObject[3];
			HP = transform.Find("Health").GetComponent<LineRenderer>();
			SP = transform.Find("Special").GetComponent<LineRenderer>();
			pics = new SpriteRenderer[3];
			pics[MAIN_CHR] = transform.Find("Picture").GetComponent<SpriteRenderer>();
			pics[SEC_CHR] = transform.Find("Fighter2").GetComponent<SpriteRenderer>();
			pics[THR_CHR] = transform.Find("Fighter3").GetComponent<SpriteRenderer>();
			loadedAlready = true;
		}
	}

	public void SetAllChrProf() {
		SetChrProf(characters[MAIN_CHR], MAIN_CHR, 499);
		SetChrProf(characters[SEC_CHR], SEC_CHR, 200);
		SetChrProf(characters[THR_CHR], THR_CHR, 200);
	}

    // Update is called once per frame
  void Update() {
		HP.SetPosition(1, new Vector3((float)chr.curHP / chr.maxHP * 5.0f, 0, 1));
		SP.SetPosition(1, new Vector3((float)chr.curSpecial / chr.maxSpecial * 5.0f, 0, 1));

		if(Input.GetKeyDown(KeyCode.Return)) {
			if(Time.timeScale != 0) { 
				transform.Find("PauseMenu").gameObject.SetActive(true);
				Time.timeScale = 0;
			}
		}

		if(chr.isIdle && !chr.mob.isHit) {
			if(characters[SEC_CHR]!=null && Input.GetKeyDown(KeyCode.Alpha1) && !characters[SEC_CHR].GetComponent<Character>().mob.isDead) {
				SwapChr(characters[SEC_CHR].GetComponent<Character>(), characters[ProfileHud.MAIN_CHR].transform.position, ProfileHud.SEC_CHR);
			} else if(characters[THR_CHR] != null && Input.GetKeyDown(KeyCode.Alpha2) && !characters[THR_CHR].GetComponent<Character>().mob.isDead) {
				SwapChr(characters[THR_CHR].GetComponent<Character>(), characters[ProfileHud.MAIN_CHR].transform.position, ProfileHud.THR_CHR);
			}
		}

		if(chr.mob.isDead) {
			if(characters[SEC_CHR] != null && !characters[SEC_CHR].GetComponent<Character>().mob.isDead) {
				SwapChr(characters[SEC_CHR].GetComponent<Character>(), characters[MAIN_CHR].transform.position, SEC_CHR);
			} else if(characters[THR_CHR] != null && !characters[THR_CHR].GetComponent<Character>().mob.isDead) {
				SwapChr(characters[THR_CHR].GetComponent<Character>(), characters[MAIN_CHR].transform.position, THR_CHR);
			} else {
				transform.Find("FTB").gameObject.SetActive(true);
			}
		}
	}
	
	public void UnPause() {
		Time.timeScale = 1;
		transform.Find("PauseMenu").gameObject.SetActive(false);
	}

	public void SetChrProf(GameObject setChr, int numChr, int setSize) {
		if(setChr == null)
			return;
		if(numChr == MAIN_CHR) {
			activeChr = setChr;
			chr = activeChr.GetComponent<Character>();
		}
		pics[numChr].sprite = setChr.GetComponent<Character>().profilePic;
		float size = pics[numChr].sprite.rect.width;
		pics[numChr].transform.localScale = new Vector3(setSize / size, setSize / size, setSize / size);
	}

	private void SwapChr(Character newChr, Vector3 oldPos, int targetChr) {
		if(!chr.mob.isDead)
			chr.JumpAway();
		newChr.transform.position = oldPos;
		newChr.mob.ScaleCharacter();
		SwapCharacters(targetChr);
		chr = newChr;
	}

	public GameObject SwapCharacters(int swapWith) {
		GameObject holdChr = characters[MAIN_CHR];
		if(characters[SEC_CHR] != null && swapWith == SEC_CHR) {
			characters[MAIN_CHR] = characters[SEC_CHR];
			characters[SEC_CHR] = holdChr;
		} else if(characters[THR_CHR] != null) {
			characters[MAIN_CHR] = characters[THR_CHR];
			characters[THR_CHR] = holdChr;
		}
		SetChrProf(characters[MAIN_CHR], MAIN_CHR, 499);
		SetChrProf(characters[SEC_CHR], SEC_CHR, 200);
		SetChrProf(characters[THR_CHR], THR_CHR, 200);
		characters[MAIN_CHR].SetActive(true);
		return characters[MAIN_CHR];
	}


}
