public abstract class Object
{
    public int id;
    public string name;
}

[System.Serializable]
public class User : Object
{
    public string email;
    public string password;
    public string date_created;
}