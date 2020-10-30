using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private Animator anim;
	private CharacterController controller;
	public Camera camera;

	public float speed = 600.0f;
	public float turnSpeed = 400.0f;
	public float JUMP_SPEED = 8f;
	private Vector3 moveDirection = Vector3.zero;
	public float gravity = 10.0f;

	void Start() 
	{
		controller = GetComponent<CharacterController>();
		anim = gameObject.GetComponentInChildren<Animator>();
	}

	void Update()
	{
		Vector3 camForward = new Vector3(camera.transform.forward.x, 0 , camera.transform.forward.z);
		Vector3 camRight = new Vector3(camera.transform.right.x, 0, camera.transform.right.z);

		if (controller.isGrounded) // TODO: 下坡判斷會一下grounded一下不grounded
		{
			anim.SetBool("IsGrounded", true);
			// moveDirection = transform.forward * Input.GetAxis("Vertical") * speed;
			moveDirection = (camForward * Input.GetAxis("Vertical") + camRight * Input.GetAxis("Horizontal")).normalized * speed;
			

			if (Input.GetButton("Jump"))
			{
				moveDirection.y = JUMP_SPEED;
				anim.SetBool("IsGrounded", false);
			}
			else if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) // Character animation
			{
				anim.SetInteger("AnimationPar", 1);

				// Character rotation
				Quaternion rotation = Quaternion.LookRotation(moveDirection, Vector3.up);
				transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.1f);
			}  
			else 
			{
				anim.SetInteger("AnimationPar", 0);
			}
		}

		controller.Move(moveDirection * Time.deltaTime);
		moveDirection.y -= gravity * Time.deltaTime;
	}
}
