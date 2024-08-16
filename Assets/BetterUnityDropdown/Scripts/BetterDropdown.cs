using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace BetterUnityDropdown
{
    public class BetterDropdown : MonoBehaviour
    {
        public int Value = -1;
        public string DefaultText = "Select item";
        public List<DropdownItemData> Data = new();
        [SerializeField] private TextMeshProUGUI targetText;
        [SerializeField] private GameObject blockerPrefab;
        [SerializeField] private RectTransform optionsObject;
        [SerializeField] private RectTransform optionsContent;
        [SerializeField] private AnimationDropdownTypes dropdownAnimationType = AnimationDropdownTypes.Shrinking;
        [SerializeField] private float _shrinkSpeed = 70;
        [SerializeField] private float _fadeSpeed = 4;
        [SerializeField] private float _maximumDropdownHeight = 350;
        [SerializeField] private AutoSizeLayoutDropdown optionsDropdown;
        [SerializeField] private AnimationCurve curveShrinking;
        [SerializeField] private Canvas _backCanvasSorting;
        [SerializeField] private Canvas _optionsCanvasSorting;
        private readonly List<OptionScript> _spawnedList = new();
        private GameObject _firstObj;
        private float _targetPos;
        private float _targetFade;
        private float _startPos;
        private float _startFade;
        private float _ratioShrinking = 1;
        private float _ratioFading = 1;
        private RectTransform _targetCanvas;
        private GameObject _currentBlocker;
        private bool _isOpened;
        private CanvasGroup _optionCanvasGroup;

        public Action<int> OnValueChanged;

        public void SetDefaultText()
        {
            Value = -1;
            targetText.text = DefaultText;
        }

        /// <summary>
        /// Add item data
        /// </summary>
        /// <param name="item">Item data</param>
        public void Add(DropdownItemData item)
        {
            Data.Add(item);
        }

        /// <summary>
        /// Add an array of item data
        /// </summary>
        /// <param name="items">Array of item data</param>
        public void Add(DropdownItemData[] items)
        {
            Data.AddRange(items);
        }

        /// <summary>
        /// Delete all items
        /// </summary>
        public void ClearAll()
        {
            Data.Clear();
        }

        /// <summary>
        /// Change state of the dropdown Open/Close
        /// </summary>
        public void ChangeState()
        {
            if (!_isOpened)
            {
                Open();
            }
            else
            {
                Close();
            }
        }

        /// <summary>
        /// Open options method
        /// </summary>
        public void Open()
        {
            if (Data.Count == 0)
            {
                return;
            }

            switch (dropdownAnimationType)
            {
                case AnimationDropdownTypes.None:

                    break;
                case AnimationDropdownTypes.Shrinking:
                    if (_ratioShrinking < 0.99f && _ratioShrinking > 0.01f)
                    {
                        return;
                    }
                    break;
                case AnimationDropdownTypes.Fading:
                    if (_ratioFading < 0.99f && _ratioFading > 0.01f)
                    {
                        return;
                    }
                    break;
                case AnimationDropdownTypes.ShrinkingAndFading:
                    if (_ratioFading < 0.99f && _ratioFading > 0.01f && _ratioShrinking < 0.99f && _ratioShrinking > 0.01f)
                    {
                        return;
                    }
                    break;
            }

            _isOpened = true;
            _spawnedList.Clear();
            for (int i = 0; i < Data.Count; i++)
            {
                var newOption = Instantiate(_firstObj, optionsContent).GetComponent<OptionScript>();
                newOption.gameObject.SetActive(true);
                newOption.Init(i, Data[i]);
                _spawnedList.Add(newOption);
            }
            optionsObject.GetChild(0).GetComponent<AutoSizeLayoutDropdown>().UpdateAllRect();
            StartCoroutine(WaitForSeveralFrames());
            _currentBlocker = Instantiate(blockerPrefab, _targetCanvas);
            _currentBlocker.GetComponent<Button>().onClick.AddListener(new UnityEngine.Events.UnityAction(Close));
            _currentBlocker.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            _currentBlocker.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        }

        /// <summary>
        /// Close options method
        /// </summary>
        public void Close()
        {
            switch (dropdownAnimationType)
            {
                case AnimationDropdownTypes.None:

                    break;
                case AnimationDropdownTypes.Shrinking:
                    if (_ratioShrinking < 0.99f && _ratioShrinking > 0.01f)
                    {
                        return;
                    }
                    break;
                case AnimationDropdownTypes.Fading:
                    if (_ratioFading < 0.99f && _ratioFading > 0.01f)
                    {
                        return;
                    }
                    break;
                case AnimationDropdownTypes.ShrinkingAndFading:
                    if (_ratioFading < 0.99f && _ratioFading > 0.01f && _ratioShrinking < 0.99f && _ratioShrinking > 0.01f)
                    {
                        return;
                    }
                    break;
            }

            _isOpened = false;
            _ratioShrinking = 0;
            _ratioFading = 0;
            _startPos = optionsObject.sizeDelta.y;
            _startFade = 1;
            _targetPos = 0;
            _targetFade = 0;
            Destroy(_currentBlocker);
            if (dropdownAnimationType == AnimationDropdownTypes.None)
            {
                optionsObject.sizeDelta = new Vector2(optionsObject.sizeDelta.x, 0);
                Closed();
            }
        }

        public void SelectOptionWithoutNotify(int id)
        {
            Value = id;
            targetText.text = Data[id].Text;
            for (int i = 0; i < _spawnedList.Count; i++)
            {
                _spawnedList[i].SetSelectState(i == Value);
            }
            Close();
        }

        /// <summary>
        /// Select option
        /// </summary>
        /// <param name="id">ID of the option</param>
        public void SelectOption(int id)
        {
            switch (dropdownAnimationType)
            {
                case AnimationDropdownTypes.None:

                    break;
                case AnimationDropdownTypes.Shrinking:
                    if (_ratioShrinking < 0.99f)
                    {
                        return;
                    }
                    break;
                case AnimationDropdownTypes.Fading:
                    if (_ratioFading < 0.99f)
                    {
                        return;
                    }
                    break;
                case AnimationDropdownTypes.ShrinkingAndFading:
                    if (_ratioFading < 0.99f && _ratioShrinking < 0.99f)
                    {
                        return;
                    }
                    break;
            }
            Value = id;
            targetText.text = Data[id].Text;
            for (int i = 0; i < _spawnedList.Count; i++)
            {
                _spawnedList[i].SetSelectState(i == Value);
            }
            Close();
            OnValueChanged?.Invoke(id);
        }

        private RectTransform FindCanvas(RectTransform currentParent)
        {
            if (currentParent.GetComponent<Canvas>())
            {
                return currentParent;
            }

            return FindCanvas(currentParent.parent.GetComponent<RectTransform>());
        }

        private void Start()
        {
            _firstObj = optionsContent.GetChild(0).gameObject;
            _firstObj.SetActive(false);
            optionsObject.gameObject.SetActive(false);
            _targetCanvas = FindCanvas(GetComponent<RectTransform>());
            _optionsCanvasSorting.overrideSorting = false;
            _backCanvasSorting.overrideSorting = false;
            _optionsCanvasSorting.sortingOrder = 100;
            _backCanvasSorting.sortingOrder = 100;
            optionsObject.sizeDelta = new Vector2(optionsObject.sizeDelta.x, 0);
            if (Value != -1)
            {
                SelectOption(Value);
            }
            else
            {
                SetDefaultText();
            }
            _optionCanvasGroup = optionsObject.GetComponent<CanvasGroup>();
        }

        /// <summary>
        /// Update for animations
        /// </summary>
        private void Update()
        {
            optionsDropdown.maxSize = _maximumDropdownHeight;
            if (!optionsObject.gameObject.activeSelf)
            {
                return;
            }
            switch (dropdownAnimationType)
            {
                case AnimationDropdownTypes.Shrinking:
                    _ratioShrinking = Mathf.Clamp(_ratioShrinking + (_shrinkSpeed * Time.deltaTime) * curveShrinking.Evaluate(_ratioShrinking), 0, 1);
                    optionsObject.sizeDelta = new Vector2(optionsObject.sizeDelta.x, Mathf.Lerp(_startPos, _targetPos, _ratioShrinking));
                    if (_ratioShrinking > 0.99f && _targetPos == 0)
                    {
                        Closed();
                    }
                    break;
                case AnimationDropdownTypes.Fading:
                    _ratioFading = Mathf.Clamp(_ratioFading + (_fadeSpeed * Time.deltaTime), 0, 1);
                    _optionCanvasGroup.alpha = Mathf.Lerp(_startFade, _targetFade, _ratioFading);
                    if (_ratioFading > 0.99f && _targetPos == 0)
                    {
                        Closed();
                    }
                    break;
                case AnimationDropdownTypes.ShrinkingAndFading:
                    _ratioShrinking = Mathf.Clamp(_ratioShrinking + (_shrinkSpeed * Time.deltaTime) * curveShrinking.Evaluate(_ratioShrinking), 0, 1);
                    optionsObject.sizeDelta = new Vector2(optionsObject.sizeDelta.x, Mathf.Lerp(_startPos, _targetPos, _ratioShrinking));

                    _ratioFading = Mathf.Clamp(_ratioFading + (_fadeSpeed * Time.deltaTime), 0, 1);
                    _optionCanvasGroup.alpha = Mathf.Lerp(_startFade, _targetFade, _ratioFading);
                    if (_ratioFading > 0.99f && _ratioShrinking > 0.99f && _targetPos == 0)
                    {
                        Closed();
                    }
                    break;
                default:
                    break;
            }
        }

        private IEnumerator WaitForSeveralFrames()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            optionsObject.gameObject.SetActive(true);
            _optionsCanvasSorting.overrideSorting = true;
            _backCanvasSorting.overrideSorting = true;
            _optionsCanvasSorting.sortingOrder = 30000;
            _backCanvasSorting.sortingOrder = 30000;
            _ratioShrinking = 0;
            _ratioFading = 0;
            _startPos = optionsObject.sizeDelta.y;
            _startFade = 0;
            _targetPos = optionsObject.GetChild(0).GetComponent<RectTransform>().sizeDelta.y;
            _targetFade = 1;
            if (dropdownAnimationType == AnimationDropdownTypes.None || dropdownAnimationType == AnimationDropdownTypes.Fading)
            {
                optionsObject.sizeDelta = new Vector2(optionsObject.sizeDelta.x, optionsObject.GetChild(0).GetComponent<RectTransform>().sizeDelta.y);
            }
            if (dropdownAnimationType == AnimationDropdownTypes.Shrinking || dropdownAnimationType == AnimationDropdownTypes.None)
            {
                _optionCanvasGroup.alpha = 1;
            }
        }

        private void Closed()
        {
            _optionsCanvasSorting.overrideSorting = false;
            _backCanvasSorting.overrideSorting = false;
            _optionsCanvasSorting.sortingOrder = 100;
            _backCanvasSorting.sortingOrder = 100;
            optionsObject.gameObject.SetActive(false);
            for (int i = 0; i < _spawnedList.Count; i++)
            {
                Destroy(_spawnedList[i].gameObject);
            }
            _spawnedList.Clear();
            if (dropdownAnimationType == AnimationDropdownTypes.Fading)
            {
                optionsObject.sizeDelta = new Vector2(optionsObject.sizeDelta.x, 0);
            }
        }
    }
}