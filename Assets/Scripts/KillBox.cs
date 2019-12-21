using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
	[SerializeField]
	int damage;
	[SerializeField]
	GameObject hitSprite;
	[SerializeField]
	bool unBlockable;
	[SerializeField]
	float rangeZ = 5;
	[SerializeField]
	Vector2 throwBackRange;

	Character chr;
	Collider2D col;
	ContactFilter2D filter;
	bool hasHit;
    // Start is called before the first frame update
    void Start()
    {
			col = transform.GetComponent<Collider2D>();
			filter = new ContactFilter2D();
			hasHit = false;
      if(transform.parent.parent.gameObject.layer == LayerMask.NameToLayer("Player")) {
			filter.SetLayerMask(LayerMask.GetMask("Enemy"));
			} else {
			filter.SetLayerMask(LayerMask.GetMask("Player"));
			}
			chr = transform.parent.parent.GetComponent<Character>();
    }

		private void OnDisable() {
			hasHit = false;
		}

		// Update is called once per frame
		void Update()
		{
				if(!hasHit) {			
					List<Collider2D> colResults = new List<Collider2D>();
					int colCount =  col.OverlapCollider(filter, colResults);
					for(int i = 0; i < colCount; i++) {
						if(Mathf.Abs(colResults[i].transform.position.z - transform.position.z) <= rangeZ) {
							if(colResults[i].GetType()!=typeof(EdgeCollider2D) && colResults[i].GetComponent<Character>().Hit(col, unBlockable, damage, false, hitSprite, chr, throwBackRange)) {
								colResults[i].GetComponent<Character>().AddSpecial(30);
								transform.parent.parent.GetComponent<Character>().Hit(col, unBlockable, 0, true, hitSprite, null, throwBackRange);
							}
							hasHit = true;
						}
					}
				}
		}
	
	}
