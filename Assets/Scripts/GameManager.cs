using MicroJam10.Player;
using System.Collections;
using UnityEngine;

namespace MicroJam10
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { private set; get; }

        [SerializeField]
        private Light _globalLight;

        [SerializeField]
        private GameObject _blackScreen;

        [SerializeField]
        private Light[] _pentacleLights;

        [SerializeField]
        private GameObject _bloodMiddle;

        private float _timer;

        private bool _turnToRed;

        public bool DidRitualStart { private set; get; }

        private const int SpotsCount = 5;
        public int SpotsLighted { set; get; }
        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (_turnToRed && _timer < 1f)
            {
                _timer += Time.deltaTime * .5f;
                var val = Mathf.Lerp(1f, .2f, _timer);
                _globalLight.color = new(1f, val, val);
                if (_timer >= 1f)
                {
                    StartCoroutine(Die());
                }
            }
        }

        public void CheckVictory()
        {
            if (SpotsLighted == SpotsCount)
            {
                DidRitualStart = true;
                _turnToRed = true;
                PlayerController.Instance.ResetState();
            }
        }

        private IEnumerator Die()
        {
            _blackScreen.SetActive(true);
            PlayerController.Instance.Die();
            yield return new WaitForSeconds(.2f);
            _blackScreen.SetActive(false);
            _globalLight.color = Color.white;
            _bloodMiddle.SetActive(true);
            foreach (var p in _pentacleLights)
            {
                p.enabled = false;
            }
        }
    }
}
