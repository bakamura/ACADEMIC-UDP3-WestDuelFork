using System.Net;
using TMPro;
using UnityEngine;

public class NameTagApplier : MonoBehaviour {

    [SerializeField] private GameObject _nameTagPrefab;
    [SerializeField] private TextMeshProUGUI _nameBox;
    private TextMeshProUGUI _playerNameBox;

    private void Start() {
        _playerNameBox = Instantiate(_nameTagPrefab, FindObjectOfType<PlayerMovement>().transform).GetComponentInChildren<TextMeshProUGUI>();
        _playerNameBox.text = _nameBox.text;
    }

    public void ChangeName() {
        _playerNameBox.text = _nameBox.text;
        FindObjectOfType<DataPacker>().ChangeName(_nameBox.text);
    }

    public void ChangeNameTag(IPEndPoint ip, string newName) {
        DataPacking.ipToNametag[ip].text = newName;
    }

    public void CreateNameTag(Transform parentTransform, string name, IPEndPoint ip) {
        TextMeshProUGUI tmpui = Instantiate(_nameTagPrefab, parentTransform).GetComponentInChildren<TextMeshProUGUI>();
        tmpui.text = name;
        DataPacking.ipToNametag.Add(ip, tmpui);
    }

}
