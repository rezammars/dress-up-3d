using UnityEngine;
using UnityEngine.UI;

public class CharacterRotation : MonoBehaviour
{
    public Transform characterRoot; // Objek karakter yang akan diputar
    public Button rotateLeftButton, rotateRightButton; // Tombol untuk rotasi
    public float rotationSpeed = 100f; // Kecepatan rotasi

    private void Start()
    {
        rotateLeftButton.onClick.AddListener(RotateLeft);
        rotateRightButton.onClick.AddListener(RotateRight);
    }

    public void RotateLeft()
    {
       characterRoot.Rotate(0, -rotationSpeed * Time.deltaTime, 0);
    }

    public void RotateRight()
    {
       characterRoot.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
