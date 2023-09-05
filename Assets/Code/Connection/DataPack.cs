[System.Serializable]
public class DataPack {

    public bool updated = true; // Used by unpacker to prevent updating when data isn't received, is always sent (true)
    public string senderName = "";
    public float[] senderPos = new float[3] { 0, 0, 0 };
    public float[] senderVelocity = new float[3] { 0, 0, 0 };

}
