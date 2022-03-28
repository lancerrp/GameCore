set CsDir=%cd%\tools\cs
set TarDir=%cd%\..\develop\Assets\config

XCOPY %CsDir%\*.* %TarDir% /E /y

pause