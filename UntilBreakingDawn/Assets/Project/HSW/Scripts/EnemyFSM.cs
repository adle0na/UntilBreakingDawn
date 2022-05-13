using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum EnemyState
{
    None = -1,
    Idle = 0,
    Wander,
    Pursuit,
}

public class EnemyFSM : MonoBehaviour
{
    [Header("Persuit")]
    [SerializeField] private float _targetRecognitionRange = 8;
    [SerializeField] private float _persuitLimitRange      = 10;

    private EnemyState      _enemyState = EnemyState.None;

    private Status          _status;
    private NavMeshAgent    _navMeshAgent;
    private Transform       _target;
    private EnemyMemoryPool _enemyMemoryPool;

    public void Setup(Transform target)
    {
        _status               = GetComponent<Status>();
        _navMeshAgent         = GetComponent<NavMeshAgent>();
        _target               = target;
        this._enemyMemoryPool = _enemyMemoryPool;

        _navMeshAgent.updateRotation = false;
    }

    private void OnEnable()
    {
        ChangeState(EnemyState.Idle);
    }

    private void OnDisable()
    {
        StopCoroutine(_enemyState.ToString());

        _enemyState = EnemyState.None;
    }

    public void ChangeState(EnemyState newState)
    {
        if (_enemyState == newState) return;

        StopCoroutine(_enemyState.ToString());
        _enemyState = newState;

        StartCoroutine(_enemyState.ToString());
    }

    private IEnumerator Idle()
    {
        StartCoroutine("AutoChangeFromIdleToWander");

        while (true)
        {
            DistanceCheck();

            yield return null;
        }
    }

    private IEnumerator AutoChangeFromIdleToWander()
    {
        int changeTime = Random.Range(1, 5);

        yield return new WaitForSeconds(changeTime);

        ChangeState(EnemyState.Wander);
    }

    private IEnumerator Wander()
    {
        float currentTime = 0;
        float maxTime = 10;

        _navMeshAgent.speed = _status.WalkSpeed;

        _navMeshAgent.SetDestination(CalculateWanderPosition());

        Vector3 to = new Vector3(_navMeshAgent.destination.x, 0, _navMeshAgent.destination.z);
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);
        transform.rotation = Quaternion.LookRotation(to - from);

        while (true)
        {
            currentTime += Time.deltaTime;

            to = new Vector3(_navMeshAgent.destination.x, 0, _navMeshAgent.destination.z);
            from = new Vector3(transform.position.x, 0, transform.position.z);
            if ((to - from).sqrMagnitude < 0.01f || currentTime >= maxTime)
            {
                ChangeState(EnemyState.Idle);
            }

            DistanceCheck();

            yield return null;
        }
    }

    private Vector3 CalculateWanderPosition()
    {
        float wanderRadius = 10;
        int wanderJitter = 0;
        int wanderJitterMin = 0;
        int wanderJitterMax = 360;

        Vector3 rangePosition = Vector3.zero;
        Vector3 rangeScale = Vector3.one * 100.0f;

        wanderJitter = Random.Range(wanderJitterMin, wanderJitterMax);
        Vector3 targetPosition = transform.position + SetAngle(wanderRadius, wanderJitter);

        targetPosition.x = Mathf.Clamp(targetPosition.x, rangePosition.x - rangeScale.x * 0.5f, rangePosition.x * 0.5f);
        targetPosition.y = 0;
        targetPosition.z = Mathf.Clamp(targetPosition.z, rangePosition.z - rangeScale.z * 0.5f, rangePosition.z * 0.5f);

        return targetPosition;
    }

    private Vector3 SetAngle(float radius, int angle)
    {
        Vector3 position = Vector3.zero;

        position.x = Mathf.Cos(angle) * radius;
        position.z = Mathf.Sin(angle) * radius;

        return position;
    }

    private IEnumerator Persuit()
    {
        while (true)
        {
            _navMeshAgent.speed = _status.RunSpeed;

            _navMeshAgent.SetDestination(_target.position);

            LookRotationToTarget();

            DistanceCheck();

            yield return null;
        }
    }

    private void LookRotationToTarget()
    {
        Vector3 to = new Vector3(_target.position.x, 0, _target.position.z);
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);

        transform.rotation = Quaternion.LookRotation(to - from);
    }

    private void DistanceCheck()
    {
        if (_target == null) return;

        float distance = Vector3.Distance(_target.position, transform.position);

        if (distance <= _targetRecognitionRange)
        {
            ChangeState(EnemyState.Pursuit);
        }
        else if (distance >= _persuitLimitRange)
        {
            ChangeState(EnemyState.Wander);
        }
    }

    public void TakeDamage(int damage)
    {
        bool isDie = status.DecreaseHP(damage);

        if (isDie == true)
        {
            EnemyMemoryPool.DeactivateEnemy(gameObject);
        }
    }
}