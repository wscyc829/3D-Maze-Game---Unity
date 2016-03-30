using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	void Update(){
		
	}
	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.CompareTag ("PickUp")) {
			Destroy (collision.gameObject);
		}
	}

	void LoseHP(int lose){
	
	}

	void DoBlinking(){
		
	}
}
