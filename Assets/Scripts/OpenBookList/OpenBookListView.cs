using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class OpenBookListView : MonoBehaviour
{
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _editButton;
    [SerializeField] private TMP_Text _booksCountText;
    [SerializeField] private TMP_Text _booksTitleText;
    [SerializeField] private TMP_Text _booksDescriptionText;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action BackButtonClicked;
    public event Action EditButtonClicked;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _backButton.onClick.AddListener(OnBackButtonClicked);
        _editButton.onClick.AddListener(OnEditButtonClicked);
    }

    private void OnDisable()
    {
        _backButton.onClick.RemoveListener(OnBackButtonClicked);
        _editButton.onClick.RemoveListener(OnEditButtonClicked);
    }

    public void SetTitleText(string text)
    {
        _booksTitleText.text = text;
    }

    public void SetDescriptionText(string text)
    {
        _booksDescriptionText.text = text;
    }

    public void SetBookCountText(int count)
    {
        _booksCountText.text = count.ToString();
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    private void OnBackButtonClicked() => BackButtonClicked?.Invoke();
    private void OnEditButtonClicked() => EditButtonClicked?.Invoke();
}
