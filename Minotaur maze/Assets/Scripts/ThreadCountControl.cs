using TMPro;
using UnityEngine;

public class ThreadCountControl : MonoBehaviour
{
    private int _count;
    [SerializeField] private TextMeshProUGUI countText;

    private void Start()
    {
        _count = 100;
    }

    public int GetCount()
    {
        return _count;
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

    private void Update()
    {
        countText.text = GetCount().ToString();
    }
}
