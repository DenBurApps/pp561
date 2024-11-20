using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookCategoryPlane : MonoBehaviour
{
    [SerializeField] private Genres _genre;
    [SerializeField] private Image _selectedImage;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Button _selectedButton;

    private Color _defaultButtonColor;
    private Color _defaultTextColor;

    public event Action<BookCategoryPlane> ButtonSelected;

    public Genres Genre => _genre;

    private void Start()
    {
        _selectedImage.enabled = false;
        
        _defaultButtonColor = _selectedButton.image.color;
        _defaultTextColor = _text.color;
    }

    private void OnEnable()
    {
        _selectedButton.onClick.AddListener(OnSelected);
    }

    private void OnDisable()
    {
        _selectedButton.onClick.RemoveListener(OnSelected);
    }

    public void SetColor(Color textColor, Color buttonColor)
    {
        _selectedButton.image.color = buttonColor;
        _text.color = textColor;
    }

    public void ReturnToDefault()
    {
        SetColor(_defaultTextColor, _defaultButtonColor);
        _selectedButton.enabled = true;
        _selectedImage.enabled = false;
        _selectedButton.onClick.AddListener(OnSelected);
    }

    private void OnSelected()
    {
        _selectedImage.enabled = true;
        _selectedButton.enabled = false;
        ButtonSelected?.Invoke(this);
        _selectedButton.onClick.RemoveListener(OnSelected);
    }
}