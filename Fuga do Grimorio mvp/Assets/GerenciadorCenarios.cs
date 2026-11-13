
using UnityEngine;

public class GerenciadorCenarios : MonoBehaviour
{
    [Header("Cenarios")]
    public GameObject cenario1;
    public GameObject cenario2;
    public GameObject cenario3;

    private int cenarioAtual = 1;

    void Start()
    {
        TrocarCenario(1);
    }

    public void BossDerrotado()
    {
        if (cenarioAtual < 3)
        {
            TrocarCenario(cenarioAtual + 1);
        }
    }

    void TrocarCenario(int numero)
    {
        cenarioAtual = Mathf.Clamp(numero, 1, 3);

        if (cenario1 != null)
            cenario1.SetActive(cenarioAtual == 1);

        if (cenario2 != null)
            cenario2.SetActive(cenarioAtual == 2);

        if (cenario3 != null)
            cenario3.SetActive(cenarioAtual == 3);

        Debug.Log("Cenario atual: " + cenarioAtual);
    }
}
