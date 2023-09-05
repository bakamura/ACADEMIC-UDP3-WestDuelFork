using System.IO;
using System.Net;
using System.Threading;
using UnityEngine;

public class DataUnpacker : MonoBehaviour {

    [Header("Cache")]

    private Vector3 _v3C = Vector3.zero;
    private IPEndPoint _ipC;
    private NameTagApplier _applier;

    private void Awake() {
        Thread receiverThread = new Thread(ReceivePack);
        receiverThread.Start();
        _applier = FindObjectOfType<NameTagApplier>();
    }

    private void FixedUpdate() {
        ImplementPack();
    }

    public void ReceivePack() {
        while (true) {
            DataPacking.memoryStream = new MemoryStream(DataPacking.udpClient.Receive(ref _ipC));
            if(DataPacking.partnerIps.Contains(_ipC)) DataPacking.ipToData[_ipC] = (DataPack)DataPacking.binaryF.Deserialize(DataPacking.memoryStream);
        }
    }

    private void ImplementPack() {
        foreach (IPEndPoint ip in DataPacking.partnerIps) {
            if (DataPacking.ipToData[ip].updated) {
                DataPacking.ipToData[ip].updated = false;
                DataPacking.ipToRb[ip].gameObject.transform.position = FloatArrayToVector3(DataPacking.ipToData[ip].senderPos);
                DataPacking.ipToRb[ip].velocity = FloatArrayToVector3(DataPacking.ipToData[ip].senderVelocity);
                _applier.ChangeNameTag(ip, DataPacking.ipToData[ip].senderName);
            }
        }
    }

    private Vector3 FloatArrayToVector3(float[] floatArr) {
        _v3C[0] = floatArr[0];
        _v3C[1] = floatArr[1];
        _v3C[2] = floatArr[2];


        return _v3C;
    }

}
