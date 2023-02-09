@echo off
echo building....
"C:\Program Files\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin\msbuild.exe" /property:Configuration=Release;Version=1.0.16.0
copy downloadeqrof\bin\Release\downloadeqrof.exe bin\fippy.exe

