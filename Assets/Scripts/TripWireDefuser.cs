
public class TripWireDefuser : bombGoal
{
    public override void Defuse()
    {
        GameManager._instance.isDefusingTrap = true;

        base.Defuse();
    }

    public override void SetDefusedState()
    {
        GameManager._instance.isDefusingGrenade = false;
        Destroy(transform.parent.gameObject); 
        GameManager._instance.defuseLabel.SetActive(false);
    }
}