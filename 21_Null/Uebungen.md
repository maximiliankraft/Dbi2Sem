# Übungsaufgaben zu NULL-Values

## Vorraussetzungen

- Eine Datenbank erstellt mit dem SchulDbGenerator

## NULL als Kriterium in der WHERE-Clause

Führe eine Abfrage durch um folgendes herrauszufinden:
 - alle Schüler die an der Hausnummer 2 wohnen
 - alle Schüler die nicht an der Hausnummer 2 wohnen
 - alle Schüler mit undefinierter Hausnummer
<hr>

<br>
<details>
  <summary>Lösung anzeigen</summary>
  
  ```sql

  - SELECT count(*) FROM SCHULE.SCHUELER WHERE S_HAUSNUMMER = '2'; -- 16
  - SELECT count(*) FROM SCHULE.SCHUELER WHERE S_HAUSNUMMER <> '2' ; -- 2029
  - SELECT count(*) FROM SCHULE.SCHUELER WHERE S_HAUSNUMMER IS NULL; -- 547
  
  ```
</details>
<br>

- Reichen die erste und die zweite Abfrage um alle Schüler zu erhalten? Falls nein, wie müsste man die Abfragen anpassen?
<br>
<details>
  <summary>Lösung anzeigen</summary>
  Nein reicht nicht!

  ```sql
  - SELECT count(*) FROM SCHULE.SCHUELER WHERE S_HAUSNUMMER = '2'; -- 16
  - SELECT count(*) FROM SCHULE.SCHUELER WHERE S_HAUSNUMMER <> '2' OR S_HAUSNUMMER IS NULL ; -- 2576
  ```
</details>
<br>

## Joins mit Nullwerten

Erstelle einen 
 - `JOIN`
 - `FULL JOIN`
 - `LEFT JOIN` 
 - und `RIGHT JOIN` 
 
 zwischen zwei Tabellen. Wie ändert sich die Zeilenanzahl je nach Joinart?

 <details>
  <summary>Lösung anzeigen</summary>

  ```sql
  - SELECT  ABT_NAME, ABT_LEITER, L.L_NAME,L.L_VORNAME FROM SCHULE.ABTEILUNG JOIN LEHRER L on L.L_NR = ABTEILUNG.ABT_LEITER; -- 15
  - SELECT  ABT_NAME, ABT_LEITER, L.L_NAME,L.L_VORNAME FROM SCHULE.ABTEILUNG RIGHT JOIN LEHRER L on L.L_NR = ABTEILUNG.ABT_LEITER; -- 291
  - SELECT  ABT_NAME, ABT_LEITER, L.L_NAME,L.L_VORNAME FROM SCHULE.ABTEILUNG LEFT JOIN LEHRER L on L.L_NR = ABTEILUNG.ABT_LEITER; -- 15
   - SELECT  ABT_NAME, ABT_LEITER, L.L_NAME,L.L_VORNAME FROM SCHULE.ABTEILUNG FULL JOIN LEHRER L on L.L_NR = ABTEILUNG.ABT_LEITER; -- 291

  ```
</details>

 ### Joins mit unterschiedlichen Typen 

Die folgende Abfrage liefert bei manchen Abfragen den Fehler `ORA-01722: Ungültige Zahl`. Woran könnte das liegen?

```sql
  - SELECT * FROM SCHULE.PRUEFUNG [FULL|LEFT|RIGHT|...] JOIN SCHULE.SCHUELER ON SCHULE.PRUEFUNG.P_NOTE = SCHULE.SCHUELER.S_HAUSNUMMER;

```

<br>
 <details>
    <summary>Lösung anzeigen</summary>

    Da es sich um verschiedene Datentypen handelt welche nicht 1:1 abgebildet werden können. Jede Zahl kann als String abgebildet werden, aber nicht jeder String als Zahl. 1 -> '1'; '1' -> 1 'Auto' -> ???. Man muss also beide Typen auf den gemeinsamen Nenner Strings zusammenführen. Dies ist mit der `TO_CHAR`-Methode möglich.

     ```sql
      SELECT * FROM SCHULE.PRUEFUNG LEFT JOIN SCHULE.SCHUELER ON TO_CHAR(SCHULE.PRUEFUNG.P_NOTE) = TO_CHAR(SCHULE.SCHUELER.S_HAUSNUMMER);

     ```

 </details>
 <br>

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

<br>
 <details>
    <summary>Lösung anzeigen</summary>

    - Die Abfrage mit `NVL` ist kleiner. `AVG` ignoriert standardmäßig Nullwerte. Durch `NVL` kommt allerdings öfter der Wert `0` dazu was das Endergebniss verringert.

    - Die Ergebnisse würden gleich bleiben. Den Wert ignorieren oder +0 kommt aufs gleiche raus.

    - `count(...)` zählt - so wie `AVG` - die Nullwerte nicht mit

 </details>
 <br>

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
<br>
 <details>
    <summary>Lösung anzeigen</summary>
    - `1 [+,-,*,/] NULL` ergibt immer `NULL`
    - `1 [+,-,*,/] 0` ergibt überall `0` ausser bei der Division. Dort kommt eine Fehlermeldung.
    <hr>
    - `'str' || NULL` ergibt `'str'`. Dies ist jedoch kein ANSI-Konformes verhalten. Laut ANSI sollte `NULL` das Ergebnis sein.

 </details>
<br>


## Nullwerte in Unterabfragen

Führe Abfragen durch welche dem folgenden Schema folgen:
```sql
SELECT * FROM <table> WHERE <column> IN  (SELECT * FROM <table> UNION SELECT NULL FROM DUAL)
```

```sql
SELECT * FROM <table> WHERE <column> NOT IN  (SELECT * FROM <table> UNION SELECT NULL FROM DUAL)
```

Wie unterscheiden sich die Ergebnisse einmal mit und einmal ohne `NOT`-Schlüsselwort?
<br>
 <details>
  <summary>Lösung anzeigen</summary>
  
  ```sql

  - SELECT * FROM SYSTEM.PATIENT WHERE DISEASES IN (SELECT 1 FROM DUAL UNION SELECT NULL FROM DUAL); -- 1 Ergebnis
  - SELECT * FROM SYSTEM.PATIENT WHERE DISEASES NOT IN (SELECT 1 FROM DUAL UNION SELECT NULL FROM DUAL); -- 0 Ergebnisse
  
  ```

  Daraus lässt sich schliessen: `NULL` in Unterabfragen wird übersprungen, ignoriert.
</details>
<br>

## Weitere Unterlagen

 - [SQL Joins Explained | Socratica](https://www.youtube.com/watch?v=9yeOJ0ZMUYw)