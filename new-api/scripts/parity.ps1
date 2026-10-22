# scripts/parity.ps1 — Fase 5: rodar a coleção de requests contra a API C# e a
# Go e apontar divergências de status/corpo. Detalhes em docs/parity-requests.md.
#
# Lições da rodada suja (NÃO repetir):
# 1. `dotnet run --project <caminho>` usa o DIRETÓRIO DO PROJETO como cwd —
#    `Data Source=data/finndb.sqlite` é relativo a ele. Para não tocar o banco
#    real (api/data/finndb.sqlite), copiamos o código do `api/` para um
#    diretório temporário e rodamos essa CÓPIA (robocopy exclui bin/obj/
#    wwwroot/.env/api/data).
# 2. PowerShell 5.1 mutila argumentos JSON passados a curl.exe (aspas somem).
#    Os corpos vão para ARQUIVOS temporários (UTF-8 sem BOM) e usam `-d @arquivo`.
# 3. wwwroot é excluído da cópia C# e WEB_ROOT da Go aponta para algo
#    inexistente: o request [33] compara o ramo "SPA fallback sem wwwroot" (404).
$ErrorActionPreference = "Continue"

$raizGo = Split-Path -Parent $PSScriptRoot                  # ...\finn\new-api (projeto Go)
$repositorio = Split-Path -Parent $raizGo                   # ...\finn
$fonteApi = Join-Path $repositorio "api"
$trabalho = Join-Path $env:TEMP "opencode\parity\$(Get-Date -Format 'yyyyMMdd-HHmmss')"
New-Item -ItemType Directory -Force $trabalho | Out-Null
$arqLog = Join-Path $trabalho "log.txt"

function Log-Escreve($s) { Add-Content -Path $arqLog -Value $s; Write-Host $s }

$diretoriaC = Join-Path $trabalho "cs\api"                   # CÓPIA do projeto C#
$diretoriaGo = Join-Path $trabalho "go"
New-Item -ItemType Directory -Force $diretoriaGo | Out-Null

$portC = 18101
$portGo = 18102
$webUrl = "http://localhost:3000"

# ---- env para as duas APIs (chaves mortas: caminhos de aportes exercem erro) ----
$env:PORT = ""
$env:WEB_URL = $webUrl
$env:B3_API_URL = "https://brapi.dev/api/quote"
$env:B3_API_KEY = "chave-invalida-paridade"
$env:CRYPTO_API_URL = "https://api.coingecko.com/api/v3/simple/price"
$env:CRYPTO_API_KEY = "chave-invalida-paridade"

