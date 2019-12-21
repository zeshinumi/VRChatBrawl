using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNav : MonoBehaviour
{
    public void PlayGame() {
			SceneManager.LoadScene("HomeWorld");
		}

		public void QuitGame() {
			Application.Quit();
		}
}
