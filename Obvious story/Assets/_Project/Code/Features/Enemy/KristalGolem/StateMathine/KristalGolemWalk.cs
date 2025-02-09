using UnityEngine;

public class KristalGolemWalk : StateMachineBehaviour
{
    private KristalGolem _kristalGolem;
    private float _time;
    private Transform _targetTransform;
    private Transform[] _movingPoints;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _kristalGolem = animator.GetComponent<KristalGolem>();
        _time = 0;
        _movingPoints = _kristalGolem.GetMovingPoints();
        _targetTransform = _movingPoints[Random.Range(0, _movingPoints.Length)];
        _kristalGolem.NormalMovingSpeedActivate();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_kristalGolem.RaycastToPlayer(_kristalGolem.EnemyData.AngryDistance))
            animator.SetBool(_kristalGolem.EnemyData.IsAngry, true);
        else
        {
            _kristalGolem.FlipEnemyToTarget(_targetTransform);
            _kristalGolem.Move(_targetTransform);

            if (Vector2.Distance(_kristalGolem.transform.position, _targetTransform.position) < _kristalGolem.EnemyData.StoppingDistance)
                _targetTransform = _movingPoints[Random.Range(0, _movingPoints.Length)];

            _time += Time.deltaTime;
            if (_time >= _kristalGolem.RunningTime)
                animator.SetBool(_kristalGolem.EnemyData.IsIdle, true);
        }
    }
}
