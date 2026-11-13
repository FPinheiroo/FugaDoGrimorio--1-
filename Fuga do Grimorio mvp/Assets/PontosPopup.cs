
using UnityEngine;
using TMPro;

public class PontosPopup : MonoBehaviour
{
    public float velocidadeSubida = 1.5f;
    public float duracao = 1f;

    private TextMeshPro texto;
    private float tempoRestante;

    void Awake()
    {
        texto = GetComponent<TextMeshPro>();
        tempoRestante = duracao;
    }

    public void Configurar(int pontos)
    {
        texto.text = "+" + pontos;
        texto.color = Color.yellow;
    }

    void Update()
    {
        transform.position += Vector3.up *
            velocidadeSubida * Time.deltaTime;

        tempoRestante -= Time.deltaTime;

        Color cor = texto.color;
        cor.a = Mathf.Clamp01(tempoRestante / duracao);
        texto.color = cor;

        if (tempoRestante <= 0)
        {
            Destroy(gameObject);
        }
    }
}
