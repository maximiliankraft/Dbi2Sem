# Arten von Joins

- Nested loops
    - Für jeden Eintrag innerhalb einer Spalte wird nach einem  passenden Eintrag in einer anderen Tabelle gesucht. Kann auf jede Art von Join-Bedingung angewandt werden.  Kann ggf. durch Indices beschleunigt werden. *O(n^2)*

- Merge joins
    - Zuerst werden beide Tabellen an den zu zusammenführenden Spalten sortiert. Dadurch kann man frühzeitig aufhören nach weiteren Elementen zu suchen. Es ist garantiert das nichts passendes mehr kommen wird. Kann durch Indices beschleunigt werden, Indices sind bereits sortiert. Wird erst bei großen Datenmengen herangezogen. *O(n log n) + O(n log n)*

- Hash joins
    - Von jedem Element in den beiden Tabellen wird ein Hashwert erzeugt. Zum suchen von übereinstimmenden Elementen muss man jetzt nur noch nachschauen ob es einen Wert in beiden Tabellen gibt. Es wird garantiert dass jedes Element nur einmal angeschaut wird. Jedoch muss das Joinkriterium ein Vergleich (gleichzeichen) sein.   *O(n) + O(n) + O(1) + O(1)*

- Adaptive plans
    - Der Execution Plan ändert sich unter Zuhilfenahme von Heuristiken während der Laufzeit um die wahrscheinliche schnellste Lösung zu finden


## Übungsaufgaben zu Joins


```sql
-- Plant mit dem Namen `abt_lh1` anlegen für ein join-statement
explain plan set statement_id = 'abt_lh1' FOR
select
    L_NR, ABT_LEITER, ABT_NAME, L_EINTRITTSJAHR
FROM SCHULE.ABTEILUNG a JOIN SCHULE.LEHRER l ON a.ABT_LEITER = l.L_NR GROUP BY L_NR, ABT_LEITER, ABT_NAME, L_EINTRITTSJAHR;

-- weiteren plan anlegen
explain plan set statement_id = 'abt_lh2' FOR
select * FROM SCHULE.ABTEILUNG a JOIN SCHULE.LEHRER l ON a.ABT_LEITER = l.L_NR;

-- templates für das erzeugen und löschen und von indices. Wie ändern sich die Pläne wenn ein Index existiert?
create unique index abt_lh on SCHULE.ABTEILUNG(ABT_LEITER);
create unique index lh_nr on SCHULE.LEHRER(L_NR);

drop index SCHULE.abt_lh;
drop index SCHULE.lh_nr;


-- Plan `abt_lh2` anzeigen
SELECT PLAN_TABLE_OUTPUT
  FROM TABLE(DBMS_XPLAN.DISPLAY(NULL, 'abt_lh2','BASIC'));

```

- Gib den Plan von einer beliebigen Join-Abfrage ab. Wie kannst du die Joinstrategie ändern? Welche Bedingungen müssen erfüllt sein?

- Erzeuge einen Join welcher Nested Loops verwendet. Gib das Statement und den Plan in die Abgabe.



- Erzeuge einen Join welcher Hash Joins verwendet. Gib das Statement und den Plan in die Abgabe.


    

<!-- Quelle: https://yewtu.be/watch?v=pJWCwfv983Q -->