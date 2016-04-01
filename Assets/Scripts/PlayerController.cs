using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float falling_distance;

	void Update(){
		if(transform.position.y < falling_distance){
			GameController.instance.GameOver ();
		}
	}

	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.CompareTag ("Enemy")) {
			GameController.instance.GameOver ();
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag ("Goal")){
			GameController.instance.GameWin ();
		}
	}
}
