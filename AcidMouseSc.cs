using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Rendering;

public class AcidMouseSc : BaseEnemySc
{
	public GameObject acidLakeObj;

	private Vector3 targetPos;

	public int acidLakeDamage;

	public float acidLakeMaxCooldown;
	private float acidLakeCurrentCooldown;

	private void Start()
	{
		base.Start();

		acidLakeCurrentCooldown = acidLakeMaxCooldown;
	}

	private void Update()
	{
		base.Update();
	}

	private void FixedUpdate()
	{
		base.FixedUpdate();

		EnemyMovement();
		PutAcidLake();
	}

	void PutAcidLake()
	{
		acidLakeCurrentCooldown -= Time.deltaTime;

		if(acidLakeCurrentCooldown < 0 )
		{
			Instantiate(acidLakeObj, transform.position, Quaternion.identity);
			acidLakeCurrentCooldown = acidLakeMaxCooldown;
		}
	}

	public override void EnemyDie()
	{
		// put acid after died for last time
		Instantiate(acidLakeObj, transform.position, Quaternion.identity);

		Instantiate(deathBloodSplashObj, transform.position, Quaternion.identity);
		Instantiate(deathParticle, transform.position, Quaternion.identity);
		Instantiate(moneyDrop, transform.position, Quaternion.identity);

		Destroy(gameObject);
	}
	public override void EnemyMovement()
	{
		enemyAnim.SetBool("isWalking", true);

		if(transform.position == targetPos)
		{
			targetPos = PickUpTargetPosition();
		}

		transform.position = Vector2.MoveTowards(transform.position, targetPos, enemyMoveSpeed * Time.deltaTime);

		if(transform.position.x > targetPos.x && isFaceRightEnemy == true)
		{
			EnemyFlipFace();
		}
		else if (transform.position.x < targetPos.x && isFaceRightEnemy == false)
		{
			EnemyFlipFace();
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
