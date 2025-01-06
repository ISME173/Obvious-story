using UnityEngine;
using Zenject;

public class ParallaxObject : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float _parallaxStrenght;
    [SerializeField] private bool _disableVerticalParallax;

    [Inject] private PlayerMoving _playerMoving;
    private Vector3 _targetPreviousPosition;

    private void Start() => _targetPreviousPosition = _playerMoving.transform.position;
    private void LateUpdate() => ParallaxCalculate();

    private void ParallaxCalculate()
    {
        Vector3 delta = _playerMoving.transform.position - _targetPreviousPosition;

        if (_disableVerticalParallax)
            delta.y = 0;

        _targetPreviousPosition = _playerMoving.transform.position;

        transform.position += delta * _parallaxStrenght; 
    }
}
