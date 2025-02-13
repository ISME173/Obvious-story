using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthView : MonoBehaviour
{
    [Header("View states")]
    [SerializeField] private PlayerHealthManager _playerHealthManager;
    [SerializeField] private List<HeartUI> _heartUIList;
    [SerializeField, Min(0)] private float _distanceBetweenTwoHearts;

    [Header("Object pool states")]
    [SerializeField] private HeartUI _heartUIPrefab;
    [SerializeField, Min(0)] private int _numberOfReserveHearts;

    private ObjectPool<HeartUI> _heartUIPool;

    private void Awake()
    {
        _heartUIPool = new ObjectPool<HeartUI>(transform, _heartUIPrefab);

        for (int i = 0; i < _numberOfReserveHearts; i++)
            _heartUIPool.Put(Instantiate(_heartUIPrefab));

        if (_playerHealthManager == null)
            _playerHealthManager = FindAnyObjectByType<PlayerHealthManager>();

        if (_heartUIList == null)
        {
            HeartUI[] heartUIs = GetComponentsInChildren<HeartUI>();
            for (int i = 0; i < heartUIs.Length; i++)
            {
                heartUIs[i].gameObject.SetActive(false);
                _heartUIList.Add(heartUIs[i]);
            }
        }
        else
        {
            for (int i = 0; i < _heartUIList.Count; i++)
                _heartUIList[i].gameObject.SetActive(false);
        }

        GameEvents.Instance.OnPlayReadOnly.AddListener(DrawHearts);
    }

    private void OnEnable()
    {
        _playerHealthManager.DamageTaken += DrawHeartsBeforePlayerTakeDamage;
        _playerHealthManager.HealingHearts += DrawHeartsBeforeHealing;

        SceneLoaoder.Instance.OnRestartLevelLoadedReadOnly.AddListener(DrawHearts);
        SceneLoaoder.Instance.OnNextLevelLoaodedReadOnly.AddListener(DrawHearts);
        GameEvents.Instance.OnPlayReadOnly.AddListener(DrawHearts);
    }
    private void OnDisable()
    {
        _playerHealthManager.DamageTaken -= DrawHeartsBeforePlayerTakeDamage;
        _playerHealthManager.HealingHearts -= DrawHeartsBeforeHealing;
    }

    private void DrawHearts()
    {
        gameObject.SetActive(true);

        for (int i = 0; i < _heartUIList.Count; i++)
            _heartUIList[i].Enable();
    }

    private void DrawHeartsBeforeHealing(int heartsCount)
    {
        if (heartsCount > _heartUIList.Count)
        {
            int addedHeartsCount = heartsCount - _heartUIList.Count;
            for (int i = 0; i < addedHeartsCount; i++)
            {
                HeartUI heartUI = _heartUIPool.Get();
                heartUI.transform.SetParent(transform);
                heartUI.transform.localPosition = new Vector3(_heartUIList.Last().transform.localPosition.x + _distanceBetweenTwoHearts, 0, 0);
                heartUI.transform.localScale = new Vector3(1, 1, 1);
                _heartUIList.Add(heartUI);
                heartUI.Enable();
            }
        }
    }
    private void DrawHeartsBeforePlayerTakeDamage(int damage)
    {
        for (int i = 0; i < damage; i++)
        {
            if (_heartUIList.Count == 0)
                return;

            HeartUI heartUI = _heartUIList.Last();
            float secondsForPutToPool = heartUI.Disable();
            StartCoroutine(TimeToPutToPool(secondsForPutToPool, heartUI));
            _heartUIList.Remove(heartUI);
        }
    }
    private IEnumerator TimeToPutToPool(float seconds, HeartUI objectToPutPool)
    {
        yield return new WaitForSeconds(seconds);
        _heartUIPool.Put(objectToPutPool);
    }
}
