using MicroJam10.Craft;
using MicroJam10.Player;
using MicroJam10.SO;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace MicroJam10
{
    public class GameManager : MonoBehaviour
    {
        private string[] hints = new[]
        {
            ""
        };

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
        private FormulaInfo _winFormula, _knifeFormula;

        [SerializeField]
        private Animator _deathAnim;

        [SerializeField]
        private TMP_Text _gameoverText;

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

        private bool IsRecipeValide(FormulaInfo formula)
        {
            for (int i = 0; i < formula.Props.Length; i++)
            {
                if (formula.Props[i].Name != _spots[i].Prop.Info.Name)
                {
                    return false;
                }
            }
            return true;
        }

        private void Loose()
        {
            _deathAnim.enabled = true;
            _gameoverText.text = "Press 'enter' to restart";
        }

        private IEnumerator Die()
        {
            _blackScreen.SetActive(true);

            yield return new WaitForSeconds(.2f);
            _blackScreen.SetActive(false);

            if (IsRecipeValide(_knifeFormula))
            {
                PlayerController.Instance.GetKnived();
            }
            else if (IsRecipeValide(_winFormula))
            {
                // TODO
            }
            else
            {
                PlayerController.Instance.Die();
                Loose();
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
