using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	public int max_hp;

	private int hp;
	private Transform hp_bar;
	private Vector3 init_scale;

	void Start () {
		hp = max_hp;
		hp_bar = transform.FindChild ("HP Bar");
		init_scale = hp_bar.localScale;
	}

	void Update () {
		if (hp <= 0) {
			Destroy (gameObject);
		}
	}

	public void LoseHP(int lose){
		hp -= lose;

		float scale_x = init_scale.x * hp / max_hp;
		hp_bar.transform.localScale = new Vector3(scale_x, init_scale.y, init_scale.z);
	}


}
