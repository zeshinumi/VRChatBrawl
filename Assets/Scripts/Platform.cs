using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
	[SerializeField]
	bool isMoving;

	private void OnCollisionEnter2D(Collision2D collision) {
		GameObject colObj = collision.gameObject;
		if((colObj.layer == LayerMask.NameToLayer("Player") ||
			colObj.layer == LayerMask.NameToLayer("Enemy")) &&
			collision.collider.bounds.min.y >= transform.position.y) {
			collision.transform.GetComponent<Character>().SetGrounded(true);
				if(isMoving) { colObj.transform.parent = transform; }
		}
		
	}

	private void OnCollisionExit2D(Collision2D collision) {
		GameObject colObj = collision.gameObject;
		if(colObj.layer == LayerMask.NameToLayer("Player") ||
			colObj.layer == LayerMask.NameToLayer("Enemy")) {
				collision.transform.GetComponent<Character>().SetGrounded(false);
				if(isMoving) { colObj.transform.parent = null; }
		}
	}
}
