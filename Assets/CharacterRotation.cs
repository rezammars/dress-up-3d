using UnityEngine;

public class CharacterRotation : MonoBehaviour
{
    public Transform characterRoot; // Objek karakter yang akan diputar
    public float rotationSpeed = 5f; // Kecepatan rotasi saat swipe

    private Vector2 lastMousePosition; // Menyimpan posisi terakhir sentuhan/mouse

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Saat menyentuh layar pertama kali
        {
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0)) // Saat drag/swipe
        {
            Vector2 delta = (Vector2)Input.mousePosition - lastMousePosition; // Hitung perbedaan posisi
            float rotationAmount = delta.x * rotationSpeed * Time.deltaTime; // Hitung rotasi berdasarkan pergerakan horizontal

            characterRoot.Rotate(0, -rotationAmount, 0); // Putar karakter
            lastMousePosition = Input.mousePosition; // Update posisi terakhir
        }
    }
}