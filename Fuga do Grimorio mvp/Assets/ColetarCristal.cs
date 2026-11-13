
using UnityEngine;

public class ColetarCristal : MonoBehaviour
{
    private bool coletado = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (coletado) return;

        InventarioCristal inventario =
            collision.GetComponentInParent<InventarioCristal>();

        if (inventario == null) return;

        if (inventario.AdicionarCristal())
        {
            coletado = true;
            Destroy(gameObject);
        }
    }
}
