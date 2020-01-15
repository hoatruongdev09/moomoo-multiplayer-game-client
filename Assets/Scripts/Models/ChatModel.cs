[System.Serializable]
public class ChatModel {
    public int id;
    public string text;

    public ChatModel (int id, string text) {
        this.id = id;
        this.text = text;
    }
}