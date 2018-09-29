using System;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

public class Character : MonoBehaviour
{
	public float speed;
	public Vector2 direction;
	
	private Rigidbody2D rb;
	private bool isXFirst;
	[SerializeField]
	private bool isRotating;
	private bool isMoving;
	
	[SerializeField]
	private float rotationValue = 0f;
	Vector3 rotation, current;
	float percent = 0;

	private GameObject objectToHold;
	private float timerToPickUp = 0f;
	
	// Use this for initialization
	private void Start()
	{
		objectToHold = null;
		isXFirst = false;
		isMoving = false;
		direction = Vector2.zero;
		rb = GetComponent<Rigidbody2D>();

		rotation = current = Vector3.zero;
	}
	
	// Update is called once per frame
	private void Update()
	{
		direction.Set(Input.GetAxisRaw("Vertical"),Input.GetAxisRaw("Vertical"));

		if ((int) direction.x == 0 && (int) direction.y == 0 || !isMoving)
		{
			Rotate();
			isXFirst = false;
			isMoving = false;
		}

		if (!isRotating)
		{
			isMoving = true;
			rb.MovePosition(rb.position + (direction * new Vector2(transform.up.x, transform.up.y) * speed * Time.deltaTime));
		}		
	}

	/// <summary>
	/// When we press Q or E it'll rotate right or left, if held down will rotate until they let go
	/// going to the next closest quadrant so if they left go at for say 56 degrees itll go to 90
	/// </summary>
	private void Rotate()
	{
		if (Input.GetKey(KeyCode.Q) && !isRotating)
		{
			isRotating = true;
			rotationValue += 90;
		}
		else if (Input.GetKey(KeyCode.E) && !isRotating)
		{
			isRotating = true;
			rotationValue -= 90;
		}
		
		if (!isRotating)
		{
			rotationValue = rb.rotation;
		}
		else
		{
			BeginRotating();
		}
	}

	/// <summary>
	/// Begins the rotation 
	/// </summary>
	private void BeginRotating()
	{
		Debug.Log("Begin the rotation");
		//Check to see if they are pretty much zero but floating point comparison needs to be fixed
		if (Math.Abs(rb.rotation - rotationValue) > 0.5)
		{
			current.Set(0, 0, rb.rotation);
			rotation.Set(0, 0, rotationValue);

			percent += (1f / 2400f); 

			current = Vector3.Slerp(current, rotation, percent);
			rb.MoveRotation(current.z);
		}
		else
		{
			rb.rotation = rotationValue;
			isRotating = false;
		}
	}

	private void PickUpItem(GameObject goHit)
	{
		if (objectToHold == null)
		{
			objectToHold = goHit;
			foreach (Transform child in transform)
			{
				objectToHold.transform.position = child.transform.position;
				objectToHold.transform.SetParent(child.transform);
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Button"))
		{
			print("Button");
			objectToHold.tag = "DroppedPickUp";
			objectToHold.transform.parent = null;
			objectToHold.transform.position = other.gameObject.transform.position;
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		Debug.Log("hit");
		if (other.CompareTag("PickUp"))
		{
			PickUpItem(other.gameObject);
		}
	}
}
