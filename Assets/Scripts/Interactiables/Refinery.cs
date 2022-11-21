using System.Text;

public class Refinery : Interactialbes, IInteractable
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
        RefineryUi.refineryUi.SetOpen(State);
        SetPlayerState(State);
    }
}
