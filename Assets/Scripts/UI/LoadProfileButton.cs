
public class LoadProfileButton : DoubleClickAction
{
    public PauseMenu Parent;

    public string ProfileName;

    protected override void DoubleAction()
    {
        Parent.LoadGame(ProfileName);
    }
}
