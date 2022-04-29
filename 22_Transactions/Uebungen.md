# Übungen zu Transaktionen


## Theoriefragen

<!-- 50 Punkte -->

- Erkläre Folgende Begriffe in eigenen Worten: <!-- 16.6 -->
    - Dirty Read:<!-- Daten lesen welche noch nicht comitted wurden -->
    - Non-Repeatable Read: <!--Daten wurde in einer anderen Transaktion geändert, dieser inkonsistente Zustand ist in allen Transaktionen sichtbar-->
    - Phantom Read: <!-- Daten welche eingefügt oder geändert werden sind sofort wirksam und wirken sich somit auf aggregatsfunktionen aus -->

- Wie unterscheiden sich die Isolationslevel <!-- 16.6 -->
<!-- https://stackoverflow.com/a/27229277/17996814 -->
    - Read Commited: <!-- alles was vor dem query geschehen ist, ist sichtbar -->
    - Serialized: <!-- alles was vor der transaktion geschehen ist, ist sichtbar -->


- Wann sind Transaktionen Sinnvoll?: <!-- 16.6 -->
    - Logging von Nutzerzugriffen auf eine Webseite: 
    <!-- nein -->
    <br><br>
    - Buchhaltungsprogramme mit wenigen gleichzeitigen Usern:
    <!-- ja-->
    <br><br>
    - Backend einer Börse mit sehr vielen gleichzeitigen Usern:
    <!-- würde alles sehr verlangsamen, nur kontingente für verscheidne user reservieren-->
    <br><br>
    - Terminänderung in Webuntis:
    <!-- ja-->
    <br><br>



<!-- Praktischer Teil: 50 Punkte -->
## Vorbereitungen
- Erstelle 2 verschiedene User und öffne mit beiden Sessions zur Datenbank

<br><br><br><br>

- Erstelle eine Tabelle dessen Daten dann durch Transaktionen verändert werden
<br><br><br><br>
- Gewähre beiden Usern Zugriff auf die Tabelle
<br><br><br><br>


## Daten doppelt anlegen <!-- 16.6P-->

1. Füge Daten in die Tabelle mit dem 1. User ein *ohne* commit
2. Füge die selben Daten mit dem 2. User ebenfalls ein, setzte sofort ein commit ab
    - Wann wird die Transaktion ausgeführt?
    <br><br>
3. Setze ein Commit bei der Transaktion vom 1. User ab
    - was geschieht in der 2. Transaktion?


## Tabellenstruktur ändern <!-- 16.6P -->
1. Lösche alle Daten mit dem 1. User ohne commit
2. Füge zu der Tabelle eine neue Spalte hinzu
    - Was passiert mit der Transaktion vom 1. User
    
## Neue Daten einfügen <!-- 16.6P -->
1. Lege eine Hälfte der Daten beim 1. User die andere Hälfte beim 2. User an ohne commit
    - was passiert bei der Transaktion im 2. User?
