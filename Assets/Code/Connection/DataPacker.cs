using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using UnityEngine;

public class DataPacker : MonoBehaviour {

    [Header("Players")]

    [SerializeField] private GameObject _playerPrefab;

    [Header("Cache")]

    private DataPack _dataPack = new DataPack();

    [SerializeField] private Rigidbody _playerRb;
    private float[] _floatArrayC = new float[3] { 0, 0, 0 };
    private byte[] _byteArrayC;

    private void FixedUpdate() {
        PreparePack();
        SendPack();
    }

    private void PreparePack() {
        _dataPack.senderPos = Vector3ToFloatArray(_playerRb.transform.position);
        _dataPack.senderVelocity = Vector3ToFloatArray(_playerRb.velocity);
        print($"pos: {new Vector3(_dataPack.senderPos[0], _dataPack.senderPos[1], _dataPack.senderPos[2])} / vel: {new Vector3(_dataPack.senderVelocity[0], _dataPack.senderVelocity[1], _dataPack.senderVelocity[2])}");
    }

    private void SendPack() {
        DataPacking.memoryStream = new MemoryStream();
        DataPacking.binaryF.Serialize(DataPacking.memoryStream, _dataPack);
        _byteArrayC = DataPacking.memoryStream.ToArray();
        foreach (IPEndPoint ip in DataPacking.partnerIps) DataPacking.udpClient.Send(_byteArrayC, _byteArrayC.Length, ip);
    }

    public void AddReceiverIp(string receiverIp) {
        receiverIp = Regex.Replace(receiverIp, @"[^0-9.]", "");
        // Debug.Log(receiverIp.Length);

        if (IPAddress.TryParse(receiverIp, out IPAddress receiverAddress)) {
            IPEndPoint ip = new IPEndPoint(receiverAddress, 11000);
            if (DataPacking.partnerIps.Contains(ip)) {
                Debug.LogWarning("Trying to Add IP that's already in partner list! (IP: " + ip.ToString() + ")");
                return;
            }
            DataPacking.partnerIps.Add(ip);
            DataPacking.ipToData.Add(ip, new DataPack()); //
            GameObject go = Instantiate(_playerPrefab);
            DataPacking.ipToRb.Add(ip, go.GetComponent<Rigidbody>()); //
            FindObjectOfType<NameTagApplier>().CreateNameTag(go.transform, DataPacking.ipToData[ip].senderName, ip);
        }
        else Debug.LogWarning("Trying to Add Invalid IP Address!");
    }

    public void ChangeName(string newName) {
        _dataPack.senderName = newName;
    }

    private float[] Vector3ToFloatArray(Vector3 v3) {
        _floatArrayC[0] = v3.x;
        _floatArrayC[1] = v3.y;
        _floatArrayC[2] = v3.z;

        return new float[3] { v3[0], v3[1], v3[2] };
    }

    private void OnApplicationQuit() {
        DataPacking.udpClient.Close();
    }
}
