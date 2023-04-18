# PaczkiMagazyn
System wspomagający wysyłkę paczek z magazynu

## Możliwości systemu

1. Zarządzanie użytkownikami i dostepem do aplikacji
2. Zarządzanie klientami
3. Konfigurowalny słownik dostaw
4. Zarządzanie produktami
5. Obsługa wysyłki paczek

## Opis aplikacji

### 1. Zarządzanie użytkownikami i dostepem do aplikacji
1. Logowanie do systemu loginem i hasłem
2. Możliwość samodzielnej rejestracji w systemie
3. Lista użytkowników: 
   - Kolumny: Imię, Nazwisko, Login, Email, Rola
   - Listowanie z możliwością filtrowania i sortowania użytkowników
   - Dodawanie/edycja użytkowników z możliwością edycji hasła
   - Usuwanie użytkowników
  
### 2. Zarządzanie klientami
1. Lista klientów:
   - Kolumny: Nazwa, NIP, REGON, PESEL, Email, Telefon, Adres
   - Listowanie z możliwością filtrowania i sortowania klientów
   - Dodawanie/edycja klienta jako firma lub osoba prywatna z adresem
   - Usuwanie klientów
2. Wybór klienta podczas tworzenia paczki

### 3. Konfigurowalny słownik dostaw
1. Lista dostaw
   - Kolumny: Nazwa, Cena za kg, Cena ubezpieczenia [% wartości], Link do śledzenia
   - Listowanie dostaw
   - Dodawanie/edycja dostaw
   - Usuwanie dostaw
  
### 4. Zarządzanie produktami
1. Lista produktów
   - Kolumny: Nazwa produktu, Cechy, Cena, Waga 1szt [kg], Ilość w magazynie
   - Listowanie z możliwością filtrowania i sortowania produktów
   - Podświetlenie na liście na czerwono produktów z 0 stanem na magazynie
   - Podświetlenie na liście na żółto produktów z niskim (1-5 szt) stanem na magazynie
   - Dodawanie/edycja produktów
   - Usuwanie produktów

### 5. Zarządzanie paczkami
1. Lista paczek
   - Kolumny: Status paczki, Data utworzenia, Data odbioru, Data dostarczenia, Ubezpieczenie, Koszt transportu, Nr listu przewozowego, Wartość, Klient
   - Listowanie z możliwością filtrowania i sortowania paczek
   - Dodawanie/edycja paczek dla klienta
   - Usuwanie paczek
   - Mozliwość przejścia do widoku paczki przez kliknięcie na rekord paczki
2. Widok paczki
   - Dane paczki
  
  Wyświetlenie podstawowych danych paczki: Data utworzenia, Osoba tworząca, Status paczki
  
   - Dane klienta
  
  Wyświetlenie danych klienta: Imię i nazwisko/nazwa, NIP i REGON/Pesel, Adres
  
   - Szczegóły paczki
  
  Wyświetlenie szczegółowych danych paczki: Status w formie progressbara, Ilośc spakowanych produktów przez magazyniera w formie progressbara, Wartość produktów paczki, Waga paczki
  
   - Szczegóły dostawy
  
  Wyświetlenie szczegółowych danych dostawy: Dostawca, Numer listu przewozowego, Data odbioru paczki, Data dostarczenia paczki, Koszt transportu, Ubezpieczenie
  
   - Produkty w paczce
  
  Lista produktów zawierająca dane zawartości paczki
  
