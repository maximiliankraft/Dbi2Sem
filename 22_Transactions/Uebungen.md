# Übungen zu Transaktionen


## Theoriefragen

- Erkläre Folgende Begriffe in eigenen Worten:
    - Dirty Read:
    - Non-Repeatable Read
    - Phantom Read

- Wie unterscheiden sich die Isolationslevel
    - Read Commited:
    - Serialized:


- Wann sind Transaktionen Sinnvoll?:
    - Logging von Nutzerzugriffen auf eine Webseite: 
    <br><br>
    - Buchhaltungprogramme mit wenigen gleichzeitigen Usern:
    <br><br>
    - Backend einer Börse mit sehr vielen gleichzeitigen Usern:
    <br><br>
    - Terminänderung in Webuntis:
    <br><br>

## Vorbereitungen
- Erstelle 2 verschiedene User und öffne mit beiden Sessions zur Datenbank

<br><br><br><br>

- Erstelle eine Tabelle dessen Daten dann durch Transaktion verändert werden
<br><br><br><br>
- Gewähre beiden Usern Zugriff auf die Tabelle
<br><br><br><br>


## Daten doppelt anlegen

1. Füge Daten in die Tabelle mit dem 1. User ein *ohne* commit
2. Füge die selben Daten mit dem 2. User ebenfalls ein, setzte sofort ein commit ab
    - Wann wird die Transaktion ausgeführt?
    <br><br>
3. Setze ein Commit bei der Transaktion vom 1. User ab
    - was geschieht in der 2. Transaktion?


## Tabellenstruktur ändern
1. Lösche alle Daten mit dem 1. User ohne commit
2. Füge zu der Tabelle eine neue Spalte hinzu
    - Was passiert mit der Transaktion vom 1. User
    
## Neue Daten einfügen
1. Lege eine Hälfte der Daten beim 1. User die andere häöfte beim 2. User an ohne commit
    - was passiert bei der Transaktion im 2. User?







