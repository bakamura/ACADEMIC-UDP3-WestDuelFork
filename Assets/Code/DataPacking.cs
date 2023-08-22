using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class DataPacking {

    [Header("Cache")]

    [HideInInspector] public static List<IPEndPoint> partnerIps = new List<IPEndPoint>();
    [HideInInspector] public static Dictionary<IPEndPoint,DataPack> ipToData = new Dictionary<IPEndPoint, DataPack>();
    [HideInInspector] public static Dictionary<IPEndPoint, Rigidbody> ipToRb = new Dictionary<IPEndPoint, Rigidbody>();
    [HideInInspector] public static UdpClient udpClient = new UdpClient(11000);
    [HideInInspector] public static MemoryStream memoryStream;
    [HideInInspector] public readonly static BinaryFormatter binaryF = new BinaryFormatter();


}
