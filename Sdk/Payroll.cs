using System.Collections.Concurrent;

namespace Harmony.ScriptSdk;

public static class Payroll
{
    static readonly ConcurrentDictionary<int, Models.Employee> _employees = new(new[]
    {
        new KeyValuePair<int,Models.Employee>(1,new Models.Employee(1,"Alex",80000)),
        new KeyValuePair<int,Models.Employee>(2,new Models.Employee(2,"Jamie",95000))
    });
    static readonly ConcurrentDictionary<Guid, Models.Paystub> _paystubs = new();
    public static Task<IReadOnlyList<Models.Employee>> GetAllEmployeesAsync()
        => Task.FromResult<IReadOnlyList<Models.Employee>>(_employees.Values.ToList());
    public static Task<decimal> GetSalaryByEmployeeIdAsync(int employeeId)
        => Task.FromResult(_employees.TryGetValue(employeeId, out var e) ? e.Salary : 0m);
    public static Task<Models.Paystub?> GetPaystubByIdAsync(Guid id)
        => Task.FromResult(_paystubs.GetValueOrDefault(id));
}