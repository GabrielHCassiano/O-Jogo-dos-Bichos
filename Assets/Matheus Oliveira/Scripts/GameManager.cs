using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Criando uma inst�ncia do GameManager.
    public static GameManager instance;

    private void Awake()
    {
        // Garantindo que o GameManager n�o ser� deletado em transi��o de cena
        // e que se tiver 2 GameManagers, um ser� deletado.
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}
