@echo off
set /p nazwaPanstwa="Podaj nazwe pliku z danymi "
set /p minuty="Podaj czas dzialania algorytmu w minutach "
set /p liczbaPupulacji="Podaj liczebnosc populacji "
set /p wspolczynnikMutacji="Podaj wspolczynnik mutacji "
set /p stopienMutacji="Podaj stopien mutacji "
set /p ileRazy="Ile razy wykonac "

call PmxRuletkaWartoscowa.exe %nazwaPanstwa% %minuty% %liczbaPupulacji% %wspolczynnikMutacji% %stopienMutacji% %ileRazy%
