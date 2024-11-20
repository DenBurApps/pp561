using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BookListSaver : MonoBehaviour
{
    private string _filePath;

    private void Start()
    {
        _filePath = Path.Combine(Application.persistentDataPath, "booklists.json");
    }
    
    public void SaveBookList(BookListData bookListData)
    {
        string json = JsonUtility.ToJson(bookListData);
        
        File.WriteAllText(_filePath, json);
        Debug.Log("Book list saved to " + _filePath);
    }

    public void SaveMultipleBookLists(List<BookListData> bookListDatas)
    {
        BookListCollection collection = new BookListCollection { BookLists = bookListDatas };

        string json = JsonUtility.ToJson(collection);

        File.WriteAllText(_filePath, json);
        Debug.Log("Book lists saved to " + _filePath);
    }
    
    public BookListData LoadBookList()
    {
        if (File.Exists(_filePath))
        {
            string json = File.ReadAllText(_filePath);
            
            BookListData bookListData = JsonUtility.FromJson<BookListData>(json);
            Debug.Log("Book list loaded from " + _filePath);
            return bookListData;
        }

        Debug.LogWarning("No saved book list found.");
        return null;
    }
    
    public List<BookListData> LoadMultipleBookLists()
    {
        if (File.Exists(_filePath))
        {
            string json = File.ReadAllText(_filePath);

            BookListCollection collection = JsonUtility.FromJson<BookListCollection>(json);
            Debug.Log("Book lists loaded from " + _filePath);
            return collection.BookLists;
        }

        Debug.LogWarning("No saved book lists found.");
        return new List<BookListData>();
    }
    
}

[System.Serializable]
public class BookListCollection
{
    public List<BookListData> BookLists;
}