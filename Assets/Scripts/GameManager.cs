using MicroJam10.Craft;
using MicroJam10.Player;
using MicroJam10.SO;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

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
        private FormulaInfo _winFormula, _knifeFormula;

        [SerializeField]
        private Animator _deathAnim;

        [SerializeField]
        private TMP_Text _gameoverText;

        private bool _pendingRestart;

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
            if (SpotsLighted == SpotsCount && ExecutionSpot.Instance.IsPlayerInside)
            {
                DidRitualStart = true;
                _turnToRed = true;
                PlayerController.Instance.PlayEndAnim();
                PlayerController.Instance.ResetState();
            }
        }

        private bool IsRecipeValide(FormulaInfo formula, out string hint)
        {
            for (int i = 0; i < formula.Props.Length; i++)
            {
                if (formula.Props[i].Name != _spots[i].Prop.Info.Name)
                {
                    hint = formula.Props[i].Hint;
                    return false;
                }
            }
            hint = null;
            return true;
        }

        private void Loose(string hint)
        {
            _deathAnim.enabled = true;
            _gameoverText.text = $"{hint}\nPress 'enter' to restart";
        }

        private IEnumerator Die()
        {
            _blackScreen.SetActive(true);

            yield return new WaitForSeconds(.2f);
            _blackScreen.SetActive(false);

            string hint;
            if (IsRecipeValide(_knifeFormula, out var _))
            {
                PlayerController.Instance.GetKnived();
                StartCoroutine(WaitAndLoose("Congratulation for finding the secret ending!"));
            }
            else if (IsRecipeValide(_winFormula, out hint))
            {
                PlayerController.Instance.PlayWinAnim();
            }
            else
            {
                PlayerController.Instance.Die();
                StartCoroutine(WaitAndLoose(hint));
            }

            _globalLight.color = Color.white;
            _bloodMiddle.SetActive(true);
            foreach (var p in _pentacleLights)
            {
                p.enabled = false;
            }

            _pendingRestart = true;
        }

        private IEnumerator WaitAndLoose(string hint)
        {
            yield return new WaitForSeconds(2f);
            Loose(hint);
        }

        public void Restart()
        {
            if (_pendingRestart)
            {
                SceneManager.LoadScene("Main");
            }
        }
    }
}
