using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesEyes : Checker
{
    private Enemies enemy;
    private GameObject players;

    [SerializeField] private float attackCooldown;
    private float attackTimer;
    private bool isPlayerWarning = false;
    private void Start()
    {
        enemy = obj.GetComponent<Enemies>();
    }
    private void Update()
    {
        if(attackTimer >= 0 && enemy.IsNormal())
        {
            attackTimer -= Time.deltaTime;
        }
        if(isPlayerWarning && attackTimer <= 0)
        {
            Attack();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            players = other.gameObject;
            isPlayerWarning = true;
            Debug.Log("Catch player");
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (players != null)
            {
                //Do sthing
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerWarning = false;
        }
    }
    private void Attack()
    {
        if(!enemy.IsAttacking)
        {
            attackTimer = attackCooldown;
            enemy.IsAttacking = true;
            enemy.PlayAnim("Atk");
            StartCoroutine(EndAttack());
        }
    }
    private IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(0.5f);
        enemy.IsAttacking = false;
    }
    public void ResetAttack()
    {
        attackTimer = attackCooldown;
    }
}
