namespace Harmony.ScriptSdk;

public class Models
{
   public sealed record Employee(int Id, string Name, decimal Salary);
    public sealed record Paystub(Guid Id, int EmployeeId, decimal Net);
}

public sealed class Globals
{
    public CancellationToken CancellationToken { get; }
    public Globals(CancellationToken ct) { CancellationToken = ct; }
}