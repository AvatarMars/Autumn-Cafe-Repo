using UnityEngine;
using UnityEngine.UI;

public class PatienceUI : MonoBehaviour
{
    [SerializeField] private Image _patienceImage;
    [SerializeField] private GameObject _patienceUi;
    [SerializeField] private Transform _player;

    private Timer _timer;

    // Start is called before the first frame update
    void Start()
    {
        _timer = GetComponent<Timer>();
        _player = GameObject.Find("FPSController").transform;
    }

    // Update is called once per frame
    void Update()
    {
        _patienceImage.fillAmount = _timer.CurrentTime / _timer.MaxTime;
        _patienceUi.transform.LookAt(_player);
    }
}
