@echo off
set nest="C:\nDoctor-Release\Nest\nDoctor"
nant -buildfile:nDoctor.build.xml -D:build-mode=release -D:release-dir="c:\nDoctor-Release" Clean-Up
pause
