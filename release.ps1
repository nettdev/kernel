$regex = "<PackageVersion>[0-9]{1,5}.[0-9]{1,5}.[0-9]{1,5}<\/PackageVersion>"
$version=$args[0]
$packageVersion = "<PackageVersion>$($version)</PackageVersion>"

$presentation = (Get-Content "./Mobnet.SharedKernel.csproj") -replace $regex, $packageVersion

$presentation | Set-Content "./Mobnet.SharedKernel.csproj"