# ---- coleção (docs/parity-requests.md) ----
$pedidos = @(
    @{ n = "01"; m = "GET";    u = "/api/saldos?saldoNome=Corrente" }
    @{ n = "02"; m = "GET";    u = "/api/saldos?saldoNome=Investimentos" }
    @{ n = "03"; m = "GET";    u = "/api/saldos?saldoNome=Zzz" }
    @{ n = "04"; m = "POST";   u = "/api/saldos"; b = '{"nome":"Corrente","valor":"12.34"}' }
    @{ n = "05"; m = "GET";    u = "/api/saldos?saldoNome=Corrente" }
    @{ n = "06"; m = "GET";    u = "/api/salario" }
    @{ n = "07"; m = "POST";   u = "/api/salario"; b = '{"valor":"100000.00"}' }
    @{ n = "08"; m = "GET";    u = "/api/salario" }
    @{ n = "09"; m = "POST";   u = "/api/salario"; b = '{"valor":"-1"}' }
    @{ n = "10"; m = "POST";   u = "/api/salario/adicionar"; b = '{}' }
    @{ n = "11"; m = "GET";    u = "/api/movimentacoes/categorias" }
    @{ n = "12"; m = "POST";   u = "/api/movimentacoes"; b = '{"valor":"25.50","tipo":0,"categoriaId":1,"descricao":"mercado","data":"2026-03-15"}' }
    @{ n = "13"; m = "GET";    u = "/api/movimentacoes?inicio=2026-03-01&fim=2026-03-31" }
    @{ n = "14"; m = "POST";   u = "/api/movimentacoes"; b = '{"valor":"300.00","tipo":0,"categoriaId":6,"descricao":"aluguel","data":"2026-04-10","quantidadeParcelas":3,"primeiroVencimento":"2026-05-01"}' }
    @{ n = "15"; m = "GET";    u = "/api/movimentacoes/pendentes" }
    @{ n = "16"; m = "POST";   u = "/api/movimentacoes/pendentes"; b = '{}' }
    @{ n = "17"; m = "GET";    u = "/api/movimentacoes/pendentes" }
    @{ n = "18"; m = "POST";   u = "/api/movimentacoes"; b = '{"valor":"19.90","tipo":0,"categoriaId":5,"descricao":"streaming","persistente":true}' }
    @{ n = "19"; m = "POST";   u = "/api/movimentacoes/persistentes"; b = '{"valor":"21.90","descricao":"net+","tipo":0,"categoriaId":5}' }
    @{ n = "20"; m = "GET";    u = "/api/movimentacoes/persistentes/2" }
    @{ n = "21"; m = "PUT";    u = "/api/movimentacoes/persistentes"; b = '{"persistenteId":1,"valor":"29.90","descricao":"net premium","categoriaId":4}' }
    @{ n = "22"; m = "GET";    u = "/api/movimentacoes/persistentes/1" }
    @{ n = "23"; m = "DELETE"; u = "/api/movimentacoes/persistentes/1" }
    @{ n = "24"; m = "GET";    u = "/api/movimentacoes/persistentes/1" }
    @{ n = "25"; m = "GET";    u = "/api/movimentacoes/persistentes/999" }
    @{ n = "26"; m = "DELETE"; u = "/api/movimentacoes/999" }
    @{ n = "27"; m = "DELETE"; u = "/api/movimentacoes/2" }
    @{ n = "28"; m = "POST";   u = "/api/aportes"; b = '{"identificador":"PETR4","quantidade":100,"preco":10.5,"categoria":1,"dataCompra":"2026-01-05"}' }
    @{ n = "29"; m = "POST";   u = "/api/aportes/PETR4"; b = '{"quantidade":50,"tipoStr":"FOO"}' }
    @{ n = "30"; m = "GET";    u = "/api/aportes" }
    @{ n = "31"; m = "GET";    u = "/api/aportes/categorias" }
    @{ n = "32"; m = "GET";    u = "/api/inexistente" }
    @{ n = "33"; m = "GET";    u = "/" }
)

$regexTS = '\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(\.\d+)?(Z|[-+]\d{2}:\d{2})?'
$arquivosCorpo = @()
$encUtf8 = New-Object System.Text.UTF8Encoding($false)

function Normalizar-Corpo($corpo) {
    # C# serializa non-ASCII como \uXXXX; Go manda UTF-8 cru (decisão: manter
    # raw). Decodificamos os \uXXXX para comparar conteúdo.
    $corpo = [regex]::Replace($corpo, '\\u(?<h>[0-9a-fA-F]{4})', {
        param($m)
        return [char][Convert]::ToInt32($m.Groups['h'].Value, 16)
    })
    # B5 (escala decimal, aceite): C# preserva "25.50"/"0.0"/"100000.00";
    # Go normaliza. Trimamos zeros à direita DOS LADOS (no-op na Go) e
    # hashtags de timestamps viram <TS>.
    $corpo = [regex]::Replace($corpo, '\.(\d*?[1-9])0+(?=\D|$)', '.$1')
    $corpo = [regex]::Replace($corpo, '\.0+(?=\D|$)', '')
    return ($corpo -replace $script:regexTS, '<TS>').Trim()
}

