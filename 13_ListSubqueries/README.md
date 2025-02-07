# Unterabfragen, die Listen liefern (IN, NOT IN, EXISTS)

Eine Liste ist hier ein Ergebnis, welches aus einer Spalte, aber mehreren Werten besteht. Wir
haben solche Listen bereits in Kombination mit dem *IN* Operator kennengelernt, nämlich indem
wir Stringlisten fix angegeben haben.

Die folgende Abfrage liefert alle Unterrichtsstunden, die in *DBI1*, *DBI1x* oder *DBI1y* abgehalten
werden.

```sql
SELECT *
FROM Stunde s
WHERE s.St_Gegenstand IN ('DBI1', 'DBI1x', 'DBI1y');
```

Die logische Bedeutung haben wir auch schon kennengelernt, denn die obere Abfrage entspricht einer
*OR* Verknüpfung aus den einzelnen Elementen der Liste.

```sql
SELECT *
FROM Stunde s
WHERE s.St_Gegenstand = 'DBI1' OR s.St_Gegenstand = 'DBI1x' OR s.St_Gegenstand = 'DBI1y';
```

> **Merke:** Der IN Operator prüft, ob ein Element in der angegebenen Liste vorkommt. Das zu
> prüfende Element und die Liste müssen den gleichen Typ haben. Er entspricht
> dem Existenzquantor in der Aussagenlogik und prüft, ob das Element in der Liste existiert.

## Unterabfragen mit IN

Wir können nicht nur fixe Werte, sondern auch Unterabfragen in IN angeben. Diese Abfragen dürfen
natürlich nur 1 Spalte, allerdings beliebig viele Werte zurückgeben.

Das folgende Beispiel ermittelt alle Räume, in denen unterrichtet wird. Dafür wird zuerst die
Liste der Räume abgefragt, in denen unterrichtet wird. Danach wird für jeden Raum geprüft, ob
er Element dieser Liste ist.

![](in_operator.png)

Natürlich kann man das Beispiel auch mit einem JOIN gelöst werden, allerdings wird dann jeder
Raum mehrfach angezeigt (1x pro Unterrichtsstunde). Erst die Gruppierung löst das Problem:

```sql
SELECT r.R_ID, r.R_Plaetze, r.R_Art
FROM Raum r INNER JOIN Stunde s ON (r.R_ID = s.St_Raum)
GROUP BY r.R_ID, r.R_Plaetze, r.R_Art;
```

Wir sehen, dass Unterabfragen oft intuitiver als die entsprechende JOIN Lösung zu lesen ist.

Folgendes Beispiel hat keine (einfache) Alternative als JOIN. Wir wollen nun wissen, in welchen
Räumen DBI1 **und** POS1 unterrichtet wird. Der erste Ansatz, den viele Studierende wählen,
führt zu keinem Ergebnis:

```sql
SELECT r.*
FROM Raum r INNER JOIN Stunde s ON (r.R_ID = s.St_Raum)
WHERE s.St_Gegenstand = 'DBI1' AND s.St_Gegenstand = 'POS1';
```

Das Ergebnis ist natürlich leer, denn keine Unterrichtsstunde ist zugleich DBI und POS. Die Lösung
führt über 2 Vergleiche mit IN:

```sql
SELECT *
FROM Raum r
WHERE
    r.R_ID IN (SELECT s.St_Raum FROM Stunde s WHERE s.St_Gegenstand = 'DBI1') AND
    r.R_ID IN (SELECT s.St_Raum FROM Stunde s WHERE s.St_Gegenstand = 'POS1');
```

Im Gegensatz zur vorigen Lösung werden hier 2 Mengen geschnitten. Die Menge der Räume, in denen
DBI1 unterrichtet wird und die Menge der Räume, in denen POS1 unterrichtet wird.

![](in_operator_combined.png)

## NOT IN

Nun wollen wir die Räume wissen, in denen niemals DBI1 unterrichtet wird. Auch hier wählen viele
oft den falschen Ansatz mit einem JOIN und der Abfrage auf ungleich DBI:

