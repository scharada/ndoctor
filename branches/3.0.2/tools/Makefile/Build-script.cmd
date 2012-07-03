@echo off
echo build script:

set "slnfile=%1"
set "slndir=%2"

echo sln-file:%slnfile%
echo sln-dir :%slndir%

nant -buildfile:nDoctor.build.xml -D:build-mode=debug -D:sln-file=%slnfile% -D:sln-dir=%slndir% Post-Build
