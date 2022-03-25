# Transaktionen

## ACID: Transaktionsregeln
> [Quelle: Introduction to Transactions, Oracle Docs](https://docs.oracle.com/cd/E11882_01/server.112/e40540/transact.htm#CNCPT117)

Transaktionen in Oracle arbeiten nach dem `ACID`-Prinzip

- **A**tomicity
  - Es werden entweder alle Befehle einer Transaktion durchgeührt oder keine. Wenn es zu einem Fehler kommt wird der Zustand vor der Transaktion wiederhergestellt. 
- **C**onsistency
  - Vor und nach einer Transaktion wird ein Konsistener Datenstand garantiert. Es darf z.B nicht sein dass bei einer Überweisung das Abbuchen von einem Konto nicht mit einer Gutschrift auf einem anderen einher geht. 
- **I**solation
  - Jede Transaktion arbeitet für sich und beeinflusst keine anderen Transaktionen. Möchte man z.B zwei Überweisungen vom selben Konto durchführen muss man warten bis die erste abgeschlossen ist.
- **D**urability
  - Jede Transaktion ist Permanent. Die Datenbank garantiert dass Sie den Zustand einer jeden Transaktion durch Backups wiederherstellen kann. 



Folien zur Theorie dazu sind auf
http://www.griesmayer.com/content/Oracle/Semester_5/02_Transaction/Folie_Transaction.pdf
abrufbar.

In SQL Developer muss unter *Preferences* - *Advanced* der Punkt *Autocommit* für diese Übung deaktiviert.
werden. In DBeaver muss in der Symbolleiste statt Auto der Punkt *Manual Commit* aktiviert werden.

## Isloationslevel
> Quelle: [Isolation levels and concurrency, Oracle Docs](https://docs.oracle.com/javadb/10.8.3.0/devguide/cdevconcepts15366.html)

 - Read Comitted
    - während der Transaktion wird als Ausgangszustand der Moment vor dem letzten *Kommando* festgelegt

 - Serializeable
    - während der Transaktion wird als Ausgangszustand der Moment vor Beginn der *Transaktion* festgelegt

Read Comitted kann schneller mit neuen Transaktionen beginnen, jedoch weniger Konsistenz garantieren. 



## Arten von Dateninkonsitenzen

- Dirty Read
  - Lesen und berücksichtigen von Daten welche noch nicht Comitted wurden
- Nonrepeatable read
  - Lesen von Daten innerhalb einer Transaktion liefert verschiedene (nicht wiederholbare) Ergebnisse
- Phantom read
  - Beeinflusst das Abfrageergebniss durch hinzufügen oder ändern von Daten


| **Isolation Level**            | **Table-Level Locking**                                      | **Row-Level Locking**                                        |
|--------------------------------|--------------------------------------------------------------|--------------------------------------------------------------|
| TRANSACTION\_READ\_UNCOMMITTED | Dirty reads, nonrepeatable reads, and phantom reads possible | Dirty reads, nonrepeatable reads, and phantom reads possible |
| TRANSACTION\_READ\_COMMITTED   | Nonrepeatable reads and phantom reads possible               | Nonrepeatable reads and phantom reads possible               |
| TRANSACTION\_REPEATABLE\_READ  | Phantom reads not possible because entire table is locked    | Phantom reads possible                                       |
| TRANSACTION\_SERIALIZABLE      | None                                                         | None                                                         |



## Anlegen von 2 Usern

Wir verbinden uns mit dem User System zur Datenbank und legen 2 Benutzer an.

```sql
-- DROP USER User1 CASCADE;
CREATE USER User1 IDENTIFIED BY oracle;
GRANT CONNECT, RESOURCE, CREATE VIEW TO User1;
GRANT UNLIMITED TABLESPACE TO User1;

-- DROP USER User2 CASCADE;
CREATE USER User2 IDENTIFIED BY oracle;
GRANT CONNECT, RESOURCE, CREATE VIEW TO User2;
GRANT UNLIMITED TABLESPACE TO User2;
```

Nach dem Anlegen der User werden in SQL Developer oder DBeaver 2 Oracle Verindungen erstellt: Eine
unter dem Benutzer *User1* und eine unter dem Benutzer *User2*. Öffnen Sie dann für diese User sowie
dem System User ein SQL Fenster.

## COMMIT mit mehreren Usern

### Operationen unter User1

Nun erstellen wir die Tabelle *ACCOUNTS*

```sql
CREATE TABLE ACCOUNTS
(
  ACCOUNT_ID  INTEGER      NOT NULL PRIMARY KEY,
  FIRST_NAME  VARCHAR(15)  NOT NULL,
  BIRTH_DATE  DATE         NOT NULL,
  AMOUNT      DECIMAL(8,2) NOT NULL
);
```

Damit User2 auf diese Tabelle zugreifen kann, gewähren wir das Recht für User2:

```sql
GRANT SELECT ON User1.ACCOUNTS TO User2;
GRANT DELETE ON User1.ACCOUNTS TO User2;
GRANT INSERT ON User1.ACCOUNTS TO User2;
GRANT UPDATE ON User1.ACCOUNTS TO User2;
```

Nun werden einige Datensätze eingefügt. Sie sind unter User1 sichtbar:

```sql
DELETE FROM   ACCOUNTS;
INSERT INTO ACCOUNTS VALUES (1, 'Thomas', TO_DATE('1973-07-14', 'yyyy-mm-dd'),  500.50);
INSERT INTO ACCOUNTS VALUES (2, 'Andera', TO_DATE('1975-08-20', 'yyyy-mm-dd'),  100.00);
INSERT INTO ACCOUNTS VALUES (3, 'Marion', TO_DATE('1981-12-12', 'yyyy-mm-dd'), -200.00);
INSERT INTO ACCOUNTS VALUES (4, 'Verena', TO_DATE('1977-01-27', 'yyyy-mm-dd'),  900.00);
INSERT INTO ACCOUNTS VALUES (5, 'Kurt',   TO_DATE('1975-02-28', 'yyyy-mm-dd'),  800.40);

SELECT * FROM   ACCOUNTS;

```

### Operationen unter User2

Das SELECT zeigt keine Datensätze:

```sql
SELECT * FROM  User1.ACCOUNTS;
```

### Operationen unter User1

Wir setzen ein *COMMIT* ab, damit die Transaktion geschrieben wird.

```sql
COMMIT;
```

### Operationen unter User2

Das SELECT zeigt jetzt die Änderung der Datensätze:

```sql
SELECT * FROM  User1.ACCOUNTS;
```

### Operationen unter User1

Wir aktualisieren den Wert von *AMOUNT* bei User1 ohne COMMIT.

```sql
UPDATE ACCOUNTS SET AMOUNT = 100 WHERE ACCOUNT_ID = 1;
```

### Operationen unter User2

Die Änderung ist noch nicht sichtbar. Wir starten ebenfalls eine Änderung:

```sql
SELECT * FROM  User1.ACCOUNTS;
UPDATE User1.ACCOUNTS SET AMOUNT = 200 WHERE ACCOUNT_ID = 2;
```

Erst beim *COMMIT* des jeweiligen Users wird die Änderung sichtbar.

### Gleichzeitiges Ändern von Datensätzen

Wird der gleiche Datensatz von beiden Usern veränedrt, so wartet das 2. COMMIT bis das erste
COMMIT durchgeführt wurde.

## Busy wait

Fügen Sie in die Tabelle *ACCOUNTS* wieder als Grundzustand die Musterdaten ein:

```sql
DELETE FROM   ACCOUNTS;
COMMIT;
INSERT INTO ACCOUNTS VALUES (1, 'Thomas', TO_DATE('1973-07-14', 'yyyy-mm-dd'),  500.50);
INSERT INTO ACCOUNTS VALUES (2, 'Andera', TO_DATE('1975-08-20', 'yyyy-mm-dd'),  100.00);
INSERT INTO ACCOUNTS VALUES (3, 'Marion', TO_DATE('1981-12-12', 'yyyy-mm-dd'), -200.00);
INSERT INTO ACCOUNTS VALUES (4, 'Verena', TO_DATE('1977-01-27', 'yyyy-mm-dd'),  900.00);
INSERT INTO ACCOUNTS VALUES (5, 'Kurt',   TO_DATE('1975-02-28', 'yyyy-mm-dd'),  800.40);
COMMIT;
```

Führen Sie nun die folgenden SQL Anweisungen in SQL Developer unter dem entsprechenden User aus:

| User1                                                                       | User2                                                                               |
| --------------------------------------------------------------------------- | ----------------------------------------------------------------------------------- |
| *UPDATE ACCOUNTS SET FIRST_NAME = 'Klaus' WHERE ACCOUNT_ID = 1;* |                                                                                     |
|                                                                             | *UPDATE User1.ACCOUNTS SET FIRST_NAME = 'Michael' WHERE ACCOUNT_ID = 1;* |

Es entsteht ein *Busy wait*. Sie müssen nun unter *User1* ein *COMMIT* absetzen, um die Daten
schreiben zu können. Um den Lock zu beobachten, führen Sie vor dem *COMMIT* folgendes SQL Statement 
unter User1 aus:

```sql
SELECT SID,
       DECODE ( block,
                    0, 'Not Blocking',
                    1, 'Blocking',
                    2, 'Global'
               ) status,
        DECODE (lmode,
                    0, 'None',
                    1, 'Null',
                    2, 'Row-S (SS)',
                    3, 'Row-X (SX)',
                    4, 'Share',
                    5, 'S/Row-X (SSX)',
                    6, 'Exclusive', TO_CHAR(lmode)
                ) mode_held,
        DECODE (REQUEST,
                    0, 'None',
                    1, 'Null',
                    2, 'Row-S (SS)',
                    3, 'Row-X (SX)',
                    4, 'Share',
                    5, 'S/Row-X (SSX)',
                    6, 'Exclusive', TO_CHAR(lmode)
                ) mode_request
FROM   v$lock
WHERE  TYPE = 'TM';
```

Sie bekommen folgende Ausgabe:

| SID | STATUS       | MODE_HELD  | MODE_REQUEST |
| --- | ------------ | ---------- | ------------ |
| 261 | Not Blocking | Row-X (SX) | None         |
| 261 | Not Blocking | Row-X (SX) | None         |

Nach dem *COMMIT* unter *User1* verschwindet zwar der Busy Wait, der Datenstz mit der Account ID 1 ist jedoch
durch User2 weiterhin gesperrt:

| SID | STATUS       | MODE_HELD  | MODE_REQUEST |
| --- | ------------ | ---------- | ------------ |
| 261 | Not Blocking | Row-X (SX) | None         |

Erst nach einem *COMMIT* unter *User2* verschwindet der Lock.

## Weitere Beispiele

| User1                                                                 | User2                                                                           |
| --------------------------------------------------------------------- | ------------------------------------------------------------------------------- |
| *UPDATE CUSTOMER SET BALANCE = BALANCE + 100 WHERE  CUSTOMER_ID = 6;* | Was sieht User2?                                                                |
| Was sieht User1?                                                      | *UPDATE GRIESMAYER.CUSTOMER SET BALANCE = BALANCE - 500 WHERE CUSTOMER_ID = 1;* |
| COMMIT;                                                               | Was sieht User2                                                                 |
| Was sieht User1?                                                      | COMMIT;                                                                         |

## Deadlock

Führen Sie in SQLDeveloper folgende Anweisungen unter den entsprechenden Usern aus:

| User1                                                                                                                                                      | User2                                                                                                                                                                      |
| ---------------------------------------------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| *UPDATE ACCOUNTS SET FIRST_NAME = 'Klaus' WHERE ACCOUNT_ID = 1;*<br>*UPDATE ACCOUNTS SET FIRST_NAME = 'Klaus' WHERE ACCOUNT_ID = 3;* |                                                                                                                                                                            |
|                                                                                                                                                            | *UPDATE User1.ACCOUNTS SET FIRST_NAME = 'Michael' WHERE ACCOUNT_ID = 2;*<br>*UPDATE User1.ACCOUNTS SET FIRST_NAME = 'Michael' WHERE ACCOUNT_ID = 1;* |
| *UPDATE ACCOUNTS SET FIRST_NAME = 'Klaus' WHERE ACCOUNT_ID = 2;*                                                                                |                                                                                                                                                                            |

Nachdem Sie die Anweisungen unter User2 ausgeführt haben, ist dieser im Zustand *Busy wait*. Führen Sie nun 
unter User1 das *UPDATE* Statement aus, bekommt einer der beiden User eine Fehlermeldung:

```text
Fehler beim Start in Zeile: 2 in Befehl -
UPDATE User1.ACCOUNTS SET FIRST_NAME = 'Michael' WHERE ACCOUNT_ID = 1
Fehlerbericht -
ORA-00060: Deadlock beim Warten auf Ressource festgestellt
```
