
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChoqueHorizontal : MonoBehaviour
{
    [Header("Prefab do Choque")]
    public GameObject prefabChoque;

    [Header("Quantidade")]
    [Min(1)] public int quantidadeChoques = 1;
    [Min(2)] public int quantidadeFaixas = 5;

    [Header("Dificuldade Progressiva")]
    [Min(1)] public int maximoChoques = 4;

    [Header("Tempos")]
    public float tempoAviso = 1.2f;
    public float duracaoChoque = 0.5f;
    public float intervaloAtaques = 2f;

    [Header("Area do Ataque")]
    public float alturaMinima = -4f;
    public float alturaMaxima = 4f;
    public float posicaoX = 0f;

    [Header("Espaco para Desviar")]
    [Min(1)] public int faixasLivresMinimas = 1;
    [Min(1)] public int distanciaMinimaEntreChoques = 1;

    private Coroutine rotinaAtaque;

    private List<GameObject> choquesAtivos =
        new List<GameObject>();

    public void ConfigurarDificuldade(int bossesDerrotados)
    {
        quantidadeChoques = Mathf.Clamp(
            1 + bossesDerrotados,
            1,
            maximoChoques
        );

        Debug.Log(
            "OLHO ARCANO: " +
            quantidadeChoques +
            " lasers por ataque!"
        );
    }

    void OnEnable()
    {
        rotinaAtaque = StartCoroutine(CicloAtaques());
    }

    void OnDisable()
    {
        PararAtaques();
    }

    void OnDestroy()
    {
        PararAtaques();
    }

    void PararAtaques()
    {
        if (rotinaAtaque != null)
        {
            StopCoroutine(rotinaAtaque);
            rotinaAtaque = null;
        }

        LimparChoques();
    }

    void LimparChoques()
    {
        foreach (GameObject choque in choquesAtivos)
        {
            if (choque != null)
            {
                choque.SetActive(false);
                Destroy(choque);
            }
        }

        choquesAtivos.Clear();
    }

    IEnumerator CicloAtaques()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            yield return ExecutarAtaque();

            yield return new WaitForSeconds(
                intervaloAtaques
            );
        }
    }

    IEnumerator ExecutarAtaque()
    {
        if (prefabChoque == null)
        {
            Debug.LogWarning(
                "Prefab do choque nao configurado!"
            );

            yield break;
        }

        List<int> faixas = SortearFaixas();

        List<ChoqueHorizontal> choquesCriados =
            new List<ChoqueHorizontal>();

        foreach (int faixa in faixas)
        {
            float t = (faixa + 0.5f) / quantidadeFaixas;

            float y = Mathf.Lerp(
                alturaMinima,
                alturaMaxima,
                t
            );

            Vector3 posicao = new Vector3(
                posicaoX,
                y,
                0f
            );

            GameObject objeto = Instantiate(
                prefabChoque,
                posicao,
                Quaternion.identity
            );

            ChoqueHorizontal choque =
                objeto.GetComponent<ChoqueHorizontal>();

            if (choque != null)
            {
                choquesAtivos.Add(objeto);

                choque.IniciarAviso();
                choquesCriados.Add(choque);
            }
            else
            {
                Destroy(objeto);

                Debug.LogWarning(
                    "Prefab sem ChoqueHorizontal!"
                );
            }
        }

        yield return new WaitForSeconds(tempoAviso);

        foreach (ChoqueHorizontal choque in choquesCriados)
        {
            if (choque != null)
                choque.AtivarChoque();
        }

        yield return new WaitForSeconds(duracaoChoque);

        foreach (ChoqueHorizontal choque in choquesCriados)
        {
            if (choque != null)
            {
                GameObject objeto = choque.gameObject;

                objeto.SetActive(false);

                choquesAtivos.Remove(objeto);

                Destroy(objeto);
            }
        }
    }

    List<int> SortearFaixas()
    {
        List<int> disponiveis = new List<int>();

        for (int i = 0; i < quantidadeFaixas; i++)
            disponiveis.Add(i);

        for (int i = 0; i < disponiveis.Count; i++)
        {
            int indice = Random.Range(
                i,
                disponiveis.Count
            );

            int temp = disponiveis[i];
            disponiveis[i] = disponiveis[indice];
            disponiveis[indice] = temp;
        }

        List<int> escolhidas = new List<int>();

        int limite = Mathf.Min(
            quantidadeChoques,
            quantidadeFaixas - faixasLivresMinimas
        );

        limite = Mathf.Max(0, limite);

        foreach (int faixa in disponiveis)
        {
            bool podeAdicionar = true;

            foreach (int escolhida in escolhidas)
            {
                if (Mathf.Abs(faixa - escolhida) <
                    distanciaMinimaEntreChoques)
                {
                    podeAdicionar = false;
                    break;
                }
            }

            if (!podeAdicionar)
                continue;

            escolhidas.Add(faixa);

            if (escolhidas.Count >= limite)
                break;
        }

        return escolhidas;
    }
}
