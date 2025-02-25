using UnityEngine;

public class SkeletonWalk : StateMachineBehaviour
{
    private Skeleton _skeleton;
    private float _time;
    private Transform _targetTransform;
    private Transform[] _movingPoints;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _skeleton = animator.GetComponent<Skeleton>();
        _time = 0;
        _movingPoints = _skeleton.GetMovingPoints();
        _targetTransform = _movingPoints[Random.Range(0, _movingPoints.Length)];
        _skeleton.NormalMovingSpeedActivate();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_skeleton.RaycastToPlayer(_skeleton.EnemyData.AngryDistance))
            animator.SetBool(_skeleton.EnemyData.IsAngry, true);
        else
        {
            _skeleton.FlipEnemyToTarget(_targetTransform);
            _skeleton.Move(_targetTransform);

            if (Vector2.Distance(_skeleton.transform.position, _targetTransform.position) < _skeleton.EnemyData.StoppingDistance)
                _targetTransform = _movingPoints[Random.Range(0, _movingPoints.Length)];

            _time += Time.deltaTime;
            if (_time >= _skeleton.RunningTime)
                animator.SetBool(_skeleton.EnemyData.IsIdle, true);
        }
    }
}
