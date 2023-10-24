using System;

[Serializable]
public class UserData
{
    public int userLevel;
    
    public int userCoin;

    public int userScore;

    public float userTimePlay;
    
    public UserData()
    {
        userLevel = 1;
        userCoin = 1000;
        userScore = 0;
        userTimePlay = 0;
    }

}
