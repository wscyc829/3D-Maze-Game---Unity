using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour {
	[System.Serializable]
	public class Bullet
	{
		public int type;
		public GameObject bullet;

		public Sprite bullet_image;

		public float max_bullet;
		public float cur_bullet;
		public float reload_time;

		public bool CanShoot(){
			if (cur_bullet > 0) {
				return true;
			}

			return false;
		}
	}

	public int cur_bullet_type = 1;
	public float bullet_force;

	public Image bullet_selected_image;

	public Transform shot_point;
	public Transform holder;

	public AudioSource shoot_sound;
	public AudioSource drop_sound;

	public Bullet bullet_circle;
	public Bullet bullet_cross_red;
	public Bullet bullet_cross_blue;

	public Text cur_bullet_num_text;

	void Start(){
		StartCoroutine (ReloadBulletCircle ());
		StartCoroutine (ReloadBulletCrossRed ());
		StartCoroutine (ReloadBulletCrossBlue ());
	}

	void Update () {
		if (Input.GetButtonDown("Fire1")) {
			
			if (cur_bullet_type == bullet_circle.type) {
				if(bullet_circle.CanShoot()){
					shoot_sound.Play ();

					bullet_circle.cur_bullet--;

					GameObject instance = Instantiate (bullet_circle.bullet, shot_point.position, transform.rotation) as GameObject;

					Rigidbody temp_rb = instance.GetComponent<Rigidbody> ();
					temp_rb.AddForce (transform.forward * bullet_force);

					instance.transform.SetParent (holder);
				}
			} else if(cur_bullet_type == bullet_cross_red.type){
				if(bullet_cross_red.CanShoot()){
							
					drop_sound.Play ();

					bullet_cross_red.cur_bullet--;

					GameObject instance = Instantiate (bullet_cross_red.bullet, shot_point.position, transform.rotation) as GameObject;

					instance.transform.SetParent (holder);
				}
				
			} else if(cur_bullet_type == bullet_cross_blue.type){
				if(bullet_cross_blue.CanShoot()){
					drop_sound.Play ();

					bullet_cross_blue.cur_bullet--;
					GameObject instance = Instantiate (bullet_cross_blue.bullet, shot_point.position, transform.rotation) as GameObject;

					instance.transform.SetParent (holder);	
				}
			}
		}

		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			cur_bullet_type = bullet_circle.type;
			bullet_selected_image.sprite = bullet_circle.bullet_image;
		} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			cur_bullet_type = bullet_cross_red.type;
			bullet_selected_image.sprite = bullet_cross_red.bullet_image;
		} else if (Input.GetKeyDown (KeyCode.Alpha3)) {
			cur_bullet_type = bullet_cross_blue.type;
			bullet_selected_image.sprite = bullet_cross_blue.bullet_image;
		}

		if (cur_bullet_type == bullet_circle.type) {
			cur_bullet_num_text.text = bullet_circle.cur_bullet.ToString();
		} else if(cur_bullet_type == bullet_cross_red.type){
			cur_bullet_num_text.text = bullet_cross_red.cur_bullet.ToString();
		} else if(cur_bullet_type == bullet_cross_blue.type){
			cur_bullet_num_text.text = bullet_cross_blue.cur_bullet.ToString();
		}
	}

	IEnumerator ReloadBulletCircle(){
		while (true) {
			yield return new WaitForSeconds (bullet_circle.reload_time);
			if (bullet_circle.cur_bullet < bullet_circle.max_bullet) {
				bullet_circle.cur_bullet++;
			}
		}
	}

	IEnumerator ReloadBulletCrossBlue(){
		while (true) {
			yield return new WaitForSeconds (bullet_cross_blue.reload_time);
			if (bullet_cross_blue.cur_bullet < bullet_cross_blue.max_bullet) {
				bullet_cross_blue.cur_bullet++;
			}
		}
	}

	IEnumerator ReloadBulletCrossRed(){
		while (true) {
			yield return new WaitForSeconds (bullet_cross_red.reload_time);
			if (bullet_cross_red.cur_bullet < bullet_cross_red.max_bullet) {
				bullet_cross_red.cur_bullet++;
			}
		}
	}
}
