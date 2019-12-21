using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileHandler : MonoBehaviour
{

	public const string P1_PREFABS_NAME = "P1_Prefabs";
	public const string P2_PREFABS_NAME = "P2_Prefabs";
	public const string WORLD_PREFABS_NAME = "World_Prefabs";
	public const string FR_PREFABS_NAME = "FriendRequest_Prefabs";
	public const string FRIENDS_PREFABS_NAME = "Friends_Prefabs";
	public const string CONTROLS = "Controls";

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void NewGame() {
		string worlds = "Pug";
		string players = "Zeshin";
		string fr = "Ryan:Scorpian";
		Save(P1_PREFABS_NAME, players);
		Save(WORLD_PREFABS_NAME, worlds);
		Save(FR_PREFABS_NAME, fr);
	}

	public string[] Load(string item, char splitChr) {
		if(PlayerPrefs.HasKey(item)) {
			return PlayerPrefs.GetString(item).Split(splitChr);
		} else {
			return null;
		}
	}

	public void Save(string item, string data) {
		PlayerPrefs.SetString(item, data);
	}

	public void AddToList(string from, string newItem) {
		string[] list = Load(from)

	}


	///////////// Controls ////////////
	public void SaveControls(List<KeyCode> keys) {
		Save(CONTROLS, KeysToString(keys));
	}

	public List<KeyCode> LoadControls() {
		List<KeyCode> keyList = new List<KeyCode>();
		string[] intentStrings = Load(FileHandler.CONTROLS, ',');
		if(intentStrings == null) {
			return CreateDefaultKeys();
		}
		for(int i = 0; i < intentStrings.Length; i++) {
			keyList.Add((KeyCode)System.Enum.Parse(typeof(KeyCode), intentStrings[i]));
		}
		return keyList;
	}

	public List<KeyCode> ResetKeys() {
		List<KeyCode> keys = CreateDefaultKeys();
		SaveControls(keys);
		return keys;
	}

	private string KeysToString(List<KeyCode> keyList) {
		return string.Join(",", keyList.ConvertAll<string>(i => i.ToString()).ToArray());
	}

	private List<KeyCode> CreateDefaultKeys() {
		List<KeyCode> keyList = new List<KeyCode>();
		keyList.Add(KeyCode.W);
		keyList.Add(KeyCode.A);
		keyList.Add(KeyCode.A);
		keyList.Add(KeyCode.None);
		keyList.Add(KeyCode.D);
		keyList.Add(KeyCode.D);
		keyList.Add(KeyCode.S);
		keyList.Add(KeyCode.Space);
		keyList.Add(KeyCode.Q);
		keyList.Add(KeyCode.E);
		keyList.Add(KeyCode.F);
		keyList.Add(KeyCode.R);
		keyList.Add(KeyCode.C);
		PlayerPrefs.SetString(CONTROLS, KeysToString(keyList));
		return keyList;
	}
}
