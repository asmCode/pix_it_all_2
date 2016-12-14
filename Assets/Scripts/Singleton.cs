public class Singleton<T>
    where T : new()
{
    protected static T m_instance;

    public static T GetInstance()
    {
        if (m_instance == null)
            m_instance = new T();

        return m_instance;
    }
}
