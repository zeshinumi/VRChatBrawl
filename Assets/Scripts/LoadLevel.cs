using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevel : MonoBehaviour
{
	[SerializeField]
	GameObject MainCamera;

	private GameObject[] activeChrs;
	private FileHandler files;

	//private string P1_PREFABS_NAME = "P1_Prefabs";

  // Start is called before the first frame update
  void Awake()
  {
		string[] chrsPrefabs = files.Load(FileHandler.P1_PREFABS_NAME, ':');
		ProfileHud p1_hud = MainCamera.GetComponentInChildren<ProfileHud>();
		p1_hud.Start();
		for(int i = 0; i < chrsPrefabs.Length; i++) {			
			GameObject newChar = Instantiate(Resources.Load("Characters/" + chrsPrefabs[i]) as GameObject, transform.position, Quaternion.identity);
			newChar.GetComponent<Character>().isPlayer = true;
			newChar.GetComponent<Character>().SetButtonSequence();
			newChar.layer = LayerMask.NameToLayer("Player");
			newChar.name = newChar.name.Replace("(Clone)", "");

			if(i == 0) {
				newChar.transform.position = transform.position;
				newChar.SetActive(true);
			} else {
				newChar.SetActive(false);
			}
			p1_hud.characters[i] = newChar;
		}
		p1_hud.SetAllChrProf();
				
  }


		// Update is called once per frame
		void Update()
    {
        
    }
}
