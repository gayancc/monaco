using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace monacoeditor;

public sealed class ScriptCompiler
{
    readonly SafeWorkspace _safe;
    readonly ScriptOptions _options;
    public ScriptCompiler(SafeWorkspace safe)
    {
        _safe = safe;
        _options = ScriptOptions.Default
            .WithReferences(_safe.References())
            .WithImports("System", "System.Linq", "System.Collections.Generic", "System.Threading.Tasks", "Harmony.ScriptSdk");
    }
    public (IReadOnlyList<Diagnostic> diagnostics, bool ok) GetDiagnostics(string code, CancellationToken ct)
    {
        if (!_safe.IsCodeSafe(code)) return (new List<Diagnostic> { Diagnostic.Create(new DiagnosticDescriptor("SAFE001", "Unsafe", "Code violates allow-list", "Security", DiagnosticSeverity.Error, true), Location.None) }, false);
        var script = CSharpScript.Create(code, _options, typeof(Harmony.ScriptSdk.Globals));
        var comp = script.GetCompilation();
        var diags = comp.GetDiagnostics(ct);
        return (diags, diags.All(d => d.Severity != DiagnosticSeverity.Error));
    }
    public async Task<object?> ExecuteAsync(string code, Harmony.ScriptSdk.Globals globals, TimeSpan timeout, CancellationToken ct)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        cts.CancelAfter(timeout);
        if (!_safe.IsCodeSafe(code)) throw new InvalidOperationException("Unsafe code");
        var script = CSharpScript.Create(code, _options, typeof(Harmony.ScriptSdk.Globals));
        var state = await script.RunAsync(globals, cts.Token);
        return state.ReturnValue;
    }
}


