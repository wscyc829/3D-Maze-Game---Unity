using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
	public static GameController instance = null;

	public AudioSource game_over_sound;

	void Awake(){
		if (instance == null) {
			instance = this;
		} else if(instance != this) {
			Destroy (gameObject);
		}
	}

	void Update () {
	}

	public void GameOver(){
		game_over_sound.Play ();
		StartCoroutine (DelayGameOver ());
	}

	public void GameWin(){
		SceneManager.LoadScene ("game win");
	}

	IEnumerator DelayGameOver(){
		yield return new WaitForSeconds (1.05f);

		SceneManager.LoadScene ("game over");
	}

}