### 6. Schemat działania wysyłki paczki w systemie
  
  1. Użytkownik z rolą sprzedawca dodaje paczkę za pomoca przycisku Dodaj nową paczkę
  2. W oknie dodawania może wybrac z listy klienta, którego wcześniej utworzył
  3. Po kliknięciu zapisz, paczka dodaje się do systemu na statusie Nowa i otwiera się jej widok
  
  Edycja paczki
  
  Status **Nowa** (paczki z tym statusem widzi i edytuje tylko Sprzedawca):
  1. Sprzedawca może dodawac produkty przez kliknięcie Dodaj produkt do paczki
  2. Po wyszukaniu wybranego produktu na liście produktów i kliknięciu jego rekordu, zostanie on automatycznie dodany do listy produktów paczki
  3. Po zamknięciu okna wyszukiwania produktów, można zmienić ilość produktów w pozycji zamówienia paczki
  4. Jeżeli w paczce istnieje przynajmniej 1 pozycja, na górze widoku zostanie odblokowany przycisk Przeslij do magazynu
  5. Kliknięcie Prześlij do magazynu powoduje zmianę statusu paczki na Towar zamówiony (Sprzedawca już nie będzie mógł edytowac pozycji)
  
  Status **Towar zamówiony** (paczki z tym statusem widzi Magazynier i Sprzedawca, edytuje tylko Sprzedawca)
  1. Magazynier na tym statusie może przeglądać paczkę
  2. Jeżeli dane w zamówieniu są poprawne i rozpoczyna pracę, musi zmienić status paczki, klikając w przycisk na górze widoku Kompletacja paczki
  
  Status **Kompletacja paczki** (paczki z tym statusem widzi Magazynier i Sprzedawca, edytuje tylko Sprzedawca)
  1. Magazynier na tym statusie może zaznaczyć przy wybranej pozycji paczki, czy towar jest spakowany. Zaznaczenie produktu jako spakowany do paczki automatycznie odejmuje daną liczę produktów z magazynu.
  2. Jeżeli na magazynie sa braki, tj. wybrana pozycja paczki wymaga spakowania większej ilości produktów niż jest na magazynie, system nie pozwoli na zaznaczenie pozycji jako spakowana. Istnieją 2 drogi rozwiązania:
     - Magazynier klika na górze widoku Wstrzymaj paczkę (oczekiwanie na towar) i w tym momencie Sprzedawca może zmienić ilość produktów w pozycji lub całkiem usunąć pozycję
     - Magazynier może zwięszyć ilość dostepnych produktów na magazynie w Zarządzaniu paczkami
  3. Jeżeli wszystkie pozycje będa zaznaczone, odblokowany zostanie przycisk na górze widoku Towar przygotowany do wysyłki
  4. Po kliknięciu przycisku Towar przygotowany do wysyłki i potwierdzeniu czynności przez użytkownika, system przeliczy takie dane, jak:
     - Wartość paczki
     - Wagę paczki

  Status **Wstrzymany** (paczki z tym statusem widzi edytuje Magazynier i Sprzedawca)
  1. Sprzedawca może zmienić ilość produktów w pozycji lub całkiem usunąć pozycję
  2. Magazynier może uzupełniać paczkę przez pakowanie pozycji
  3. Jeżeli wszystkie pozycje będa zaznaczone, odblokowany zostanie przycisk na górze widoku Towar przygotowany do wysyłki
  
  Status **Towar przygotowany do wysyłki** (paczki z tym statusem widzi Magazynier i Sprzedawca, edytuje tylko Sprzedawca)
  1. Magazynier na tym statusie może kliknąć przycisk Oczekiwanie na kuriera
  2. Po kliknięciu Oczekiwanie na kuriera, nalezy wprowadzić takie dane, jak:
     - Wybranie dostawcy (z konfigurowalnego słownika dostaw)
     - Ubezpieczenie (zaznaczyć, jeżeli wymagane - doliczy się cena zgodnie z ustawieniami ze słownika dostaw)
     - Numer listu przewozowego
     - Data odbioru przez kuriera
     - Przewidywana data dostawy
   3. Kliknięcie zapisz spowoduje wprowadzenie danych o dostawie do systemu, oraz podliczenie łącznej ceny dostawy łącznie z ubezpeiczeniem (o ile wybrane) zgodnie ze skonfigurowaną opcją dostawy w słowniku dostaw.
 
  Status **Oczekiwanie na kuriera** (paczki z tym statusem widzi Magazynier i Sprzedawca, edytuje tylko Sprzedawca)
  1. Na tym statusie Magazynier może kliknąć tylko przycisk Towar odebrany przez kuriera
  
  Status **Towar odebrany przez kuriera** (paczki z tym statusem widzi Magazynier i Sprzedawca)
  1. Na tym statusie można tylko przeglądać widok paczki
