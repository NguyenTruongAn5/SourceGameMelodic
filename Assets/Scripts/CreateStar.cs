using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateStar : MonoBehaviour
{
    [SerializeField] GameObject _star_1_Shadow;
    [SerializeField] GameObject _star_2_Shadow;
    [SerializeField] GameObject _star_3_Shadow;
    [SerializeField] GameObject _star;
    [SerializeField] Transform _land_1;
    [SerializeField] Transform _land_2;
    public static CreateStar Instance { get; private set; }
    public int _currentBeat = 0;
    public int _totalNote = 0;
    public bool _activeStar = false;
    public bool _activeStar1 = true;
    public bool _activeStar2 = true;
    public bool _activeStar3 = true;
    public int count = 0;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        Instance = this;
    }
    private void Update()
    {
        CreateStarCurrent();
        CheckShowStar();
        ShowStar();
    }
    public void SetCurrentBeat(int currentBeat)
    {
        _currentBeat = currentBeat;
    }
    public void SetTotalBeat(int total)
    {
        _totalNote = total;
    }
    public void SetActiveStar(bool active)
    {
        _activeStar = active;
    }
    private void CreateStarCurrent()
    {
        if (_totalNote / 3 == _currentBeat)
        {
            if (_activeStar1)
            {
                Instantiate(_star, _land_1);
                _activeStar1 = false;
            }
        }
        if (_totalNote / 2 == _currentBeat)
        {
            if(_activeStar2)
            {
                Instantiate(_star, _land_2);
                _activeStar2 = false;
            }
        }
        if (_totalNote == _currentBeat)
        {
            if (_activeStar3)
            {
                Instantiate(_star, _land_1);
                _activeStar3 = false;
            }
        }
    }
    private void CheckShowStar()
    {
        if (_currentBeat >= Mathf.CeilToInt(_totalNote / 3) && _currentBeat < Mathf.CeilToInt(_totalNote / 2))
        {
            if (_activeStar)
            {
                count++;
            }
        }
        if (_currentBeat >= Mathf.CeilToInt(_totalNote / 2) && _currentBeat < Mathf.CeilToInt(_totalNote))
        {
            if (_activeStar)
            {
                count++;
            }
        }
        if (_currentBeat >= _totalNote)
        {
            if (_activeStar)
            {
                count++;
            }
            
        }
    }
    private void ShowStar()
    {
        if (count == 1)
        {
            _star_1_Shadow.SetActive(true);
            _activeStar = false;
        }
        if (count == 2)
        {
            _star_2_Shadow.SetActive(true);
            _activeStar = false;
        }
        if (count == 3)
        {
            _star_3_Shadow.SetActive(true);
            _activeStar = false;
        }
    }
}
