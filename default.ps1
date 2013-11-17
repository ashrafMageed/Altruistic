#http://patrick.lioi.net/2013/03/19/socks-then-shoes/

Framework '4.0'

properties {
  $project = 'Altruistic'
  $configuration = 'Release'
  $src = resolve-path '.\src'
}

task default -depends Test

task Test -depends Compile {
    $fixieRunner = join-path $src "\packages\Fixie.0.0.1.106\lib\net45\Fixie.console.exe"
    exec { & $fixieRunner $src\$project.Tests\bin\$configuration\$project.Tests.dll }

    if ($lastexitcode -gt 0)
    {
        throw "{0} unit tests failed." -f $lastexitcode
    }
    if ($lastexitcode -lt 0)
    {
        throw "Unit test run was terminated by a fatal error."
    }
}

task Compile {
  exec { msbuild /t:clean /v:q /nologo /p:Configuration=$configuration $src\$project.sln }
  exec { msbuild /t:build /v:q /nologo /p:Configuration=$configuration $src\$project.sln }
}