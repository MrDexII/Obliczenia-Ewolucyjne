# Obliczenia-Ewolucyjne
## Opis, cel i zakres projektu.


<p>
Celem projektu było opracowanie algorytmu ewolucyjnego, który będzie rozwiązywać problem komiwojażera.
Zadanie polegało na optymalizacji czasu przejazdu przez wszystkie miasta w danym kraju, oraz ilości baterii, 
które pozwalają na poruszanie pojazdem. Baterie można ładować w co piątym mieście, zaczynając od pierwszego. 
Każda bateria pozwala na przebycie 1000 jednostek odległości. Ilość baterii ma wpływ na prędkość poruszania się, 
która wyznaczana jest ze wzoru V = 1-0.01*liczba_baterii, a więc im więcej baterii tym wolniej się poruszamy.
Implementacja odbyła się w języku programowania C#. Algorytm miał wykorzystywać dwie metody krzyżowania:
</p>
<ul>
<li>PMX</li>
<li>Przez wymianę podtras</li>
</ul>
<p>i dwie metody selekcji:</p>
<ul>
<li>Ruletka wartościowa</li>
<li>Turniej</li>
</ul>
<p>
Jednym z zadań było przetestowanie zaimplementowanych wariantów algorytmów, dla Szwecji i Grecji, 
których dane zostały pozyskane ze strony http://www.math.uwaterloo.ca/tsp/world/countries.html, 
oraz porównanie wyników z innymi, wybranymi algorytmami.
</p>
