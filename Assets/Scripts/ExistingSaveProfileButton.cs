
public class ExistingSaveProfileButton : DoubleClickAction
{
    public PauseMenu Parent;

    public string ProfileName;

    protected override void DoubleAction()
    {
        Parent.SaveGameOverExistingProfile(ProfileName);
    }
}
