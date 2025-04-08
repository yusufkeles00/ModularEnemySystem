using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CaseHolderEnemySc : BaseEnemySc
{
	public GameObject caseObjectDrop;

	public float maxDistanceToCharacter;
	private float distanceToCharacter;

	private void Start()
	{
		base.Start();
		//Invoke("EnemyDie", 4f);
	}

	private void Update()
	{
		base.Update();

		distanceToCharacter = Vector2.Distance(transform.position, targetCharacter.transform.position);
	}

	private void FixedUpdate()
	{
		base.FixedUpdate();

		EnemyMovement();
	}

	public override void EnemyMovement()
	{
		base.EnemyMovement();

		if (distanceToCharacter >= maxDistanceToCharacter)
		{
			transform.position = Vector2.MoveTowards(transform.position, targetCharacter.transform.position, enemyMoveSpeed * Time.deltaTime);
		}
		else if (distanceToCharacter < maxDistanceToCharacter)
		{
			//Move to further position from player
			transform.position = Vector2.MoveTowards(transform.position, targetCharacter.transform.position, -1 * enemyMoveSpeed * Time.deltaTime);
		}

		if (transform.position.x > targetCharacter.transform.position.x && isFaceRightEnemy == true)
		{
			EnemyFlipFace();
		}
		else if (transform.position.x < targetCharacter.transform.position.x && isFaceRightEnemy == false)
		{
			EnemyFlipFace();
		}

		if (distanceToCharacter > maxDistanceToCharacter)
		{
			enemyAnim.SetBool("isWalking", true);
		}
		else
		{
			enemyAnim.SetBool("isWalking", false);
		}
	}

	public override void EnemyDie()
	{
		Instantiate(deathBloodSplashObj, transform.position, Quaternion.identity);
		Instantiate(deathParticle, transform.position, Quaternion.identity);
		Instantiate(moneyDrop, transform.position, Quaternion.identity);
		Instantiate(caseObjectDrop, transform.position, Quaternion.identity);

		Destroy(gameObject);
	}
}
