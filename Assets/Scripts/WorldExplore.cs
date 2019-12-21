using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldExplore : MonoBehaviour
{
		[SerializeField]
		BoxCollider2D wcLeft;
		[SerializeField]
		BoxCollider2D wcRight;
		[SerializeField]
		Transform camera;

		[SerializeField]
		float left_edge;
		[SerializeField]
		float right_edge;

		ContactFilter2D filter;

	// Start is called before the first frame update
	void Start()
    {
			filter = new ContactFilter2D();
			filter.SetLayerMask(LayerMask.GetMask("Player"));
		}

    // Update is called once per frame
    void Update()
    {
			List<Collider2D> colResults = new List<Collider2D>();
			int colCount = wcLeft.OverlapCollider(filter, colResults);
			for(int i = 0; i < colCount; i++) {
				TranslateLevel(colResults[i].transform.GetComponent<Character>(), new Vector3(camera.position.x <= left_edge ? 0 : -0.1f, 0, 0));
			}
			colCount = wcRight.OverlapCollider(filter, colResults);
			for(int i = 0; i < colCount; i++) {
				TranslateLevel(colResults[i].transform.GetComponent<Character>(), new Vector3(camera.position.x >= right_edge ? 0 : 0.1f, 0, 0));
			}

		}
	
		private void TranslateLevel(Character chr, Vector3 move) {
		if(chr.mob.intent.moveLeftRight != Intent.DONT_USE && chr.mob.canMove) {
				camera.position += move;
			}
		}
}
