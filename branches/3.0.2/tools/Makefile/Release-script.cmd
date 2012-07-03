@echo off
nant -buildfile:nDoctor.build.xml -D:build-mode=release Release
pause
