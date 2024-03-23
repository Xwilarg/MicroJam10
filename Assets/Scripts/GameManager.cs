﻿using MicroJam10.Player;
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

        private float _timer;

        private bool _turnToRed;

        public bool DidRitualStart { private set; get; }

        private const int SpotsCount = 5;
        private int _spotsLighted;
        public int SpotsLighted
        {
            set
            {
                _spotsLighted = value;
                if (_spotsLighted == SpotsCount)
                {
                    DidRitualStart = true;
                    _turnToRed = true;
                }
            }
            get => _spotsLighted;
        }

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

        private IEnumerator Die()
        {
            _blackScreen.SetActive(true);
            PlayerController.Instance.Die();
            yield return new WaitForSeconds(.2f);
            _blackScreen.SetActive(false);
        }
    }
}