using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {
	public int type = 1;

	public GameObject bullet;
	public GameObject cross_red;
	public GameObject cross_blue;
	public Transform shot_point;
	public float force;

	public Transform holder;

	void Update () {
		if (Input.GetButtonDown("Fire1")) {
			if (type == 1) {
				GameObject instance = Instantiate (bullet, shot_point.position, transform.rotation) as GameObject;

				Rigidbody temp_rb = instance.GetComponent<Rigidbody> ();
				temp_rb.AddForce (transform.forward * force);

				instance.transform.SetParent (holder);
			} else if(type == 2){
				GameObject instance = Instantiate (cross_red, shot_point.position, transform.rotation) as GameObject;

				instance.transform.SetParent (holder);
			} else if(type == 3){
				GameObject instance = Instantiate (cross_blue, shot_point.position, transform.rotation) as GameObject;

				instance.transform.SetParent (holder);
			}
		}
	}
}
