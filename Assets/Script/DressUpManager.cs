using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DressUpManager : MonoBehaviour
{
    [Header("UI Katalog")]
    public CanvasGroup panelKatalog; // Panel katalog utama
    public Transform katalogContainer; // Tempat item katalog ditampilkan
    public GameObject itemPrefab; // Prefab untuk setiap item katalog

    [Header("Daftar Item Kategori")]
    public List<Sprite> rambutIcons;
    public List<GameObject> rambutModels;
    public List<Sprite> bajuIcons;
    public List<GameObject> bajuModels;
    public List<Sprite> sepatuIcons;
    public List<GameObject> sepatuModels;

    private Dictionary<string, List<Sprite>> iconDatabase;
    private Dictionary<string, List<GameObject>> modelDatabase;

    private GameObject currentItem;
    private string currentCategory = "";

    void Start()
    {
        // Inisialisasi panel katalog tersembunyi
        panelKatalog.alpha = 0;
        panelKatalog.gameObject.SetActive(false);

        // Inisialisasi dictionary untuk mempermudah pengelolaan kategori
        iconDatabase = new Dictionary<string, List<Sprite>>()
        {
            { "Rambut", rambutIcons },
            { "Baju", bajuIcons },
            { "Sepatu", sepatuIcons }
        };

        modelDatabase = new Dictionary<string, List<GameObject>>()
        {
            { "Rambut", rambutModels },
            { "Baju", bajuModels },
            { "Sepatu", sepatuModels }
        };
    }

    public void ShowCatalog(string category)
    {
        // Jika kategori yang diklik sama dengan yang sedang aktif, tutup katalog
        if (currentCategory == category)
        {
            StartCoroutine(FadeOut(panelKatalog));
            currentCategory = "";
            return;
        }

        // Update kategori saat ini
        currentCategory = category;
        PopulateCatalog(category);
        StartCoroutine(FadeIn(panelKatalog));
    }

    void PopulateCatalog(string category)
    {
        // Bersihkan katalog sebelum menampilkan item baru
        foreach (Transform child in katalogContainer)
        {
            Destroy(child.gameObject);
        }

        // Pastikan kategori ada di database
        if (!iconDatabase.ContainsKey(category) || !modelDatabase.ContainsKey(category))
        {
            Debug.LogError("Kategori tidak ditemukan: " + category);
            return;
        }

        List<Sprite> icons = iconDatabase[category];
        List<GameObject> models = modelDatabase[category];

        if (icons.Count == 0 || models.Count == 0)
        {
            Debug.LogWarning("Kategori " + category + " belum memiliki item!");
            return;
        }

        // Buat item di katalog
        for (int i = 0; i < icons.Count; i++)
        {
            GameObject newItem = Instantiate(itemPrefab, katalogContainer);
            Image previewImage = newItem.transform.Find("PreviewModel").GetComponent<Image>();

            if (previewImage != null)
            {
                previewImage.sprite = icons[i];
            }
            else
            {
                Debug.LogError("PreviewModel tidak ditemukan dalam itemPrefab!");
            }

            int index = i; // Variabel lokal untuk menghindari closure issue
            newItem.GetComponent<Button>().onClick.AddListener(() => SelectItem(models[index]));
        }
    }

    void SelectItem(GameObject model)
    {
        if (model == null)
        {
            Debug.LogError("Model yang dipilih tidak ditemukan!");
            return;
        }

        // Nonaktifkan item sebelumnya
        if (currentItem != null)
        {
            currentItem.SetActive(false);
        }

        // Aktifkan model yang dipilih
        model.SetActive(true);
        currentItem = model;
    }

    IEnumerator FadeIn(CanvasGroup panel)
    {
        panel.gameObject.SetActive(true);
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
        panel.gameObject.SetActive(false);
    }
}