# Envia via curl.exe; corpo JSON vira ARQUIVO (PS 5.1 mutila JSON em argv) e
# o CORPO DE RESPOSTA também vai para arquivo — o stdout do PS completa
# non-ASCII (mojibake); só o marcador de código, SI dono só ASCII, vira piping.
function Enviar($porta, $p) {
    $url = "http://localhost:$porta$($p.u)"
    $arquivoSaida = Join-Path $script:trabalho ("resposta-{0}-{1}.txt" -f $p.n, (Get-Random))
    $argumentos = @("-s", "-w", "[[CODIGO:%{http_code}]]", "-X", $p.m, $url, "-o", $arquivoSaida)
    if ($p.b) {
        $arquivo = Join-Path $script:trabalho ("corpo-{0}-{1}.json" -f $p.n, $p.m)
        [System.IO.File]::WriteAllText($arquivo, $p.b, $script:encUtf8)
        $script:arquivosCorpo += $arquivo
        $argumentos += @("-H", "Content-Type: application/json", "-d", "@$arquivo")
    }
    $saida = & curl.exe @argumentos 2>$null | Out-String
    $codigo = ""
    $corpo = ""
    if ($saida -match '\[\[CODIGO:(\d+)\]\]') {
        $codigo = $Matches[1]
    }
    if (Test-Path $arquivoSaida) {
        $corpo = [System.IO.File]::ReadAllText($arquivoSaida, $script:encUtf8)
    }
    return @{ codigo = $codigo; corpo = (Normalizar-Corpo $corpo) }
}

# ---- build Go ----
Log-Escreve "== compilando Go..."
Push-Location $raizGo
& go build -o (Join-Path $trabalho "go-api.exe") ./cmd/api
$goBuildOk = ($LASTEXITCODE -eq 0)
Pop-Location
if (-not $goBuildOk) { Log-Escreve "build Go falhou"; exit 1 }

# ---- C#: cópia isolada do projeto (NUNCA rodar dotnet contra api\ real) ----
Log-Escreve "== copiando api\ -> $diretoriaC (sem bin/obj/wwwroot/data/.env)..."
# /XD data: SEM a exclusão do diretório data\ a cópia levaria o banco real
# (api\data\finndb.sqlite) e a "DB fresca" do C# viraria o banco do usuário.
$rbc = robocopy $fonteApi $diretoriaC /E /NFL /NDL /NJH /NJS /NP /XD bin obj wwwroot data .git "/XF" ".env" "*.env" 2>&1
if ($LASTEXITCODE -ge 8) {
    Log-Escreve "robocopy falhou (exit $LASTEXITCODE):"
    $rbc | Select-Object -Last 10 | ForEach-Object { Log-Escreve ("  " + $_) }
    exit 1
}
if (Test-Path (Join-Path $diretoriaC "wwwroot")) { Log-Escreve "ERRO: wwwroot entrou na copia"; exit 1 }
if (Test-Path (Join-Path $diretoriaC "data"))    { Log-Escreve "ERRO: data entrou na copia"; exit 1 }
# data\ é excluída em nome do banco real, mas EF precisa da PASTA para criar
# o SQLite (SQLite Error 14 sem ela — e o POST movimentações async-void mata
# o processo).
New-Item -ItemType Directory -Force (Join-Path $diretoriaC "data") | Out-Null

Log-Escreve "== build C# (Release, copia isolada)..."
Push-Location $diretoriaC
& dotnet build -c Release --nologo -v q 2>&1 | Out-Null
$buildOk = ($LASTEXITCODE -eq 0)
Pop-Location
if (-not $buildOk) { Log-Escreve "build C# falhou"; exit 1 }

