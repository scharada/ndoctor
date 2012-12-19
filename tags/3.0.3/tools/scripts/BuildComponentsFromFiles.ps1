################################################################################
# Fill Setup.wxs with all the components that contain the nDoctor files
################################################################################
clear
$cmdPath = Split-Path($MyInvocation.MyCommand.Path)
$root = "$cmdPath\..\..\src\Setup\"
$token = '<!--<REPLACEMENT>-->'
$text = Get-Content $root'Files.wxs'
################################################################################
# Apply regex and build the file content
################################################################################
$matches = $text -match '.*Component.Id="(?<component>.*).Guid.*'
foreach( $m in $matches)
{
	$line= $m -replace '.*Component.Id="(?<component>.*).Guid.*', '<ComponentRef Id="${component}/>'
	$str +=[string]::Format("{0}{1}", $line, [Environment]::NewLine)
}

$destinationContent = Get-Content $root"Setup.wxs.template"
$newText = $destinationContent -replace $token, $str
################################################################################
# Add license file
################################################################################
cp "$root\license.rtf" $env:NEST
################################################################################
# Build the new file
################################################################################
Write-Host "Write content into $root$template..." -NoNewline
Set-Content -Path $root"Setup.wxs" -Value $newText
Write-Host " Done"
################################################################################
# Get application version
################################################################################
Write-Host "Update the version for the installer... " -NoNewline

$versionPath = "$cmdPath\..\..\src\Version.cs"
$versionPattern = '(?m:)\d+\.\d+\.\d+'

$version = Select-String -path $versionPath -Pattern $versionPattern -AllMatches | % { $_.Matches } | % { $_.Value } 

$setupwxs = Get-Content $root'Setup.wxs'
$newSetupWxs = $setupwxs -replace '\sVersion.?=.?"\d+\.\d+\.\d+\.\d+"', " Version=`"$version`""

Set-Content -Path $root'Setup.wxs' -Value $newSetupWxs
Write-Host "Done"
