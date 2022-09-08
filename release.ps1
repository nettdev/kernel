$regex = "<PackageVersion>[0-9]{1,5}.[0-9]{1,5}.[0-9]{1,5}<\/PackageVersion>"
$version=$args[0]
$packageVersion = "<PackageVersion>$($version)</PackageVersion>"

$presentation = (Get-Content "./src/SharedKernel.csproj") -replace $regex, $packageVersion

$presentation | Set-Content "./src/SharedKernel.csproj"