using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DressUpManager : MonoBehaviour
{
    [Header("UI Panel & Container")]
    public CanvasGroup panelKatalog;
    public Transform katalogContainer;
    public GameObject itemPrefab;

    [Header("Karakter & Slot")]
    public Transform characterTransform;

    [Header("Daftar Item")]
    public List<Sprite> rambutIcons;
    public List<GameObject> rambutModels;
    public List<Sprite> bajuIcons;
    public List<GameObject> bajuModels;
    public List<Sprite> sepatuIcons;
    public List<GameObject> sepatuModels;

    private Dictionary<string, (List<Sprite>, List<GameObject>, Transform)> itemDictionary;
    private string currentCategory = "";

    void Start()
    {
        panelKatalog.alpha = 0;
        panelKatalog.gameObject.SetActive(false);
        panelKatalog.blocksRaycasts = false;

        itemDictionary = new Dictionary<string, (List<Sprite>, List<GameObject>, Transform)>
        {
            { "Rambut", (rambutIcons, rambutModels, characterTransform.Find("SlotRambut")) },
            { "Baju", (bajuIcons, bajuModels, characterTransform.Find("SlotBaju")) },
            { "Sepatu", (sepatuIcons, sepatuModels, characterTransform.Find("SlotSepatu")) }
        };
    }

    public void ShowCatalog(string category)
    {
        if (currentCategory == category && panelKatalog.gameObject.activeSelf)
        {
            StartCoroutine(FadeOut(panelKatalog));
            currentCategory = "";
            return;
        }

        if (!panelKatalog.gameObject.activeSelf)
        {
            StartCoroutine(FadeIn(panelKatalog));
        }

        currentCategory = category;
        PopulateCatalog(category);
    }

    void PopulateCatalog(string category)
    {
        if (!itemDictionary.TryGetValue(category, out var data) || data.Item1.Count != data.Item2.Count)
        {
            return;
        }

        foreach (Transform child in katalogContainer)
        {
            child.gameObject.SetActive(false);
        }

        for (int i = 0; i < data.Item1.Count; i++)
        {
            GameObject newItem = (i < katalogContainer.childCount) ?
                katalogContainer.GetChild(i).gameObject :
                Instantiate(itemPrefab, katalogContainer);

            newItem.SetActive(true);

            Image previewImage = newItem.transform.Find("PreviewModel")?.GetComponent<Image>();
            if (previewImage)
            {
                previewImage.sprite = data.Item1[i];
                AdjustImageSize(previewImage, data.Item1[i]);
            }

            Button itemButton = newItem.GetComponent<Button>();
            if (itemButton)
            {
                int index = i;
                itemButton.onClick.RemoveAllListeners();
                itemButton.onClick.AddListener(() => SelectItem(data.Item2[index], category));
            }
        }
    }

    void SelectItem(GameObject modelPrefab, string category)
    {
        if (modelPrefab == null || !itemDictionary.TryGetValue(category, out var data))
        {
            return;
        }

        Transform targetSlot = data.Item3;
        if (targetSlot == null) return;

        foreach (Transform child in targetSlot)
        {
            Destroy(child.gameObject);
        }

        GameObject newModel = Instantiate(modelPrefab, targetSlot);
        newModel.name = modelPrefab.name + "_Instance";
        newModel.SetActive(true);
    }

    IEnumerator FadeIn(CanvasGroup panel)
    {
        panel.gameObject.SetActive(true);
        panel.blocksRaycasts = true;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 3;
            panel.alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }
    }

    IEnumerator FadeOut(CanvasGroup panel)
    {
        float t = 1;
        while (t > 0)
        {
            t -= Time.deltaTime * 3;
            panel.alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }
        panel.blocksRaycasts = false;
        panel.gameObject.SetActive(false);
    }

    void AdjustImageSize(Image image, Sprite sprite)
    {
        RectTransform rectTransform = image.GetComponent<RectTransform>();
        float maxWidth = 100f, maxHeight = 100f;
        float scaleFactor = Mathf.Min(maxWidth / sprite.rect.width, maxHeight / sprite.rect.height, 1f);
        rectTransform.sizeDelta = new Vector2(sprite.rect.width * scaleFactor, sprite.rect.height * scaleFactor);
    }
}