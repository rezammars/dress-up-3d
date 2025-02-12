using UnityEngine;
using UnityEngine.UI;

public class SkinColorChanger : MonoBehaviour
{
   public Renderer characterRenderer; // Mesh Renderer karakter
    public Color[] skinColors; // Pilihan warna kulit
    private int currentColorIndex = 0;

    public void ChangeSkinColor()
    {
        if (skinColors.Length == 0) return; // Jika tidak ada warna, keluar dari fungsi

        currentColorIndex = (currentColorIndex + 1) % skinColors.Length; // Pindah ke warna berikutnya
        Material[] materials = characterRenderer.materials; // Ambil semua material yang ada

        foreach (Material mat in materials)
        {
            // Cek jika material memiliki properti "_BaseColor" (URP) atau "_Color" (Standard Shader)
            if (mat.HasProperty("_Color") || mat.HasProperty("_BaseColor"))
            {
                mat.color = skinColors[currentColorIndex]; // Ganti warna material tersebut
            }
        }

        characterRenderer.materials = materials; // Terapkan perubahan
    }
}
