$regex = "<PackageVersion>[0-9]{1,5}.[0-9]{1,5}.[0-9]{1,5}<\/PackageVersion>"
$version=$args[0]
$packageVersion = "<PackageVersion>$($version)</PackageVersion>"

$presentation = (Get-Content "./src/Kernel.csproj") -replace $regex, $packageVersion

$presentation | Set-Content "./src/Kernel.csproj"