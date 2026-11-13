
using UnityEngine;

public class ColetarPocao : MonoBehaviour
{
    private bool coletada = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (coletada) return;

        InventarioPocao inventario =
            collision.GetComponentInParent<InventarioPocao>();

        if (inventario == null) return;

        if (inventario.AdicionarPocao())
        {
            coletada = true;
            Destroy(gameObject);
        }
    }
}
