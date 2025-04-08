using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class RangeEnemySc : BaseEnemySc
{
	public GameObject rangeEnemysGun;

	public float maxDistanceToCharacter; // this,
	private float distanceToCharacter;	//	and this is gonna be use for movement

	private void Start()
	{
		base.Start();
		rangeEnemysGun.GetComponent<PistolGunSc>().gunDamage += Convert.ToInt32(0.6f * (waveIndex - 1));
	}

	private void FixedUpdate()
	{
		base.FixedUpdate();
		EnemyMovement();
	}

	private void Update()
	{
		base.Update();
		distanceToCharacter = Vector2.Distance(transform.position, targetCharacter.transform.position);
	}

	public override void EnemyMovement()
	{
		base.EnemyMovement();

		if (distanceToCharacter > maxDistanceToCharacter)
		{
			transform.position = Vector2.MoveTowards(transform.position, targetCharacter.transform.position, enemyMoveSpeed * Time.deltaTime);
		}
		else if (distanceToCharacter < maxDistanceToCharacter - 1f)
		{
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
}