```sql
SELECT r.*
FROM Raum r INNER JOIN Stunde s ON (r.R_ID = s.St_Raum)
WHERE s.St_Gegenstand <> 'DBI1';
```

Angenommen in einem Raum wird neben DBI1 auch AM unterrichtet. Dieser Raum würde in die Liste
aufgenommen, obwohl er auch eine DBI1 Stunde hat. Die korrekte Lösung führt zu *NOT IN*:

```sql
SELECT *
FROM Raum r
WHERE r.R_ID NOT IN (SELECT s.St_Raum FROM Stunde s WHERE s.St_Raum IS NOT NULL AND s.St_Gegenstand = 'DBI1');
```

> **Vorsicht:** Kommt NULL in der Liste vor, so liefert NOT IN auch den Wert NULL. Daher müssen diese
> Werte ausgeschlossen werden. Folgendes Beispiel zeigt das Verhalten von NULL Werten in Kombination
> mit *NOT IN*:

```sql
SELECT 1 WHERE 'A' IN ('A', NULL);     -- Liefert 1, denn A ist sicher in der Liste.
SELECT 1 WHERE 'B' NOT IN ('A', NULL); -- Liefert kein Ergebnis, denn B ist vielleicht der NULL Wert.
```

Alternativ kann auch mit *COALESCE()* gearbeitet werden, um NULL Werte zu vermeiden:

```sql
SELECT *
FROM Raum r
WHERE r.R_ID NOT IN (SELECT COALESCE(s.St_Raum, '?') FROM Stunde s WHERE s.St_Gegenstand = 'DBI1');
```

## Abfragen mit "für alle": ein bisschen Aussagenlogik

Wir betrachten das folgende Beispiel: Welche Lehrer unterrichten nur in den HIF Klassen? Dafür
reicht das *IN* alleine nicht aus, denn es würde alle Lehrer liefern, die **unter anderem** eine
HIF Klasse unterrichten.

Wir formulieren das Problem daher um: *Ein Lehrer, der nur HIF Klassen unterrichtet* ist
gleichbedeutend mit der Aussage *Ein Lehrer, der keine nicht-HIF Klasse unterrichtet*. Diese
Umformulierung erlaubt es uns, wieder mit *IN* zu arbeiten, denn wir haben eine Existenzabfrage
vorliegen.

```sql
SELECT *
FROM Lehrer l
WHERE l.L_Nr NOT IN (SELECT s.St_Lehrer FROM Stunde s WHERE s.St_Klasse NOT LIKE '%HIF%');
```

> Abfragen, bei denen eine Eigenschaft für alle Elemente gelten muss, kann durch Negation des
> Prädikats und einer Negation der Gesamtaussage in ein Existenzproblem umgewandelt werden.

Nachfolgend wird die Bedeutung der möglichen Kombinationen dieser Abfrage beschrieben:

Liefert alle Lehrer, die **mindestens eine** HIF Klasse unterrichten.

```sql
SELECT *
FROM Lehrer l
WHERE l.L_Nr IN (SELECT s.St_Lehrer FROM Stunde s WHERE s.St_Klasse LIKE '%HIF%');
```

Liefert alle Lehrer, die **keine einzige** HIF Klasse unterrichten.

```sql
SELECT *
FROM Lehrer l
WHERE l.L_Nr NOT IN (SELECT s.St_Lehrer FROM Stunde s WHERE s.St_Klasse LIKE '%HIF%');
```

Liefert alle Lehrer, die **mindestens eine** nicht-HIF Klasse unterrichten.

```sql
SELECT *
FROM Lehrer l
WHERE l.L_Nr IN (SELECT s.St_Lehrer FROM Stunde s WHERE s.St_Klasse NOT LIKE '%HIF%');
```

## EXISTS

SQL bietet noch eine 2. Möglichkeit zu prüfen, ob ein Element im Ergebnis einer Unterabfrage
vorkommt: *EXISTS*. Dieser Operator liefert - im Gegensatz zu *IN* - nur *true* oder *false*.
*true* wird dann geliefert, wenn die Liste einen (beliebigen) Wert enthält, ansonsten wird
*false* geliefert.

