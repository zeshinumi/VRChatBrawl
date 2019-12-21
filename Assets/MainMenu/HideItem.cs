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
    // Start is called before the first frame update
  void Start()
  {
		img = gameObject.GetComponent<Image>();
		world = transform.GetComponentInParent<WorldSelect>();
		Transform FR = transform.Find("FR");
		child = FR != null ? FR.GetComponent<Image>() : null;
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

	public void LoadScene(string scene) {
		SceneManager.LoadScene(scene);
	}

	public void AddFriend(string friend) {
		
	}

	public void SelectFriend(string friend) {

	}
}
