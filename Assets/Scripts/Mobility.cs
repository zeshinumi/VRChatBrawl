using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mobility
{

	public Intent intent;
	SpriteRenderer chrRender;
	Animator anim;
	Rigidbody2D rb2D;
	BoxCollider2D cCol;

	public enum CurStateEnum {
		Idle,
		Walk,
		Run,
		Land,
		Jump,
		Block,
		Hit,
		Recover,
		KnockedOut,
		Launched,
		Die,
		AirQuick,
		AirHeavy,
		Quick,
		Heavy,
		Quick_1,
		Quick_2,
		Quick_3,
		NumStates
	}
	public string[] StateList = {
		"Idle",
		"Walk",
		"Run",
		"Land",
		"Jump",
		"Block",
		"Hit",
		"Recover",
		"KnockedOut",
		"Launched",
		"Die",
		"AirQuick",
		"AirHeavy",
		"Quick",
		"Heavy",
		"Quick_1",
		"Quick_2",
		"Quick_3"
	};
	public string currentState;

	public bool isGrounded;
	public bool inAir;
	public bool isDead;
	public bool canMove;
	bool isJumping;
	float prevYVel;
	int runSpeed;
	int jumpHeight;
	int comboBreak;
	bool flipX;
	public bool isHit;
	public bool isBlocking;
	float runMultiplyer;
	public float jumpY;
	Vector3 origonalScale;
	public bool isLoading;

	bool isSprinting;
	bool moveLeft;
	public bool isAttacking;
	public bool usingSpecial;

	public GameObject blPosition;
	public GameObject brPosition;
	public GameObject centerPosition;
	EdgeCollider2D footEdge;
	Transform playerT;

	public Mobility(Transform pPlayerT, int pRunSpeed, int pJumpHeight, bool pFlipX, float pRunMultiplyer) {
		playerT = pPlayerT;
		chrRender = playerT.GetComponent<SpriteRenderer>();
		anim = playerT.GetComponent<Animator>();
		rb2D = playerT.GetComponent<Rigidbody2D>();
		cCol = playerT.GetComponent<BoxCollider2D>();
		runSpeed = pRunSpeed;
		jumpHeight = pJumpHeight;
		intent = new Intent();
		flipX = pFlipX;
		isDead = false;
		isGrounded = true;
		runMultiplyer = pRunMultiplyer;
		jumpY = playerT.transform.position.y;
		
		float minY = playerT.GetComponent<Collider2D>().bounds.min.y;
		blPosition = new GameObject("Bottom Left");
		blPosition.transform.position = new Vector3(playerT.GetComponent<Collider2D>().bounds.min.x, minY);

		brPosition = new GameObject("Bottom Right");
		brPosition.transform.position = new Vector3(playerT.GetComponent<Collider2D>().bounds.max.x, minY);

		centerPosition = new GameObject("Center");
		centerPosition.transform.position = new Vector3(playerT.position.x, minY);
		centerPosition.transform.localScale = new Vector3(playerT.localScale.x, 1, playerT.localScale.z);
		
		centerPosition.transform.parent = playerT.transform;
		blPosition.transform.parent = playerT.transform;
		brPosition.transform.parent = playerT.transform;

		footEdge = playerT.gameObject.AddComponent<EdgeCollider2D>() as EdgeCollider2D;
		footEdge.points = new Vector2[2] { blPosition.transform.localPosition, brPosition.transform.localPosition };
		origonalScale = playerT.transform.localScale;
		
	}

	private bool CheckInAir() {
		inAir = !isGrounded && rb2D.velocity.y != 0;
		return inAir;
	}

	private void DoFlipX(bool left) {
			Vector3 chScale = playerT.localScale;
			if(left) {
				chScale.x = (flipX ? -1 : 1) * Mathf.Abs(chScale.x);
			} else {
				chScale.x = (flipX ? 1 : -1) * Mathf.Abs(chScale.x);
			}
			playerT.localScale = chScale;
	}

	private void doAirMovement() {
		if(!isHit) {
			if(intent.moveLeftRight == Intent.MOVE_LEFT) {
				rb2D.velocity = new Vector2(-runSpeed, rb2D.velocity.y);
				DoFlipX(true);
			} else if(intent.moveLeftRight == Intent.MOVE_RIGHT) {
				rb2D.velocity = new Vector2(runSpeed, rb2D.velocity.y);
				DoFlipX(false);
			} 
		}
	}

	private void doGroundAttack() {
		if(!isHit) {
			if(intent.curCombo != 0) {
				if(intent.curCombo == (int)Intent.iCombo.a && currentState!="Quick") {
					anim.Play("Quick");
					isAttacking = true;
				} else if(intent.curCombo == (int)Intent.iCombo.b && currentState!="Heavy") {
					anim.Play("Heavy");
					isAttacking = true;
				} else if(intent.curCombo > (int)Intent.iCombo.b && !isAttacking) {
					anim.Play("Quick");
					isAttacking = true;
				}
			}
			anim.SetBool("atkQuick", intent.atkQuick);
			anim.SetBool("atkHeavy", intent.atkHeavy);
		}
	}

	private void doAirAttack() {
		if(!isHit) {
			if(intent.atkQuick && intent.atkHeavy) {
				anim.Play("Quick");
			} else if(intent.atkQuick) {
				anim.Play("AirQuick");
			} else if(intent.atkHeavy) {
				anim.Play("AirHeavy");
			}
		}
	}

	public void ScaleCharacter() {
		float scaleDif = (playerT.position.z + 50) / 100;
		footEdge.offset = new Vector2(0, (playerT.position.z - 50)/5/playerT.localScale.y);
		playerT.localScale = new Vector3(origonalScale.x * scaleDif, origonalScale.y * scaleDif);
	}

		private void doGroundMovement() {
		if(!isHit && !isAttacking && canMove) {
			if(!isJumping) {
				if(intent.moveUpDown == Intent.MOVE_UP && !HitColliderWithCeiling()) {
					float offsetX = -1 * HitColliderWithWall(true) * 0.1f;
					//footEdge.offset = new Vector2(footEdge.offset.x, footEdge.offset.y - (0.1f / playerT.lossyScale.y));
					playerT.position = new Vector3(playerT.position.x + offsetX, playerT.position.y, playerT.position.z - 0.5f);
					ScaleCharacter();
					anim.Play("Walk");

				} else if(intent.moveUpDown == Intent.MOVE_DOWN && !HitColliderWithFloor()) {
					//Collider2D[] cols = new Collider2D[1];
					//int col = footEdge.GetContacts(cols);
					float offset = (0.1f / playerT.lossyScale.y);
					//footEdge.offset = new Vector2(footEdge.offset.x, footEdge.offset.y + (0.1f / playerT.lossyScale.y));
					playerT.position = new Vector3(playerT.position.x, playerT.position.y, playerT.position.z + 0.5f);
					ScaleCharacter();
					playerT.localPosition = new Vector3(playerT.localPosition.x, playerT.localPosition.y + offset, playerT.localPosition.z);
					anim.Play("Walk");

				}
			}
			if(!isSprinting &&
				(intent.moveLeftRight == Intent.MOVE_LEFT_DOUBLETAP || intent.moveLeftRight == Intent.MOVE_RIGHT_DOUBLETAP)) {
				moveLeft = intent.moveLeftRight <= Intent.MOVE_LEFT;
				isSprinting = true;
			}
			if(intent.moveLeftRight <= Intent.MOVE_LEFT) {
				isSprinting = isSprinting && moveLeft;
				anim.Play(isSprinting ? "Run" : "Walk");
				rb2D.velocity = new Vector2(-runSpeed * (isSprinting ? runMultiplyer : 1), rb2D.velocity.y);
				DoFlipX(true);
			} else if(intent.moveLeftRight >= Intent.MOVE_RIGHT) {
				isSprinting = isSprinting && !moveLeft;
				anim.Play(isSprinting ? "Run" : "Walk");
				rb2D.velocity = new Vector2(runSpeed * (isSprinting ? runMultiplyer : 1), rb2D.velocity.y);
				DoFlipX(false);
			} else if(intent.moveUpDown == Intent.DONT_USE) {
				anim.Play("Idle");
				isSprinting = false;
			}
		}
	}

	private void doJump() {
		if(!isHit) {
			anim.Play("Jump");
			isJumping = true;
			rb2D.velocity = new Vector2(rb2D.velocity.x, jumpHeight);
		}
	}

	public void KillChr() {
		isDead = true;
		intent.Clear();
		anim.SetBool("isDead", true);
		anim.Play("Launched");
	}

	public void FixedUpdate() {
		if(!isDead && !usingSpecial && !isLoading) {
			intent.Validate();
			if(intent.curCombo != (int)Intent.iCombo.None) {
				anim.SetInteger("Combo", intent.curCombo);
			}

			if(HitColliderWithWall(false) != Intent.DONT_USE)
				rb2D.velocity = new Vector2(0.0f, rb2D.velocity.y);

			if(CheckInAir()) {
				isBlocking = false;
				doAirMovement();
				doAirAttack();
				canMove = !isHit;
			} else {
				if(intent.useSpecial != Intent.DONT_USE) {
					if(intent.useSpecial == Intent.ATTACK_SPECIAL_1) {
						anim.Play("Special_1");
					} else if(intent.useSpecial == Intent.ATTACK_SPECIAL_2) {
						anim.Play("Special_2");
					} else if(intent.useSpecial == Intent.ATTACK_SPECIAL_3) {
						anim.Play("Special_3");
					}
					usingSpecial = true;
					intent.useSpecial = Intent.DONT_USE;
				} else {

					if(prevYVel < -0.001 && isJumping) {
						isJumping = false;
						if(currentState == "Launched") {
							anim.Play("KnockedOut");
							canMove = false;
						} else if(currentState != "KnockedOut") {
							anim.Play("Land");
							canMove = false;
						}
					}

					if(intent.doBlock) {
						if(canMove && !isAttacking) {
							anim.Play("Block");
							isBlocking = true;
						}
					} else {
						isBlocking = false;
						doGroundAttack();
						doGroundMovement();
					}

					if(intent.moveJump == Intent.MOVE_JUMP) {
						doJump();
					}

					if(!isJumping)
						jumpY = centerPosition.transform.position.y;

				}
			}

			prevYVel = rb2D.velocity.y;
			anim.SetBool("inAir", inAir);
		} else {
			if(!CheckInAir()) {
				if(currentState == "Launched") {
					anim.Play("KnockedOut");
				}
			}
		}
	}

	public void Hit(Collider2D col, bool hitYourself, Vector2 throwBackRange) {
		if(hitYourself || col.transform.parent.parent.name != cCol.name) {
			if(throwBackRange.x == 0 && throwBackRange.y == 0 && comboBreak < 3) {
				if(currentState != "KnockedOut" && currentState != "Launched") {
					if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
						comboBreak++;
					anim.Play("Hit");
					isHit = true;
				}
			} else {
				if(currentState != "KnockedOut" && currentState != "Launched") {
					anim.Play("Launched");
					isHit = true;
					comboBreak = 0;
					float flip = (col.transform.position.x - playerT.position.x) > 0 ? -1 : 1;
					rb2D.velocity = new Vector2(flip * throwBackRange.x, throwBackRange.y);
				}
			}
		}
	}

	private bool HitColliderWithCeiling() {
		RaycastHit2D upHit = Physics2D.Raycast(centerPosition.transform.position, Vector2.up, 0.2f, 1 << LayerMask.NameToLayer("Level"));
		if(upHit.collider != null && intent.moveUpDown == Intent.MOVE_UP) {
			intent.moveUpDown = Intent.DONT_USE;
			return true;
		}
		upHit = Physics2D.Raycast(blPosition.transform.position, Vector2.up, 0.2f, 1 << LayerMask.NameToLayer("Level"));
		if(upHit.collider != null && intent.moveUpDown == Intent.MOVE_UP) {
			intent.moveUpDown = Intent.DONT_USE;
			return true;
		}
		upHit = Physics2D.Raycast(brPosition.transform.position, Vector2.up, 0.2f, 1 << LayerMask.NameToLayer("Level"));
		if(upHit.collider != null && intent.moveUpDown == Intent.MOVE_UP) {
			intent.moveUpDown = Intent.DONT_USE;
			return true;
		}
		return false;
	}
	private bool HitColliderWithFloor() {
		RaycastHit2D upHit = Physics2D.Raycast(centerPosition.transform.position, Vector2.down, 0.2f, 1 << LayerMask.NameToLayer("Ground"));
		if(upHit.collider != null && intent.moveUpDown == Intent.MOVE_DOWN) {
			intent.moveUpDown = Intent.DONT_USE;
			return true;
		}
		upHit = Physics2D.Raycast(blPosition.transform.position, Vector2.down, 0.2f, 1 << LayerMask.NameToLayer("Ground"));
		if(upHit.collider != null && intent.moveUpDown == Intent.MOVE_DOWN) {
			intent.moveUpDown = Intent.DONT_USE;
			return true;
		}
		upHit = Physics2D.Raycast(brPosition.transform.position, Vector2.down, 0.2f, 1 << LayerMask.NameToLayer("Ground"));
		if(upHit.collider != null && intent.moveUpDown == Intent.MOVE_DOWN) {
			intent.moveUpDown = Intent.DONT_USE;
			return true;
		}
		return false;
	}
	private int HitColliderWithWall(bool ignoreMotion) {
		Vector3 newcenter = new Vector3(centerPosition.transform.position.x, jumpY);
		float scaleV = cCol.size.x / 2 * Mathf.Abs(playerT.localScale.x);
		RaycastHit2D hit = Physics2D.Raycast(newcenter, Vector2.right, scaleV, 1 << LayerMask.NameToLayer("Level"));
		
		if(hit.collider != null && (ignoreMotion || intent.moveLeftRight >= Intent.MOVE_RIGHT || rb2D.velocity.x >= 0.01f)) {
			intent.moveLeftRight = Intent.DONT_USE;
			return Intent.MOVE_RIGHT;
		}
		
		hit = Physics2D.Raycast(newcenter, Vector2.left, scaleV, 1 << LayerMask.NameToLayer("Level"));
		if(hit.collider != null && (ignoreMotion || intent.moveLeftRight <= Intent.MOVE_LEFT || rb2D.velocity.x <= -0.01f)) {
			intent.moveLeftRight = Intent.DONT_USE;
			return Intent.MOVE_LEFT;
		}
		return Intent.DONT_USE;
	}

	public void ResetFootEdge() {
		ScaleCharacter();
	}

	public void StateSwitch(AnimatorStateInfo info) {
		SetState(info);

		if(info.IsName("Idle") || info.IsName("Walk") || info.IsName("Run")) {
			anim.SetInteger("Combo", 0);
			intent.curCombo = 0;
			isAttacking = false;
			isHit = false;
			canMove = true;
			usingSpecial = false;
		} else {
			canMove = false;
		}
		if(currentState == "Quick" || currentState == "Heavy") {
			isAttacking = true;
		}else if(currentState == "Land") {
			anim.SetInteger("Combo", 0);
			intent.curCombo = 0;			
		}
	}

	public void SetState(AnimatorStateInfo info) {
		for(int i = 0; i < StateList.Length; i++) {
			if(info.IsName(StateList[i])) {
				currentState = StateList[i];
				return;
			}
		}
	}

	public void ResetCombo() {
		intent.curCombo = 0;
	}

	public void LoadCharacter() {
		anim.Play("LoadAvatar");
	}

	public void JumpAway() {
		isLoading = true;
		anim.SetBool("inAir", true);
		anim.Play("Jump");
		rb2D.velocity = new Vector2(-30, 60);
	}

}
