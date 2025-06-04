# <span style="color:blue"> Projekt Meloman </span>
## Autorzy:
* Grzegorz Lenarski: [glen@student.agh.edu.pl](mailto:glen@student.agh.edu.pl)
* Jakub Łabuz: [jlabuz@student.agh.edu.pl](mailto:jlabuz@student.agh.edu.pl)

## Opis projektu:
Aplikacja "Meloman" gromadzi informacje o utworach, ich kategoriach, autorach oraz preferencjach ich dotyczących i na podstawie tych informacji wyświetla listę najbardziej lubianych utworów.

# Sposób użycia:
1. Należy uruchomić aplikację w głównym folderze projektu, wpisując polecenie:
```bash
dotnet run
```

2. Po udanej kompilacji ujrzymy wśród informacji:
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5122

3. Należy wyświetlony link otworzyć w przeglądarce internetowej.

4. Funkcjonalności aplikacji są osiągalne przez interfejs użytkownika.

* możliwa jest również komunikacja przez REST API, na przykład:
```bash
curl -v http://localhost:5122/api/UserApi/1   -H "X-Username: admin"   -H "X-ApiKey: ce19860bd826434282bd891cc0f7a275 "
```
Wyświetlone zostaną dane użytkownika nr 1 (najprawdopodobniej początkowego administratora) w formacie JSON.