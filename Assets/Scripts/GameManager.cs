using MicroJam10.Craft;
using MicroJam10.Player;
using MicroJam10.SO;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

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
        private PentacleSpot[] _spots;

        [SerializeField]
        private GameObject _bloodMiddle;

        [SerializeField]
        private FormulaInfo _winFormula;

        private float _timer;

        private bool _turnToRed;

        public bool DidRitualStart { private set; get; }

        private const int SpotsCount = 5;
        public int SpotsLighted { set; get; }
        private void Awake()
        {
            Instance = this;

            Assert.AreEqual(_winFormula.Props.Length, _spots.Length);
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

            yield return new WaitForSeconds(.2f);
            _blackScreen.SetActive(false);

            bool isGood = true;
            for (int i = 0; i < _winFormula.Props.Length; i++)
            {
                if (_winFormula.Props[i].Name != _spots[i].Prop.Info.Name)
                {
                    isGood = false;
                    break;
                }
            }
            if (isGood)
            {
                // TODO
            }
            else
            {
                PlayerController.Instance.Die();
            }

            _globalLight.color = Color.white;
            _bloodMiddle.SetActive(true);
            foreach (var p in _pentacleLights)
            {
                p.enabled = false;
            }
        }
    }
}
