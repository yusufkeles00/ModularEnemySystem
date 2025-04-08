using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BasicEnemySc : BaseEnemySc
{
	[SerializeField] AudioSource punchAttackSound;

	private Vector2 enemyMoveDirection;

	private float distanceToCharacter;
	private float hitCooldown;
	public float maxDistanceToCharacter;
	public float maxHitCooldown;

	private void Start()
	{
		base.Start();
		hitCooldown = maxHitCooldown;
	}

	private void Update()
	{
		base.Update();
		enemyMoveDirection = targetCharacter.transform.position - transform.position;
		distanceToCharacter = Vector2.Distance(transform.position, targetCharacter.transform.position);

		hitCooldown -= Time.deltaTime;

		EnemyAttack();
	}

	private void FixedUpdate()
	{
		base.FixedUpdate();
		EnemyMovement();
	}

	void EnemyAttack()
	{
		if (hitCooldown <= 0 && distanceToCharacter <= maxDistanceToCharacter)
		{
			enemyAnim.SetTrigger("Attack");
			punchAttackSound.Play();
			targetCharacter.GetComponent<CharacterMovementScript>().CharacterTakeDamage(enemyDamage);
			hitCooldown = maxHitCooldown;
		}
	}

	public override void EnemyMovement()
	{
		base.EnemyMovement();

		if (distanceToCharacter >= maxDistanceToCharacter)
		{
			transform.position = Vector2.MoveTowards(transform.position, targetCharacter.transform.position, enemyMoveSpeed * Time.deltaTime);
		}

		if (distanceToCharacter > maxDistanceToCharacter)
		{
			enemyAnim.SetBool("isWalking", true);
		}
		else
		{
			enemyAnim.SetBool("isWalking", false);
		}

		if (transform.position.x > targetCharacter.transform.position.x && isFaceRightEnemy == true)
		{
			EnemyFlipFace();
		}
		else if (transform.position.x < targetCharacter.transform.position.x && isFaceRightEnemy == false)
		{
			EnemyFlipFace();
		}
	}
}
