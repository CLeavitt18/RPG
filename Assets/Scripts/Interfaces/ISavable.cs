
public interface ISavable
{
    bool Save(int id);
    bool Load(int id);
    void SetDefaultState(bool priority);
}
