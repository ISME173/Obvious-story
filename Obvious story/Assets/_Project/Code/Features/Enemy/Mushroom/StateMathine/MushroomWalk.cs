using UnityEngine;

public class MushroomWalk : StateMachineBehaviour
{
    private Mushroom _mushroom;
    private float _time;
    private Transform _targetTransform;
    private Transform[] _movingPoints;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _mushroom = animator.GetComponent<Mushroom>();
        _time = 0;
        _movingPoints = _mushroom.GetMovingPoints();
        _targetTransform = _movingPoints[Random.Range(0, _movingPoints.Length)];
        _mushroom.NormalMovingSpeedActivate();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_mushroom.RaycastToPlayer(_mushroom.AngryDistance))
            animator.SetBool(_mushroom.IsAngry, true);
        else
        {
            _mushroom.FlipEnemyToTarget(_targetTransform);
            _mushroom.Move(_targetTransform);

            if (Vector2.Distance(_mushroom.transform.position, _targetTransform.position) < 0.5f)
                _targetTransform = _movingPoints[Random.Range(0, _movingPoints.Length)];

            _time += Time.deltaTime;
            if (_time >= _mushroom.RunningTime)
                animator.SetBool(_mushroom.IsIdle, true);
        }
    }
}