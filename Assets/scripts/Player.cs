using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
	bool isGrounded = false;
	bool isGameover = false;
	Rigidbody2D rb;
	SpriteRenderer sr;

	[SerializeField] float jumpForce = 100f;
	[SerializeField] float moveForce = 100f;

	[SerializeField] ParticleSystem bloodPS;
	[SerializeField] ParticleSystem heartPS;

	[SerializeField] Image lifeFill;

	float life = 1f;
	//life between 0.0 & 1.0   0=dead

	void Start ()
	{
		rb = GetComponent <Rigidbody2D> ();
		sr = GetComponent <SpriteRenderer> ();
	}

	void Update ()
	{
		if (!isGameover) {
			if (Input.GetKey (KeyCode.UpArrow) && isGrounded) {
				rb.AddRelativeForce (Vector2.up * jumpForce, ForceMode2D.Impulse);
				isGrounded = false;
			}
			//---------------------------------------------------------------
			if (Input.GetKey (KeyCode.RightArrow)) {
				rb.AddRelativeForce (Vector2.right * moveForce, ForceMode2D.Impulse);
				sr.flipX = false;
			}
			//---------------------------------------------------------------
			if (Input.GetKey (KeyCode.LeftArrow)) {
				rb.AddRelativeForce (Vector2.left * moveForce, ForceMode2D.Impulse);
				sr.flipX = true;
			}
		}
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		if (!isGameover) {
			if (other.collider.tag.Equals ("ground")) {
				rb.velocity = Vector2.zero;
				isGrounded = true;
				Debug.Log ("hit grounded");
			}
			//---------------------------------------------------------------
			if (other.collider.tag.Equals ("saw")) {
				Vector2 contact = other.contacts [0].point;
				bloodPS.transform.position = contact;
				bloodPS.Play ();
				Debug.Log ("hit saw");

				RemoveLife ();
			}
			//---------------------------------------------------------------
			if (other.collider.tag.Equals ("heart")) {
				heartPS.transform.position = other.transform.position;
				heartPS.Play ();
				Destroy (other.gameObject);
				Debug.Log ("hit heart");

				AddLife ();
			}
		}
	}


	void AddLife ()
	{
		//code to add more hearts goes here
		if (life < 1f) {
			life += 0.2f;
			lifeFill.fillAmount = life;
		}
	}

	void RemoveLife ()
	{
		//code to remove hearts goes here
		if (life > 0.2f) {
			life -= 0.2f;
			lifeFill.fillAmount = life;
		}

		if (life <= 0.2f) {
			isGameover = true;
			rb.AddTorque (50f);
			rb.AddRelativeForce (Vector2.up * 6f);
			rb.constraints = RigidbodyConstraints2D.None;
			rb.GetComponent <BoxCollider2D> ().enabled = false;
			Invoke ("Restart", 3f);
		}
	}

	void Restart ()
	{
		SceneManager.LoadScene (0);
	}
}
