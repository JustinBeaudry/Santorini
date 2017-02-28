using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

	public float speed = 5.0f;
	public GameObject target;

	public void MoveUp ()
	{
		transform.LookAt ( target.transform );
		transform.Translate ( new Vector3 ( 0, speed * Time.deltaTime, speed * Time.deltaTime ) );
	}

	public void MoveDown ()
	{
		transform.LookAt ( target.transform );
		transform.Translate ( new Vector3 ( 0, -speed * Time.deltaTime, -speed * Time.deltaTime ) );
	}

	public void MoveLeft ()
	{
		transform.LookAt ( target.transform );
		transform.Translate ( new Vector3 ( -speed * Time.deltaTime, 0, 0 ) );
	}

	public void MoveRight ()
	{
		transform.LookAt ( target.transform );
		transform.Translate ( new Vector3 ( speed * Time.deltaTime, 0, 0 ) );
	}

	//	void FixedUpdate ()
	//	{
	//		float x = transform.position.x;
	//		float y = Mathf.Clamp (transform.position.y, 5.2f, 5.35f);
	//		float z = transform.position.z;
	//		transform.position = new Vector3 (x, y, z);
	//	}
}
