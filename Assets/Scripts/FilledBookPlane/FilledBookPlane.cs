using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FilledBookPlane : MonoBehaviour
{
    [SerializeField] private Sprite _noPhotoSprite;
    [SerializeField] private Button _editButton;
    [SerializeField] private TMP_Text _genreText;
    [SerializeField] private ImagePicker _bookImage;
    [SerializeField] private TMP_Text _bookTitleText;
    [SerializeField] private TMP_Text _bookAuthorText;
    [SerializeField] private Image _favouriteImage;

    private BookData _currentData;
    private bool _isActive;

    public BookData CurrentData => _currentData;
    public event Action<FilledBookPlane> EditButtonClicked;
    public event Action DataChanged;

    public bool IsActive => _isActive;

    private void OnEnable()
    {
        _editButton.onClick.AddListener(OnEditButtonClicked);
    }

    private void OnDisable()
    {
        _editButton.onClick.RemoveListener(OnEditButtonClicked);
    }

    public void SetFavouriteStatus(bool status)
    {
        CurrentData.FavouriteSelected = status;
        _favouriteImage.enabled = CurrentData.FavouriteSelected;
    }

    public void DisableEditButton()
    {
        _editButton.enabled = false;
    }

    public void SetGenre(Genres genre)
    {
        _currentData.Genre = genre;
        SetGenreText(_currentData.Genre);
    }

    public void SetStatus(Status status)
    {
        _currentData.Status = status;
    }

    public void ChangeData()
    {
        DataChanged?.Invoke();
    }

    public void UpdateText()
    {
        SetTitleText(_currentData.BookTitle);
        SetAuthorText(_currentData.Author);
        SetGenreText(_currentData.Genre);
        EnablePicture();
    }

    public void SetBookData(BookData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        _currentData = data;

        if (!string.IsNullOrEmpty(_currentData.ImagePath) && _bookImage.CurrentPath != _currentData.ImagePath)
        {
            SetBookImage(_currentData.ImagePath);
        }

        SetTitleText(_currentData.BookTitle);
        SetAuthorText(_currentData.Author);
        SetGenre(data.Genre);
        SetGenreText(_currentData.Genre);
        _favouriteImage.enabled = _currentData.FavouriteSelected;
        _isActive = true;
    }

    public void EnablePicture()
    {
        if (_currentData != null && !string.IsNullOrEmpty(_bookImage.CurrentPath))
        {
            SetBookImage(_bookImage.CurrentPath);
        }
        else
        {
            _bookImage.Image.sprite = _noPhotoSprite;
        }
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void DeleteData()
    {
        SetTitleText(string.Empty);
        SetAuthorText(string.Empty);

        if (string.IsNullOrEmpty(_currentData.ImagePath))
        {
            _bookImage.Image.sprite = _noPhotoSprite;
        }
        else
        {
            SetBookImage(_currentData.ImagePath);
        }

        _favouriteImage.enabled = false;
        _isActive = false;
        _currentData = null;
    }

    private void SetTitleText(string text)
    {
        _bookTitleText.text = text;
    }

    private void SetAuthorText(string text)
    {
        _bookAuthorText.text = text;
    }

    private void SetBookImage(string imagePath)
    {
        _bookImage.Init(imagePath);
    }

    private void SetGenreText(Genres genre)
    {
        _genreText.text = GenreExtensions.ToString(genre);
    }


    private void OnEditButtonClicked() => EditButtonClicked?.Invoke(this);
}