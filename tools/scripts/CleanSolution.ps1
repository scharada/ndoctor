########################################################
# This script cleans the nDoctor solution. It removes 
# all the directory named "bin", "obj", "release" 
# and "debug". It also removes the .suo and .user
# files
########################################################
# Variables
########################################################
param($root = "c:\Projects\ndoctor\src\")
[int]$dirCount = 0
[int]$fileCount = 0
$items = get-childitem $root -Recurse -Force
########################################################
# Main
########################################################
clear

foreach($item in $items)
{
	$name = $item.Name.ToLower()
	
	if($item.Attributes -eq "Directory" -and ($name -eq "bin" -or $name -eq "obj" -or $name -eq "release" -or $name -eq "debug" -or $name -eq "packages"))
	{
			Write-Host "Deleting"$item.FullName -ForegroundColor Green
			Remove-Item $item.FullName -Force -Recurse
			$dirCount++
	}
	else
	{
		if($item.Extension -eq ".suo" -or $item.Extension -eq ".user")
		{
			Write-Host "Deleting"$item.FullName -ForegroundColor Cyan
			Remove-Item $item.FullName -Force
			$fileCount++
		}	
	}
}
""
"Cleaning done:"
"$dirCount folder(s) deleted"
"$fileCount file(s) deleted"
