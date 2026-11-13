using UnityEngine;

public class EfeitoParallax : MonoBehaviour
{
    public float velocidade = 2f;
    private float tamanhoX;

    void Start()
    {
        // Calcula a largura exata do Sprite automaticamente
        tamanhoX = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        // Move o fundo para a esquerda de forma constante
        transform.Translate(Vector3.left * velocidade * Time.deltaTime);

        // Se a imagem saiu completamente da visão da câmera pela esquerda...
        if (transform.position.x <= -tamanhoX)
        {
            // ...teletransporta ela para o final da fila. 
            // Somamos (tamanhoX * 2) para manter os milímetros de precisão e evitar solavancos.
            transform.position = new Vector3(transform.position.x + (tamanhoX * 2f), transform.position.y, transform.position.z);
        }
    }
}