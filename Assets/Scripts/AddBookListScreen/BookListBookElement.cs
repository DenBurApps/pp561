using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BookListBookElement : MonoBehaviour
{
    [SerializeField] private Sprite _emptyPictureSprite;
    [SerializeField] private Sprite _selectedButtonSprite;
    [SerializeField] private Sprite _unselectedButtonSprite;
    
    [SerializeField] private ImagePicker _imagePicker;
    [SerializeField] private TMP_Text _bookTitle;
    [SerializeField] private TMP_Text _authorText;
    [SerializeField] private TMP_Text _genreText;
    [SerializeField] private Button _selectButton;

    private BookData _bookData;
    private bool _isActive = false;
    private bool _isSelected = false;
    
    public event Action SelectionChanged;

    public bool IsSelected => _isSelected;

    public bool IsActive => _isActive;
    public BookData BookData => _bookData;

    private void OnEnable()
    {
        _selectButton.onClick.AddListener(ProcessSelectionClicked);
    }

    private void OnDisable()
    {
        _selectButton.onClick.RemoveListener(ProcessSelectionClicked);
    }

    public void Enable()
    {
        gameObject.SetActive(true);
        _isActive = true;
    }

    public void Disable()
    {
        gameObject.SetActive(false);
        _isSelected = false;
        _selectButton.image.sprite = _unselectedButtonSprite;
        _isActive = false;
    }

    public void EnableWindow(BookData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        _bookData = new BookData(data.Genre, data.BookTitle, data.Author, data.Description, data.PublicationDate);
        _bookTitle.text = _bookData.BookTitle;
        _authorText.text = _bookData.Author;
        _genreText.text = GenreExtensions.ToString(_bookData.Genre);

        if (!string.IsNullOrEmpty(data.ImagePath))
        {
            _bookData.ImagePath = data.ImagePath;
            _imagePicker.Init(_bookData.ImagePath);
        }
        else
        {
            _imagePicker.Image.sprite = _emptyPictureSprite;
        }
    }

    public void SetSelected()
    {
        _isSelected = true;
        _selectButton.image.sprite = _selectedButtonSprite;
    }

    private void ProcessSelectionClicked()
    {
        if (_isSelected == false)
        {
            _isSelected = true;
            _selectButton.image.sprite = _selectedButtonSprite;
            SelectionChanged?.Invoke();
        }
        else
        {
            _isSelected = false;
            _selectButton.image.sprite = _unselectedButtonSprite;
            SelectionChanged?.Invoke();
        }
    }
    
}
