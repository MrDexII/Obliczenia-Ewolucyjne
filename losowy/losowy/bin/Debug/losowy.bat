@echo off
set /p nazwaPanstwa="Podaj nazwe pliku z danymi "
set /p minuty="Podaj czas dzialania algorytmu w minutach "
set /p ileRazy="Ile razy wykonac "

call losowy.exe %nazwaPanstwa% %minuty% %ileRazy%
