# Übungsaufgaben zu NULL-Values

## Vorraussetzungen

- Eine Datenbank erstellt mit dem SchulDbGenerator

## NULL als Kriterium in der WHERE-Clause

Führe eine Abfrage durch um folgendes herrauszufinden:
 - alle Schüler die an der Hausnummer 2 wohnen
 - alle Schüler die nicht an der Hausnummer 2 wohnen
 - alle Schüler mit undefinierter Hausnummer
<hr>

- Reichen die erste und die zweite Abfrage um alle Schüler zu erhalten? Falls nein, wie müsste man die Abfragen anpassen?

## Joins mit Nullwerten

Erstelle einen 
 - `JOIN`
 - `FULL JOIN`
 - `LEFT JOIN` 
 - und `RIGHT JOIN` 
 
 zwischen zwei Tabellen. Wie ändert sich die Zeilenanzahl je nach Joinart?

 ### Joins mit unterschiedlichen Typen 

Die folgende Abfrage liefert bei manchen Abfragen den Fehler `ORA-01722: Ungültige Zahl`. Woran könnte das liegen?

 ```sql
 SELECT * FROM SCHULE.PRUEFUNG [FULL|LEFT|RIGHT|...] JOIN SCHULE.SCHUELER ON SCHULE.PRUEFUNG.P_NOTE = SCHULE.SCHUELER.S_HAUSNUMMER;
 ```

## Nullwerte in Gruppenfunktionen

 - Die Spalte `S_RELIGION` aus der Tabelle `SCHUELER` beinhaltet Zahlen und Nummern. Welcher Wert der beiden Abfragen über den Durchschnittswert ist kleiner? Und warum?

```sql
SELECT AVG(S_RELIGION) FROM SCHULE.SCHUELER;
```

```sql
SELECT AVG(NVL(S_RELIGION, 0)) FROM SCHULE.SCHUELER;
```

 - Ersetzt man in den obigen Abfragen `AVG` durch `SUM`, wie würde sich das auf das Ergebniss auswirken?

 - Wie geht `count(...)` mit Nullwerten um?

 ## Nullwerte bei Berechnungen
 Führe folgende SQL-Abfragen durch:
```sql
    SELECT 1 [+,-,*,/] NULL from DUAL;
```

Gibt es einen Unterschied zu folgenden numerischen Berechnungen unter der Annahme NULL = 0?
```sql
    SELECT 1 [+,-,*,/] 0 from DUAL;
```

Wie sieht es bei Strings aus?
``````sql
SELECT 'str' || NULL FROM dual;
``````

```sql
SELECT <col> FROM <table> WHERE <col> LIKE NULL;
```



## Nullwerte in Unterabfragen

Führe Abfragen durch welche dem folgenden Schema folgen:
```sql
SELECT * FROM <table> WHERE <column> IN  (SELECT * FROM <table> UNION SELECT NULL FROM DUAL)
```

```sql
SELECT * FROM <table> WHERE <column> NOT IN  (SELECT * FROM <table> UNION SELECT NULL FROM DUAL)
```

Wie unterscheiden sich die Ergebnisse einmal mit und einmal ohne `NOT`-Schlüsselwort?


## Weitere Unterlagen

 - [SQL Joins Explained | Socratica](https://www.youtube.com/watch?v=9yeOJ0ZMUYw)