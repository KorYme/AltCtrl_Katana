public class TutoGameMode : GameMode
{
    public override GamemodeType Type => GamemodeType.Tutorial;
    public override void StartGameMode()
    {
    }

    public override void StopGameMode()
    {
        base.StopGameMode();
    }

    public override void UpdateGameMode(float deltaTime)
    {
    }
}
