using System;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

public class Character : MonoBehaviour
{
	public float speed;
	public float rotationSpeed;
	public Vector2 direction;
	
	private Rigidbody2D rb;
	private bool isXFirst;
	[SerializeField]
	private bool isRotating;
	private bool isMoving;
	
	[SerializeField]
	private int rotationDirection = 0;
	[SerializeField]
	private float rotationValue = 0f;

	private GameObject objectToHold;
	
	// Use this for initialization
	private void Start()
	{
		objectToHold = null;
		isXFirst = false;
		isMoving = false;
		direction = Vector2.zero;
		rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	private void Update()
	{
//		if(transform.rotation.z == 0)
//			direction.Set(0,  transform.up.y);
//		if(transform.rotation.z == 90)
//			direction.Set(transform.position.left, 0);
		direction.Set(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
		
		if ((int)direction.x == 0 && (int)direction.y == 0 || !isMoving)
		{
			Rotate();
			isXFirst = false;
			isMoving = false;
		}
		else
		{
			if ((int)Math.Abs(direction.x) == 1 && (int)Math.Abs(direction.y) == 0)
			{
				isXFirst = true;
			}
			else if ((int)Math.Abs(direction.x) == 0 && (int)Math.Abs(direction.y) == 1)
			{
				isXFirst = false;
			}
			else if ((int)Math.Abs(direction.x) == 1 && (int)Math.Abs(direction.y) == 1)
			{
				if (isXFirst)
				{
					direction.y = 0;
				}
				else
				{
					direction.x = 0;
				}
			}
		}

		if (!isRotating)
		{
			isMoving = true;
			rb.MovePosition(rb.position + (direction * speed * Time.deltaTime));
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
			rotationDirection = 1;
			rotationValue += 90;
		}
		else if (Input.GetKey(KeyCode.E) && !isRotating)
		{
			isRotating = true;
			rotationDirection = -1;
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
			//rb.MoveRotation(rb.rotation + rotationSpeed * Time.deltaTime * rotationDirection);
			rb.angularVelocity = rotationSpeed * Time.deltaTime * rotationDirection;
		}
		else
		{
			rb.rotation = rotationValue;
			isRotating = false;
		}
	}

	private void PickUpItem(GameObject goHit)
	{
		if (Input.GetButtonDown("Fire1"))
		{
			objectToHold = goHit;
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.CompareTag("PickUp"))
		{
			PickUpItem(other.gameObject);
		}
	}
}
