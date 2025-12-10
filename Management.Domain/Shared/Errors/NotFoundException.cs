namespace Management.Domain.Shared.Errors;

public class NotFoundException : DomainException { public NotFoundException(string m) : base(m) {} }