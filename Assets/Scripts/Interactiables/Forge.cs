using System.Text;

public class Forge : Interactialbes, IInteractable
{
    public override void SetUiOpen()
    {
        StringBuilder sb = new StringBuilder(GlobalValues.InterationKey);
        sb.Append(": ");
        sb.Append(GlobalValues.UseText);
        sb.Append(' ');
        sb.Append(Name);

        PlayerInstructionText.text = sb.ToString();

        base.SetUiOpen();
    }

    public void Interact(bool State)
    {
        ForgeUI.forgeUi.SetOpen(State);
        SetPlayerState(State);
    }
}
