@echo off
echo.
echo iLyric Merge DLL
echo.
"C:\Program Files (x86)\Microsoft\ILMerge\ILMerge.exe" /wildcards /out:iLyric\bin\Debug\iLyric_DLLMerged.exe iLyric\bin\Debug\iLyric.exe iLyric\bin\Debug\*.dll /targetplatform:v4,"C:\Windows\Microsoft.NET\Framework\v4.0.30319"
pause