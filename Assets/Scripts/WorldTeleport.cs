using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTeleport : MonoBehaviour
{
	[SerializeField]
	GameObject teleportToWorld;
	[SerializeField]
	Transform teleportDropCamera;
	[SerializeField]
	Transform teleportDropPlayer;
	[SerializeField]
	Transform cam;

	Collider2D col;
	ContactFilter2D filter;
	// Start is called before the first frame update
	void Start()
    {
			col = transform.GetComponent<Collider2D>();
			filter = new ContactFilter2D();
			filter.SetLayerMask(LayerMask.GetMask("Player"));
		}

    // Update is called once per frame
    void Update()
    {
			List<Collider2D> colResults = new List<Collider2D>();
			int colCount = col.OverlapCollider(filter, colResults);
			if(colCount >= 1) {
				transform.GetComponent<SpriteRenderer>().enabled = true;
				if(Input.GetKey(KeyCode.F)) {
					transform.GetComponent<SpriteRenderer>().enabled = false;
					teleportToWorld.SetActive(true);
					colResults[0].transform.position = teleportDropPlayer.position;
				colResults[0].GetComponent<Character>().mob.ScaleCharacter();
					cam.position = teleportDropCamera.position;
					transform.parent.gameObject.SetActive(false);
				} 
			} else {
				transform.GetComponent<SpriteRenderer>().enabled = false;
			}
	}
}
