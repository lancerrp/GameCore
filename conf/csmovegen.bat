set CsDir=%cd%\tools\cs
set TarDir=%cd%\..\develop\Assets\config\scrpits

XCOPY %CsDir%\*.* %TarDir% /E /y

pause