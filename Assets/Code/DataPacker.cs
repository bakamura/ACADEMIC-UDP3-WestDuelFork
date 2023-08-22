using System.IO;
using System.Linq;
using System.Net;
using UnityEngine;

public class DataPacker : MonoBehaviour {

    [Header("Players")]

    [SerializeField] private GameObject _playerPrefab;

    [Header("Cache")]

    private DataPack _dataPack = new DataPack();

    [SerializeField] private Rigidbody _rb;
    private float[] _floatArrayC = new float[3] { 0, 0, 0 };
    private byte[] _byteArrayC;

    private void Awake() {
        _dataPack.senderIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
    }

    private void FixedUpdate() {
        PreparePack();
        SendPack();
    }

    private void PreparePack() {
        _dataPack.senderPos = Vector3ToFloatArray(transform.position);
        _dataPack.senderVelocity = Vector3ToFloatArray(_rb.velocity);
    }

    private void SendPack() {
        DataPacking.memoryStream = new MemoryStream();
        DataPacking.binaryF.Serialize(DataPacking.memoryStream, _dataPack);
        _byteArrayC = DataPacking.memoryStream.ToArray();
        foreach(IPEndPoint ip in DataPacking.partnerIps) DataPacking.udpClient.Send(_byteArrayC, _byteArrayC.Length, ip);
    }

    public void AddReceiverIp(string receiverIp) {
        if (!IPAddress.TryParse(receiverIp, out _)) {
            Debug.LogWarning("Trying to Add Invalid IP Address!");
            return;
        }
        IPEndPoint ip = new IPEndPoint(IPAddress.Parse(receiverIp), 11000);
        if (!DataPacking.partnerIps.Contains(ip)) {
            Debug.LogWarning("Trying to Add IP that's already in partner list! (IP: " + ip.ToString() + ")");
            return;
        }
        DataPacking.partnerIps.Add(ip);
        DataPacking.ipToData.Add(ip, new DataPack());
        DataPacking.ipToRb.Add(ip, Instantiate(_playerPrefab).GetComponent<Rigidbody>()); //
    }

    private float[] Vector3ToFloatArray(Vector3 v3) {
        _floatArrayC[0] = v3.x;
        _floatArrayC[1] = v3.y;
        _floatArrayC[2] = v3.z;

        return _floatArrayC;
    }

}
