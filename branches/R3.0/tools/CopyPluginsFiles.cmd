@echo off
:: ====================================================
:: This script copy the plugins' files into the main
:: application's plugins directory. 
:: ====================================================

::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: Set this variable to have the root of the solution 
:: files
::::::::::::::::::::::::::::::::::::::::::::::::::::::::

set "prj=%~dp0..\src" rem Get the directory of the executing script
set "root=%prj%\Plugins"

echo prj: %prj%
echo root:%root%
::::::::::::::::::::::::::::::::::::::::::::::::::::::::
set "releaseMode=Debug"
set "pluginName=Debug"
set "return=start"

:refresh
set "directory=%prj%\View\Probel.NDoctor.View.Core\bin\%releaseMode%\Plugins\%pluginName%"
set "directoryFr=%directory%\fr

set "pluginDll=%root%\Probel.NDoctor.Plugins.%pluginName%\bin\%releaseMode%\Probel.NDoctor.Plugins.%pluginName%.dll"

set "rsxHelper=%root%\Probel.NDoctor.Plugins.%pluginName%\bin\%releaseMode%\fr\Probel.Helpers.resources.dll"
set "rsxDal=%root%\Probel.NDoctor.Plugins.%pluginName%\bin\%releaseMode%\fr\Probel.NDoctor.Domain.DAL.resources.dll"
set "rsxPlugin=%root%\Probel.NDoctor.Plugins.%pluginName%\bin\%releaseMode%\fr\Probel.NDoctor.Plugins.%pluginName%.resources.dll
set "rsxCore=%root%\Probel.NDoctor.Plugins.%pluginName%\bin\%releaseMode%\fr\Probel.NDoctor.View.Core.resources.dll"
set "rsxViewPlugin=%root%\Probel.NDoctor.Plugins.%pluginName%\bin\%releaseMode%\fr\Probel.NDoctor.View.Plugins.resources.dll"
goto %return%

:start
echo=======================================================================================================
echo Create directories  and files release mode: %releaseMode%
echo=======================================================================================================

echo=======================================================================================================
set "pluginName=DebugTools"
echo "Plugin: %pluginName%"
echo=======================================================================================================
set "return=r1"
goto refresh
:r1

mkdir "%directory%"
mkdir "%directoryFr%"

copy "%pluginDll%" "%directory%"
copy "%rsxHelper%" "%directory%\fr"
copy "%rsxDal%" "%directory%\fr"
copy "%rsxPlugin%" "%directory%\fr"
copy "%rsxCore%" "%directory%\fr"
copy "%rsxViewPlugin%" "%directory%\fr"

echo======================================================================================================
set "pluginName=UserSession"
echo "Plugin: %pluginName%"
echo=======================================================================================================
set "return=r2"
goto refresh
:r2

mkdir "%directory%"
mkdir "%directoryFr%"

copy "%pluginDll%" "%directory%%"
copy "%rsxHelper%" "%directory%\fr"
copy "%rsxDal%" "%directory%\fr"
copy "%rsxPlugin%" "%directory%\fr"
copy "%rsxCore%" "%directory%\fr"
copy "%rsxViewPlugin%" "%directory%\fr"

echo=======================================================================================================
set "pluginName=PatientSession"
echo "Plugin: %pluginName%"
echo=======================================================================================================
set "return=r3"
goto refresh
:r3

mkdir "%directory%"
mkdir "%directoryFr%"

copy "%pluginDll%" "%directory%"
copy "%rsxHelper%" "%directory%\fr"
copy "%rsxDal%" "%directory%\fr"
copy "%rsxPlugin%" "%directory%\fr"
copy "%rsxCore%" "%directory%\fr"
copy "%rsxViewPlugin%" "%directory%\fr"

echo=======================================================================================================
set "pluginName=PatientData"
echo "Plugin: %pluginName%"
echo=======================================================================================================
set "return=r4"
goto refresh
:r4

mkdir "%directory%"
mkdir "%directoryFr%"

copy "%pluginDll%" "%directory%"
copy "%rsxHelper%" "%directory%\fr"
copy "%rsxDal%" "%directory%\fr"
copy "%rsxPlugin%" "%directory%\fr"
copy "%rsxCore%" "%directory%\fr"
copy "%rsxViewPlugin%" "%directory%\fr"

echo=======================================================================================================
set "pluginName=BmiRecord"
echo "Plugin: %pluginName%"
echo=======================================================================================================
set "return=r5"
goto refresh
:r5

mkdir "%directory%"
mkdir "%directoryFr%"

copy "%pluginDll%" "%directory%"
copy "%rsxHelper%" "%directory%\fr"
copy "%rsxDal%" "%directory%\fr"
copy "%rsxPlugin%" "%directory%\fr"
copy "%rsxCore%" "%directory%\fr"
copy "%rsxViewPlugin%" "%directory%\fr"
copy "%root%\Probel.NDoctor.Plugins.BmiRecord\bin\%releaseMode%\System.Windows.Controls.DataVisualization.Toolkit.dll" "%directory%"
copy "%root%\Probel.NDoctor.Plugins.BmiRecord\bin\%releaseMode%\WPFToolkit.dll" "%directory%"

echo=======================================================================================================
set "pluginName=MedicalRecord"
echo "Plugin: %pluginName%"
echo=======================================================================================================
set "return=r6"
goto refresh
:r6

