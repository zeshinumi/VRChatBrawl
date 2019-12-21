using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldSelect : MonoBehaviour {
	private float mousePrevX;
	private Vector3 prevPos;
	private List<GameObject> worlds;
	private List<GameObject> frs;
	private List<GameObject> friends;
	private string WORLD_PREFABS_NAME = "World_Prefabs";
	private string FR_PREFABS_NAME = "FriendRequest_Prefabs";
	private string FRIENDS_PREFABS_NAME = "Friends_Prefabs";

	private Transform worldT;
	private Transform frT;
	private Transform friendsT;

	// Start is called before the first frame update
	void Start() {
		string[] worldPrefabs = GetWorldPrefabs();
		string[] frPrefabs = GetFRPrefabs();
		string[] friendsPrefabs = GetFriendsprefabs();
		worldT = transform.Find("Worlds");
		frT = transform.Find("FR");
		friendsT = transform.Find("Friends");
		mousePrevX = Input.mousePosition.x;
		worlds = new List<GameObject>();
		frs = new List<GameObject>();
		friends = new List<GameObject>();
		Vector3 curPos = new Vector3(worldT.position.x-180, worldT.position.y, worldT.position.z);
		for(int i = 0; i < worldPrefabs.Length; i++) {
			worlds.Add(Instantiate(Resources.Load("Worlds/" + worldPrefabs[i]) as GameObject, curPos, Quaternion.identity, worldT));
			curPos = new Vector3(curPos.x + 130, curPos.y, curPos.z);
		}
		curPos = new Vector3(frT.position.x - 180, frT.position.y, frT.position.z);
		for(int i = 0; i < frPrefabs.Length; i++) {
			frs.Add(Instantiate(Resources.Load("ProfilePics/" + frPrefabs[i]) as GameObject, curPos, Quaternion.identity, frT));
			curPos = new Vector3(curPos.x + 130, curPos.y, curPos.z);
		}
		curPos = new Vector3(friendsT.position.x - 180, friendsT.position.y, friendsT.position.z);
		for(int i = 0; i < friendsPrefabs.Length; i++) {
			friends.Add(Instantiate(Resources.Load("ProfilePics_NF/" + friendsPrefabs[i]) as GameObject, curPos, Quaternion.identity, friendsT));
			curPos = new Vector3(curPos.x + 130, curPos.y, curPos.z);
		}
	}

  // Update is called once per frame
  void Update()
  {
		if(Input.GetMouseButtonDown(0)) {
			mousePrevX = Input.mousePosition.x;
			if(Input.mousePosition.y > worldT.position.y - 45 && Input.mousePosition.y < worldT.position.y + 45) {
				prevPos = worldT.position;
			}else if(Input.mousePosition.y > frT.position.y - 45 && Input.mousePosition.y < frT.position.y + 45) {
				prevPos = frT.position;
			} else if(Input.mousePosition.y > friendsT.position.y - 45 && Input.mousePosition.y < friendsT.position.y + 45) {
				prevPos = friendsT.position;
			}
		}
		if(Input.GetMouseButton(0)) {
			if(Input.mousePosition.y > worldT.position.y - 45 && Input.mousePosition.y < worldT.position.y + 45) {
				worldT.position = new Vector3(prevPos.x + (Input.mousePosition.x - mousePrevX), prevPos.y, prevPos.z);
			}else if(Input.mousePosition.y > frT.position.y - 45 && Input.mousePosition.y < frT.position.y + 45) {
				frT.position = new Vector3(prevPos.x + (Input.mousePosition.x - mousePrevX), prevPos.y, prevPos.z);
			} else if(Input.mousePosition.y > friendsT.position.y - 45 && Input.mousePosition.y < friendsT.position.y + 45) {
				friendsT.position = new Vector3(prevPos.x + (Input.mousePosition.x - mousePrevX), prevPos.y, prevPos.z);
			}
		}
  }

	public void LoadScene(string scene) {
		SceneManager.LoadScene(scene);
	}

	private string[] GetWorldPrefabs() {
		if(PlayerPrefs.HasKey(WORLD_PREFABS_NAME)) {
			return PlayerPrefs.GetString(WORLD_PREFABS_NAME).Split(':');
		} else {
			return new string[3] { "Pug", "Pug", "Pug" };
		}
	}

	private string[] GetFRPrefabs() {
		if(PlayerPrefs.HasKey(FR_PREFABS_NAME)) {
			return PlayerPrefs.GetString(FR_PREFABS_NAME).Split(':');
		} else {
			return new string[3] { "Zeshin", "Ryan", "Scorpion" };
		}
	}

	private string[] GetFriendsprefabs() {
		if(PlayerPrefs.HasKey(FRIENDS_PREFABS_NAME)) {
			return PlayerPrefs.GetString(FRIENDS_PREFABS_NAME).Split(':');
		} else {
			return new string[3] { "Zeshin", "Ryan", "Scorpion" };
		}
	}
}
