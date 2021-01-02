using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThreadCountControl : MonoBehaviour
{
    [SerializeField] private Button threadModeButton;

    private int _count;
    private bool _isActiveThreadMode;
    private TextMeshProUGUI _countText;

    private void Start()
    {
        _count = 100;
        threadModeButton.gameObject.SetActive(true);
        _countText = threadModeButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    public int GetCount()
    {
        return _count;
    }

    public bool GetActiveThreadMode()
    {
        return _isActiveThreadMode;
    }

    public void AddCount(int value)
    {
        _count += value;
    }

    public void UpdateCountOnDistancePassed()
    {
        if (_count != 0)
        {
            _count -= 5;
        }
    }

    public void UpdateThreadMode()
    {
        _isActiveThreadMode = !_isActiveThreadMode;
    }

    private void Update()
    {
        _countText.text = GetCount().ToString();
    }
}
