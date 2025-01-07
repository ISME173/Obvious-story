using UnityEngine;

public class ParallaxObject : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float _parallaxStrenght;
    [SerializeField] private bool _disableVerticalParallax;
    [SerializeField] private Transform _followTarget;

    private Vector3 _targetPreviousPosition;

    private void Start()
    {
        if (_followTarget == null)
            _followTarget = Camera.main.transform;
        _targetPreviousPosition = _followTarget.position;
    }
    private void LateUpdate() => ParallaxCalculate();

    private void ParallaxCalculate()
    {
        Vector3 delta = _followTarget.transform.position - _targetPreviousPosition;

        if (_disableVerticalParallax)
            delta.y = 0;

        _targetPreviousPosition = _followTarget.transform.position;

        transform.position += delta * _parallaxStrenght;
    }
}
