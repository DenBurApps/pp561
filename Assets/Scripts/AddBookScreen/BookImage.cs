using System;
using UnityEngine;
using UnityEngine.UI;

public class BookImage : MonoBehaviour
{
    [SerializeField] private Button _deleteButton;
    [SerializeField] private ImagePicker _imagePicker;

    private bool _isActive;
    
    public event Action DeleteButtonClicked;

    public bool IsActive => _isActive;
    public ImagePicker ImagePicker => _imagePicker;

    private void OnEnable()
    {
        _deleteButton.onClick.AddListener(OnDeleteButtonClicked);
    }

    private void OnDisable()
    {
        _deleteButton.onClick.RemoveListener(OnDeleteButtonClicked);
    }

    public void Enable()
    {
        gameObject.SetActive(true);
        _deleteButton.gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
        _deleteButton.gameObject.SetActive(false);
    }

    private void OnDeleteButtonClicked()
    {
        _imagePicker.Image.sprite = null;
        DeleteButtonClicked?.Invoke();
    }
}
