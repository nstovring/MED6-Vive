using UnityEngine.EventSystems;

public interface ICustomMessageTarget : IEventSystemHandler
{
    // functions that can be called via the messaging system
    void LogData(SurfaceAudioPlayer cube, int testType);
    void LogData(SurfaceAudioPlayer.DataLogged cube);
    void LogData(string data);
    void LogError(string data);
    void GetRandomNewPath();
}
