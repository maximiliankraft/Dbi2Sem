# JSON in Oracle


## JSON Grundlagen

JSON steht für `Javascript Object Notation`. Der JSON-Standard beschreibt die Syntax mit welcher Objekte in Javaskript beschrieben werden können. Dabei sind Key-Value paar und Listen als Strukturen möglich. Werte können String, (Gleitkomma)Zahlen, booleans und der nullwert sein. Die Strukturen kann man beliebig oft ineinander verschachteln. 

Einfaches Key-Value Pair: 
```json
{"key": "value"}
```

Mehrere Key-Value Pairs: 
```json
{"key1": "value1", "key2": "value2"}
```

Listen:
```json
["listValue1", "listValue2", 1,2,3, true, false, null]
```

Alles kombiniert:
```json
[
    {"a":1}, 
    {
        {"b": null}
        }, 
    {
        {
            {"c": false}
        },
        {"d": true}
    }
]

```

Mit JSON lassen sich hervorrangen Semistrukturierte Daten darstellen. Hier ist eine Grundlegende Struktur erkennbar (anders als bei Binärdaten z.B) jedoch können einzelne JSON-Objekte in Anzahl der Keys oder Listenelemente variieren. Ob ein String valides JSON ist kann man z.B unter [jsonformatter.curiousconcept.com](jsonformatter.curiousconcept.com) prüfen.

## JSON aus Datenbank generieren

Oracle beinhaltet einige Funktionen welche es ermöglichen JSON-Objekte/Arrays aus bestehenden Daten zu generieren. Beispiele dazu gibt es in folgenden Quellen:

> [Blogartikel zu Oracle JSON](https://blogs.oracle.com/sql/post/how-to-store-query-and-create-json-documents-in-oracle-database)

> [Dokumentation JSON_ARRAY](https://docs.oracle.com/en/database/oracle/oracle-database/19/sqlrf/JSON_ARRAY.html#GUID-46CDB3AF-5795-455B-85A8-764528CEC43B)

> [Dokumentation JSON_VALUE](https://docs.oracle.com/database/121/SQLRF/functions093.htm#SQLRF56668)

> [Dokumentation JSON_ARRAYAGG](https://docs.oracle.com/en/database/oracle/oracle-database/12.2/sqlrf/JSON_ARRAYAGG.html)

>[Dokumentation JSON_VALUEAGG](https://docs.oracle.com/en/database/oracle/oracle-database/18/sqlrf/JSON_OBJECTAGG.html)



### Übungsaufgaben zum generieren von JSON-Strings in Oracle

Erstelle ein Array aus JSON-Objekten welche alle folgendes Format besitzen:
```json
[{"vorname": "Max", "nachname": "Mustermann"}, ...]
```

Erstelle ein Array aus JSON-Objekten aller Schüler welche in den Postleitzahlen 1030-1150 wohnen. Gib dabei den Vornamen als Key und den Nachnamen als Value an. Ein zweites Key-Value Pair soll die Postleitzahl repräsentieren.

```json
[{"<vorname>": "<nachname>", "plz": "<plz>"}]
```

> Hinweis: JSON-Funktionen in Oracle retournieren im Normalfall einen VARCHAR(4000). Falls dieser zu klein ist kann man auch einen `clob` retournieren indem man den Rückgabewert überschreibt: `json_funktion(...) returning clob`


## JSON aus Datenbank auslesen

Mit JSON-Value kann man aus einem JSON-String und einem JSON-Path einen Wert auslesen. 

> [Dokumentation zu JSON_VALUE](https://docs.oracle.com/database/121/SQLRF/functions093.htm#SQLRF56668)

### JSON-Paths

Ein JSON-Path ist ein String welcher ein JSON-Objekt auf ein Unterobjekt eingrenzt. Er ist ähnlich aufgebaut wie die Zugriffsmethoden in Java-Objekten. Mit `.` kann man auf einen Key zugreifen. Mit `[]` auf einen Index in einem Array. Das Rootelement wird mit `$` angegeben. Möchte man den Wert `1` aus `{"a": [0,1,2,3]}` herauslesen ist das mit dem Jsonpath `$.a[1]` möglich. Mit dem Onlinetool [jsonpath.curiousconcept.com](jsonpath.curiousconcept.com) kann man einen Jsonpath relativ zu einem gegebenen Jsonstring testen.