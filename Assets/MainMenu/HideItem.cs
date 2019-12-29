using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HideItem : MonoBehaviour
{
	private Image img;
	private Image child;
	private WorldSelect world;
	public bool isSelected;
    // Start is called before the first frame update
  void Start()
  {
		img = gameObject.GetComponent<Image>();
		world = transform.GetComponentInParent<WorldSelect>();
		Transform FR = transform.Find("FR");
		child = FR != null ? FR.GetComponent<Image>() : null;
		isSelected = FileHandler.GetFileHandler().P1Prefabs.Contains(gameObject.name.Replace("(Clone)",""));
		SetSelectedColor();
	}

    // Update is called once per frame
  void Update()
  {
    if(transform.position.x < 105 || transform.position.x > 615) {
			img.enabled = false;
			if(child != null)
				child.enabled = false;
		} else {
			img.enabled = true;
			if(child != null)
				child.enabled = true;
		}
  }

	private void SetSelectedColor() {
		if(isSelected) {
			img.color = Color.green;
		} else {
			img.color = Color.white;
		}
	}

	public void LoadScene(string scene) {
		SceneManager.LoadScene(scene);
	}

	public void AddFriend(string friend) {
		FileHandler.GetFileHandler().AlterList(true, FileHandler.FRIENDS_PREFABS_NAME, friend);
		FileHandler.GetFileHandler().AlterList(false, FileHandler.FR_PREFABS_NAME, friend);
	}

	public void SelectFriend(string friend) {
		List<string> selected = FileHandler.GetFileHandler().P1Prefabs;
		if(!isSelected) {
			if(selected.Count == 3) {
				FileHandler.GetFileHandler().AlterList(false, FileHandler.P1_PREFABS_NAME, selected[0]);
			}
			FileHandler.GetFileHandler().AlterList(true, FileHandler.P1_PREFABS_NAME, friend);
			isSelected = true;			

		} else {
			FileHandler.GetFileHandler().AlterList(false, FileHandler.P1_PREFABS_NAME, friend);
			isSelected = false;
		}
		SetSelectedColor();
	}
}
