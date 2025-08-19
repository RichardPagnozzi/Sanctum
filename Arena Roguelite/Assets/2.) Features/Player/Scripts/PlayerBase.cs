using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public PlayerDetails CurrentPlayerDetails { get; private set; }


    public bool TryGetPlayerDetails()
    {
        CurrentPlayerDetails = GameManager.Instance.PlayerRepository.CurrentSessionPlayerDetails;
       
        if (CurrentPlayerDetails == null)
            return false;
        
        return true;
    }
}