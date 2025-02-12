using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DressUpManager : MonoBehaviour
{
    public GameObject[] hairs, shirts, shoes; // Array pakaian
    private int hairIndex = -1, shirtIndex = -1, shoesIndex = -1; // Mulai dari -1 (tidak ada pakaian)

    public Button nextHair, prevHair, nextShirt, prevShirt, nextShoes, prevShoes;

    public GameObject dressUpUI; // UI biasa
    public GameObject photoModeUI; // UI mode foto

    public SpriteRenderer backgroundRenderer; // UI Image untuk Background
    public Sprite[] backgrounds; // Daftar pilihan background
    private int currentBackground = 0;

    public GameObject uiCanvas;

    void Start()
    {
        // Menambahkan event listener ke tombol
        nextHair.onClick.AddListener(() => ChangeItem(ref hairIndex, hairs, 1));
        prevHair.onClick.AddListener(() => ChangeItem(ref hairIndex, hairs, -1));

        nextShirt.onClick.AddListener(() => ChangeItem(ref shirtIndex, shirts, 1));
        prevShirt.onClick.AddListener(() => ChangeItem(ref shirtIndex, shirts, -1));

        nextShoes.onClick.AddListener(() => ChangeItem(ref shoesIndex, shoes, 1));
        prevShoes.onClick.AddListener(() => ChangeItem(ref shoesIndex, shoes, -1));

        // Set pakaian awal (tidak ada yang aktif)
        UpdateItems();
    }

    void ChangeItem(ref int index, GameObject[] items, int direction)
    {
        index += direction;

        if (index >= items.Length) index = -1; // Kembali ke tanpa pakaian
        if (index < -1) index = items.Length - 1; // Kembali ke terakhir

        UpdateItems();
    }

    void UpdateItems()
    {
        for (int i = 0; i < hairs.Length; i++) hairs[i].SetActive(i == hairIndex);
        for (int i = 0; i < shirts.Length; i++) shirts[i].SetActive(i == shirtIndex);
        for (int i = 0; i < shoes.Length; i++) shoes[i].SetActive(i == shoesIndex);
    }

    public void EnterPhotoMode()
    {
        dressUpUI.SetActive(false);  // Sembunyikan UI dress-up
        photoModeUI.SetActive(true); // Tampilkan UI mode foto
    }

    public void ExitPhotoMode()
    {
        dressUpUI.SetActive(true);   // Kembalikan UI dress-up
        photoModeUI.SetActive(false); // Sembunyikan UI mode foto
    }

    public void ChangeBackground()
    {
    if (backgrounds.Length == 0) return;

    currentBackground = (currentBackground + 1) % backgrounds.Length; // Loop ke background berikutnya
    backgroundRenderer.sprite = backgrounds[currentBackground]; // Ganti gambar background
    }

    public void TakeScreenshot()
    {
    StartCoroutine(CaptureScreenshot());
    }

    IEnumerator CaptureScreenshot()
{
    // Sembunyikan UI
    if (uiCanvas != null) uiCanvas.SetActive(false);

    yield return new WaitForEndOfFrame(); // Tunggu satu frame agar UI benar-benar hilang

    // Simpan screenshot
    string path = Application.persistentDataPath + "/screenshot.png";
    ScreenCapture.CaptureScreenshot(path);
    Debug.Log("Screenshot disimpan di: " + path);

    yield return new WaitForSeconds(0.5f); // Beri jeda sebelum menampilkan UI kembali

    // Tampilkan kembali UI
    if (uiCanvas != null) uiCanvas.SetActive(true);
}
}