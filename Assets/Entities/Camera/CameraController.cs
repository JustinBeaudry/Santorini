using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

  // @TODO:  Use fixed positions - SEE https://github.com/JustinBeaudry/Santorini/issues/8

  public float speed = 5.0f;
  public GameObject target;

  public void MoveUp()
  {
    transform.LookAt(target.transform);
    transform.Translate(new Vector3(0, speed * Time.deltaTime, speed * Time.deltaTime));
  }

  public void MoveDown()
  {
    transform.LookAt(target.transform);
    transform.Translate(new Vector3(0, -speed * Time.deltaTime, -speed * Time.deltaTime));
  }

  public void MoveLeft()
  {
    transform.LookAt(target.transform);
    transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
  }

  public void MoveRight()
  {
    transform.LookAt(target.transform);
    transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
  }
}
