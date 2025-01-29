using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    [SerializeField] private Transform _follow;
    [SerializeField] private float _speed;
    [Space]
    [SerializeField] private float _maxPositionY;
    [SerializeField] private float _minPositionY;
    [Space]
    [SerializeField] private float _maxPositionX;
    [SerializeField] private float _minPositionX;
    private void Awake()
    {
        if (_follow == null)
        {
            _follow = FindAnyObjectByType<PlayerMoving>().transform;
        }
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _follow.position, _speed * Time.deltaTime);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, _minPositionX, _maxPositionX),
            Mathf.Clamp(transform.position.y, _minPositionY, _maxPositionY), -1);
    }
}