mkdir "%directory%"
mkdir "%directoryFr%"

copy "%pluginDll%" "%directory%"
copy "%rsxHelper%" "%directory%\fr"
copy "%rsxDal%" "%directory%\fr"
copy "%rsxPlugin%" "%directory%\fr"
copy "%rsxCore%" "%directory%\fr"
copy "%rsxViewPlugin%" "%directory%\fr"
copy "%root%\Probel.NDoctor.Plugins.MedicalRecord\bin\%releaseMode%\Smith.WPF.HtmlEditor.dll" "%directory%"
copy "%root%\Probel.NDoctor.Plugins.MedicalRecord\bin\%releaseMode%\WPFToolkit.Extended.dll" "%directory%"

echo=======================================================================================================
set "pluginName=PictureManager"
echo "Plugin: %pluginName%"
echo=======================================================================================================
set "return=r7"
goto refresh
:r7

mkdir "%directory%"
mkdir "%directoryFr%"

copy "%pluginDll%" "%directory%"
copy "%rsxHelper%" "%directory%\fr"
copy "%rsxDal%" "%directory%\fr"
copy "%rsxPlugin%" "%directory%\fr"
copy "%rsxCore%" "%directory%\fr"
copy "%rsxViewPlugin%" "%directory%\fr"

echo=======================================================================================================
set "pluginName=FamilyManager"
echo "Plugin: %pluginName%"
echo=======================================================================================================
set "return=r8"
goto refresh
:r8

mkdir "%directory%"
mkdir "%directoryFr%"

copy "%pluginDll%" "%directory%\
copy "%rsxHelper%" "%directory%\fr"
copy "%rsxDal%" "%directory%\fr"
copy "%rsxPlugin%" "%directory%\fr"
copy "%rsxCore%" "%directory%\fr"
copy "%rsxViewPlugin%" "%directory%\fr"
copy "%root%\Probel.NDoctor.Plugins.MedicalRecord\bin\%releaseMode%\Probel.Helpers.WPF.dll" "%directory%"
echo=======================================================================================================

echo=======================================================================================================
set "pluginName=PathologyManager"
echo "Plugin: %pluginName%"
echo=======================================================================================================
set "return=r9"
goto refresh
:r9

mkdir "%directory%"
mkdir "%directoryFr%"

copy "%pluginDll%" "%directory%\
copy "%rsxHelper%" "%directory%\fr"
copy "%rsxDal%" "%directory%\fr"
copy "%rsxPlugin%" "%directory%\fr"
copy "%rsxCore%" "%directory%\fr"
copy "%rsxViewPlugin%" "%directory%\fr"
:: used for charting
copy "%root%\Probel.NDoctor.Plugins.BmiRecord\bin\%releaseMode%\System.Windows.Controls.DataVisualization.Toolkit.dll" "%directory%"
copy "%root%\Probel.NDoctor.Plugins.BmiRecord\bin\%releaseMode%\WPFToolkit.dll" "%directory%"
echo=======================================================================================================
set "pluginName=PrescriptionManager"
echo "Plugin: %pluginName%"
echo=======================================================================================================
set "return=r10"
goto refresh
:r10

mkdir "%directory%"
mkdir "%directoryFr%"

copy "%pluginDll%" "%directory%\
copy "%rsxHelper%" "%directory%\fr"
copy "%rsxDal%" "%directory%\fr"
copy "%rsxPlugin%" "%directory%\fr"
copy "%rsxCore%" "%directory%\fr"
copy "%rsxViewPlugin%" "%directory%\fr"
echo=======================================================================================================
set "pluginName=MeetingManager"
echo "Plugin: %pluginName%"
echo=======================================================================================================
set "return=r11"
goto refresh
:r11

mkdir "%directory%"
mkdir "%directoryFr%"

copy "%pluginDll%" "%directory%\
copy "%rsxHelper%" "%directory%\fr"
copy "%rsxDal%" "%directory%\fr"
copy "%rsxPlugin%" "%directory%\fr"
copy "%rsxCore%" "%directory%\fr"
copy "%rsxViewPlugin%" "%directory%\fr"
echo=======================================================================================================
set "pluginName=Administration"
echo "Plugin: %pluginName%"
echo=======================================================================================================
set "return=r12"
goto refresh
:r12

mkdir "%directory%"
mkdir "%directoryFr%"

copy "%pluginDll%" "%directory%\
copy "%rsxHelper%" "%directory%\fr"
copy "%rsxDal%" "%directory%\fr"
copy "%rsxPlugin%" "%directory%\fr"
copy "%rsxCore%" "%directory%\fr"
copy "%rsxViewPlugin%" "%directory%\fr"
echo=======================================================================================================
set "pluginName=DbImport"
echo "Plugin: %pluginName%"
echo=======================================================================================================

set "return=r13"
goto refresh
:r13

mkdir "%directory%"
mkdir "%directoryFr%"

copy "%pluginDll%" "%directory%\
copy "%rsxHelper%" "%directory%\fr"
copy "%rsxDal%" "%directory%\fr"
copy "%rsxPlugin%" "%directory%\fr"
copy "%rsxCore%" "%directory%\fr"
copy "%rsxViewPlugin%" "%directory%\fr"
echo=======================================================================================================

echo Script executed