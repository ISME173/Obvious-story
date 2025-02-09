using UnityEngine;

public class BatRun : StateMachineBehaviour
{
    private Bat _bat;
    private float _time;
    private Transform _targetTransform;
    private Transform[] _movingPoints;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _bat = animator.GetComponent<Bat>();
        _time = 0;
        _movingPoints = _bat.GetMovingPoints();
        _targetTransform = _movingPoints[Random.Range(0, _movingPoints.Length)];
        _bat.NormalMovingSpeedActivate();
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_bat.RaycastToPlayer(_bat.EnemyData.AngryDistance))
            animator.SetBool(_bat.EnemyData.IsAngry, true);
        else
        {
            _bat.FlipEnemyToTarget(_targetTransform);
            _bat.Move(_targetTransform);

            if (Vector2.Distance(_bat.transform.position, _targetTransform.position) < 0.5f)
                _targetTransform = _movingPoints[Random.Range(0, _movingPoints.Length)];

            _time += Time.deltaTime;
            if (_time >= _bat.RunningTime)
                animator.SetBool(_bat.EnemyData.IsIdle, true);
        }
    }
}
