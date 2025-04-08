using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class DasherEnemySc : BaseEnemySc
{
	public ParticleSystem dashParticle;
	public ParticleSystem.EmissionModule dashEmission;
	public ParticleSystem dashCircleParticle;
	public ParticleSystem.EmissionModule dashCircleEmission;

	private Vector2 dashDir;
	private Vector3 targetPos;

	public float dashTime;
	public float dashForce;
	public float dashRange;
	public float maxDashCooldown;
	public float currentDashCooldown;
	public float dashFreezeTime;
	private float distanceToCharacter;

	private bool isDashing = false;
	public bool canMove = true;

	private void Start()
	{
		base.Start();

		dashEmission = dashParticle.emission;
		dashCircleEmission = dashCircleParticle.emission;
		dashEmission.rateOverTime = 0f;
		dashCircleEmission.rateOverTime = 0f;

		currentDashCooldown = maxDashCooldown;
	}

	private void Update()
	{
		base.Update();

		distanceToCharacter = Vector2.Distance(transform.position, targetCharacter.transform.position);
		//dashDir = targetCharacter.transform.position - transform.position;

		currentDashCooldown -= Time.deltaTime;
	}

	private void FixedUpdate()
	{
		base.FixedUpdate();

		EnemyMovement();
	}


	IEnumerator EnemyDash()
	{
		enemyAnim.SetTrigger("DashStart");

		yield return new WaitForSeconds(dashFreezeTime);

		isDashing = true;
		dashEmission.rateOverTime = 15f;
		dashCircleEmission.rateOverTime = 8f;

		dashDir = targetCharacter.transform.position - transform.position;
		enemyRb.velocity = new Vector2(dashDir.x, dashDir.y).normalized * dashForce;
		//dasherEnemyRb.AddForce(dashDir * dashForce);

		yield return new WaitForSeconds(dashTime);

		targetPos = PickUpTargetPosition();

		enemyRb.velocity = new Vector2(0f, 0f);

		canMove = true;
		isDashing = false;
		enemyAnim.SetTrigger("DashEnd");
	}

	public override void EnemyMovement()
	{
		if (currentDashCooldown <= 0)
		{
			// go to player and dash
			targetPos = targetCharacter.transform.position;

			if (distanceToCharacter < dashRange && isDashing == false)
			{
				canMove = false;
				// dash
				StartCoroutine(EnemyDash());

				currentDashCooldown = maxDashCooldown;
			}
		}
		else if (transform.position == targetPos)
		{
			// go random position
			targetPos = PickUpTargetPosition();
		}

		// move to target
		if (canMove)
		{
			enemyAnim.SetBool("isWalking", true);
			dashEmission.rateOverTime = 0f;
			dashCircleEmission.rateOverTime = 0f;
			transform.position = Vector2.MoveTowards(transform.position, targetPos, enemyMoveSpeed * Time.deltaTime);
		}

		// flipping the face to player
		if (transform.position.x > targetPos.x && isFaceRightEnemy == true)
		{
			if(!isDashing)
			{
				EnemyFlipFace();
			}
		}
		else if (transform.position.x < targetPos.x && isFaceRightEnemy == false)
		{
			if (!isDashing)
			{
				EnemyFlipFace();
			}
		}
	}

	Vector2 PickUpTargetPosition()
	{
		float randomX = Random.Range(-6, 14);
		float randomY = Random.Range(-18, 2);

		return new Vector2(randomX, randomY);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawLine(transform.position, targetPos);
		Gizmos.color = Color.green;
	}
}
