﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : MonoBehaviour {

	//Components
	private GameManager gm;
	private Rigidbody rb;
	//private HealthPoints hp;
	
	//Settings
	[SerializeField] private float spd;
	[SerializeField] private float run_spd;
	[SerializeField] private float turn_spd;
	[SerializeField] private float back_spd;

	private bool can_move;
	
	//Variables
	private Vector2 velocity;
	
	
	//Init Player
	void Awake()
	{
		gameObject.tag = "Player";
	}
	
	void Start () {
		//Components
		gm = GameManager.instance;
		rb = gameObject.GetComponent<Rigidbody>();
		rb.constraints = RigidbodyConstraints.FreezeRotation;
		//hp = gameObject.GetComponent<HealthPoints>();
		
		//Settings
		can_move = true;

		//Variables
		velocity = Vector2.zero;
	}
	
	//Update Event
	void Update () {
		//Movement
		velocity = Vector2.zero;
		if (can_move)
		{
			//Variables
			float angle_spd = 0f;
			float move_spd = 0f;
			
			//Move forwards and backwards
			float walk_run_spd = spd;
			if (gm.getKey("run"))
			{
				walk_run_spd = run_spd;
			}

			if (gm.getKey("up"))
			{
				move_spd = walk_run_spd;
			}
			else if (gm.getKey("down"))
			{
				move_spd = -back_spd;
			}
			
			//Turning left and right
			if (gm.getKey("left"))
			{
				angle_spd = -turn_spd;
			}
			else if (gm.getKey("right"))
			{
				angle_spd = turn_spd;
			}
			
			transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + angle_spd, transform.eulerAngles.z);
			float facing_angle = (-transform.eulerAngles.y + 90) * Mathf.Deg2Rad;
			velocity = new Vector2(Mathf.Cos(facing_angle), Mathf.Sin(facing_angle)) * move_spd;
		}
	}

	//Physics
	void FixedUpdate()
	{
		if (velocity != Vector2.zero)
		{
			rb.MovePosition(transform.position + new Vector3(velocity.x, 0, velocity.y));
		}
	}

	void OnTriggerEnter(Collider collision)
	{
		//Check for Camera Trigger
		if (collision.gameObject.CompareTag("Finish"))
		{
			if (collision.GetComponent<CameraTrigger>() != null)
			{
				collision.GetComponent<CameraTrigger>().switchCamera();
			}
		}
	}

	//Getter & Setters
	public bool canmove
	{
		set
		{
			can_move = value;
		}
	}
	
}
