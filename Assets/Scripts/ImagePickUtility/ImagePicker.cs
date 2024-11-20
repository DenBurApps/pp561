using System;
using UnityEngine;
using UnityEngine.UI;

public class ImagePicker : MonoBehaviour
{
    [SerializeField] private PreferedLocale preferedExtension = PreferedLocale.Width;
    [SerializeField] private Image _image;

    public string CurrentPath;

    private Texture imageTexture;
    private Vector2 standartSize;

    public Image Image => _image;

    public void OnDisable()
    {
        Destroy(imageTexture);
    }

    public void OnDestroy()
    {
        Destroy(imageTexture);
    }

    public void Init(string path)
    {
        var imgTransform = _image.GetComponent<RectTransform>();
        standartSize = new Vector2(imgTransform.sizeDelta.x, imgTransform.sizeDelta.y);

        if (path != "" && path != null)
        {
            _image.enabled = true;

            Destroy(imageTexture);
            GetImageFromGallery.SetImage(path, _image);
            imageTexture = _image.mainTexture;

            CurrentPath = path;
        }
        else
        {
            _image.enabled = false;
        }

        SetNormalSize();
    }

    private void SetNormalSize()
    {
        Texture texture = _image.mainTexture;

        float differenceInImage;

        if (preferedExtension == PreferedLocale.Width)
        {
            differenceInImage = standartSize.x / texture.width;
        }
        else
        {
            differenceInImage = standartSize.y / texture.height;
        }

        float normalWidth = texture.width * differenceInImage;
        float normalHeight = texture.height * differenceInImage;
        var imgTransform = _image.GetComponent<RectTransform>();

        imgTransform.sizeDelta = new Vector2(normalWidth, normalHeight);
    }
}

[Serializable]
public enum PreferedLocale
{
    Width,
    Height
}