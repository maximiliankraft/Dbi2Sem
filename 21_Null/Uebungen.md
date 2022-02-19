# Übungsaufgaben zu NULL-Values

## Vorraussetzungen

- Eine Datenbank erstellt mit dem SchulDbGenerator

## NULL in der WHERE-Clause

Führe eine Abfrage durch um:
 - alle Schüler die an der Hausnummer 2 wohnen
 - alle Schüler die nicht an der Hausnummer 2 wohnen
 - alle Schüler mit undefinierter Hausnummer

herrauszufinden.  

- Reichen die erste und die zweite Abfrage um alle Schüler zu erhalten? Falls nein, wie müsste man die Abfragen anpassen?

## Joins mit Nullwerten

Erstelle einen 
 - `JOIN`
 - `FULL JOIN`
 - `LEFT JOIN` 
 - und `RIGHT JOIN` 
 
 zwischen zwei Tabellen. Wie ändert sich die Zeilenanzahl je nach Joinart?

## Nullwerte in Aggregatsfunktionen

Die Spalte `S_RELIGION` aus der Tabelle `SCHUELER` beinhaltet Zahlen und Nummern. Welcher Wert der beiden Abfragen über den Durchschnittswert ist kleiner? Und warum?

```sql
SELECT AVG(S_RELIGION) FROM SCHULE.SCHUELER;
```

```sql
SELECT AVG(NVL(S_RELIGION, 0)) FROM SCHULE.SCHUELER;
```



