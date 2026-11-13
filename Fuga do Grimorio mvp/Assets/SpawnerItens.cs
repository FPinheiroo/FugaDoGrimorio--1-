
using UnityEngine;

public class SpawnerItens : MonoBehaviour
{
    [Header("Prefabs dos Itens")]
    public GameObject moedaArcana;
    public GameObject escudoArcano;
    public GameObject pocaoVida;
    public GameObject cristalPoder;

    [Header("Configuracoes")]
    public float intervaloSpawn = 5f;
    public float posicaoX = 11f;
    public float minY = -3f;
    public float maxY = 3f;

    private float contador;

    void Update()
    {
        contador += Time.deltaTime;

        if (contador >= intervaloSpawn)
        {
            GerarItem();
            contador = 0f;
        }
    }

    void GerarItem()
    {
        float sorteio = Random.Range(0f, 100f);

        GameObject item;

        if (sorteio < 70f)
        {
            item = moedaArcana;
        }
        else if (sorteio < 82.5f)
        {
            item = escudoArcano;
        }
        else if (sorteio < 95f)
        {
            item = pocaoVida;
        }
        else
        {
            item = cristalPoder;
        }

        if (item == null)
        {
            Debug.LogWarning("Prefab do item nao configurado!");
            return;
        }

        float y = Random.Range(minY, maxY);

        Vector3 posicao = new Vector3(posicaoX, y, 0f);

        Instantiate(item, posicao, Quaternion.identity);
    }
}
