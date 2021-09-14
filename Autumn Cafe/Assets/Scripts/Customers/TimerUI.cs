using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private Image _referenceImage;
    [SerializeField] private Timer _timer;

    // Update is called once per frame
    void Update()
    {
        _referenceImage.fillAmount = _timer.CurrentTime / _timer.MaxTime;
    }
}
