using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
		[SerializeField]
		GameObject[] characters;
		[SerializeField]
		Transform world;
		[SerializeField]
		int minTimeInterval;
		[SerializeField]
		int maxTimeInterval;
		[SerializeField]
		int maxSpawn;

	public int spawned;
	float timer;


	public void RemoveSpawned() {
		spawned--;
	}

	// Start is called before the first frame update
	void Start() {
		spawned = 1;
		foreach(GameObject chara in characters) {
			chara.layer = LayerMask.NameToLayer("Enemy");
			chara.GetComponent<Character>().isPlayer = false;
		}
	}

    // Update is called once per frame
    void Update()
    {
			if(timer > 0) { 
				timer -= Time.deltaTime;
			} else {
				if(spawned <= maxSpawn) {
					spawned++;
					GameObject created = Instantiate(characters[Random.Range(0, characters.Length)], transform.position, Quaternion.identity, world);
					created.GetComponent<Character>().spawner = this.gameObject;
					timer = Random.Range(minTimeInterval, maxTimeInterval);
				}
			}

		}
}
