@echo off
setx NEST=C:\nDoctor-Release\Nest\nDoctor
echo %NEST% 

nant -buildfile:nDoctor.build.xml -D:build-mode=release -D:release-dir="c:\nDoctor-Release" Clean-Up
pause
