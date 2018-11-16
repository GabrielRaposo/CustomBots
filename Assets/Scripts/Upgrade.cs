using UnityEngine;

public class Upgrade : MonoBehaviour
{
    [HideInInspector] public bool active;
    public int health;

    public virtual void Initiate(int PlayerID, Rigidbody2D rb)
    {
        //chamado assim que o componente é adicionado no player
    }

    public virtual void RotateAround(Vector3 direction)
    {
        //controla a direção para a qual o componente aponta 
    }

    public virtual void Call()
    {
        //apetar o botão de input
    }

    public virtual void Release()
    {
        //soltar o botão de input
    }

    public virtual void Interrupt()
    {
        //desativa tudo quando recebe dano ou termina a partida
    }
}
