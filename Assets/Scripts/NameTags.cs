using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameTags : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private PlayerManager player;

    // Start is called before the first frame update
    void Start()
    {
        SetName();
    }

    private void SetName() => nameText.text = player.username;
}
