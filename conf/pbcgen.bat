set ProtoDir=%cd%\proto
set CsDir=%cd%\tools\cs
set TempDir="%cd%\temp"

if exist %TempDir% rd /s/q %TempDir%

md %cd%\temp

XCOPY %cd%\tools\pbc\*.* %TempDir% /E
XCOPY %ProtoDir%\*.* %TempDir% /E

cd %TempDir%

for /r %%j in (*.proto) do (
	protogen -i:"%%j" -o:%CsDir%\%%~nj.cs -p:xml
)

cd ..
rd /s/q %TempDir%

pause