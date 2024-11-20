using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookListMainScreenPlane : MonoBehaviour
{
    [SerializeField] private TMP_Text _bookListTitleText;
    [SerializeField] private TMP_Text _booksInListText;
    [SerializeField] private Button _openButton;

    private List<BookData> _bookDatas = new List<BookData>();
    private BookListData _bookListData;


    private string _bookListTitle;
    private int _booksInList;
    private bool _isActive;

    public event Action<BookListMainScreenPlane> BookListOpened;
    public event Action DataUpdated;
    
    public bool IsActive => _isActive;
    public BookListData BookListData => _bookListData;

    private void OnEnable()
    {
        _openButton.onClick.AddListener(OnBookListOpened);
    }

    private void OnDisable()
    {
        _openButton.onClick.RemoveListener(OnBookListOpened);
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void AddBookData(BookData bookData)
    {
        if (bookData == null)
            throw new ArgumentNullException(nameof(bookData));

        _bookDatas.Add(bookData);
    }

    public void UpdateData()
    {
        DataUpdated?.Invoke();
    }

    public void ResetData()
    {
        _bookListTitle = string.Empty;
        _booksInList = 0;
        _bookListData = null;
        _bookListTitleText.text = _bookListTitle;
        _booksInListText.text = _booksInList.ToString();
        _isActive = false;
    }

    public void SetData(BookListData bookListData)
    {
        if (bookListData == null)
            throw new ArgumentNullException(nameof(bookListData));

        _bookListData = bookListData;
        _bookListTitleText.text = _bookListData.BookListTitle;
        _booksInListText.text = _bookListData.BooksInList.ToString();
        _isActive = true;
    }

    private void OnBookListOpened() => BookListOpened?.Invoke(this);
}

[Serializable]
public class BookListData
{
    public string BookListTitle;
    public string BookListDescription;
    public int BooksInList;
    public List<BookData> BookDatas;


    public BookListData(string bookListTitle, int booksInList, List<BookData> bookDatas, string bookListDescription)
    {
        BookListTitle = bookListTitle;
        BooksInList = booksInList;
        BookDatas = bookDatas;
        BookListDescription = bookListDescription;
    }
}