Da es nur darum geht, ob überhaupt Elemente geliefert werden, gibt unsere Unterabfrage * zurück.
Ob 1, NULL, *, ... verwendet wird ist Geschmackssache, denn die Datenbank verwirft das Ergebnis
ohnehin. Es ist nur eine syntaktische notwendigkeit, da bei SELECT eine Spaltenliste angegeben werden
muss.

```sql
-- Liefert frue, denn es gibt Datensätze in der Tabelle Prüfung.
SELECT EXISTS(SELECT * FROM Pruefung p);
-- Liefert false, denn es gibt keine Prüfungen im Gegenstand XXX.
SELECT EXISTS(SELECT * FROM Pruefung p WHERE p.P_Gegenstand = 'XXX');
```

Unterabfragen mit *EXISTS* sind fast immer korrelierend, das bedeutet dass Werte der äußeren Abfrage
verwendet werden. Das folgende Beispiel liefert die Liste aller Räume, in denen
überhaupt Unterricht statt findet:

```sql
SELECT *
FROM Raum r
WHERE EXISTS (SELECT * FROM Stunde s WHERE s.St_Raum == r.R_ID);
```

Andere Beispiele sind:

```sql
-- Gibt alle Klassen aus, in denen Schüler sind.
SELECT *
FROM Klasse k
WHERE EXISTS(SELECT * FROM Schueler s WHERE s.S_Klasse = k.K_Nr);

-- Gibt alle Klassen aus, in denen keine Schüler sind.
SELECT *
FROM Klasse k
WHERE NOT EXISTS(SELECT * FROM Schueler s WHERE s.S_Klasse = k.K_Nr);

-- Die Räume, in denen DBI1 unterrichtet wird, werden durch folgende Abfrage geliefert:
SELECT *
FROM Raum r
WHERE EXISTS (SELECT * FROM Stunde s WHERE s.St_Raum == r.R_ID AND s.St_Gegenstand == 'DBI1');
```

### EXISTS oder IN?

Die Stärke von *EXISTS* ist der Umgang mit mehreren Schlüsselteilen. Da *IN* nur eine Spalte
liefern kann, gibt es ein Problem wenn eine Tabelle einen mehrteiligen Schlüssel hat.

Das folgende Beispiel gibt alle Doppelstunden der 5AHIF aus. Dabei geht die Abfrage so vor:
Eine Doppelstunde ist dann gegeben, wenn in der Stundentabelle für die nächste Stunde (*St_Stunde + 1*)
ein Datensatz mit gleichem Tag, Klasse und Gegenstand existiert.

```sql
SELECT s1.St_Tag, s1.St_Stunde, s1.St_Gegenstand
FROM Stunde s1
WHERE EXISTS(SELECT *
             FROM Stunde s2
             WHERE s2.St_Tag = s1.St_Tag AND
                   s2.St_Klasse = s1.St_Klasse AND
                   s2.St_Gegenstand = s1.St_Gegenstand AND
                   s2.St_Stunde = s1.St_Stunde + 1) AND
s1.St_Klasse = '5AHIF'
ORDER BY s1.St_Tag, s1.St_Stunde;
```

Mit *IN* müssten wir ebenso eine korrespondierende Abfrage schreiben, die dann eine Spalte
(z. B. den Gegenstand) liefert, der dann verglichen werden kann. Das ist etwas willkürlich.

> **Hinweis:** Würde die *IN* Abfrage ohnehin korrespondierend sein, ist EXISTS meist die
> klarere Alternative.

Es gibt auch *NOT EXISTS*, welche die Aussage von *EXISTS* verneint.

Die Eigenschaften der jeweiligen Operatoren sind

- Abfragen mit *IN* sind oft nicht korrespondierend und können separat getestet werden.
- Bei mehrteiligen Schlüsseln ist die Abfrage mit *EXISTS* leichter zu schreiben.
- EXISTS ist fast immer korrespondierend und kann daher schwerer getestet werden.
- Über die Performance wird viel diskutiert. Der Optimizer der Datenbank arbeitet aber schon
  so gut, dass ein einfaches Umstellen der Abfrage keinen Mehrwert mehr bringt. Schreiben Sie
  daher die Abfragen so, wie es für Sie am klarsten erscheint.

