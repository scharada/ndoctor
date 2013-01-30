@echo off
setx nest "C:\Projects\ndoctor\src\View\Probel.NDoctor.View.Core\bin\Debug"
echo %nest%
nant -buildfile:nDoctor.build.xml -D:build-mode=debug -D:release-dir=C:\Projects\ndoctor -D:code-dir=C:\Projects\ndoctor Clean-Up
pause
