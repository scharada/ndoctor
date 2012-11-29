clear 

$path = 'C:\Users\jibedoubleve\AppData\Roaming\Probel\nDoctor\'
$backup = $path + 'Database - Copy.db'
$database = $path + 'Database.db'

Write-Host 'Replacing current database with the backup...' -NoNewline
Copy-Item -Path $backup -Destination $database -Force 
Write-Host 'Done'
