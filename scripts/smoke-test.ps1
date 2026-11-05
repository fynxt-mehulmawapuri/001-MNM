# Smoke test for the MCP stdio server: start the exe, send initialize + tools/list, verify the reply.
param(
    [Parameter(Mandatory)] [string] $ExePath,
    [string] $ExpectedTool = 'GetMonkeyList',
    [int] $TimeoutSeconds = 30
)

$ErrorActionPreference = 'Stop'

$psi = [System.Diagnostics.ProcessStartInfo]::new($ExePath)
$psi.RedirectStandardInput = $true
$psi.RedirectStandardOutput = $true
$psi.RedirectStandardError = $true
$psi.UseShellExecute = $false

$process = [System.Diagnostics.Process]::Start($psi)

function Read-Response([int] $id) {
    $deadline = [DateTime]::UtcNow.AddSeconds($TimeoutSeconds)
    while ([DateTime]::UtcNow -lt $deadline) {
        $task = $process.StandardOutput.ReadLineAsync()
        if (-not $task.Wait($TimeoutSeconds * 1000)) { break }
        $line = $task.Result
        if ($null -eq $line) { throw 'Server closed stdout before responding.' }
        $msg = $line | ConvertFrom-Json
        if ($msg.id -eq $id) { return $msg }
    }
    throw "Timed out waiting for response to request $id."
}

try {
    $process.StandardInput.WriteLine('{"jsonrpc":"2.0","id":1,"method":"initialize","params":{"protocolVersion":"2024-11-05","capabilities":{},"clientInfo":{"name":"smoke-test","version":"1.0"}}}')
    $init = Read-Response 1
    if (-not $init.result) { throw "initialize failed: $($init | ConvertTo-Json -Depth 5 -Compress)" }
    Write-Host "initialize OK: server '$($init.result.serverInfo.name)'"

    $process.StandardInput.WriteLine('{"jsonrpc":"2.0","method":"notifications/initialized"}')
    $process.StandardInput.WriteLine('{"jsonrpc":"2.0","id":2,"method":"tools/list"}')
    $tools = Read-Response 2
    $names = @($tools.result.tools | ForEach-Object { $_.name })
    Write-Host "tools/list OK: $($names -join ', ')"

    if ($names -notcontains $ExpectedTool) { throw "Expected tool '$ExpectedTool' not found." }
    Write-Host 'Smoke test PASSED'
}
finally {
    if (-not $process.HasExited) { $process.Kill() }
}
