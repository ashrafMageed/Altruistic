namespace Altruistic
{
    public interface ICreateMock
    {

        MockingWrapper<T> Get<T>() where T : class;
    }
}
