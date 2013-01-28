namespace CallWall.Contract
{
    public sealed class PersonalIdentifier : IPersonalIdentifier
    {
        private readonly IProviderDescription _provider;
        private readonly string _identifierType;
        private readonly string _value;

        public PersonalIdentifier(string identifierType, string value, IProviderDescription providerDescription)
        {
            _identifierType = identifierType;
            _value = value;
            _provider = providerDescription;
        }

        public IProviderDescription Provider { get { return _provider; } }

        public string IdentifierType { get { return _identifierType; } }

        public string Value { get { return _value; } }
    }
}