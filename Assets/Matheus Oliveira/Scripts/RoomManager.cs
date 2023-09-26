using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;

    [Header("Options")]
    public GameObject playerControllerPrefab;

    private void Awake()
    {
        instance = this;
    }
}
