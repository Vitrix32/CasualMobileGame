using System;

public class MiscEvents
{
    public event Action onEnterCombat;
    public void EnterCombat() 
    {
        if (onEnterCombat != null) 
        {
            EnterCombat();
        }
    }
}
