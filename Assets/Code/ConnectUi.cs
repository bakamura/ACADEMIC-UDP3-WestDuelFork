using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConnectUi : MonoBehaviour {

    [SerializeField] private GameObject _connectMenu;
    [SerializeField] private TextMeshProUGUI _inputField;
    [SerializeField] private DataPacker _packer;

    public void ShowConnectMenu() {
        _connectMenu.SetActive(!_connectMenu.activeSelf);
    }

    public void ConnectToIp() {
        _packer.AddReceiverIp(_inputField.text);
    }
}
