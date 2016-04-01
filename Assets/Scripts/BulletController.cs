using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {
	public int damage;
	public float speed;
	public float destroy_time;

	private Rigidbody rb;

	void Start () {
		Invoke ("DestroyByTime", destroy_time);
	}

	void Update(){
		RaycastHit[] hit = Physics.RaycastAll (transform.position, transform.forward, 0.5f);

		if (hit.Length > 0) {
			for (int i = 0; i < hit.Length; i++) {
				if (hit [i].transform.CompareTag ("Enemy")) {
					EnemyController ec = hit [i].transform.GetComponent<EnemyController> ();
					ec.LoseHP (damage);
					Destroy (gameObject);
				}
			}
		}
	}

	void DestroyByTime(){
		Destroy (gameObject);
	}

}
