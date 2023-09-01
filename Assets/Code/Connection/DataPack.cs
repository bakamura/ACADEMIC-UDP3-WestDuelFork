[System.Serializable]
public class DataPack {

    public bool updated = true; // Used by unpacker to prevent updating when data isn't received, is always sent (true)
    public string senderName;
    public float[] senderPos;
    public float[] senderVelocity;

}