# ---- C#: migra (SQLite fresco dentro da cópia) + sobe ----
Log-Escreve "== C# --migrate (projeto copiado = data\ nap copia)..."
$env:PORT = "$portC"
Push-Location $diretoriaC
$migrado = & dotnet run -c Release --no-build -- --migrate 2>&1
Pop-Location
$migrado | Select-Object -Last 2 | ForEach-Object { Log-Escreve ("  cs: " + $_) }
if (-not (Test-Path (Join-Path $diretoriaC "data\finndb.sqlite"))) {
    Log-Escreve "AVISO: banco C# nao apareceu em $diretoriaC\data\finndb.sqlite"
}

Log-Escreve "== subindo API C# na porta $portC..."
$procC = Start-Process -FilePath "dotnet" -ArgumentList @("run", "-c", "Release", "--no-build") `
    -WorkingDirectory $diretoriaC -PassThru `
    -RedirectStandardOutput (Join-Path $trabalho "cs-stdout.log") `
    -RedirectStandardError (Join-Path $trabalho "cs-stderr.log")

Log-Escreve "== subindo API Go na porta $portGo..."
$env:PORT = "$portGo"
$env:DB_PATH = Join-Path $diretoriaGo "data\finn.sqlite"
$env:WEB_ROOT = Join-Path $trabalho "wwwroot-inexistente"
$procGo = Start-Process -FilePath (Join-Path $trabalho "go-api.exe") -PassThru `
    -RedirectStandardOutput (Join-Path $trabalho "go-stdout.log") `
    -RedirectStandardError (Join-Path $trabalho "go-stderr.log")

function Esperar($porta, $proc) {
    foreach ($i in 1..90) {
        try {
            $tcp = New-Object Net.Sockets.TcpClient("localhost", $porta)
            $tcp.Close()
            return $true
        } catch {
            if ($proc.HasExited) { return $false }
            Start-Sleep -Milliseconds 500
        }
    }
    return $false
}
if (-not (Esperar $portC $procC)) {
    Log-Escreve "API C# nao subiu; logs:"
    Get-Content (Join-Path $trabalho "cs-stdout.log") -ErrorAction SilentlyContinue | ForEach-Object { Log-Escreve ("  cs-out: " + $_) }
    Get-Content (Join-Path $trabalho "cs-stderr.log") -ErrorAction SilentlyContinue | ForEach-Object { Log-Escreve ("  cs-err: " + $_) }
}
if (-not (Esperar $portGo $procGo)) {
    Log-Escreve "API Go nao subiu; logs:"
    Get-Content (Join-Path $trabalho "go-stdout.log") -ErrorAction SilentlyContinue | ForEach-Object { Log-Escreve ("  go-out: " + $_) }
    Get-Content (Join-Path $trabalho "go-stderr.log") -ErrorAction SilentlyContinue | ForEach-Object { Log-Escreve ("  go-err: " + $_) }
}

# ---- coleção + diff ----
$diverg = 0
foreach ($p in $pedidos) {
    $rc = Enviar $portC $p
    $rg = Enviar $portGo $p
    if (($rc.codigo -eq $rg.codigo) -and ($rc.corpo -eq $rg.corpo)) {
        Log-Escreve ("[{0}] {1} {2} -> {3} IGUAL" -f $p.n, $p.m, $p.u, $rc.codigo)
    } else {
        $diverg++
        Log-Escreve ("[{0}] {1} {2} DIVERGE (cs={3} go={4})" -f $p.n, $p.m, $p.u, $rc.codigo, $rg.codigo)
        Log-Escreve ("    cs: {0}" -f $rc.corpo)
        Log-Escreve ("    go: {0}" -f $rg.corpo)
    }
}

Log-Escreve ""
if ($diverg -eq 0) { Log-Escreve "== PARIDADE TOTAL na colecao ($($pedidos.Count) requests)" }
else { Log-Escreve "== $diverg divergencias de $($pedidos.Count). Log completo: $arqLog" }

Stop-Process -Id $procC.Id -Force -ErrorAction SilentlyContinue
Stop-Process -Id $procGo.Id -Force -ErrorAction SilentlyContinue
