using Zenject;

public class TransferDataInstaller : MonoInstaller
{
    private readonly TransferData TestData = new();

    public override void InstallBindings()
    {
        Container.BindInstance(TestData).WithId("TestData");
    }
}
