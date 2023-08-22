using System.IO;
using System.Net;
using System.Threading;
using UnityEngine;

public class DataUnpacker : MonoBehaviour {

    [Header("Cache")]

    private Vector3 _v3C;
    private IPEndPoint _ipC;

    private void Awake() {
        Thread receiverThread = new Thread(ReceivePack);
        receiverThread.Start();
    }

    private void FixedUpdate() {
        ImplementPack();
    }

    public void ReceivePack() {
        foreach (IPEndPoint ip in DataPacking.partnerIps) {
            _ipC = ip;
            DataPacking.memoryStream = new MemoryStream(DataPacking.udpClient.Receive(ref _ipC));
            DataPacking.ipToData[ip] = (DataPack)DataPacking.binaryF.Deserialize(DataPacking.memoryStream);
        }
    }

    private void ImplementPack() {
        foreach (IPEndPoint ip in DataPacking.partnerIps) {
            DataPacking.ipToRb[ip].gameObject.transform.position = FloatArrayToVector3(DataPacking.ipToData[ip].senderPos);
            DataPacking.ipToRb[ip].velocity = FloatArrayToVector3(DataPacking.ipToData[ip].senderVelocity);
            DataPacking.ipToData[ip].updated = false;
        }
    }

    private Vector3 FloatArrayToVector3(float[] floatArr) {
        _v3C[0] = floatArr[0];
        _v3C[1] = floatArr[1];
        _v3C[2] = floatArr[2];

        return _v3C;
    }

}
