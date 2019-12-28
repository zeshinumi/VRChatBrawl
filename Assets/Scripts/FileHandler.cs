﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public sealed class FileHandler : MonoBehaviour
{
	private static readonly FileHandler _instance = new FileHandler();

	public const string P1_PREFABS_NAME = "P1_Prefabs";
	public const string P2_PREFABS_NAME = "P2_Prefabs";
	public const string WORLD_PREFABS_NAME = "World_Prefabs";
	public const string FR_PREFABS_NAME = "FriendRequest_Prefabs";
	public const string FRIENDS_PREFABS_NAME = "Friends_Prefabs";
	public const string CONTROLS = "Controls";

	private List<string> _p1Prefabs;
	private List<string> _p2Prefabs;
	private List<string> _worldPrefabs;
	private List<string> _friendRequests;
	private List<string> _friends;
	private List<string> _controls;

	private FileHandler() {
		_p1Prefabs = Load(P1_PREFABS_NAME, ',');
		_p2Prefabs = Load(P2_PREFABS_NAME, ',');
		_worldPrefabs = Load(WORLD_PREFABS_NAME, ',');
		_friendRequests = Load(FR_PREFABS_NAME, ',');
		_friends = Load(FRIENDS_PREFABS_NAME, ',');
		_controls = Load(CONTROLS, ',');
	}

	public static FileHandler GetFileHandler() {
		return _instance;
	}

	public List<string> P1Prefabs {
		get {
			return _p1Prefabs;
		}
	}
	public List<string> P2Prefabs {
		get {
			return _p2Prefabs;
		}
	}
	public List<string> WorldPrefabs {
		get {
			return _worldPrefabs;
		}
	}
	public List<string> FriendRequests {
		get {
			return _friendRequests;
		}
	}
	public List<string> Friends {
		get {
			return _friends;
		}
	}
	public List<string> Controls {
		get {
			return _controls;
		}
	}

	public void AddToList(string type, string name) {
		switch(type) {
			case P1_PREFABS_NAME:
				_p1Prefabs.Add(name);
				break;
			case P2_PREFABS_NAME:
				_p2Prefabs.Add(name);
				break;
			case WORLD_PREFABS_NAME:
				_worldPrefabs.Add(name);
				break;
			case FR_PREFABS_NAME:
				_friendRequests.Add(name);
				break;
			case FRIENDS_PREFABS_NAME:
				_friends.Add(name);
				break;
			case CONTROLS:
				_controls.Add(name);
				break;
		}
	}

	/*public static FileHandler Instance() {
			get {
			return instance;
			}
		}
	*/

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

	public List<string> Load(string item, char splitChr) {
		if(PlayerPrefs.HasKey(item)) {
			string[] returnList = PlayerPrefs.GetString(item).Split(splitChr);
			List<string> stringList = new List<string>();
			foreach(string bit in returnList) {
				stringList.Add(bit);
			}
			return stringList;
		} else {
			return new List<string>();
		}
	}
	public void Save(string item, string data) {
		PlayerPrefs.SetString(item, data);
	}


	///////////// Controls ////////////
	public void SaveControls(List<KeyCode> keys) {
		Save(CONTROLS, KeysToString(keys));
	}

	public List<KeyCode> LoadControls() {
		List<KeyCode> keyList = new List<KeyCode>();
		string[] intentStrings = _controls.ToArray(); //Load(FileHandler.CONTROLS, ',');
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
