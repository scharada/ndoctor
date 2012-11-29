clear 

$path = 'C:\Users\jibedoubleve\AppData\Roaming\Probel\nDoctor\'
$backup = $path + 'Database - Copy.db'
$database = $path + 'Database.db'

Copy-Item -Path $backup -Destination $database -Force 
