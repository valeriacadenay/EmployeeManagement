namespace Management.Domain.Shared.Errors;

public class DomainException : Exception { public DomainException(string m) : base(m) {} }