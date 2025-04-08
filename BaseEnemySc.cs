using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BaseEnemySc : MonoBehaviour, IDamageable
{
	public SortingGroup EnemySortingGroup;
	private EnemySpawnerSc enemySpawner;

	public Rigidbody2D enemyRb;
	public Animator enemyAnim;

	private GameObject popUpTextManagerObj;
	public GameObject targetCharacter;
	public GameObject moneyDrop;
	public GameObject deathParticle;
	public GameObject deathBloodSplashObj;
	public GameObject damageBloodSplashObj;

	public Vector3 popTextOffset;

	public int enemyHealth;
	public int enemyDamage;
	public int waveIndex;

	public float enemyMoveSpeed;

	public bool isFaceRightEnemy = false;

	protected void Start()
	{
		targetCharacter = GameObject.FindGameObjectWithTag("Player");

		popUpTextManagerObj = GameObject.FindGameObjectWithTag("PopUpManager");

		enemySpawner = FindObjectOfType<EnemySpawnerSc>();

		waveIndex = enemySpawner.currentWaveCount + 1;
		enemyHealth += 2 * (waveIndex - 1);
		enemyDamage += Convert.ToInt32(0.6f * (waveIndex - 1));
	}

	protected void Update()
	{
		if(enemyHealth <= 0)
		{
			EnemyDie();
		}

		EnemiesLayer();
	}

	protected void FixedUpdate()
	{
		EnemyMovement();
		EnemyFlipFace();
	}

	public virtual void EnemyMovement()
	{
		// Empty! for now...
	}

	public void TakeDamage(int damage, bool isCritical)
	{
		enemyAnim.SetTrigger("HitTrigger");

		if (isCritical)
		{
			damage *= 2;
		}

		if (damage > enemyHealth && isCritical == false)
		{
			damage = enemyHealth;
		}
		enemyHealth -= damage;

		if (isCritical)
		{
			popUpTextManagerObj.GetComponent<PopUpText>().CreateText("", damage, gameObject.transform, popTextOffset, Color.yellow);
		}
		else
		{
			popUpTextManagerObj.GetComponent<PopUpText>().CreateText("", damage, gameObject.transform, popTextOffset, Color.white);
		}
		Instantiate(damageBloodSplashObj, transform.position, Quaternion.identity);
	}

	public virtual void EnemyDie()
	{
		Instantiate(deathBloodSplashObj, transform.position, Quaternion.identity);
		Instantiate(deathParticle, transform.position, Quaternion.identity);
		Instantiate(moneyDrop, transform.position, Quaternion.identity);

		Destroy(gameObject);
	}

	public void EnemiesLayer()
	{
		if (transform.position.y > targetCharacter.transform.position.y)
		{
			EnemySortingGroup.sortingOrder = targetCharacter.gameObject.GetComponent<SortingGroup>().sortingOrder - 1;
		}
		else
		{
			EnemySortingGroup.sortingOrder = targetCharacter.gameObject.GetComponent<SortingGroup>().sortingOrder + 1;
		}
	}

	public void EnemyFlipFace()
	{
		isFaceRightEnemy = !isFaceRightEnemy;
		transform.Rotate(0f, 180f, 0f);
	}
}
