
using UnityEngine;

public class MoverItem : MonoBehaviour
{
    [Header("Movimento Horizontal")]
    public float velocidade = 7f;
    public float limiteEsquerdo = -12f;

    [Header("Movimento Vertical")]
    public float minY = -3f;
    public float maxY = 3f;
    public float velocidadeVertical = 4f;

    private float direcaoVertical = -1f;

    void Start()
    {
        // Comeca descendo se estiver na metade superior
        // e subindo se estiver na metade inferior
        float meio = (minY + maxY) / 2f;

        direcaoVertical = transform.position.y >= meio ? -1f : 1f;
    }

    void Update()
    {
        Vector3 posicao = transform.position;

        // Move para a esquerda
        posicao.x -= velocidade * Time.deltaTime;

        // Sobe e desce pelo cenario inteiro
        posicao.y += direcaoVertical * velocidadeVertical * Time.deltaTime;

        if (posicao.y >= maxY)
        {
            posicao.y = maxY;
            direcaoVertical = -1f;
        }
        else if (posicao.y <= minY)
        {
            posicao.y = minY;
            direcaoVertical = 1f;
        }

        transform.position = posicao;

        if (transform.position.x < limiteEsquerdo)
        {
            Destroy(gameObject);
        }
    }
}
