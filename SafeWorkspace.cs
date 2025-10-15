using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace monacoeditor;

public sealed class SafeWorkspace
{
    static readonly string[] AllowedNamespaces = new[]
    {
"System","System.Linq","System.Collections.Generic","System.Threading","System.Threading.Tasks","Harmony.ScriptSdk"
};
    static readonly string[] BlockedNamespacePrefixes = new[]
    {
"System.IO","System.Net","System.Reflection","System.Runtime.InteropServices","Microsoft.Win32"
};
    public bool IsCodeSafe(string code)
    {
        var tree = CSharpSyntaxTree.ParseText(SourceText.From(code));
        var root = tree.GetRoot();
        foreach (var u in root.DescendantNodes().OfType<Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax>())
        {
            var ns = u.Name?.ToString();
            if (!AllowedNamespaces.Any(a => ns != null && ns.StartsWith(a, StringComparison.Ordinal)))
                return false;
        }
        foreach (var id in root.DescendantNodes().OfType<Microsoft.CodeAnalysis.CSharp.Syntax.IdentifierNameSyntax>())
        {
            var s = id.ToString();
            if (BlockedNamespacePrefixes.Any(p => s.StartsWith(p, StringComparison.Ordinal)))
                return false;
        }
        return true;
    }
    public ImmutableArray<MetadataReference> References()
    {
        var refs = new List<MetadataReference>
{
MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
MetadataReference.CreateFromFile(typeof(Task).Assembly.Location)
};
        var runtimeDir = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
        var names = new[] { "System.Runtime.dll", "System.Console.dll", "System.Linq.dll", "System.Collections.dll", "System.Private.CoreLib.dll" };
        foreach (var n in names)
        {
            var p = Path.Combine(runtimeDir, n);
            if (File.Exists(p)) refs.Add(MetadataReference.CreateFromFile(p));
        }
        return refs.ToImmutableArray();
    }
}