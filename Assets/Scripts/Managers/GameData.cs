public class GameData {

    private static GameData instance = null;

    // /////////
    // Variables
    // /////////

    private int m_TotalPegs = 3;
    private int m_TotalDiscs = 3;

    /// <summary>
    /// Constructor
    /// </summary>
    private GameData() { }

    /// <summary>
    /// Gets instances
    /// </summary>
    /// <returns>Instance of GameData</returns>
    public static GameData GetInstance()
    {
        if (instance == null)
        {
            instance = new GameData();
        }
        return instance;
    }

    // /////////////////
    // Getters & Setters
    // /////////////////

    public int TotalPegs
    {
        get
        {
            return m_TotalPegs;
        }
        set
        {
            m_TotalPegs = value;
        }
    }

    public int TotalDiscs
    {
        get
        {
            return m_TotalDiscs;
        }
        set
        {
            m_TotalDiscs = value;
        }
    }
}
