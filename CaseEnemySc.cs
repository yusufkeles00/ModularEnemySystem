using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class CaseEnemySc : BaseEnemySc
{
	public GameObject bulletRedObj;
	public GameObject bulletRedGameObj;
	public GameObject caseEnemyDeathParticle;

	public Transform shotPos;

	private Vector3 targetPosition;

	public float caseEnemyMaxCooldown;
	private float caseEnemyCurrentCooldown;

	private void Start()
	{
		base.Start();

		caseEnemyCurrentCooldown = caseEnemyMaxCooldown;
		PickUpTargetPosition();
	}

	private void Update()
	{
		base.Update();

		if (transform.position == targetPosition)
		{
			PickUpTargetPosition();
		}

		GunShot();

		caseEnemyCurrentCooldown -= Time.deltaTime;
	}

	private void FixedUpdate()
	{
		base.FixedUpdate();

		transform.position = Vector2.MoveTowards(transform.position, targetPosition, enemyMoveSpeed * Time.deltaTime);
	}

	void GunShot()
	{
		if (caseEnemyCurrentCooldown <= 0)
		{
			bulletRedGameObj = Instantiate(bulletRedObj, shotPos.position, Quaternion.identity);
			//bulletRedGameObj.GetComponent<BasicBulletSc>().gunGameObj = gameObject;

			caseEnemyCurrentCooldown = caseEnemyMaxCooldown;
		}
	}

	public override void EnemyDie()
	{
		Instantiate(deathBloodSplashObj, transform.position, Quaternion.identity);
		Instantiate(deathParticle, transform.position, Quaternion.identity);
		Instantiate(moneyDrop, transform.position, Quaternion.identity);

		Instantiate(caseEnemyDeathParticle, transform.position, Quaternion.identity);

		Destroy(gameObject);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawLine(transform.position, targetPosition);
		Gizmos.color = Color.green;
	}

	void PickUpTargetPosition()
	{
		float randomX = Random.Range(-6, 14);
		float randomY = Random.Range(-18, 2);

		targetPosition = new Vector2(randomX, randomY);
	}
}
