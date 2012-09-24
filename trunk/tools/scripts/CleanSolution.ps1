param($root = "c:\Projects\ndoctor\src\")
$items = get-childitem $root -Recurse -Force
[int]$dirCount = 0
[int]$fileCount = 0

clear

foreach($item in $items)
{
	$name = $item.Name.ToLower()
	
	if($item.Attributes -eq "Directory")
	{		
		if($name -eq "bin" -or $name -eq "obj" -or $name -eq "release" -or $name -eq "debug")
		{
			Write-Host "Deleting"$item.FullName -ForegroundColor Green
			Remove-Item $item.FullName -Force -Recurse
			$dirCount++
		}
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
