namespace Altruistic
{
    public class TestObjectCreator : ICreateTestObject
    {
        private readonly IObjectBuilderAdapter _objectBuilder;

        public TestObjectCreator(IObjectBuilderAdapter objectBuilder)
        {
            _objectBuilder = objectBuilder;
        }

        public TObject CreateDummy<TObject>()
        {
            return _objectBuilder.CreateNew<TObject>();
        }
    }
}
