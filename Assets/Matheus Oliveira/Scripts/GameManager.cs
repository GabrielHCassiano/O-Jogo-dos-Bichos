using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Criando uma instância do GameManager.
    public static GameManager instance;

    private void Awake()
    {
        // Garantindo que o GameManager não será deletado em transição de cena
        // e que se tiver 2 GameManagers, um será deletado.
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}