## Übungen

Bearbeiten Sie die folgenden Abfragen.

**(1)** In welchen Klassen der Abteilung HIF kommt das Fach NW2 nicht im Stundenplan vor? Hinweis:
Arbeiten Sie mit der Menge der Klassen, in denen NW2 unterrichtet wird.

**(2)** Welche Gegenstände werden gar nicht geprüft? Lösen Sie die Aufgabe mit einem LEFT JOIN und danach
mit einer Unterabfrage. Hinweis: Arbeiten Sie mit der Menge der Gegenstände, die in der
Prüfungstabelle eingetragen sind.

**(3)** Welche Gegenstände werden nur praktisch geprüft (*P_Art* ist p)? Können Sie die Aufgabe auch mit
LEFT JOIN lösen? Begründen Sie wenn nicht. Hinweis: Arbeiten Sie mit der Menge der Gegenstände,
die NICHT praktisch geprüft werden. Betrachten Sie außerdem nur Gegenstände, die überhaupt geprüft
werden. Würden Gegenstände, die gar nicht geprüft werden, sonst aufscheinen? Macht das einen
(aussagenlogischen) Sinn? Vorsicht, denn es gibt auch Prüfungen mit Prüfungsart NULL.

**(4)** Gibt es Prüfungen im Fach BWM, die von Lehrern abgenommen wurden, die die Klasse gar nicht
unterrichten? Hinweis: Arbeiten Sie über die Menge der Lehrer, die den angezeigten Schüler unterrichten.

**(5)** Für die Maturaaufsicht in POS werden Lehrer benötigt, die zwar in POS (Filtern nach POS%) unterrichten,
aber in keiner 5. HIF Klasse (*K_Schulstufe* ist 13 und *K_Abteilung* ist HIF) sind.

**(6)** Lösen Sie das vorige Beispiel mit anderen Bedingungen: Geben Sie die Lehrer aus, die weder
in einer 1. Klasse (*K_Schulstufe* ist 13) noch in einer HIF Klasse (*K_Abteilung* ist HIF) unterrichten.
Wie ändert sich Ihre Abfrage?

**(7)** Welche Klassen der HIF Abteilung haben auch in den Abendstunden (*Stundenraster.Str_IstAbend* = 1)
Unterricht?

**(8)** Welche Lehrer haben Montag und Freitag frei, also keinen Unterricht an diesen Tagen in der
Stundentabelle? Anmerkung, die nichts mit der Lösung zu tun hat: Religion und die Freifächer
wurden - in Abweichung zu den Realdaten - nicht importiert.

**(9)** Wie das vorherige Beispiel, allerdings sollen nur Lehrer, die auch Stunden haben (also in der
Tabelle Stunden überhaupt vorkommen), berücksichtigt werden? Anmerkung, die nichts mit der Lösung
zu tun hat: Religion und die Freifächer wurden  - in Abweichung zu den Realdaten - nicht importiert.

**(10)** Schwer, sozusagen ein SQL Hyperstar Problem: Welche Klassenvorstände unterrichten nur
in Abteilungen, die auch der Klasse
entsprechen, von der sie Klassenvorstand sind? Diese Abfrage hat eine besondere Schwierigkeit: Da
Lehrer auch von mehreren Klassen Klassenvorstand sein können, die in verschiedenen Abteilungen sein
können (z. B. Tag und Abend) brauchen Sie hier geschachtelte Unterabfragen.

1. Das Problem ist durch eine Negierung zu lösen, da IN den Existenzquantor darstellt, und wir hier
   einen Allquantor brauchen.
2. Finden Sie zuerst heraus, in welchen Abteilungen der Lehrer KV ist.
3. Finden Sie die Lehrer heraus, die nicht in der Liste der Abteilungen aus (2) unterrichten.
4. Der Lehrer darf nicht in der Liste von (3) vorkommen.
5. Betrachten Sie zum Schluss nur die Lehrer, die auch KV sind. Lehrer, die kein KV sind, würden
   nämlich aussagenlogisch auch nur in Abteilungen unterrichten, von denen sie KV sind.
   
