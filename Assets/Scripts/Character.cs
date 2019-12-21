using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	[SerializeField]
	public bool isPlayer;
	[SerializeField]
	int runSpeed;
	[SerializeField]
	int jumpHeight;
	[SerializeField]
	public int HP = 100;
	[SerializeField]
	bool flipX;
	[SerializeField]
	bool deleteThis;
	[SerializeField]
	public GameObject spawner;
	[SerializeField]
	public float runMultiplyer;
	[SerializeField]
	public Sprite profilePic;

	public Mobility mob;
	public int curHP;
	public int maxHP;
	public int curSpecial;
	public int maxSpecial;
	bool movingRight;
	bool wasAttacking;
	float doubleTapTime_lr;
	int doubleTapKey_lr;
	ButtonSequence buttonSequence;
	SpriteRenderer render;

	bool wasBlocking = false;
	bool isBlocking = false;
	public bool isIdle;

	float attackCooldown;

	private void Awake() {
		movingRight = true;
		maxHP = HP;
		curHP = HP;
		curSpecial = 0;
		maxSpecial = 300;
		mob = new Mobility(transform, runSpeed, jumpHeight, flipX, runMultiplyer);
		render = transform.GetComponent<SpriteRenderer>();
		if(isPlayer) {
			SetButtonSequence();
		}
	}

	public void SetButtonSequence() {
		buttonSequence = transform.gameObject.AddComponent<ButtonSequence>() as ButtonSequence;
		buttonSequence.intent = mob.intent;
	}

	// Start is called before the first frame update
	void Start()
  {
	}

	public void SetGrounded(bool isGrounded) {
		mob.isGrounded = isGrounded;
		if(isGrounded && buttonSequence != null) {
			buttonSequence.ResetCombo();
			mob.ResetCombo();
		}
	}

	public bool Hit(Collider2D col, bool unBlockable, int dmg, bool hitYourself, GameObject hitSprite, Character enemy, Vector2 throwBackRange) {
		if(!mob.isDead && !IsLoading && (!mob.isBlocking || unBlockable) && !mob.usingSpecial) {
			if(hitSprite != null) {
				Vector3 newPos = new Vector3(transform.position.x, transform.position.y, -5.0f);
				Instantiate(hitSprite, newPos, Quaternion.identity);
			}
			curHP -= dmg;
			AddSpecial(dmg);
			if(enemy!=null)
				enemy.GetComponent<Character>().AddSpecial(dmg/2);
			if(curHP <= 0) {
				mob.KillChr();
				if(spawner != null) {
					spawner.GetComponent<Spawner>().RemoveSpawned();
				}
			} else {
				mob.Hit(col, hitYourself, throwBackRange);
			}
		} else if(mob.isBlocking) {
			AddSpecial(1);
		}
		return !wasBlocking && isBlocking;
	}

	private void PlayerUpdate() {
		if( Input.GetKey(mob.intent.IntentToKey(Intent.MOVE_RIGHT))) {
			if(buttonSequence.IsDoubleTap(mob.intent.IntentToKey(Intent.MOVE_RIGHT))) {
				mob.intent.moveLeftRight = Intent.MOVE_RIGHT_DOUBLETAP;
			} else {
				mob.intent.moveLeftRight = Intent.MOVE_RIGHT;
			}
		} else if(Input.GetKey(mob.intent.IntentToKey(Intent.MOVE_LEFT))) {
			if(buttonSequence.IsDoubleTap(mob.intent.IntentToKey(Intent.MOVE_LEFT))) {
				mob.intent.moveLeftRight = Intent.MOVE_LEFT_DOUBLETAP;
			} else {
				mob.intent.moveLeftRight = Intent.MOVE_LEFT;
			}
		} else {
			mob.intent.moveLeftRight = Intent.DONT_USE;
			doubleTapKey_lr = Intent.DONT_USE;
		}

		if(Input.GetKey(mob.intent.IntentToKey(Intent.MOVE_JUMP))) {
			mob.intent.moveJump = Intent.MOVE_JUMP;
		} else {
			mob.intent.moveJump = Intent.DONT_USE;
		}

		if(Input.GetKey(mob.intent.IntentToKey(Intent.MOVE_UP))) {
			mob.intent.moveUpDown = Intent.MOVE_UP;
		}else if(Input.GetKey(mob.intent.IntentToKey(Intent.MOVE_DOWN))) {
			mob.intent.moveUpDown = Intent.MOVE_DOWN;
		} else {
			mob.intent.moveUpDown = Intent.DONT_USE;
		}

		if(!mob.inAir && DoingSpecial() && curSpecial>=100) {
			if(curSpecial < 200) {
				curSpecial -= 100;
				mob.intent.useSpecial = Intent.ATTACK_SPECIAL_1;
			}else if(curSpecial < 300 && curSpecial >= 200) {
				curSpecial -= 200;
				mob.intent.useSpecial = Intent.ATTACK_SPECIAL_2;
			} else {
				curSpecial -= 300;
				mob.intent.useSpecial = Intent.ATTACK_SPECIAL_3;
			}
		}

		mob.intent.atkQuick = Input.GetKey(mob.intent.IntentToKey(Intent.ATTACK_QUICK));
		mob.intent.atkHeavy = Input.GetKey(mob.intent.IntentToKey(Intent.ATTACK_HEAVY));

		mob.intent.doBlock = Input.GetKey(mob.intent.IntentToKey(Intent.BLOCK));
		wasBlocking = isBlocking;
		isBlocking = mob.intent.doBlock;
	}	

	private void EnemyUpdate() {
		if(!mob.inAir) {
			Bounds mobBounds = transform.GetComponent<Collider2D>().bounds;
			Vector2 leadCorner = new Vector2( movingRight ? mobBounds.max.x : mobBounds.min.x, mobBounds.min.y - 0.05f);
			Vector2 leadFace = new Vector2(movingRight ? mobBounds.max.x + 0.05f : mobBounds.min.x - 0.05f, mobBounds.center.y);

			if(!Physics2D.Linecast(transform.position, leadCorner, 1 << LayerMask.NameToLayer("Ground")) ||
					Physics2D.Linecast(transform.position, leadFace, 1 << LayerMask.NameToLayer("Wall"))) {
				movingRight = !movingRight;
			}
			bool canAttack = --attackCooldown <= 0;

			RaycastHit2D hit = Physics2D.Linecast(transform.position, leadFace, 1 << LayerMask.NameToLayer("Player"));
			if(hit.collider != null && Mathf.Abs(hit.collider.transform.position.z - transform.position.z) <= 5) {
				string hitState = hit.collider.GetComponent<Character>().mob.currentState;
				if(canAttack && hitState != "KnockedOut" && hitState != "Recover") {
					attackCooldown = 100;
					mob.intent.curCombo = (int)Intent.iCombo.aaa;
				}
			} else {
				mob.intent.atkQuick = false;
				mob.intent.moveLeftRight = movingRight ? Intent.MOVE_RIGHT : Intent.MOVE_LEFT;
			}
		}
		mob.FixedUpdate();
	}

	private bool DoingSpecial() {
		return Input.GetKey(mob.intent.IntentToKey(Intent.ATTACK_SPECIAL)) ||
			(Input.GetKey(mob.intent.IntentToKey(Intent.ATTACK_QUICK)) &&
			Input.GetKey(mob.intent.IntentToKey(Intent.ATTACK_HEAVY)));
	}

	public void IsIdle() {		
		isIdle = true;
		if(isPlayer)
			buttonSequence.ResetCombo();
	}

	public void ResetFootEdge() {
		mob.ResetFootEdge();
	}

	private void FixedUpdate() {
		render.sortingOrder = (int)transform.position.z;

		if(deleteThis && !isPlayer)
			Destroy(transform.gameObject);

		if(!mob.isDead) {
			if(isPlayer) {
				PlayerUpdate();
			} else {
				EnemyUpdate();
			}
		}
		mob.FixedUpdate();
	}

	public void AddSpecial(int amt) {
		curSpecial += amt;
		if(curSpecial > maxSpecial)
			curSpecial = maxSpecial;
	}

	public void OnEnable() {
		isIdle = false;
		IsLoading = true;
		mob.LoadCharacter();
	}

	public bool IsLoading {
		get { return mob.isLoading; }
		set { mob.isLoading = value; }
	}

	public void JumpAway() {
		isIdle = false;
		mob.JumpAway();
	}


}
