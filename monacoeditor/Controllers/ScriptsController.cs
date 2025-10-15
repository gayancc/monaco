using Harmony.ScriptSdk;
using Microsoft.AspNetCore.Mvc;

namespace monacoeditor.Controllers;

[ApiController]
[Route("api/scripts")]
public sealed class ScriptsController : ControllerBase
{
    private readonly ScriptCompiler _compiler;
    public ScriptsController(ScriptCompiler compiler) => _compiler = compiler;


    public sealed record CodeDto(string code);


    [HttpPost("diagnostics")]
    public async Task<IActionResult> Diagnostics([FromBody] CodeDto dto, CancellationToken ct)
    {
        var (diags, ok) = _compiler.GetDiagnostics(dto.code, ct);
        var res = diags.Select(d => new { id = d.Id, sev = d.Severity.ToString(), msg = d.GetMessage() });
        return Ok(new { ok, diagnostics = res });
    }


    [HttpPost("run")]
    public async Task<IActionResult> Run([FromBody] CodeDto dto, CancellationToken ct)
    {
        var result = await _compiler.ExecuteAsync(dto.code, new Globals(ct), TimeSpan.FromSeconds(5), ct);
        return Ok(new { result });
    }
}