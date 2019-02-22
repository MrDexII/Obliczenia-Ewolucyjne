@echo off
set /p nazwaPanstwa="Podaj nazwe pliku z danymi "
set /p ileRazy="Ile razy wykonac "

call StrategiaZachlanna.exe %nazwaPanstwa% %ileRazy%
