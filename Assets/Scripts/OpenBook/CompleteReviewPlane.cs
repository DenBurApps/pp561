using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompleteReviewPlane : MonoBehaviour
{
    [SerializeField] private Sprite _selectedStarSprite;
    [SerializeField] private Sprite _defaultStarSprite;
    
    [SerializeField] private Button _editButton;
    [SerializeField] private TMP_Text _reviewText;
    [SerializeField] private Image[] _stars;

    private int _starsCount = 0;

    public event Action<string, int> EditButtonClicked;

    private void Start()
    {
        Disable();
    }

    private void OnEnable()
    {
        _editButton.onClick.AddListener(OnEditButtonClicked);
    }

    private void OnDisable()
    {
        _editButton.onClick.RemoveListener(OnEditButtonClicked);
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void OpenWindow(int stars, string reviewText)
    {
        SetStarsActive(stars);
        SetReviewText(reviewText);
    }
    
    public void SetStarsActive(int count)
    {
        for (int i = 0; i < _stars.Length; i++)
        {
            if (i < count)
            {
                _stars[i].sprite = _selectedStarSprite;
            }
            else
            {
                _stars[i].sprite = _defaultStarSprite;
            }
        }

        _starsCount = count;
    }

    public void SetReviewText(string text)
    {
        _reviewText.text = text;
    }

    private void ResetData()
    {
        foreach (var star in _stars)
        {
            star.sprite = _defaultStarSprite;
        }

        _reviewText.text = string.Empty;
    }

    private void OnEditButtonClicked()
    {
        EditButtonClicked?.Invoke(_reviewText.text, _starsCount);
    }
}
