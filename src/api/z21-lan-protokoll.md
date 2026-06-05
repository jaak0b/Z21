# Z21 LAN Protokoll Spezifikation

**Dokumentenversion 1.13 — 06.11.2023**

---

## Rechtliches, Haftungsausschluss

Die Firma Modelleisenbahn GmbH erklärt ausdrücklich, in keinem Fall für den Inhalt in diesem Dokument oder für in diesem Dokument angegebene weiterführende Informationen rechtlich haftbar zu sein.

Die Rechtsverantwortung liegt ausschließlich beim Verwender der angegebenen Daten oder beim Herausgeber der jeweiligen weiterführenden Information.

Für sämtliche Schäden die durch die Verwendung der angegebenen Informationen oder durch die Nicht-Verwendung der angegebenen Informationen entstehen übernimmt die Modelleisenbahn GmbH, Plainbachstraße 4, A-5101 Bergheim, Austria, ausdrücklich keinerlei Haftung.

Die Modelleisenbahn GmbH, Plainbachstraße 4, A-5101 Bergheim, Austria, übernimmt keinerlei Gewähr für die Aktualität, Korrektheit, Vollständigkeit oder Qualität der bereitgestellten Informationen. Haftungsansprüche, welche sich auf Schäden materieller, immaterieller oder ideeller Art beziehen, die durch die Nutzung oder Nichtnutzung der dargebotenen Informationen verursacht wurden, sind grundsätzlich ausgeschlossen.

Die Modelleisenbahn GmbH behält es sich vor, die bereitgestellten Informationen ohne gesonderte Ankündigung zu verändern, zu ergänzen oder zu löschen.

Alle innerhalb des Dokuments genannten und gegebenenfalls durch Dritte geschützten Marken- und Warenzeichen unterliegen uneingeschränkt den Bestimmungen des jeweils gültigen Kennzeichenrechts und den Besitzrechten der jeweiligen eingetragenen Eigentümer.

Das Copyright für veröffentlichte, von der Modelleisenbahn GmbH erstellte Informationen, bleibt in jedem Fall allein bei der Modelleisenbahn GmbH.

Eine Vervielfältigung oder Verwendung der bereitgestellten Informationen in anderen elektronischen oder gedruckten Publikationen ist ohne ausdrückliche Zustimmung nicht gestattet.

Sollten Teile oder einzelne Formulierungen des Haftungsausschlusses der geltenden Rechtslage nicht, nicht mehr oder nicht vollständig entsprechen, bleiben die übrigen Teile des Haftungsausschlusses in ihrem Inhalt und ihrer Gültigkeit davon unberührt.

### Impressum

- Apple, iPad, iPhone, iOS are trademarks of Apple Inc., registered in the U.S. and other countries.
- App Store is a service mark of Apple Inc.
- Android is a trademark of Google Inc.
- Google Play is a service mark of Google Inc.
- RailCom und XpressNet sind eingetragene Warenzeichen der Firma Lenz Elektronik GmbH.
- Motorola is a registered trademark of Motorola Inc., Tempe-Phoenix, USA.
- LocoNet is a registered trademark of Digitrax, Inc.

Alle Rechte, Änderungen, Irrtümer und Liefermöglichkeiten vorbehalten. Spezifikationen und Abbildungen ohne Gewähr. Änderung vorbehalten.

*Herausgeber: Modelleisenbahn GmbH, Plainbachstraße 4, A-5101 Bergheim, Austria*

---

## Änderungshistorie

| Datum | Version | Änderung |
|---|---|---|
| 06.02.2013 | 1.00 | Beschreibung der LAN Schnittstelle für Z21 FW Version 1.10, 1.11 und SmartRail FW Version 1.12 |
| 20.03.2013 | 1.01 | Z21 FW Version 1.20 — `LAN_SET_BROADCASTFLAGS`: neue Flags; `LAN_GET_HWINFO`: neuer Befehl; `LAN_SET_TURNOUTMODE`: MM-Format; LocoNet: Gateway-Funktionalität. SmartRail FW 1.13 — `LAN_GET_HWINFO`: neuer Befehl |
| 29.10.2013 | 1.02 | Z21 FW Version 1.22: Decoder CV Lesen und Schreiben; POM Lesen und Accessory Decoder: neue Befehle; LocoNet Dispatch und Gleisbesetztmelder; `LAN_LOCONET_DISPATCH_ADDR`: neue Antwort; `LAN_SET_BROADCASTFLAGS`: neues Flag; `LAN_LOCONET_DETECTOR`: neuer Befehl |
| 12.02.2014 | 1.03 | Z21 FW Version 1.23 — Korrektur lange Fahrzeugadresse in Kapitel 4; `LAN_X_MM_WRITE_BYTE`; `LAN_LOCONET_DETECTOR`: Erweiterung für LISSY |
| 25.03.2014 | 1.04 | Z21 FW Version 1.24 — `LAN_SET_BROADCASTFLAGS`: Flag 0x00010000; Kapitel 5: Weichenadressierung; `LAN_X_GET_TURNOUT_INFO`: Erweiterung Queue-Bit; `LAN_X_DCC_WRITE_REGISTER` |
| 21.01.2015 | 1.05 | Z21 FW Version 1.25 und 1.26 — Kapitel 4: Fahrstufen und Format; `LAN_X_DCC_READ_REGISTER`; `LAN_X_DCC_WRITE_REGISTER`; `LAN_LOCONET_Z21_TX` Binary State Control Instruction |
| 05.04.2016 | 1.06 | Z21 FW Version 1.28 — Kapitel 2 System Status Versionen: z21start; `LAN_GET_HW_INFO`; `LAN_GET_CODE` |
| 19.04.2017 | 1.07 | Z21 FW Version 1.29 und 1.30 — Kapitel 8 RailCom; Kapitel 10 CAN: Belegtmelder |
| 15.01.2018 | 1.08 | Kapitel 9 LocoNet: Lissy Beispiele |
| 23.05.2019 | 1.09 | Kapitel 4: Codierung der Geschwindigkeitsstufen; Kapitel 7 R-BUS: 10808 und 10819 hinzugefügt; Kapitel 9.3.1: Korrektur Binary State Control Instruction |
| 28.01.2021 | 1.10 | Z21 FW Version 1.40 — Kapitel 2 `LAN_GET_HWINFO`: weitere HW-Typen; Kapitel 5: Erweiterte Zubehördecoder DCCext; Kapitel 11 zLink |
| 11.08.2021 | 1.11 | Z21 FW Version 1.41 — Kapitel 10 CAN: Booster |
| 28.02.2022 | 1.12 | Z21 FW Version 1.42 — Kapitel 2.18 SystemState: cseRCN213, Capabilities; Kapitel 4: DCC Funktionen ≥ F29, Binary States; Kapitel 6: Tippfehler POM Read „111001MM" 0xE4 ausgebessert; Kapitel 10.2 und 11.2: Booster Management |
| 20.06.2023 | 1.13 | Z21 FW Version 1.43 — Kapitel 4: Motorola-Bit in `LAN_X_LOCO_INFO`; Kapitel 4: neue Befehle für Purge und E-STOP; Kapitel 12 Modellzeit |

---

## Inhaltsverzeichnis

1. **Grundlagen**
   - 1.1 Kommunikation
   - 1.2 Z21 Datensatz (Aufbau, X-BUS Protokoll Tunnelung, LocoNet Tunnelung)
   - 1.3 Kombinieren von Datensätzen in einem UDP-Paket
2. **System, Status, Versionen** (2.1–2.21)
3. **Einstellungen** (3.1–3.4)
4. **Fahren** (4.1–4.6)
5. **Schalten** (5.1–5.6)
6. **Decoder CV Lesen und Schreiben** (6.1–6.14)
7. **Rückmelder – R-BUS** (7.1–7.3)
8. **RailCom** (8.1–8.2)
9. **LocoNet** (9.1–9.5)
10. **CAN** (10.1–10.2)
11. **zLink** (11.1–11.3)
12. **Modellzeit** (12.1–12.4)
- Anhang A – Befehlsübersicht

---

## 1 Grundlagen

### 1.1 Kommunikation

Die Kommunikation mit der Z21 erfolgt per UDP über die Ports **21105** oder **21106**. Steuerungsanwendungen am Client (PC, App, ...) sollten in erster Linie den Port 21105 verwenden.

Die Kommunikation erfolgt immer asynchron, d.h. zwischen einer Anforderung und der entsprechenden Antwort können z.B. Broadcast-Meldungen auftreten. *(Abbildung 1: Beispiel Sequenz Kommunikation)*

Es wird erwartet, dass jeder Client einmal pro Minute mit der Z21 kommuniziert, da er sonst aus der Liste der aktiven Teilnehmer entfernt wird. Wenn möglich sollte sich ein Client beim Beenden mit dem Befehl `LAN_LOGOFF` bei der Zentrale abmelden.

### 1.2 Z21 Datensatz

#### 1.2.1 Aufbau

Ein Z21-Datensatz (eine Anforderung oder Antwort) ist folgendermaßen aufgebaut:

| DataLen (2 Byte) | Header (2 Byte) | Data (n Bytes) |
|---|---|---|

- **DataLen** (little endian): Gesamtlänge über den ganzen Datensatz inklusive DataLen, Header und Data, d.h. `DataLen = 2 + 2 + n`.
- **Header** (little endian): Beschreibt das Kommando bzw. die Protokollgruppe.
- **Data**: Aufbau und Anzahl hängen vom Kommando ab.

Falls nicht anders angegeben, ist die Byte-Reihenfolge **Little-Endian** (zuerst low byte, danach high byte).

#### 1.2.2 X-BUS Protokoll Tunnelung

Mit dem Z21-LAN-Header `0x40` (`LAN_X_xxx`) werden Anforderungen und Antworten übertragen, welche an das X-BUS-Protokoll angelehnt sind. Gemeint ist dabei nur das Protokoll — diese Befehle haben nichts mit dem physikalischen X-BUS der Z21 zu tun, sondern sind ausschließlich an die LAN-Clients bzw. die Z21 gerichtet.

Der eigentliche X-BUS-Befehl liegt im Feld **Data**. Das letzte Byte ist eine Prüfsumme und wird als XOR über den X-BUS-Befehl berechnet. Beispiel:

| DataLen | Header | X-Header | DB0 | DB1 | XOR-Byte |
|---|---|---|---|---|---|
| 0x08 0x00 | 0x40 0x00 | h | x | y | h XOR x XOR y |

#### 1.2.3 LocoNet Tunnelung

*Ab Z21 FW Version 1.20.*

Mit den Z21-LAN-Headern `0xA0` und `0xA1` (`LAN_LOCONET_Z21_RX`, `LAN_LOCONET_Z21_TX`) werden Meldungen, die von der Z21 am LocoNet-Bus empfangen bzw. gesendet werden, an den LAN-Client weitergeleitet. Der LAN-Client muss dazu die LocoNet-Meldungen mittels [2.16 LAN_SET_BROADCASTFLAGS](#216-lan_set_broadcastflags) abonniert haben.

Über den Z21-LAN-Header `0xA2` (`LAN_LOCONET_FROM_LAN`) kann der LAN-Client Meldungen auf den LocoNet-Bus schreiben.

Damit kann die Z21 als **Ethernet/LocoNet Gateway** verwendet werden, wobei die Z21 gleichzeitig der LocoNet-Master ist, welcher die Refresh-Slots verwaltet und die DCC-Pakete generiert. Die eigentliche LocoNet-Meldung liegt im Feld **Data**.

Beispiel: LocoNet-Meldung `OPC_MOVE_SLOTS <0><0>` („DISPATCH_GET") von Z21 empfangen:

| DataLen | Header | OPC | ARG1 | ARG2 | CKSUM |
|---|---|---|---|---|---|
| 0x08 0x00 | 0xA0 0x00 | 0xBA | 0x00 | 0x00 | 0x45 |

### 1.3 Kombinieren von Datensätzen in einem UDP-Paket

In den Nutzdaten eines UDP-Pakets können auch mehrere, voneinander unabhängige Z21-Datensätze gemeinsam an einen Empfänger gesendet werden. Jeder Empfänger muss diese kombinierten UDP-Pakete interpretieren können.

Beispiel: ein kombiniertes UDP-Paket mit drei Datensätzen (`LAN_X_GET_TURNOUT_INFO #4`, `LAN_X_GET_TURNOUT_INFO #5`, `LAN_RMBUS_GETDATA #0`) ist gleichwertig mit den drei einzeln nacheinander gesendeten UDP-Paketen.

Das UDP-Paket muss in eine Ethernet MTU passen, d.h. abzüglich IPv4- und UDP-Header stehen maximal `1500 - 20 - 8 = 1472` Bytes Nutzdaten zur Verfügung.

---

## 2 System, Status, Versionen

### 2.1 LAN_GET_SERIAL_NUMBER

Auslesen der Seriennummer der Z21.

**Anforderung an Z21:** `DataLen=0x04 0x00`, `Header=0x10 0x00`, kein Data.

**Antwort von Z21:** `DataLen=0x08 0x00`, `Header=0x10 0x00`, `Data=` Seriennummer 32 Bit (little endian).

### 2.2 LAN_LOGOFF

Abmelden des Clients von der Z21.

**Anforderung an Z21:** `DataLen=0x04 0x00`, `Header=0x30 0x00`, kein Data. **Antwort:** keine.

Verwenden Sie beim Abmelden die gleiche Portnummer wie beim Anmelden. *Anmerkung:* das Anmelden erfolgt implizit mit dem ersten Befehl des Clients (z.B. `LAN_SYSTEMSTATE_GETDATA`).

### 2.3 LAN_X_GET_VERSION

Auslesen der X-Bus Version der Z21.

**Anforderung an Z21:**

| DataLen | Header | X-Header | DB0 | XOR-Byte |
|---|---|---|---|---|
| 0x07 0x00 | 0x40 0x00 | 0x21 | 0x21 | 0x00 |

**Antwort von Z21:**

| DataLen | Header | X-Header | DB0 | DB1 | DB2 | XOR-Byte |
|---|---|---|---|---|---|---|
| 0x09 0x00 | 0x40 0x00 | 0x63 | 0x21 | XBUS_VER | CMDST_ID | 0x60 |

- **XBUS_VER**: X-Bus Protokoll Version (0x30 = V3.0, 0x36 = V3.6, 0x40 = V4.0, …)
- **CMDST_ID**: Command station ID (0x12 = Z21 Gerätefamilie)

### 2.4 LAN_X_GET_STATUS

Anfordern des Zentralenstatus.

**Anforderung an Z21:** `Header=0x40 0x00`, X-Header `0x21`, DB0 `0x24`, XOR `0x05`.

**Antwort:** siehe [2.12 LAN_X_STATUS_CHANGED](#212-lan_x_status_changed). Dieser Zentralenstatus ist identisch mit dem CentralState im SystemStatus, siehe [2.18](#218-lan_systemstate_datachanged).

### 2.5 LAN_X_SET_TRACK_POWER_OFF

Abschalten der Gleisspannung.

**Anforderung an Z21:** X-Header `0x21`, DB0 `0x80`, XOR `0xA1`. **Antwort:** siehe [2.7](#27-lan_x_bc_track_power_off).

### 2.6 LAN_X_SET_TRACK_POWER_ON

Einschalten der Gleisspannung bzw. Beenden von Notstop oder Programmiermodus.

**Anforderung an Z21:** X-Header `0x21`, DB0 `0x81`, XOR `0xA0`. **Antwort:** siehe [2.8](#28-lan_x_bc_track_power_on).

### 2.7 LAN_X_BC_TRACK_POWER_OFF

Wird von der Z21 an die registrierten Clients versendet, wenn ein Client `LAN_X_SET_TRACK_POWER_OFF` gesendet hat, ein anderes Eingabegerät (multiMaus) die Gleisspannung abgeschaltet hat, und der Client den Broadcast (Flag 0x00000001) aktiviert hat.

**Z21 an Client:** X-Header `0x61`, DB0 `0x00`, XOR `0x61`.

### 2.8 LAN_X_BC_TRACK_POWER_ON

Analog zu 2.7, beim Einschalten der Gleisspannung. **Z21 an Client:** X-Header `0x61`, DB0 `0x01`, XOR `0x60`.

### 2.9 LAN_X_BC_PROGRAMMING_MODE

Wird versendet, wenn die Z21 durch `LAN_X_CV_READ` oder `LAN_X_CV_WRITE` in den CV-Programmiermodus versetzt wurde (Broadcast-Flag 0x00000001). **Z21 an Client:** X-Header `0x61`, DB0 `0x02`, XOR `0x63`.

### 2.10 LAN_X_BC_TRACK_SHORT_CIRCUIT

Wird bei einem Kurzschluss versendet (Broadcast-Flag 0x00000001). **Z21 an Client:** X-Header `0x61`, DB0 `0x08`, XOR `0x69`.

### 2.11 LAN_X_UNKNOWN_COMMAND

Antwort auf eine ungültige Anforderung. **Z21 an Client:** X-Header `0x61`, DB0 `0x82`, XOR `0xE3`.

### 2.12 LAN_X_STATUS_CHANGED

Wird versendet, wenn der Client den Status explizit mit [2.4](#24-lan_x_get_status) angefordert hat.

**Z21 an Client:** `Header=0x40 0x00`, X-Header `0x62`, DB0 `0x22`, DB1 = Status, dann XOR-Byte.

Bitmasken für Zentralenstatus:

```c
#define csEmergencyStop          0x01  // Der Nothalt ist eingeschaltet
#define csTrackVoltageOff        0x02  // Die Gleisspannung ist abgeschaltet
#define csShortCircuit           0x04  // Kurzschluss
#define csProgrammingModeActive  0x20  // Der Programmiermodus ist aktiv
```

Identisch mit `SystemState.CentralState`, siehe [2.18](#218-lan_systemstate_datachanged).

### 2.13 LAN_X_SET_STOP

Aktiviert den Notstop: die Loks werden angehalten, aber die Gleisspannung bleibt eingeschaltet.

**Anforderung an Z21:** `DataLen=0x06 0x00`, `Header=0x40 0x00`, X-Header `0x80`, XOR `0x80`. **Antwort:** siehe [2.14](#214-lan_x_bc_stopped).

### 2.14 LAN_X_BC_STOPPED

Wird versendet, wenn der Notstop ausgelöst wurde (Broadcast-Flag 0x00000001). **Z21 an Client:** X-Header `0x81`, DB0 `0x00`, XOR `0x81`.

### 2.15 LAN_X_GET_FIRMWARE_VERSION

Auslesen der Firmware-Version der Z21.

**Anforderung an Z21:** X-Header `0xF1`, DB0 `0x0A`, XOR `0xFB`.

**Antwort von Z21:** X-Header `0xF3`, DB0 `0x0A`, DB1 = V_MSB, DB2 = V_LSB, dann XOR.
- DB1: Höherwertiges Byte der Firmware Version
- DB2: Niederwertiges Byte der Firmware Version
- Version im BCD-Format. Beispiel: `... 0xf3 0x0a 0x01 0x23 0xdb` → „Firmware Version 1.23".

### 2.16 LAN_SET_BROADCASTFLAGS

Setzen der Broadcast-Flags in der Z21. Diese Flags werden pro Client (IP + Portnummer) eingestellt und müssen beim nächsten Anmelden neu gesetzt werden.

**Anforderung an Z21:** `Header=0x50 0x00`, `Data=` Broadcast-Flags 32 Bit (little endian). Broadcast-Flags sind eine OR-Verknüpfung folgender Werte:

| Flag | Bedeutung |
|---|---|
| `0x00000001` | Automatisch generierte Broadcasts/Meldungen zu Fahren und Schalten. Abonniert: 2.7 PowerOff, 2.8 PowerOn, 2.9 ProgrammingMode, 2.10 ShortCircuit, 2.14 Stopped, 4.4 LOCO_INFO (Lok-Adresse muss abonniert sein), 5.3 TURNOUT_INFO |
| `0x00000002` | Änderungen der Rückmelder am R-Bus → 7.1 `LAN_RMBUS_DATACHANGED` |
| `0x00000004` | Änderungen bei RailCom-Daten der abonnierten Loks → 8.1 `LAN_RAILCOM_DATACHANGED` |
| `0x00000100` | Änderungen des Z21-Systemzustands → 2.18 `LAN_SYSTEMSTATE_DATACHANGED` |
| `0x00010000` | *(ab FW 1.20)* Ergänzt Flag 0x00000001; Client bekommt `LAN_X_LOCO_INFO` ohne vorheriges Abonnieren der Lok-Adressen (alle Loks!). Nur für vollwertige PC-Steuerungen, nicht für mobile Handregler. Ab FW V1.20–V1.23: für **alle** Loks; ab FW V1.24: für **alle geänderten** Loks |
| `0x01000000` | Meldungen vom LocoNet-Bus an LAN Client weiterleiten (ohne Loks und Weichen) |
| `0x02000000` | Lok-spezifische LocoNet-Meldungen: OPC_LOCO_SPD, OPC_LOCO_DIRF, OPC_LOCO_SND, OPC_LOCO_F912, OPC_EXP_CMD |
| `0x04000000` | Weichen-spezifische LocoNet-Meldungen: OPC_SW_REQ, OPC_SW_REP, OPC_SW_ACK, OPC_SW_STATE |
| `0x08000000` | *(ab FW 1.22)* Status-Meldungen von Gleisbesetztmeldern am LocoNet-Bus → 9.5 `LAN_LOCONET_DETECTOR` |
| `0x00040000` | *(ab FW 1.29)* RailCom-Daten automatisch, ohne vorheriges Abonnieren (alle Loks). Nur für vollwertige PC-Steuerungen → 8.1 `LAN_RAILCOM_DATACHANGED` |
| `0x00080000` | *(ab FW 1.30)* Status-Meldungen von Gleisbesetztmeldern am CAN-Bus → 10.1 `LAN_CAN_DETECTOR` |
| `0x00020000` | *(ab FW 1.41)* CAN-Bus Booster Status-Meldungen → 10.2.3 `LAN_CAN_BOOSTER_SYSTEMSTATE_CHGD` |
| `0x00000010` | *(ab FW 1.43)* Fastclock Modellzeit Meldungen → 12.2 `LAN_FAST_CLOCK_DATA` |

**Antwort:** keine.

Berücksichtigen Sie die Auswirkungen auf die Netzwerkauslastung — besonders bei den Flags `0x00010000`, `0x00040000`, `0x02000000` und `0x04000000`. IP-Pakete dürfen vom Router bei Überlast gelöscht werden, und UDP bietet keine Erkennungsmechanismen. Bei Flag 0x00000100 (Systemzustand) ist abzuwägen, ob nicht 0x00000001 mit den entsprechenden `LAN_X_BC_xxx`-Broadcasts die sinnvollere Alternative ist.

### 2.17 LAN_GET_BROADCASTFLAGS

Auslesen der Broadcast-Flags. **Anforderung:** `Header=0x51 0x00`, kein Data. **Antwort:** `Header=0x51 0x00`, Broadcast-Flags 32 Bit (little endian).

### 2.18 LAN_SYSTEMSTATE_DATACHANGED

Änderung des Systemzustandes melden. Wird asynchron gemeldet, wenn der Client den Broadcast (Flag 0x00000100) aktiviert hat oder den Systemzustand explizit mit [2.19](#219-lan_systemstate_getdata) angefordert hat.

**Z21 an Client:** `DataLen=0x14 0x00`, `Header=0x84 0x00`, `Data=` SystemState (16 Bytes).

SystemState (16-bit Werte little endian):

| Offset | Typ | Name | Einheit | Bedeutung |
|---|---|---|---|---|
| 0 | INT16 | MainCurrent | mA | Strom am Hauptgleis |
| 2 | INT16 | ProgCurrent | mA | Strom am Programmiergleis |
| 4 | INT16 | FilteredMainCurrent | mA | geglätteter Strom am Hauptgleis |
| 6 | INT16 | Temperature | °C | interne Temperatur in der Zentrale |
| 8 | UINT16 | SupplyVoltage | mV | Versorgungsspannung |
| 10 | UINT16 | VCCVoltage | mV | interne Spannung, identisch mit Gleisspannung |
| 12 | UINT8 | CentralState | bitmask | siehe unten |
| 13 | UINT8 | CentralStateEx | bitmask | siehe unten |
| 14 | UINT8 | reserved | | |
| 15 | UINT8 | Capabilities | bitmask | siehe unten, ab Z21 V1.42 |

```c
// CentralState
#define csEmergencyStop          0x01  // Der Nothalt ist eingeschaltet
#define csTrackVoltageOff        0x02  // Die Gleisspannung ist abgeschaltet
#define csShortCircuit           0x04  // Kurzschluss
#define csProgrammingModeActive  0x20  // Der Programmiermodus ist aktiv

// CentralStateEx
#define cseHighTemperature       0x01  // zu hohe Temperatur
#define csePowerLost             0x02  // zu geringe Eingangsspannung
#define cseShortCircuitExternal  0x04  // am externen Booster-Ausgang
#define cseShortCircuitInternal  0x08  // am Hauptgleis oder Programmiergleis
#define cseRCN213                0x20  // Weichenadressierung gem. RCN213 (ab FW 1.42)

// Capabilities (ab FW 1.42)
#define capDCC                   0x01  // beherrscht DCC
#define capMM                    0x02  // beherrscht MM
//#define capReserved            0x04  // reserviert
#define capRailCom               0x08  // RailCom ist aktiviert
#define capLocoCmds              0x10  // akzeptiert LAN-Befehle für Lokdecoder
#define capAccessoryCmds         0x20  // akzeptiert LAN-Befehle für Zubehördecoder
#define capDetectorCmds          0x40  // akzeptiert LAN-Befehle für Belegtmelder
#define capNeedsUnlockCode       0x80  // benötigt Freischaltcode (z21start)
```

`SystemState.Capabilities` verschafft dem Client einen Überblick über den Feature-Umfang. Ist `Capabilities == 0`, handelt es sich vermutlich um eine ältere Firmware — bei älteren Versionen sollte Capabilities nicht ausgewertet werden.

### 2.19 LAN_SYSTEMSTATE_GETDATA

Anfordern des aktuellen Systemzustandes. **Anforderung:** `Header=0x85 0x00`, kein Data. **Antwort:** siehe [2.18](#218-lan_systemstate_datachanged).

### 2.20 LAN_GET_HWINFO

*Ab Z21 FW Version 1.20 und SmartRail FW Version V1.13.* Auslesen von Hardware-Typ und Firmware-Version.

**Anforderung:** `Header=0x1A 0x00`, kein Data.

**Antwort:** `DataLen=0x0C 0x00`, `Header=0x1A 0x00`, `Data=` HwType 32 Bit + FW Version 32 Bit (beide little endian).

```c
#define D_HWT_Z21_OLD             0x00000200  // "schwarze Z21" (ab 2012)
#define D_HWT_Z21_NEW             0x00000201  // "schwarze Z21" (ab 2013)
#define D_HWT_SMARTRAIL           0x00000202  // SmartRail (ab 2012)
#define D_HWT_z21_SMALL           0x00000203  // "weiße z21" Starterset (ab 2013)
#define D_HWT_z21_START           0x00000204  // "z21 start" Starterset (ab 2016)
#define D_HWT_SINGLE_BOOSTER      0x00000205  // 10806 "Z21 Single Booster" (zLink)
#define D_HWT_DUAL_BOOSTER        0x00000206  // 10807 "Z21 Dual Booster" (zLink)
#define D_HWT_Z21_XL              0x00000211  // 10870 "Z21 XL Series" (ab 2020)
#define D_HWT_XL_BOOSTER          0x00000212  // 10869 "Z21 XL Booster" (ab 2021, zLink)
#define D_HWT_Z21_SWITCH_DECODER  0x00000301  // 10836 "Z21 SwitchDecoder" (zLink)
#define D_HWT_Z21_SIGNAL_DECODER  0x00000302  // 10836 "Z21 SignalDecoder" (zLink)
```

FW Version im BCD-Format. Beispiel: `... 0x00 0x02 0x00 0x00 0x20 0x01 0x00 0x00` → „Hardware Typ 0x200, Firmware Version 1.20". Für ältere Firmware ggf. [2.15](#215-lan_x_get_firmware_version) verwenden (V1.10/V1.11 = Z21 ab 2012, V1.12 = SmartRail ab 2012).

### 2.21 LAN_GET_CODE

Prüfen und Auslesen des SW Feature-Umfangs. Besonders bei „z21 start" interessant, um zu prüfen, ob Fahren und Schalten per LAN gesperrt oder erlaubt ist.

**Anforderung:** `Header=0x18 0x00`, kein Data. **Antwort:** `Header=0x18 0x00`, Code (8 Bit).

```c
#define Z21_NO_LOCK          0x00  // keine Features gesperrt
#define z21_START_LOCKED     0x01  // "z21 start": Fahren und Schalten per LAN gesperrt
#define z21_START_UNLOCKED   0x02  // "z21 start": alle Feature-Sperren aufgehoben
```

---

## 3 Einstellungen

Die hier beschriebenen Einstellungen werden in der Z21 persistent gespeichert. Sie können vom Anwender auf Werkseinstellung zurückgesetzt werden, indem die STOP-Taste gedrückt gehalten wird, bis die LEDs violett blinken.

### 3.1 LAN_GET_LOCOMODE

Lesen des Ausgabeformats (DCC, MM) für eine Lok-Adresse. Es können max. 256 verschiedene Lok-Adressen abgelegt werden; jede Adresse ≥ 256 ist automatisch DCC.

**Anforderung:** `Header=0x60 0x00`, `Data=` Lok-Adresse 16 bit (**big endian**).

**Antwort:** `Header=0x60 0x00`, Lok-Adresse 16 Bit (big endian) + Modus 8 bit.
- Lok-Adresse: 2 Byte, big endian (zuerst high byte).
- Modus: `0` = DCC, `1` = MM.

### 3.2 LAN_SET_LOCOMODE

Setzen des Ausgabeformats (persistent). **Anforderung:** `Header=0x61 0x00`, Lok-Adresse 16 Bit (big endian) + Modus 8 bit. **Antwort:** keine.

*Anmerkungen:* Jede Lok-Adresse ≥ 256 bleibt automatisch DCC. Die Fahrstufen (14, 28, 128) werden ebenfalls persistent gespeichert (automatisch beim Fahrbefehl, siehe [4.2](#42-lan_x_set_loco_drive)).

### 3.3 LAN_GET_TURNOUTMODE

Lesen der Einstellungen für eine Funktionsdecoder-Adresse („Accessory Decoder" RP-9.2.1). Max. 256 Adressen; jede ≥ 256 ist automatisch DCC.

**Anforderung:** `Header=0x70 0x00`, Funktionsdecoder-Adresse 16 bit (big endian).
**Antwort:** `Header=0x70 0x00`, Funktionsdecoder-Adresse 16 Bit (big endian) + Modus 8 bit (`0`=DCC, `1`=MM).

An der LAN-Schnittstelle und in der Z21 werden Funktionsdecoder-Adressen ab 0 adressiert, in der Visualisierung der Apps/multiMaus jedoch ab 1. Beispiel: multiMaus Weichenadresse #3 entspricht in der Z21 der Adresse 2.

### 3.4 LAN_SET_TURNOUTMODE

Setzen des Ausgabeformats für eine Funktionsdecoder-Adresse (persistent). **Anforderung:** `Header=0x71 0x00`, Funktionsdecoder-Adresse 16 Bit (big endian) + Modus 8 bit. **Antwort:** keine.

MM-Funktionsdecoder werden ab Z21 FW 1.20 unterstützt; SmartRail unterstützt sie nicht. Jede Adresse ≥ 256 bleibt automatisch DCC.

---

## 4 Fahren

Ein Client kann Lok-Infos mit [4.1 LAN_X_GET_LOCO_INFO](#41-lan_x_get_loco_info) abonnieren, um über Änderungen (durch andere Clients/Handregler) informiert zu werden. Zusätzlich muss der Broadcast (Flag 0x00000001) aktiviert sein. *(Abbildung 2: Beispiel Sequenz Lok-Steuerung.)*

Maximal **16 Lok-Adressen pro Client** können abonniert werden (FIFO). Weiteres Pollen ist möglich, sollte aber mit Rücksicht auf die Netzwerkauslastung erfolgen.

### 4.1 LAN_X_GET_LOCO_INFO

Anfordern des Status einer Lok (und Abonnieren, nur mit Flag 0x00000001).

**Anforderung an Z21:**

| DataLen | Header | X-Header | DB0 | DB1 | DB2 | XOR-Byte |
|---|---|---|---|---|---|---|
| 0x09 0x00 | 0x40 0x00 | 0xE3 | 0xF0 | Adr_MSB | Adr_LSB | XOR |

`Lok-Adresse = (Adr_MSB & 0x3F) << 8 + Adr_LSB`. Bei Lok-Adressen ≥ 128 müssen die beiden höchsten Bits in DB1 auf 1 gesetzt sein: `DB1 = (0xC0 | Adr_MSB)`.

**Antwort:** siehe [4.4 LAN_X_LOCO_INFO](#44-lan_x_loco_info).

### 4.2 LAN_X_SET_LOCO_DRIVE

Verändern der Fahrstufe eines Lok-Decoders.

**Anforderung an Z21:**

| DataLen | Header | X-Header | DB0 | DB1 | DB2 | DB3 | XOR |
|---|---|---|---|---|---|---|---|
| 0x0A 0x00 | 0x40 0x00 | 0xE4 | 0x1S | Adr_MSB | Adr_LSB | RVVVVVVV | XOR |

`Lok-Adresse = (Adr_MSB & 0x3F) << 8 + Adr_LSB` (≥ 128: `DB1 = 0xC0 | Adr_MSB`).

`0x1S` = Anzahl der Fahrstufen je nach Schienenformat:
- `S=0`: DCC 14 Fahrstufen bzw. MMI mit 14 Fahrstufen und F0
- `S=2`: DCC 28 Fahrstufen bzw. MMII mit 14 realen Fahrstufen und F0-F4
- `S=3`: DCC 128 Fahrstufen (alias „126" ohne Stops) bzw. MMII mit 28 realen Fahrstufen und F0-F4

`RVVVVVVV`: R = Richtung (1 = vorwärts), V = Geschwindigkeit (Codierung abhängig von S). Bei MM erfolgt die Umrechnung von DCC- in MM-Fahrstufe automatisch in der Z21.

**Fahrstufen-Codierung „DCC 14"** (`R000 VVVV`):

| Code | Speed | Code | Speed | Code | Speed |
|---|---|---|---|---|---|
| R000 0000 | Stop | R000 0110 | Step 5 | R000 1100 | Step 11 |
| R000 0001 | E-Stop | R000 0111 | Step 6 | R000 1101 | Step 12 |
| R000 0010 | Step 1 | R000 1000 | Step 7 | R000 1110 | Step 13 |
| R000 0011 | Step 2 | R000 1001 | Step 8 | R000 1111 | Step 14 (max) |
| R000 0100 | Step 3 | R000 1010 | Step 9 | | |
| R000 0101 | Step 4 | R000 1011 | Step 10 | | |

**Fahrstufen-Codierung „DCC 28"** (`R00V5 VVVV`, Zwischenschritt im Bit V5):

| Code | Speed | Code | Speed |
|---|---|---|---|
| R000 0000 | Stop | R000 1000 | Step 13 |
| R001 0000 | Stop¹ | R001 1000 | Step 14 |
| R000 0001 | E-Stop | R000 1001 | Step 15 |
| R001 0001 | E-Stop¹ | … | … |
| R000 0010 | Step 1 | R001 1111 | Step 28 (max) |
| R001 0010 | Step 2 | | |

¹ Verwendung nicht empfohlen.

**Fahrstufen-Codierung „DCC 128"** (`RVVV VVVV`): `R000 0000`=Stop, `R000 0001`=E-Stop, `R000 0010`=Step 1, … `R111 1111`=Step 126 (max).

**Antwort:** keine Standardantwort, [4.4 LAN_X_LOCO_INFO](#44-lan_x_loco_info) an Clients mit Abo. Eine Änderung der Fahrstufenzahl (14/28/128) wird automatisch persistent gespeichert.

### 4.3 Funktionen für Fahrzeugdecoder

Funktionsbefehle F0–F12 werden am Gleis (wie Fahrstufe/Richtung) regelmäßig prioritätsgesteuert wiederholt. Befehle ab F13 werden nach einer Änderung dreimal ausgegeben und danach (gem. RCN-212, aus Rücksicht auf die Bandbreite) nicht mehr regelmäßig wiederholt.

#### 4.3.1 LAN_X_SET_LOCO_FUNCTION

Schalten einer Einzelfunktion.

| DataLen | Header | X-Header | DB0 | DB1 | DB2 | DB3 | XOR |
|---|---|---|---|---|---|---|---|
| 0x0A 0x00 | 0x40 0x00 | 0xE4 | 0xF8 | Adr_MSB | Adr_LSB | TTNN NNNN | XOR |

`Lok-Adresse = (Adr_MSB & 0x3F) << 8 + Adr_LSB` (≥ 128: `DB1 = 0xC0 | Adr_MSB`).
- `TT` Umschalttyp: `00`=aus, `01`=ein, `10`=umschalten, `11`=nicht erlaubt.
- `NNNNNN` Funktionsindex: `0x00`=F0 (Licht), `0x01`=F1 usw.

Bei MMI nur F0, bei MMII F0–F4. Bei DCC F0–F28, ab FW 1.42 erweitert F0–F31. **Antwort:** keine Standardantwort, 4.4 LOCO_INFO an Clients mit Abo.

#### 4.3.2 LAN_X_SET_LOCO_FUNCTION_GROUP

Schaltet eine ganze Funktionsgruppe (bis zu 8 Funktionen) mit einem Befehl. Ab FW 1.42 bis F31, mit Einschränkungen bis F68. Der Client sollte den aktuellen Zustand aller Funktionen mitverfolgen (Befehl eher für PC-Steuerungen geeignet).

| DataLen | Header | X-Header | DB0 | DB1 | DB2 | DB3 | XOR |
|---|---|---|---|---|---|---|---|
| 0x0A 0x00 | 0x40 0x00 | 0xE4 | Group | Adr_MSB | Adr_LSB | Functions | XOR |

Group und Functions:

| Nr | Group | Bit7 | Bit6 | Bit5 | Bit4 | Bit3 | Bit2 | Bit1 | Bit0 | Anm. |
|---|---|---|---|---|---|---|---|---|---|---|
| 1 | 0x20 | 0 | 0 | 0 | F0 | F4 | F3 | F2 | F1 | (A) |
| 2 | 0x21 | 0 | 0 | 0 | 0 | F8 | F7 | F6 | F5 | |
| 3 | 0x22 | 0 | 0 | 0 | 0 | F12 | F11 | F10 | F9 | |
| 4 | 0x23 | F20 | F19 | F18 | F17 | F16 | F15 | F14 | F13 | (B) |
| 5 | 0x28 | F28 | F27 | F26 | F25 | F24 | F23 | F22 | F21 | (B) |
| 6 | 0x29 | F36 | F35 | F34 | F33 | F32 | F31 | F30 | F29 | (C)(D)(E) |
| 7 | 0x2A | F44 | F43 | F42 | F41 | F40 | F39 | F38 | F37 | (D)(E) |
| 8 | 0x2B | F52 | F51 | F50 | F49 | F48 | F47 | F46 | F45 | (D)(E) |
| 9 | 0x50 | F60 | F59 | F58 | F57 | F56 | F55 | F54 | F53 | (D)(E) |
| 10 | 0x51 | F68 | F67 | F66 | F65 | F64 | F63 | F62 | F61 | (D)(E) |

- (A) MMI nur F0, MMII bis max. F4.
- (B) DCC F13–F28 mit diesem Befehl erst ab FW V1.24.
- (C) DCC F29–F31 ab FW V1.42, inkl. Rückmeldung an die LAN-Clients.
- (D) DCC F32–F68 ab FW V1.42, **ohne** Rückmeldung; Befehle nur am Gleis ausgegeben.
- (E) Es kann nicht gewährleistet werden, dass DCC-Funktionsbefehle ≥ F29 von allen Decodern verstanden werden (2022: nur sehr wenige Typen, getestet F29–F31 mit „Loksound 5").

**Antwort:** keine Standardantwort; für F0–F31 erfolgt Rückmeldung 4.4 LOCO_INFO an Clients mit Abo.

#### 4.3.3 LAN_X_SET_LOCO_BINARY_STATE

*Ab Z21 FW Version 1.42.* Sendet ein DCC „Binary State" Kommando an einen Lok-Decoder.

| DataLen | Header | X-Header | DB0 | DB1 | DB2 | DB3 | DB4 | XOR |
|---|---|---|---|---|---|---|---|---|
| 0x0A 0x00 | 0x40 0x00 | 0xE5 | 0x5F | AH | AL | FLLL LLLL | HHHH HHHH | XOR |

`Lok-Adresse = (AH & 0x3F) << 8 + AL` (≥ 128: `DB1 = 0xC0 | AH`).
- `F`: oberstes Bit legt fest, ob der Binärzustand ein- oder ausgeschaltet ist.
- `LLLLLLL`: niederwertige 7 Bits der Binärzustandsadresse.
- `HHHHHHHH`: höherwertige 8 Bits der Binärzustandsadresse.
- `15-Bit Binärzustandsadresse = (HHHHHHHH << 7) + (LLLLLLL & 0x7F)`.

Erlaubt: Binärzustandsadressen **29 bis 32767**. Adressen 1–28 sind reserviert, Adresse 0 ist Broadcast. Adressen < 128 (HHHHHHHH == 0) werden gem. RCN-212 als „kurze Form" ausgegeben, ≥ 128 als „lange Form". Befehle werden dreimal am Gleis ausgegeben und danach nicht mehr wiederholt. **Antwort:** keine (auch keine Benachrichtigung an andere Clients).

### 4.4 LAN_X_LOCO_INFO

Wird als Antwort auf [4.1](#41-lan_x_get_loco_info) gesendet, aber auch ungefragt, wenn der Lok-Status verändert wurde, der Broadcast (Flag 0x00000001) aktiviert ist und die Lok-Adresse abonniert wurde.

**Z21 an Client:** `DataLen = 7 + n`, `Header=0x40 0x00`, X-Header `0xEF`, Lok-Information, XOR-Byte. Paketlänge variiert mit `7 ≤ n ≤ 14`. Ab FW 1.42 ist `DataLen ≥ 15 (n ≥ 8)` zur Übertragung von F29–F31.

| Position | Daten | Bedeutung |
|---|---|---|
| DB0 | Adr_MSB | beide höchsten Bits ignorieren |
| DB1 | Adr_LSB | `Lok-Adresse = (Adr_MSB & 0x3F) << 8 + Adr_LSB` |
| DB2 | `000MBKKK` | M=1: MM-Lok (ab FW 1.43); B=1: Lok von anderem X-BUS Handregler gesteuert („besetzt"); KKK = Fahrstufeninfo (0=14, 2=28, 4=128) |
| DB3 | `RVVVVVVV` | R = Richtung (1=vorwärts), V = Geschwindigkeit (Codierung abh. von KKK) |
| DB4 | `0DSLFGHJ` | D = Doppeltraktion; S = Smartsearch; L = F0 (Licht); F = F4; G = F3; H = F2; J = F1 |
| DB5 | F5–F12 | F5 ist Bit0 (LSB) |
| DB6 | F13–F20 | F13 ist Bit0 (LSB) |
| DB7 | F21–F28 | F21 ist Bit0 (LSB) |
| DB8 | F29–F31 | ab FW 1.42 (falls DataLen ≥ 15); F29 ist Bit0 (LSB) |
| DBn | optional | für zukünftige Erweiterungen |

### 4.5 LAN_X_SET_LOCO_E_STOP

*Ab Z21 FW Version 1.43.* Hält eine Lok an. Bei DCC wird die Fahrstufe „E-STOP" (RCN-212) ausgegeben; bei MM die Fahrstufe 0 („Stop").

| DataLen | Header | X-Header | DB0 | DB2 | XOR |
|---|---|---|---|---|---|
| 0x08 0x00 | 0x40 0x00 | 0x92 | Adr_MSB | Adr_LSB | XOR |

`Lok-Adresse = (Adr_MSB & 0x3F) << 8 + Adr_LSB` (≥ 128: `DB1 = 0xC0 | Adr_MSB`). **Antwort:** keine Standardantwort, 4.4 LOCO_INFO an Clients mit Abo.

### 4.6 LAN_X_PURGE_LOCO

*Ab Z21 FW Version 1.43.* Nimmt eine Lok aus der Z21 heraus; die Fahrbefehle für diese Lok am Gleis werden beendet (bis ein neuer Fahr-/Funktionsbefehl an dieselbe Adresse kommt). Damit kann z.B. eine PC-Steuerung die Anzahl der Loks und den Datendurchsatz am Gleis beeinflussen.

| DataLen | Header | X-Header | DB0 | DB1 | DB2 | XOR |
|---|---|---|---|---|---|---|
| 0x09 0x00 | 0x40 0x00 | 0xE3 | 0x44 | Adr_MSB | Adr_LSB | XOR |

`Lok-Adresse = (Adr_MSB & 0x3F) << 8 + Adr_LSB` (≥ 128: `DB1 = 0xC0 | Adr_MSB`). Keine Antwort an Aufrufer/andere Clients.

---

## 5 Schalten

Meldungen zum Schalten von Funktionsdecodern („Accessory Decoder" RP-9.2.1, z.B. Weichendecoder).

Die Visualisierung der Weichennummer ist bei vielen DCC-Systemen unterschiedlich gelöst. Gemäß DCC gibt es pro Accessorydecoder-Adresse vier Ports mit je zwei Ausgängen. Übliche Visualisierungen:
1. Nummerierung ab 1, DCC-Adresse ab 1, je 4 Ports (ESU, Uhlenbrock): Weiche #1 = Addr 1/Port 0; #5 = Addr 2/Port 0; #6 = Addr 2/Port 1.
2. Nummerierung ab 1, DCC-Adresse ab 0, je 4 Ports (Roco, Lenz): Weiche #1 = Addr 0/Port 0; #5 = Addr 1/Port 0; #6 = Addr 1/Port 1.
3. Virtuelle Weichennummer mit frei konfigurierbarer DCC-Adresse/Port (Twin-Center).
4. Darstellung DCC-Adresse / Port (Zimo).

Umsetzung der Input-Parameter (FAdr_MSB, FAdr_LSB, A, P) in den DCC Accessory Befehl. DCC Basic Accessory Decoder Packet Format: `{preamble} 0 10AAAAAA 0 1aaaCDDd 0 EEEEEEEE 1`

```c
UINT16 FAdr = (FAdr_MSB << 8) + FAdr_LSB;
UINT16 Dcc_Addr = FAdr >> 2;
aaaAAAAAA = (~Dcc_Addr & 0x1C0) | (Dcc_Addr & 0x003F);  // DCC Adresse
C = A;             // Ausgang aktivieren oder deaktivieren
DD = FAdr & 0x03;  // Port
d = P;             // Weiche nach links oder rechts
```

Beispiel: FAdr=0 → DCC-Addr 0/Port 0; FAdr=3 → DCC-Addr 0/Port 3; FAdr=4 → DCC-Addr 1/Port 0. Bei MM gilt: FAdr=0 → MM-Addr 1; FAdr=1 → MM-Addr 2; …

Ein Client kann Funktions-Infos abonnieren (Broadcast-Flag 0x00000001). Die tatsächliche Stellung der Weiche hängt von Verkabelung/Konfiguration ab; daher wird auf „gerade"/„abzweigend" bewusst verzichtet.

### 5.1 LAN_X_GET_TURNOUT_INFO

Anfordern des Status einer Weiche/Schaltfunktion.

| DataLen | Header | X-Header | DB0 | DB1 | XOR |
|---|---|---|---|---|---|
| 0x08 0x00 | 0x40 0x00 | 0x43 | FAdr_MSB | FAdr_LSB | XOR |

`Funktions-Adresse = (FAdr_MSB << 8) + FAdr_LSB`. **Antwort:** siehe [5.3](#53-lan_x_turnout_info).

### 5.2 LAN_X_SET_TURNOUT

Schalten einer Weiche.

| DataLen | Header | X-Header | DB0 | DB1 | DB2 | XOR |
|---|---|---|---|---|---|---|
| 0x09 0x00 | 0x40 0x00 | 0x53 | FAdr_MSB | FAdr_LSB | 10Q0A00P | XOR |

`Funktions-Adresse = (FAdr_MSB << 8) + FAdr_LSB`. `1000A00P`:
- `A=0` Weichenausgang deaktivieren / `A=1` aktivieren
- `P=0` Ausgang 1 wählen / `P=1` Ausgang 2 wählen
- `Q=0` Kommando sofort ausführen
- `Q=1` (ab FW V1.24) Weichenbefehl in Z21-Queue einfügen und zum nächstmöglichen Zeitpunkt am Gleis ausgeben

**Antwort:** keine Standardantwort, [5.3](#53-lan_x_turnout_info) an Clients mit Abo. Das Q-Flag wurde ab FW V1.24 eingeführt.

#### 5.2.1 LAN_X_SET_TURNOUT mit Q=0

Bei `Q=0` verhält sich die Z21 kompatibel zu früheren Versionen: der Befehl wird sofort ausgegeben. Das Activate (A=1) wird ausgegeben, bis das entsprechende Deactivate gesendet wird. Es darf zu einem Zeitpunkt nur ein Weichenstellbefehl aktiv sein. Die korrekte Reihenfolge (Activate → Deactivate) und das Timing der Schaltdauer liegen in der Verantwortung des LAN-Clients.

- **Falsch:** mehrere Weichen gleichzeitig aktivieren, dann gemeinsam deaktivieren.
- **Richtig:** je Weiche: aktivieren → ~100 ms warten → deaktivieren → ~50 ms warten, dann nächste.

*(Abbildung 3: DCC Sniff am Gleis bei Q=0.)*

#### 5.2.2 LAN_X_SET_TURNOUT mit Q=1

Bei `Q=1` wird der Befehl in einer internen FIFO-Queue eingereiht und beim Generieren des Gleissignals viermal am Gleis ausgegeben. Das befreit den Client von der Serialisierung — Schaltbefehle dürfen gemischt gesendet werden (Fahrstraßen!). Der Client kümmert sich nur noch um das Timing des Deactivate; bei manchen DCC-Decodern kann es entfallen, bei MM jedoch nicht (z.B. k83 ohne Endabschaltung).

**Vermischen Sie keinesfalls Schaltbefehle mit Q=0 und Q=1.** *(Abbildung 4: DCC Sniff am Gleis bei Q=1.)*

### 5.3 LAN_X_TURNOUT_INFO

Antwort auf [5.1](#51-lan_x_get_turnout_info), aber auch ungefragt bei Statusänderung (Broadcast-Flag 0x00000001).

| DataLen | Header | X-Header | DB0 | DB1 | DB2 | XOR |
|---|---|---|---|---|---|---|
| 0x09 0x00 | 0x40 0x00 | 0x43 | FAdr_MSB | FAdr_LSB | 000000ZZ | XOR |

`Funktions-Adresse = (FAdr_MSB << 8) + FAdr_LSB`. `000000ZZ`:
- `ZZ=00` Weiche noch nicht geschaltet
- `ZZ=01` Weiche steht gemäß „P=0"
- `ZZ=10` Weiche steht gemäß „P=1"
- `ZZ=11` ungültige Kombination

*(Abbildung 5: Beispiel Sequenz Weiche schalten.)*

### 5.4 LAN_X_SET_EXT_ACCESSORY

*Ab Z21 FW V1.40.* Sendet einen DCC-Befehl im „erweiterten Zubehördecoder Paketformat" (DCCext) an einen Erweiterten Zubehördecoder (siehe RCN-213 Abschnitt 2.3).

| DataLen | Header | X-Header | DB0 | DB1 | DB2 | DB3 | XOR |
|---|---|---|---|---|---|---|---|
| 0x0A 0x00 | 0x40 0x00 | 0x54 | Adr_MSB | Adr_LSB | DDDDDDDD | 0x00 | XOR |

`RawAddress = (Adr_MSB << 8) + Adr_LSB`.
- **RawAddress**: Die RawAddress für den ersten erweiterten Zubehördecoder ist gem. RCN-213 die Adresse 4 (in Anwenderdialogen als „Adresse 1" dargestellt). Adressierung strikt nach RCN-213, ohne abweichende Verschiebung.
- **DDDDDDDD**: über Bits 0–7 werden die 256 möglichen Zustände übertragen, im Erweiterten Zubehördecoder-Paketformat gem. RCN-213.

Hinweis: Der **10836 Z21 switch DECODER** interpretiert DDDDDDDD als `RZZZZZZZ`:
- `ZZZZZZZ` = Einschaltzeit (Auflösung 100 ms). 0 = Ausgang aus; 127 = dauerhaft eingeschaltet (bis zum nächsten Befehl).
- Bit 7 `R` wählt den Ausgang: R=1 „grün" (gerade), R=0 „rot" (abzweigend).

Der **10837 Z21 signal DECODER** interpretiert DDDDDDDD als einen von 256 Signalbegriffen (Wertebereich abhängig vom Signaltyp). Beispiele: `0`=absoluter Haltebegriff, `4`=Fahrt 40 km/h, `16`=freie Fahrt, `65 (0x41)`=Rangieren erlaubt, `66 (0x42)`=Dunkelschaltung, `69 (0x45)`=Ersatzsignal. Konkrete Werte siehe `https://www.z21.eu/de/produkte/z21-signal-decoder/signaltypen` unter „DCCext".

**Antwort:** keine Standardantwort, oder [5.6](#56-lan_x_ext_accessory_info) an Clients mit Abo.

Beispiel: `0x0A 0x00 0x40 0x00 0x54 0x00 0x04 0x05 0x00 0x55` → an Decoder RawAddress=4 (Anwender-Adresse 1) Wert DDDDDDDD=5. Beim 10836 switch DECODER: Ausgang 1 „rot" (Klemme 1A) ein, nach 5×100 ms automatisch aus.

„Notaus-Befehl für Erweiterte Zubehördecoder" (RCN-213, 2.4) = Wert 0 für RawAddress=2047: `0x0A 0x00 0x40 0x00 0x54 0x07 0xFF 0x00 0x00 0xAC`.

### 5.5 LAN_X_GET_EXT_ACCESSORY_INFO

*Ab Z21 FW V1.40.* Abfragen des letzten an einen Erweiterten Zubehördecoder übertragenen Befehls.

| DataLen | Header | X-Header | DB0 | DB1 | DB2 | XOR |
|---|---|---|---|---|---|---|
| 0x09 0x00 | 0x40 0x00 | 0x44 | Adr_MSB | Adr_LSB | 0x00 | XOR |

`RawAddress = (Adr_MSB << 8) + Adr_LSB`. DB2 reserviert (mit 0 initialisieren). **Antwort:** siehe [5.6](#56-lan_x_ext_accessory_info).

### 5.6 LAN_X_EXT_ACCESSORY_INFO

Antwort auf [5.5](#55-lan_x_get_ext_accessory_info), aber auch ungefragt, wenn jemand anderes ein Kommando an einen Erweiterten Zubehördecoder sendet (Broadcast-Flag 0x00000001).

| DataLen | Header | X-Header | DB0 | DB1 | DB2 | DB3 | XOR |
|---|---|---|---|---|---|---|---|
| 0x0A 0x00 | 0x40 0x00 | 0x44 | Adr_MSB | Adr_LSB | DDDDDDDD | Status | XOR |

`RawAddress = (Adr_MSB << 8) + Adr_LSB`. DDDDDDDD = Zustand (Erweitertes Zubehördecoder-Paketformat). Status: `0x00` = Data Valid, `0xFF` = Data Unknown.

---

## 6 Decoder CV Lesen und Schreiben

Meldungen zum Lesen/Schreiben von Decoder-CVs (Configuration Variable, RP-9.2.2, RP-9.2.3). Ob bit- oder byteweiser Zugriff erfolgt, hängt von den Z21-Einstellungen ab.

### 6.1 LAN_X_CV_READ

CV im Direct-Mode auslesen.

| DataLen | Header | X-Header | DB0 | DB1 | DB2 | XOR |
|---|---|---|---|---|---|---|
| 0x09 0x00 | 0x40 0x00 | 0x23 | 0x11 | CVAdr_MSB | CVAdr_LSB | XOR |

`CV-Adresse = (CVAdr_MSB << 8) + CVAdr_LSB`, mit 0=CV1, 1=CV2, 255=CV256, usw. **Antwort:** 2.9 ProgrammingMode an Clients mit Abo, sowie Ergebnis [6.3](#63-lan_x_cv_nack_sc)/[6.4](#64-lan_x_cv_nack)/[6.5](#65-lan_x_cv_result).

### 6.2 LAN_X_CV_WRITE

CV im Direct-Mode überschreiben.

| DataLen | Header | X-Header | DB0 | DB1 | DB2 | DB3 | XOR |
|---|---|---|---|---|---|---|---|
| 0x0A 0x00 | 0x40 0x00 | 0x24 | 0x12 | CVAdr_MSB | CVAdr_LSB | Value | XOR |

`CV-Adresse = (CVAdr_MSB << 8) + CVAdr_LSB`. **Antwort:** wie 6.1.

### 6.3 LAN_X_CV_NACK_SC

Wird bei fehlerhafter Programmierung wegen Kurzschluss am Gleis automatisch an den auslösenden Client geschickt. **Z21 an Client:** X-Header `0x61`, DB0 `0x12`, XOR `0x73`.

### 6.4 LAN_X_CV_NACK

Wird gesendet, wenn das ACK vom Decoder ausbleibt. Bei byteweisem Zugriff kann das Lesen lange dauern. **Z21 an Client:** X-Header `0x61`, DB0 `0x13`, XOR `0x72`.

### 6.5 LAN_X_CV_RESULT

„Positives ACK", an den auslösenden Client.

| DataLen | Header | X-Header | DB0 | DB1 | DB2 | DB3 | XOR |
|---|---|---|---|---|---|---|---|
| 0x0A 0x00 | 0x40 0x00 | 0x64 | 0x14 | CVAdr_MSB | CVAdr_LSB | Value | XOR |

`CV-Adresse = (CVAdr_MSB << 8) + CVAdr_LSB`. *(Abbildung 6: Beispiel Sequenz CV Lesen.)*

### 6.6 LAN_X_CV_POM_WRITE_BYTE

Schreibt eine CV eines Lokdecoders (NMRA S-9.2.1 Abschnitt C) auf dem Hauptgleis (POM „Programming on the Main"). Normaler Betriebsmodus (Gleisspannung ein, Programmiermodus aus). Keine Rückmeldung.

| DataLen | Header | X-Header | DB0 | DB1..DB5 | XOR |
|---|---|---|---|---|---|
| 0x0C 0x00 | 0x40 0x00 | 0xE6 | 0x30 | POM-Parameter | XOR |

POM-Parameter:

| Pos | Daten | Bedeutung |
|---|---|---|
| DB1 | Adr_MSB | |
| DB2 | Adr_LSB | `Lok-Adresse = (Adr_MSB & 0x3F) << 8 + Adr_LSB` |
| DB3 | `111011MM` | Option `0xEC`; MM = CVAdr_MSB |
| DB4 | CVAdr_LSB | `CV-Adresse = (MM << 8) + CVAdr_LSB` (0=CV1, …) |
| DB5 | Value | neuer CV-Wert |

**Antwort:** keine.

### 6.7 LAN_X_CV_POM_WRITE_BIT

Wie 6.6, aber schreibt ein Bit einer CV (POM). DB3 = `111010MM` (Option `0xE8`), DB5 = `0000VPPP` (PPP = Bit-Position, V = neuer Bit-Wert). **Antwort:** keine.

### 6.8 LAN_X_CV_POM_READ_BYTE

*Ab Z21 FW Version 1.22.* Liest eine CV eines Lokdecoders auf dem Hauptgleis (POM). RailCom muss in der Z21 aktiviert, der Decoder RailCom-fähig sein (CV28 Bit 0/1 und CV29 Bit 3 = 1, Zimo).

POM-Parameter: DB3 = `111001MM` (Option `0xE4`), DB5 = `0`. **Antwort:** [6.4](#64-lan_x_cv_nack) oder [6.5](#65-lan_x_cv_result).

### 6.9 LAN_X_CV_POM_ACCESSORY_WRITE_BYTE

*Ab Z21 FW Version 1.22.* Schreibt eine CV eines Accessory Decoders (NMRA S-9.2.1 Abschnitt D) auf dem Hauptgleis (POM). Keine Rückmeldung. `Header X-Header=0xE6`, DB0 `0x31`.

POM-Parameter:

| Pos | Daten | Bedeutung |
|---|---|---|
| DB1 | aaaaa | Decoder_Adresse MSB |
| DB2 | AAAACDDD | `aaaaaAAAACDDD = ((Decoder_Addresse & 0x1FF) << 4) \| CDDD`. CDDD=0000 → CV bezieht sich auf ganzen Decoder; C=1 → DDD = Ausgangsnummer |
| DB3 | `111011MM` | Option `0xEC`; MM = CVAdr_MSB |
| DB4 | CVAdr_LSB | `CV-Adresse = (MM << 8) + CVAdr_LSB` |
| DB5 | Value | neuer CV-Wert |

**Antwort:** keine.

### 6.10 LAN_X_CV_POM_ACCESSORY_WRITE_BIT

*Ab Z21 FW Version 1.22.* Wie 6.9, aber Bit-Schreiben. DB3 = `111010MM` (Option `0xE8`), DB5 = `0000VPPP`. **Antwort:** keine.

### 6.11 LAN_X_CV_POM_ACCESSORY_READ_BYTE

*Ab Z21 FW Version 1.22.* Liest eine CV eines Accessory Decoders (POM). RailCom muss aktiviert sein, der Decoder RailCom-fähig. DB3 = `111001MM` (Option `0xE4`), DB5 = `0`. **Antwort:** [6.4](#64-lan_x_cv_nack) oder [6.5](#65-lan_x_cv_result).

### 6.12 LAN_X_MM_WRITE_BYTE

*Ab Z21 FW Version 1.23.* Überschreibt ein Register eines Motorola-Decoders auf dem Programmiergleis.

| DataLen | Header | X-Header | DB0 | DB1 | DB2 | DB3 | XOR |
|---|---|---|---|---|---|---|---|
| 0x0A 0x00 | 0x40 0x00 | 0x24 | 0xFF | 0 | RegAdr | Value | XOR |

`RegAdr`: 0=Register1, …, 78=Register79. `0 ≤ Value ≤ 255` (manche Decoder nur 0–80). **Antwort:** 2.9 ProgrammingMode an Clients mit Abo, sowie [6.3](#63-lan_x_cv_nack_sc) oder [6.5](#65-lan_x_cv_result).

*Anmerkung:* Die Z21 verwendet den „6021-Programmiermodus" für MM-Decoder (nur Schreiben, kein Lesen, keine Erfolgsprüfung außer Kurzschlusserkennung). Funktioniert für viele Decoder von ESU, Zimo, Märklin — nicht zwingend für alle MM-Decoder (z.B. nicht mit DIP-Schaltern). `LAN_X_CV_RESULT` bedeutet hier nur „Programmiervorgang beendet", nicht „erfolgreich". Beispiel: `0x0A 0x00 0x40 0x00 0x24 0xFF 0x00 0x00 0x05 0xDE` → „Ändere Lokdecoder-Adresse (Register1) auf 5".

### 6.13 LAN_X_DCC_READ_REGISTER

*Ab Z21 FW Version 1.25.* Liest ein Register eines DCC-Decoders im Registermodus (S-9.2.3) auf dem Programmiergleis.

| DataLen | Header | X-Header | DB0 | DB1 | XOR |
|---|---|---|---|---|---|
| 0x08 0x00 | 0x40 0x00 | 0x22 | 0x11 | REG | XOR |

`REG`: 0x01=Register1, …, 0x08=Register8. `0 ≤ Value ≤ 255`. **Antwort:** 2.9 ProgrammingMode an Clients mit Abo, sowie [6.3](#63-lan_x_cv_nack_sc) oder [6.5](#65-lan_x_cv_result). *Registermodus nur für sehr alte DCC-Decoder; Direct CV bevorzugen.*

### 6.14 LAN_X_DCC_WRITE_REGISTER

*Ab Z21 FW Version 1.25.* Überschreibt ein Register eines DCC-Decoders im Registermodus (S-9.2.3).

| DataLen | Header | X-Header | DB0 | DB2 | DB3 | XOR |
|---|---|---|---|---|---|---|
| 0x09 0x00 | 0x40 0x00 | 0x23 | 0x12 | REG | Value | XOR |

`REG`: 0x01–0x08. `0 ≤ Value ≤ 255`. **Antwort:** wie 6.13. *Direct CV bevorzugen.*

---

## 7 Rückmelder – R-BUS

Die Rückmeldemodule (Bestellnummer 10787, 10808 und 10819) am R-BUS können mit folgenden Kommandos ausgelesen und konfiguriert werden.

### 7.1 LAN_RMBUS_DATACHANGED

Änderung am Rückmeldebus melden. Asynchron, wenn der Broadcast (Flag 0x00000002) aktiviert ist oder der Status explizit angefordert wurde.

| DataLen | Header | Gruppenindex (1 Byte) | Rückmelder-Status (10 Byte) |
|---|---|---|---|
| 0x0F 0x00 | 0x80 0x00 | … | … |

- **Gruppenindex:** `0` = Module mit Adressen 1–10, `1` = Module mit Adressen 11–20.
- **Rückmelder-Status:** 1 Byte pro Rückmelder, 1 Bit pro Eingang. Zuordnung statisch aufsteigend.

Beispiel: GruppenIndex=1, Status `0x01 0x00 0xC5 0x00 ...` → „Rückmelder 11 Kontakt auf Eingang 1; Rückmelder 13 Kontakt auf Eingang 8,7,3 und 1".

### 7.2 LAN_RMBUS_GETDATA

Anfordern des aktuellen Status. **Anforderung:** `Header=0x81 0x00`, Gruppenindex (1 Byte). **Antwort:** siehe 7.1.

### 7.3 LAN_RMBUS_PROGRAMMODULE

Ändern der Rückmelder-Adresse. **Anforderung:** `Header=0x82 0x00`, Adresse (1 Byte). **Antwort:** keine.

Adresse = neue Adresse (Wertebereich 0 und 1…20). Der Programmierbefehl wird am R-BUS ausgegeben, bis dieser Befehl erneut mit Adresse=0 gesendet wird. Während der Programmierung darf sich kein anderes Modul am R-BUS befinden. *(Abbildung 7: Beispiel Sequenz Rückmeldemodul programmieren.)*

---

## 8 RailCom

Die Z21 unterstützt RailCom durch:
- Erzeugung der RailCom-Lücke am Gleissignal.
- Globaler Empfänger in der Z21.
- Lokale Empfänger, z.B. in den Belegtmeldern 10808 (Lokerkennung; Kanal-2-Daten über CAN ab FW V1.29).
- POM-Lesen (siehe [6.8](#68-lan_x_cv_pom_read_byte) ab FW V1.22).
- Lokadressen-Erkennung bei Belegtmeldern (siehe [9.5](#95-lan_loconet_detector) ab V1.22 und [10.1](#101-lan_can_detector) ab V1.30).
- Decoder-Geschwindigkeit und Decoder-QoS ab FW V1.29.

Voraussetzung: Decoder RailCom-fähig, CV28/CV29 korrekt konfiguriert, Option „RailCom" in der Z21 aktiviert.

### 8.1 LAN_RAILCOM_DATACHANGED

Ab FW V1.29. Antwort auf [8.2](#82-lan_railcom_getdata); auch ungefragt, wenn sich RailCom-Daten ändern und der Client den Broadcast (Flag 0x00000004) mit abonnierter Lok-Adresse oder den Broadcast 0x00040000 (alle Loks) aktiviert hat.

**Z21 an Client:** `DataLen=0x11 0x00`, `Header=0x88 0x00`, RailComDaten.

| Offset | Typ | Name | Bedeutung |
|---|---|---|---|
| 0 | UINT16 | LocoAddress | Adresse des erkannten Decoders |
| 2 | UINT32 | ReceiveCounter | Empfangszähler in Z21 |
| 6 | UINT16 | ErrorCounter | Empfangsfehlerzähler in Z21 |
| 8 | UINT8 | reserved | |
| 9 | UINT8 | Options | Flags-Bitmaske (siehe unten) |
| 10 | UINT8 | Speed | Geschwindigkeit 1 oder 2 (falls unterstützt) |
| 11 | UINT8 | QoS | Quality of Service (falls unterstützt) |
| 12 | UINT8 | reserved | |

```c
#define rcoSpeed1  0x01  // CH7 subindex 0
#define rcoSpeed2  0x02  // CH7 subindex 1
#define rcoQoS     0x04  // CH7 subindex 7
```

Die Struktur kann in Zukunft vergrößert werden — bei der Auswertung unbedingt DataLen berücksichtigen.

### 8.2 LAN_RAILCOM_GETDATA

RailCom-Daten anfordern (ab FW V1.29). **Anforderung:** `Header=0x89 0x00`, Typ 8 bit + LocoAddress 16 bit (little endian).
- Typ `0x01` = RailCom-Daten für gegebene Lokadresse anfordern.
- LocoAddress: Lokadresse; `0` = nächste Lok im Ringbuffer.

**Antwort:** siehe [8.1](#81-lan_railcom_datachanged).

---

## 9 LocoNet

*Ab Z21 FW Version 1.20.* Die Z21 kann als Ethernet/LocoNet Gateway verwendet werden, wobei sie gleichzeitig der LocoNet-Master ist.

Damit der Client Meldungen vom LocoNet bekommt, muss er die entsprechenden Meldungen mittels [2.16](#216-lan_set_broadcastflags) abonniert haben.
- Empfangene Meldungen → Header `LAN_LOCONET_Z21_RX`.
- Selbst gesendete Meldungen → Header `LAN_LOCONET_Z21_TX`.
- Mit `LAN_LOCONET_FROM_LAN` kann der Client selbst Meldungen auf den Bus schreiben (andere Clients mit Abo werden ebenfalls per `LAN_LOCONET_FROM_LAN` benachrichtigt; nur der Absender nicht).

*(Abbildung 8: Beispiel Sequenz Ethernet/LocoNet Gateway.)* Selbst triviale Vorgänge am Bus können erheblichen Netzwerkverkehr erzeugen. Diese Funktionalität ist primär für PC-Steuerungen gedacht. Wägen Sie die Flags 0x02000000 (Loks) und 0x04000000 (Weichen) genau ab — verwenden Sie zum konventionellen Fahren/Schalten möglichst die Befehle aus den Kapiteln 4, 5 und 6. Das LocoNet-Protokoll selbst wird hier nicht beschrieben (siehe Digitrax bzw. Hardware-Hersteller).

### 9.1 LAN_LOCONET_Z21_RX

*Ab FW 1.20.* Asynchron, wenn der Broadcast (Flags 0x01000000/0x02000000/0x04000000) aktiviert ist und eine Meldung am LocoNet-Bus empfangen wurde.

| DataLen | Header | Data |
|---|---|---|
| 0x04+n 0x00 | 0xA0 0x00 | LocoNet Meldung inkl. CKSUM (n Bytes) |

### 9.2 LAN_LOCONET_Z21_TX

*Ab FW 1.20.* Analog zu 9.1, wenn die Z21 eine Meldung auf den Bus geschrieben hat. `Header=0xA1 0x00`, LocoNet Meldung inkl. CKSUM (n Bytes).

### 9.3 LAN_LOCONET_FROM_LAN

*Ab FW 1.20.* Ein Client schreibt eine Meldung auf den LocoNet-Bus. Wird auch asynchron an andere Clients gemeldet (Flags 0x01000000/0x02000000/0x04000000), wenn ein anderer Client geschrieben hat.

| DataLen | Header | Data |
|---|---|---|
| 0x04+n 0x00 | 0xA2 0x00 | LocoNet Meldung inkl. CKSUM (n Bytes) |

#### 9.3.1 DCC Binary State Control Instruction per LocoNet OPC_IMM_PACKET

Ab FW V1.42 wird zum Schalten von Binary States das neue Kommando [4.3.3 LAN_X_SET_LOCO_BINARY_STATE](#433-lan_x_set_loco_binary_state) empfohlen. Der folgende (etwas veraltete) Absatz bleibt zur Vollständigkeit:

Ab FW V1.25 können mittels `LAN_LOCONET_FROM_LAN` und dem LocoNet-Befehl `OPC_IMM_PACKET` beliebige DCC-Pakete am Gleisausgang generiert werden, darunter die Binary State Control Instruction („F29…F32767"). Das gilt auch für die weiße z21 (virtueller LocoNet-Stack). Zum Aufbau siehe LocoNet Spec bzw. NMRA S-9.2.1 (Feature Expansion Instruction).

### 9.4 LAN_LOCONET_DISPATCH_ADDR

*Ab FW 1.20.* Eine Lok-Adresse zum LocoNet-Dispatch vorbereiten („DISPATCH_PUT").

**Anforderung:** `Header=0xA3 0x00`, Lok-Adresse 16 bit (little endian).

**Antwort:**
- FW < 1.22: keine.
- FW ≥ 1.22: `Header=0xA3 0x00`, Lok-Adresse 16 bit (little endian) + Ergebnis 8 bit.
  - `0` = „DISPATCH_PUT" fehlgeschlagen (z.B. Z21 als Slave, Master hat abgelehnt, Adresse bereits zugeteilt).
  - `>0` = erfolgreich; Wert = aktuelle LocoNet Slot-Nummer für die Lok-Adresse.

*(Abbildung 9: Beispiel Sequenz LocoNet Dispatch per LAN-Client.)*

### 9.5 LAN_LOCONET_DETECTOR

*Ab FW 1.22.* Abfragen/Benachrichtigung über Belegtstatus von LocoNet-Gleisbesetztmeldern, ohne das LocoNet-Protokoll selbst verarbeiten zu müssen.

*Unterschied:* Roco 10787 (R-BUS) basiert auf mechanischen Schaltkontakten; LocoNet-Gleisbesetztmelder basieren üblicherweise auf Strommessung bzw. Transponder/Infrarot/RailCom (im Idealfall nur eine Meldung bei Statusänderung).

**Anforderung (Status abfragen):**

| DataLen | Header | Typ 8 bit | Reportadresse 16 bit (little endian) |
|---|---|---|---|
| 0x07 0x00 | 0xA4 0x00 | … | … |

- `0x80`: „Stationary Interrogate Request" (SIC) gem. Digitrax (auch Blücher-Elektronik). Reportadresse hier 0 (don't care).
- `0x81`: Reportadresse für Uhlenbrock-Besetztmelder (z.B. UB63320 über LNCV 17; Default 1017). Nur zum Abfragen, nicht mit Rückmelderadresse zu verwechseln. Am LocoNet-Bus über Weichenstellbefehle implementiert → Wert um 1 dekrementiert übergeben. Beispiel: `0x07 0x00 0xA4 0x00 0x81 0xF8 0x03` → Status aller Besetztmelder mit Reportadresse 1017 (`= 0x03F8 + 1 = 1016 + 1`).
- `0x82`: Statusabfrage für LISSY (ab FW 1.23). Bei Uhlenbrock LISSY entspricht die Reportadresse der Rückmelderadresse; Rückmeldung abhängig vom LISSY-Betriebsmodus.

Bei einer Anfrage können mehrere Besetztmelder gleichzeitig angesprochen werden → mehrere Antworten, ggf. mehrfach pro Eingang.

**Antwort:**

| DataLen | Header | Typ 8 bit | Rückmelderadresse 16 bit (little endian) | Info[n] |
|---|---|---|---|---|
| 0x07+n 0x00 | 0xA4 0x00 | … | … | … |

Asynchron, wenn der Broadcast (Flag 0x08000000) aktiviert ist und eine Meldung empfangen wurde (Statusänderung oder explizite Abfrage). **Rückmelderadresse:** jedem Eingang zugeordnet, vom Anwender konfigurierbar (z.B. via LNCV).

| Typ | Bedeutung | n | Info |
|---|---|---|---|
| `0x01` | Besetzt/Frei (z.B. Uhlenbrock 63320, Blücher GBM16XL; LocoNet OPC_INPUT_REP, X=1) | 1 | Info[0]=0 → frei (LO), =1 → belegt (HI) |
| `0x02` | Transponder Enters Block (z.B. Blücher GBM16XN, OPC_MULTI_SENSE) | 2 | Transponderadresse 16 Bit LE: Info[0]=Low, Info[1]=High |
| `0x03` | Transponder Exits Block | 2 | wie 0x02 |
| `0x10` | LISSY Lokadresse (ab FW 1.23; Uhlenbrock-Übergabeformat, LNCV 15=1) | 3 | Info[0/1]=Lokadresse 16 Bit LE; Info[2]=`0 DIR1 DIR0 0 K3 K2 K1 K0` |
| `0x11` | LISSY Belegtzustand (ab FW 1.23) | 1 | Info[0]=0 → Block frei, =1 → belegt |
| `0x12` | LISSY Geschwindigkeit (ab FW 1.23) | 2 | Info[0/1]=Geschwindigkeit 16 Bit LE |

Hinweise zu Typ 0x02/0x03 (GBM16XN): Transponderadresse identifiziert das Fahrzeug (Lok-Adresse via RailCom). Zur Rückmelderadresse +1 addieren, um die im GBM16XN konfigurierte Adresse zu erhalten. Das Bit unter Maske 0x1000 (Fahrtrichtung) kollidiert mit dem Adressraum langer Lok-Adressen — diese Konfiguration wird nicht empfohlen.

Hinweise zu Typ 0x10 (LISSY): Loks 1…9999, Wagen 10000…16382. `DIR1=0` → DIR0 ignorieren; `DIR1=1` → DIR0=0 vorwärts, DIR0=1 rückwärts; K3..K0 = 4-Bit Klasseninformation. *Beispielkonfigurationen für Lissy-Empfänger 68610 (LNCV-Tabellen) siehe Original.* Typ wird künftig um weitere IDs erweitert.

---

## 10 CAN

### 10.1 LAN_CAN_DETECTOR

*Ab Z21 FW Version 1.30.* Der Roco CAN-Belegtmelder 10808 wird ab FW 1.30 unterstützt. Vier Verwendungsweisen:
1. **R-BUS-Emulation**: Belegtmelder als R-BUS-Melder (siehe Kapitel 7).
2. **LocoNet-Emulation**: als LocoNet-Melder (siehe 9.5; Typ 0x01 belegt/frei, Typ 0x02/0x03 Transponder).
3. **LISSY-Emulation**: durch LISSY/Marco-Meldungen (siehe 9.5; Typ 0x10 Lokadresse, Typ 0x11 Belegtzustand).
4. **Direkter Zugriff** durch `LAN_CAN_DETECTOR` (siehe unten).

Emulation konfigurierbar über das Z21 Maintenance Tool. Werkseinstellung: R-BUS=ein, LocoNet=ein, LISSY=aus. Der direkte Zugriff (`0xC4`) ist am schnellsten und ressourcenschonendsten — empfohlen bei vielen CAN-Belegtmeldern.

**Anforderung:**

| DataLen | Header | Typ 8 bit | CAN-NetworkID 16 bit (little endian) |
|---|---|---|---|
| 0x07 0x00 | 0xC4 0x00 | 0x00 | … |

- Typ `0x00`: Abfrage des Belegtmelders mit gegebener CAN-NetworkID. `0xD000` = „alle CAN-Belegtmelder". Beispiel: `0x07 0x00 0xC4 0x00 0x00 0x00 0xD0`.

**Antwort:**

| DataLen | Header | NId 16 | Addr 16 | Port 8 | Typ 8 | Value1 16 | Value2 16 |
|---|---|---|---|---|---|---|---|
| 0x0E 0x00 | 0xC4 0x00 | … | … | … | … | … | … |

Asynchron, wenn der Broadcast (Flag 0x00080000) aktiviert ist und eine Meldung empfangen wurde. Alle 16-bit Werte little endian.
- **NId**: unveränderbare CAN-NetworkID.
- **Addr**: konfigurierbare Moduladresse.
- **Port**: Eingang (0–7).
- **Typ**: `0x01` Belegtstatus; `0x11`–`0x1F` erkannte Lokadressen (0x11 = 1./2., 0x12 = 3./4., …, 0x1F = 29./30.).

Falls Typ = `0x01` (Belegtstatus), Value1:

| Wert | Bedeutung |
|---|---|
| 0x0000 | Frei, ohne Spannung |
| 0x0100 | Frei, mit Spannung |
| 0x1000 | Besetzt, ohne Spannung |
| 0x1100 | Besetzt, mit Spannung |
| 0x1201 | Besetzt, Überlast 1 |
| 0x1202 | Besetzt, Überlast 2 |
| 0x1203 | Besetzt, Überlast 3 |

Falls Typ = `0x11`–`0x1F` (RailCom Lokadressen): Value1/Value2 = erste/zweite erkannte Lokadresse inkl. Richtung. `0` = keine Adresse erkannt bzw. Listenende. In den obersten 2 Bits: `0x` keine Richtung, `10` vorwärts, `11` rückwärts; in den untersten 14 Bits die Lokadresse.

### 10.2 CAN Booster

*Ab Z21 FW Version 1.41.* LAN-Befehle für CAN-Booster-Management (Roco 10806, 10807, 10869). Funktionieren nur, wenn die Booster über CAN-Bus (nicht B-BUS) mit der Z21 verbunden sind.

#### 10.2.1 LAN_CAN_DEVICE_GET_DESCRIPTION

Bezeichnung (Freitext) aus CAN-Booster auslesen. **Anforderung:** `Header=0xC8 0x00`, NId 16 bit. **Antwort:** `DataLen=0x16 0x00`, `Header=0xC8 0x00`, NId 16 bit + `UINT8 Name[16]`.

NId = CAN-NetworkID (0xC101–0xC1FF). Name = nullterminierter String, ISO 8859-1 (Latin-1). *Hinweis:* nicht zwei Requests schnell hintereinander senden; zuerst Antwort abwarten. Die NetworkIDs aller Booster liefert [10.2.3](#1023-lan_can_booster_systemstate_chgd).

#### 10.2.2 LAN_CAN_DEVICE_SET_DESCRIPTION

Bezeichnung überschreiben. **Anforderung:** `Header=0xC9 0x00`, NId 16 bit + `UINT8 Name[16]`. **Antwort:** keine. Rest von Data mit 0x00 auffüllen; nach 16 Zeichen wird abgeschnitten. Nicht erlaubt: `"` (0x22) und `\` (0x5C).

#### 10.2.3 LAN_CAN_BOOSTER_SYSTEMSTATE_CHGD

Systemzustand des CAN-Boosters melden (ca. einmal pro Sekunde, pro Booster und Ausgang). Asynchron, wenn der Broadcast (Flag 0x00020000) aktiviert ist und mind. ein Booster über CAN verbunden ist.

**Z21 an Client:** `DataLen=0x0E 0x00`, `Header=0xCA 0x00`, CANBoosterSystemState (10 Bytes).

| Offset | Typ | Name | Wert |
|---|---|---|---|
| 0 | UINT16 | NId | 0xC101…0xC1FF (CAN-NetworkID) |
| 2 | UINT16 | Booster_OutputPort | 1 = erste Endstufe, 2 = zweite (nur 10807) |
| 4 | UINT16 | Booster_State | bitmask (siehe unten) |
| 6 | UINT16 | Booster_VCCVoltage | mV (Spannung an der Endstufe) |
| 8 | UINT16 | Booster_Current | mA (Strom an der Endstufe) |

```c
#define bsBgActive          0x0001  // Bremsgenerator aktiv (ZCAN SSP)
#define bsShortCircuit       0x0020  // Kurzschluss an Endstufe (ZCAN UES)
#define bsTrackVoltageOff    0x0080  // Gleisspannung ist abgeschaltet (OFF)
#define bsRailComActive      0x0800  // RailCom-Cutout aktiv
#define bsOutputDisabled     0x0100  // Booster Ausgang deaktiviert (by user) — ab Booster FW V1.11
```

#### 10.2.4 LAN_CAN_BOOSTER_SET_TRACKPOWER

Booster Management: Gleisausgänge deaktivieren/reaktivieren. **Anforderung:** `Header=0xCB 0x00`, NId 16 bit + Power 8 bit.

- `0x00` alle Ausgänge deaktivieren / `0xFF` reaktivieren
- *(ab FW V1.42 + Booster FW V1.11)* `0x10`/`0x11` erster Ausgang aus/ein, `0x20`/`0x22` zweiter Ausgang aus/ein (Z21 dual BOOSTER)

Ausgänge können nur eingeschaltet werden, wenn die Zentrale eingeschaltet ist und ein gültiges Gleissignal sendet. Einstellungen nicht persistent. **Antwort:** bei Änderung [10.2.3](#1023-lan_can_booster_systemstate_chgd) an Clients mit Abo.

---

## 11 zLink

Die zLink-Schnittstelle erlaubt es, Endgeräte mit kleinerem Microcontroller ohne eigenes LAN/WLAN ins Netzwerk zu integrieren. Endgeräte (Stand 06/2021): 10806 single BOOSTER, 10807 dual BOOSTER, 10869 XL BOOSTER, 10836 switch DECODER, 10837 signal DECODER.

### 11.1 Adapter

An die zLink-Schnittstelle kann ein Adapter angeschlossen werden — z.B. der **10838 Z21 pro LINK**.

#### 11.1.1 10838 Z21 pro LINK

Verbindet als Gateway die zLink-Schnittstelle mit dem WLAN, für:
1. Konfiguration des Endgeräts (Tasten/Display, Z21 App, Maintenance Tool).
2. Firmware Update (Z21 Updater App, Maintenance Tool).
3. Steuerung durch WLAN-Clients über das Z21 LAN Protokoll.

Im jeweiligen Endgerät ist ein zugeschnittener Z21-Protokoll-Stack implementiert; Kommandos werden wie an eine Zentrale per UDP geschickt (z.B. Boosterausgänge schalten, Systemstatus abfragen, Weichen/Signale direkt schalten, Decoder per CV-Schreibbefehl konfigurieren — sogar ohne Verbindung zum Hauptgleis). Zu beachten:
- Eingeschränkte Bandbreite: effektive Transferrate deutlich unter 1024 Bytes/s halten.
- Zwischen zwei Befehlen mind. 50 ms Pause.
- Z21 pro LINK vorzugsweise im Client Mode verwenden.
- Möglichst nur ein WLAN-Client verbinden, maximal 4 Clients.

UDP-Broadcasts möglich, aber nur zum Auffinden der Geräte empfohlen. Danach Zuordnung über Hardware-Typ (`LAN_GET_HWINFO`), Seriennummer (`LAN_GET_SERIAL_NUMBER`), IP-Adresse und konfigurierbaren Namen. Ein Befehl, den der Z21 pro LINK selbst beantwortet (nicht durchreicht), ist `LAN_ZLINK_GET_HWINFO`.

##### 11.1.1.1 LAN_ZLINK_GET_HWINFO

Abfragen der Eigenschaften des Z21 pro LINK. Als UDP-Broadcast gesendet, lassen sich die im WLAN angemeldeten Z21 pro LINK auffinden.

**Anforderung an Z21 pro LINK:** `DataLen=0x05 0x00`, `Header=0xE8 0x00`, Data[0]=`0x06` (ZLINK_MSG_TYPE_HW_INFO).

**Antwort:** `DataLen=0x3F 0x00`, `Header=0xE8 0x00`, Data[0]=`0x06` + Z_Hw_Info (58 Bytes).

| Offset | Typ | Name | Beispiel |
|---|---|---|---|
| 0 | UINT16 | HwID | 401 (0x191) |
| 2 | UINT8 | FW_Version_Major | 1 |
| 3 | UINT8 | FW_Version_Minor | 1 |
| 4 | UINT16 | FW_Version_Build | 3217 (0xC91) |
| 6 | UINT8[18] | MAC_Address (string) | „EC FA BC 4F 04 C6" |
| 24 | UINT8[33] | Name (string) | „this_is_a_quite_long_device_name" |
| 57 | UINT8 | Reserved | 0x00 |

- **HwID**: 401 (0x191) = Adapter 10838 Z21 pro LINK.
- **MAC_Address**: nullterminierte Zeichenkette, 8-bit ASCII.
- **Name**: vom Anwender konfigurierbar, max. 32 Zeichen + 0x00, ISO 8859-1 (Latin-1). Alle Zeichen nach dem ersten 0x00 ignorieren.

### 11.2 Booster 10806, 10807 und 10869

Unterstützte Befehle siehe Anhang A. Zusätzlich gibt es boosterspezifische Befehle:

#### 11.2.1 LAN_BOOSTER_GET_DESCRIPTION

Bezeichnung auslesen. **Anforderung:** `Header=0xB8 0x00`. **Antwort:** `DataLen=0x24 0x00`, `Header=0xB8 0x00`, `UINT8 Name[32]`. String ISO 8859-1, aus CAN-Kompatibilität ≤ 16 Zeichen. *Sonderfall:* `Name[0]==0xFF` → noch nie eine Bezeichnung abgelegt (als Leerstring interpretieren).

#### 11.2.2 LAN_BOOSTER_SET_DESCRIPTION

Bezeichnung überschreiben. **Anforderung:** `Header=0xB9 0x00`, `UINT8 Name[32]`. Rest mit 0x00 auffüllen; nicht erlaubt `"` (0x22) und `\` (0x5C). **Antwort:** keine.

#### 11.2.3 LAN_BOOSTER_SYSTEMSTATE_GETDATA

Anfordern des Systemzustandes. **Anforderung:** `Header=0xBB 0x00`. **Antwort:** siehe [11.2.4](#1124-lan_booster_systemstate_datachanged).

#### 11.2.4 LAN_BOOSTER_SYSTEMSTATE_DATACHANGED

Asynchron vom Booster, wenn der Broadcast (Flag 0x00000100) aktiviert ist oder der Status explizit angefordert wurde.

**Booster an Client:** `DataLen=0x1C 0x00`, `Header=0xBA 0x00`, BoosterSystemState (24 Bytes).

| Offset | Typ | Name | |
|---|---|---|---|
| 0 | INT16 | Booster_1_MainCurrent | mA |
| 2 | INT16 | Booster_2_MainCurrent | mA |
| 4 | INT16 | Booster_1_FilteredMainCurrent | mA |
| 6 | INT16 | Booster_2_FilteredMainCurrent | mA |
| 8 | INT16 | Booster_1_Temperature | °C |
| 10 | INT16 | Booster_2_Temperature | °C |
| 12 | UINT16 | SupplyVoltage | mV |
| 14 | UINT16 | Booster_1_VCCVoltage | mV |
| 16 | UINT16 | Booster_2_VCCVoltage | mV |
| 18 | UINT8 | CentralState | bitmask |
| 19 | UINT8 | CentralStateEx | bitmask |
| 20 | UINT8 | CentralStateEx2 | bitmask |
| 21 | UINT8 | Reserved1 | |
| 22 | UINT8 | CentralStateEx3 | bitmask |
| 23 | UINT8 | Reserved2 | |

```c
// CentralState
#define csTrackVoltageOff             0x02  // Die Gleisspannung ist abgeschaltet
#define csConfigMode                  0x10  // Konfigurationsmodus aktiv
#define csCanConnected                0x20  // CAN Verbindung mit Zentrale Ok

// CentralStateEx
#define cseHighTemperature            0x01  // zu hohe Temperatur
#define csePowerLost                  0x02  // zu geringe Eingangsspannung
#define cseBooster_1_ShortCircuit     0x04  // Kurzschluss an 1. Endstufe
#define cseBooster_2_ShortCircuit     0x08  // Kurzschluss an 2. Endstufe
#define cseRevPol                     0x10  // Fehler Versorgungsspannung
#define cseNoDCCInput                 0x80  // kein DCC-Eingangssignal vorhanden

// CentralStateEx2
#define cse2Booster_1_RailComActive   0x01  // RailCom aktiv 1. Endstufe
#define cse2Booster_2_RailComActive   0x02  // RailCom aktiv 2. Endstufe
#define cse2Booster_1_MasterSettings  0x04  // CAN Autosettings Ok 1. Endstufe
#define cse2Booster_2_MasterSettings  0x08  // CAN Autosettings Ok 2. Endstufe
#define cse2Booster_1_BgActive        0x10  // Bremsgenerator aktiv 1. Endstufe
#define cse2Booster_2_BgActive        0x20  // Bremsgenerator aktiv 2. Endstufe
#define cse2Booster_1_RailComFwd      0x40  // RailCom Forwarding aktiv 1. Endstufe
#define cse2Booster_2_RailComFwd      0x80  // RailCom Forwarding aktiv 2. Endstufe

// CentralStateEx3
#define cse3Booster_1_OutputInverted  0x01  // 1. Endstufe invertiert (Autoinvert)
#define cse3Booster_2_OutputInverted  0x02  // 2. Endstufe invertiert (Autoinvert)
#define cse3Booster_1_OutputDisabled  0x10  // 1. Endstufe deaktiviert (by user) — ab Booster FW V1.11
#define cse3Booster_2_OutputDisabled  0x20  // 2. Endstufe deaktiviert (by user) — ab Booster FW V1.11
```

#### 11.2.5 LAN_BOOSTER_SET_POWER

*Ab Booster FW V1.11.* Booster Management durch Anwender. Werden alle Ausgänge deaktiviert/reaktiviert, entspricht das `LAN_X_SET_TRACK_POWER_OFF`/`_ON` am Booster. Beim 10807 dual BOOSTER kann auch ein einzelner Ausgang geschaltet werden.

**Anforderung:** `Header=0xB2 0x00`, BoosterPort 8 bit + BoosterPortState 8 bit.
- BoosterPort: `0x01` erster Ausgang, `0x02` zweiter Ausgang (nur dual), `0x03` alle.
- BoosterPortState: `0x00` deaktivieren, `0x01` reaktivieren.

Einstellungen nicht persistent. **Antwort:** bei Änderung [11.2.4](#1124-lan_booster_systemstate_datachanged) an Clients mit Abo.

### 11.3 Decoder 10836 und 10837

Unterstützte Befehle siehe Anhang A; einige decoderspezifische Befehle:

#### 11.3.1 LAN_DECODER_GET_DESCRIPTION

Bezeichnung auslesen. **Anforderung:** `Header=0xD8 0x00`. **Antwort:** `DataLen=0x24 0x00`, `Header=0xD8 0x00`, `UINT8 Name[32]` (Codierung wie 11.2.1).

#### 11.3.2 LAN_DECODER_SET_DESCRIPTION

Bezeichnung überschreiben. **Anforderung:** `Header=0xD9 0x00`, `UINT8 Name[32]` (Codierung wie 11.2.2). **Antwort:** keine.

#### 11.3.3 LAN_DECODER_SYSTEMSTATE_GETDATA

Anfordern des Systemzustandes. **Anforderung:** `Header=0xDB 0x00`. **Antwort:** siehe [11.3.4](#1134-lan_decoder_systemstate_datachanged).

#### 11.3.4 LAN_DECODER_SYSTEMSTATE_DATACHANGED

Asynchron vom Decoder, wenn der Broadcast (Flag 0x00000100) aktiviert ist oder der Status explizit angefordert wurde. (Meldet sich der Signaldecoder nach 4 s nicht, kann gepollt werden.) Die Antworten von 10836 und 10837 unterscheiden sich in Aufbau/Inhalt und sind anhand von **DataLen** zu unterscheiden.

##### 11.3.4.1 SwitchDecoderSystemState (10836)

**An Client:** `DataLen=0x30 0x00`, `Header=0xDA 0x00`, SwitchDecoderSystemState (44 Bytes).

| Offset | Typ | Name | |
|---|---|---|---|
| 0 | INT16 | Current | mA (Strom) |
| 2 | INT16 | FilteredCurrent | mA (geglättet) |
| 4 | UINT16 | Voltage | mV (interne Spannung 3.3V) |
| 6 | UINT8 | CentralState | bitmask |
| 7 | UINT8 | CentralStateEx | bitmask |
| 8 | UINT8[8] | OutputStates[0..7] | Status pro Ausgang |
| 16 | UINT8[8] | OutputConfig[0..7] | Betriebsmodus pro Ausgang |
| 24 | UINT8[4] | OutputDimm[0..7] | Dimmwert pro Ausgang |
| 32 | UINT16 | Address | Erste Decoderadresse |
| 34 | UINT16 | Address2 | Zweite Decoderadresse |
| 36 | UINT8[6] | Reserved1 | |
| 42 | UINT8 | Dimmed | 1 Bit pro Ausgang |
| 43 | UINT8 | Reserved2 | |

```c
// CentralState
#define csEmergencyStop    0x01  // Not-Aus für Decoder
#define csTrackVoltageOff  0x02  // Die Gleisspannung ist abgeschaltet
#define csShortCircuit     0x04  // Kurzschluss erkannt
#define csConfigMode       0x10  // Konfigurationsmodus aktiv
// CentralStateEx
#define csePowerLost       0x02  // zu geringe Eingangsspannung
#define cseRCN213          0x20  // Adressierung gem. RCN213
#define cseNoDCCInput      0x80  // kein DCC-Eingangssignal vorhanden

// OutputState — Zustand des Ausgangs
#define oUnknown        0x00
#define oRedActive      0x11
#define oRedInactive    0x01
#define oGreenActive    0x12
#define oGreenInactive  0x02

// OutputConfig — Betriebsmodus
#define ocfgNormal   0  // Impulsbetrieb (default)
#define ocfgBlinker  1  // Wechselblinker
#define ocfgBlinkSm  2  // Wechselblinker mit Ein-/Ausblenden
#define ocfg10775    3  // Momentbetrieb wie 10775
#define ocfgK84      4  // Dauerbetrieb (z.B. Beleuchtung)
#define ocfgK84Sm    5  // Dauerbetrieb mit Ein-/Ausblenden
```

- **FilteredCurrent**: Summe interner Stromverbrauch + Verbrauch an den Klemmen.
- **OutputDimm**: 0 = Dimmung deaktiviert (volle Leistung); 1–100 = min. bis max. Leistung.
- **Address**: einer Decoderadresse entsprechen 4 Weichennummern (Addr 1 → #1–4, Addr 2 → #5–8, …).
- **Address2**: =0 → automatisch „Erste Decoderadresse + 1"; sonst analog Address.
- **Dimmed**: 1 Bit pro Ausgangspaar (0 = nicht gedimmt, 1 = gedimmt/Auf-/Abblenden). LSB = Paar 1, MSB = Paar 8.

##### 11.3.4.2 SignalDecoderSystemState (10837)

**An Client:** `DataLen=0x2E 0x00`, `Header=0xDA 0x00`, SignalDecoderSystemState (42 Bytes).

| Offset | Typ | Name | |
|---|---|---|---|
| 0 | INT16 | Current | mA (0 / reserviert) |
| 2 | INT16 | FilteredCurrent | mA (0 / reserviert) |
| 4 | UINT16 | Voltage | mV (Spannung an den Klemmen) |
| 6 | UINT8 | CentralState | bitmask |
| 7 | UINT8 | CentralStateEx | bitmask |
| 8 | UINT8[2] | OutputStates[0..1] | Ein/Aus-Status Ausgänge A1…B8 |
| 10 | UINT8[2] | BlinkStates[0..1] | Blink-Status A1…B8 |
| 12 | UINT8[4] | SignalDccExt[0..3] | DCCext aktueller Signalbegriff 1.–4. Signal |
| 16 | UINT8[4] | SignalCurrAsp[0..3] | Index aktueller Signalbegriff |
| 20 | UINT8[3] | Reserved1 | |
| 23 | UINT8 | SignalCount | Anzahl verwendeter Signale (2/3/4) |
| 24 | UINT8[4] | SignalConfig[0..3] | Signal-ID Konfiguration 1.–4. Signal |
| 28 | UINT8[4] | SignalInitAsp[0..3] | Index Initialisierung |
| 32 | UINT16 | Address | Erste Decoderadresse |
| 34 | UINT16[4] | Reserved2 | |

```c
// CentralState
#define csEmergencyStop    0x01  // Not-Aus für Decoder
#define csTrackVoltageOff  0x02  // Die Gleisspannung ist abgeschaltet
#define csShortCircuit     0x04  // Kurzschluss erkannt
#define csConfigMode       0x10  // Konfigurationsmodus aktiv
// CentralStateEx
#define csePowerLost       0x02  // zu geringe Eingangsspannung
#define cseEEPromError     0x10  // EEPROM Schreib/Lesefehler
#define cseRCN213          0x20  // Adressierung gem. RCN213
#define cseNoDCCInput      0x80  // kein DCC-Eingangssignal vorhanden
```

- **OutputStates/BlinkStates**: [0] LSB=A1 … MSB=A8; [1] LSB=B1 … MSB=B8.
- **SignalConfig** = Signal-ID (Signaltyp); **SignalDccExt** = DCCext-Wert (aktueller Signalbegriff). Werte siehe `https://www.z21.eu/de/produkte/z21-signal-decoder/signaltypen`.
- **Address**: einer Decoderadresse entsprechen 4 Signaladressen; der Decoder belegt 4 Decoderadressen = 16 Signaladressen (Addr 1 → Signaladressen 1–16, usw.).

---

## 12 Modellzeit

*Ab Z21 FW Version 1.43.* Die beschleunigte Modellzeit der Z21 steht nun auch Teilnehmern am Gleis, X-BUS und LAN zur Verfügung (Beschleunigungsfaktor ≤ 63). Die Z21 hat keine Echtzeituhr — die Modellzeit beginnt immer bei der einstellbaren Startzeit.

- DCC-Zeitmeldungen am Gleis: siehe RCN-211.
- LocoNet: Clock Slot `0x7B` ca. alle 70–100 s pollen.
- X-BUS: Zeitmeldung gem. XpressNet™ V4.0 einmal pro Modellminute.
- LAN: optional per „MRclock" Multicast an `239.50.50.20`, Port `2000` (einmal pro Modellminute, mind. dreimal pro echter Minute).

### 12.1 LAN_FAST_CLOCK_CONTROL

#### 12.1.1 Modellzeit lesen

**Anforderung:** `DataLen=0x07 0x00`, `Header=0xCC 0x00`, Data `0x21 0x2A 0x0B`. **Antwort:** siehe [12.2](#122-lan_fast_clock_data).

#### 12.1.2 Modellzeit setzen

Setzt Rate und aktuelle Modellzeit.

| DataLen | Header | Data |
|---|---|---|
| 0x0A 0x00 | 0xCC 0x00 | `0x24 0x2B DDDhhhhh 00mmmmmm 00rrrrrr` XOR-Byte |

- `DDD`: Wochentag (3 Bits), 0=Montag … 6=Sonntag.
- `hhhhh`: Stunde (5 Bits), 0–23.
- `mmmmmm`: Minute (6 Bits), 0–59.
- `rrrrrr`: Rate (6 Bits), 0–63. 0 = Modellzeit bleibt stehen (nicht empfohlen, besser [12.1.4](#1214-modellzeit-anhalten)); 1 = Echtzeit; 2 = doppelt so schnell; usw. *Die Rate wird persistent gespeichert.*
- `XOR-Byte`: XOR-Prüfsumme über Data.

**Antwort:** [12.2](#122-lan_fast_clock_data) an Clients mit Abo.

#### 12.1.3 Modellzeit starten

Startet (setzt fort) die Modellzeituhr. **Anforderung:** `Header=0xCC 0x00`, Data `0x21 0x2C 0x0D`. **Antwort:** [12.2](#122-lan_fast_clock_data) an Clients mit Abo. *Der Zustand „fcFastClockEnabled" wird persistent gespeichert.*

#### 12.1.4 Modellzeit anhalten

Hält die Modellzeituhr an. **Anforderung:** `Header=0xCC 0x00`, Data `0x21 0x2D 0x0C`. **Antwort:** [12.2](#122-lan_fast_clock_data) an Clients mit Abo. *Der Zustand „not fcFastClockEnabled" wird persistent gespeichert.*

### 12.2 LAN_FAST_CLOCK_DATA

Aktuelle Modellzeit melden. Asynchron, wenn der Broadcast (Flag 0x00000010) aktiviert ist oder die Modellzeit explizit gelesen wurde. Bei laufender Uhr ca. einmal pro Modellminute, auch bei Start/Stop/Setzen. Übersprungene Zeitmeldungen müssen Clients tolerieren (ggf. anhand des Beschleunigungsfaktors selbst weiterrechnen).

**Z21 an Client:** `DataLen=0x0C 0x00`, `Header=0xCD 0x00`, FastClockTime (8 Bytes).

| Offset | Typ | Name | Wert |
|---|---|---|---|
| 0 | UINT8 | — | 0x66 |
| 1 | UINT8 | — | 0x25 |
| 2 | UINT8 | DDDh hhhh | Wochentag und Stunde |
| 3 | UINT8 | 00mm mmmm | Minute |
| 4 | UINT8 | SHss ssss | Sekunde, mit STOP-/HALT-Flag |
| 5 | UINT8 | 00rr rrrr | Rate |
| 6 | UINT8 | FcSettings | Einstellungen-Flags |
| 7 | UINT8 | XOR-Byte | XOR-Prüfsumme über Data |

- `DDD` Wochentag (0=Montag…6=Sonntag); `hhhhh` Stunde 0–23; `mmmmmm` Minute 0–59; `ssssss` Sekunde 0–59; `rrrrrr` Rate 0–63.
- `S` STOP-Flag: Modellzeit läuft nicht (Fastclock nicht enabled oder Rate=0).
- `H` HALT-Flag: vorübergehend angehalten (Nothalt oder Kurzschluss am Gleis).
- `FcSettings`: persistente Einstellungen, siehe [12.3](#123-lan_fast_clock_settings_get).

### 12.3 LAN_FAST_CLOCK_SETTINGS_GET

Auslesen der persistenten Modellzeit-Einstellungen. **Anforderung:** `DataLen=0x05 0x00`, `Header=0xCE 0x00`, Data `0x04`.

**Antwort:** `DataLen=0x08 0x00`, `Header=0xCE 0x00`, `FcSettings | Rate | StartDDDhhhhh | StartMMMMMM` (je 8 bit).
- **Rate**: 0–63 (0 = kann nicht laufen; 1 = Echtzeit; 2 = doppelt; usw.).
- **StartDDDhhhhh**: Default-Startzeit Wochentag (3 Bits, 0=Montag…6=Sonntag) und Stunde (5 Bits, 0–23).
- **StartMMMMMM**: Default-Startzeit Minute (6 Bits, 0–59).

```c
#define fcFastClockLocoNetEn       0x01  // Ausgabe am LocoNet (polled) aktivieren
#define fcFastClockXBUSEn          0x02  // Broadcast am XBUS aktivieren
//                                 0x04  // reserved
#define fcFastClockDCCEn           0x08  // DCC Broadcast am Gleis aktivieren
#define fcFastClockMRclockEn       0x10  // Multicast an MRclock clients aktivieren
//                                 0x20  // reserved
#define fcFastClockEmergenyHaltEn  0x40  // Modellzeit beim Nothalt autom. anhalten
#define fcFastClockEnabled         0x80  // Fastclock ist aktiviert
```

`fcFastClockEmergenyHaltEn` pausiert die Modellzeit bei Nothalt/Kurzschluss. `fcFastClockEnabled` ist das Enable-Flag (wird auch indirekt über `LAN_FAST_CLOCK_CONTROL` durch Start/Stop geändert). **Werkseinstellung:** FcSettings=0x4F, Rate=1, StartDDDhhhhh=0, StartMMMMMM=0.

### 12.4 LAN_FAST_CLOCK_SETTINGS_SET

Überschreiben der persistenten Einstellungen (Parameter je 8 bit), `Header=0xCF 0x00`:

| DataLen | Data | Wirkung |
|---|---|---|
| 0x05 0x00 | FcSettings | nur FcSettings |
| 0x06 0x00 | FcSettings, Rate | FcSettings + Rate |
| 0x08 0x00 | FcSettings, Rate, StartDDDhhhhh, StartMMMMMM | FcSettings + Rate + Default-Startzeit |

Feldbeschreibung siehe [12.3](#123-lan_fast_clock_settings_get). **Antwort:** keine.

---

## Anhang A – Befehlsübersicht

### Client an Z21

Diese Meldungen können von einem Client an eine Z21 oder an ein zLink-Gerät gesendet werden. Spalten: **Z21/Z21 XL**, **z21/z21start**, **Booster (10806/10807/10869)**, **Decoder (10836/10837)**.

| Header / X-Hdr / DB0 | Name | Z21/XL | z21/start | Booster | Decoder |
|---|---|---|---|---|---|
| 0x10 | LAN_GET_SERIAL_NUMBER | ✓ | ✓ | ✓ | ✓ |
| 0x18 | LAN_GET_CODE | ✓ | ✓ | | |
| 0x1A | LAN_GET_HWINFO | ✓ | ✓ | ✓ | ✓ |
| 0x30 | LAN_LOGOFF | ✓ | ✓ | ✓ | ✓ |
| 0x40 / 0x21 / 0x21 | LAN_X_GET_VERSION | ✓ | ✓ | ✓ | ✓ |
| 0x40 / 0x21 / 0x24 | LAN_X_GET_STATUS | ✓ | ✓ | ✓ | ✓ |
| 0x40 / 0x21 / 0x80 | LAN_X_SET_TRACK_POWER_OFF | ✓ | ✓ | ✓ | ✓ |
| 0x40 / 0x21 / 0x81 | LAN_X_SET_TRACK_POWER_ON | ✓ | ✓ | ✓ | ✓ (4) |
| 0x40 / 0x22 / 0x11 | LAN_X_DCC_READ_REGISTER | ✓ | ✓ | | |
| 0x40 / 0x23 / 0x11 | LAN_X_CV_READ | ✓ | ✓ | | ✓ |
| 0x40 / 0x23 / 0x12 | LAN_X_DCC_WRITE_REGISTER | ✓ | ✓ | | |
| 0x40 / 0x24 / 0x12 | LAN_X_CV_WRITE | ✓ | ✓ | | ✓ |
| 0x40 / 0x24 / 0xFF | LAN_X_MM_WRITE_BYTE | ✓ | ✓ | | |
| 0x40 / 0x43 | LAN_X_GET_TURNOUT_INFO | ✓ | ✓ | | ✓ |
| 0x40 / 0x44 | LAN_X_GET_EXT_ACCESSORY_INFO | ✓ | ✓ | | ✓ (3) |
| 0x40 / 0x53 | LAN_X_SET_TURNOUT | ✓ | ✓ (1) | | ✓ |
| 0x40 / 0x54 | LAN_X_SET_EXT_ACCESSORY | ✓ | ✓ (1) | | ✓ |
| 0x40 / 0x80 | LAN_X_SET_STOP | ✓ | ✓ | | ✓ (5) |
| 0x40 / 0x92 | LAN_X_SET_LOCO_E_STOP | ✓ | ✓ | | |
| 0x40 / 0xE3 / 0x44 | LAN_X_PURGE_LOCO | ✓ | ✓ | | |
| 0x40 / 0xE3 / 0xF0 | LAN_X_GET_LOCO_INFO | ✓ | ✓ | | |
| 0x40 / 0xE4 / 0x1s | LAN_X_SET_LOCO_DRIVE | ✓ | ✓ (1) | | |
| 0x40 / 0xE4 / 0xF8 | LAN_X_SET_LOCO_FUNCTION | ✓ | ✓ (1) | | |
| 0x40 / 0xE4 / Group | LAN_X_SET_LOCO_FUNCTION_GROUP | ✓ | ✓ (1) | | |
| 0x40 / 0xE5 / 0x5F | LAN_X_SET_LOCO_BINARY_STATE | ✓ | ✓ | | |
| 0x40 / 0xE6 / 0x30 (0xEC) | LAN_X_CV_POM_WRITE_BYTE | ✓ | ✓ | | ✓ |
| 0x40 / 0xE6 / 0x30 (0xE8) | LAN_X_CV_POM_WRITE_BIT | ✓ | ✓ | | |
| 0x40 / 0xE6 / 0x30 (0xE4) | LAN_X_CV_POM_READ_BYTE | ✓ | ✓ | | ✓ |
| 0x40 / 0xE6 / 0x31 (0xEC) | LAN_X_CV_POM_ACCESSORY_WRITE_BYTE | ✓ | ✓ | | ✓ |
| 0x40 / 0xE6 / 0x31 (0xE8) | LAN_X_CV_POM_ACCESSORY_WRITE_BIT | ✓ | ✓ | | |
| 0x40 / 0xE6 / 0x31 (0xE4) | LAN_X_CV_POM_ACCESSORY_READ_BYTE | ✓ | ✓ | | ✓ |
| 0x40 / 0xF1 / 0x0A | LAN_X_GET_FIRMWARE_VERSION | ✓ | ✓ | ✓ | ✓ |
| 0x50 | LAN_SET_BROADCASTFLAGS | ✓ | ✓ | ✓ | ✓ |
| 0x51 | LAN_GET_BROADCASTFLAGS | ✓ | ✓ | ✓ | ✓ |
| 0x60 | LAN_GET_LOCOMODE | ✓ | ✓ | | |
| 0x61 | LAN_SET_LOCOMODE | ✓ | ✓ | | |
| 0x70 | LAN_GET_TURNOUTMODE | ✓ | ✓ | | |
| 0x71 | LAN_SET_TURNOUTMODE | ✓ | ✓ | | |
| 0x81 | LAN_RMBUS_GETDATA | ✓ | ✓ | | |
| 0x82 | LAN_RMBUS_PROGRAMMODULE | ✓ | ✓ | | |
| 0x85 | LAN_SYSTEMSTATE_GETDATA | ✓ | ✓ | | |
| 0x89 | LAN_RAILCOM_GETDATA | ✓ | ✓ | ✓ | |
| 0xA2 | LAN_LOCONET_FROM_LAN | ✓ | ✓ (1)(2) | | |
| 0xA3 | LAN_LOCONET_DISPATCH_ADDR | ✓ | | | |
| 0xA4 | LAN_LOCONET_DETECTOR | ✓ | ✓ (2) | | |
| 0xC4 | LAN_CAN_DETECTOR | ✓ | | | |
| 0xC8 | LAN_CAN_DEVICE_GET_DESCRIPTION | ✓ | | | |
| 0xC9 | LAN_CAN_DEVICE_SET_DESCRIPTION | ✓ | | | |
| 0xCB | LAN_CAN_BOOSTER_SET_TRACKPOWER | ✓ | | | |
| 0xCC | LAN_FAST_CLOCK_CONTROL | ✓ | ✓ | | |
| 0xCE | LAN_FAST_CLOCK_SETTINGS_GET | ✓ | ✓ | | |
| 0xCF | LAN_FAST_CLOCK_SETTINGS_SET | ✓ | ✓ | | |
| 0xB2 | LAN_BOOSTER_SET_POWER | | | ✓ | |
| 0xB8 | LAN_BOOSTER_GET_DESCRIPTION | | | ✓ | |
| 0xB9 | LAN_BOOSTER_SET_DESCRIPTION | | | ✓ | |
| 0xBB | LAN_BOOSTER_SYSTEMSTATE_GETDATA | | | ✓ | |
| 0xD8 | LAN_DECODER_GET_DESCRIPTION | | | | ✓ |
| 0xD9 | LAN_DECODER_SET_DESCRIPTION | | | | ✓ |
| 0xDB | LAN_DECODER_SYSTEMSTATE_GETDATA | | | | ✓ |
| 0xE8 / 0x06 | LAN_ZLINK_GET_HWINFO | | | ✓ (6) | ✓ (6) |

**Fußnoten:**
1. z21start: nur mit Freischaltcode (Artikelnummer 10814 oder 10818).
2. z21, z21start: virtueller LocoNet-Stack (z.B. bei GBM16XN mit XPN-Interface).
3. ab Decoder FW V1.11.
4. Decoder: Signallampen wieder einschalten (nur 10837).
5. Decoder: zeigt Haltebegriff, wenn in CV38 das zweite Bit (0x02) gesetzt ist (nur 10837).
6. Wird vom 10838 Z21 pro LINK beantwortet, nicht vom Endgerät (Booster oder Decoder).

### Z21 an Client

Diese Meldungen können von einer Z21 oder einem zLink-Gerät an einen Client gesendet werden.

| Header / X-Hdr / DB0 | Name | Z21/XL | z21/start | Booster | Decoder |
|---|---|---|---|---|---|
| 0x10 | Antwort auf LAN_GET_SERIAL_NUMBER | ✓ | ✓ | ✓ | ✓ |
| 0x18 | Antwort auf LAN_GET_CODE | ✓ | ✓ | | |
| 0x1A | Antwort auf LAN_GET_HWINFO | ✓ | ✓ | ✓ | ✓ |
| 0x40 / 0x43 | LAN_X_TURNOUT_INFO | ✓ | ✓ (1) | | ✓ |
| 0x40 / 0x44 | LAN_X_EXT_ACCESSORY_INFO | ✓ | ✓ (1) | | ✓ (3) |
| 0x40 / 0x61 / 0x00 | LAN_X_BC_TRACK_POWER_OFF | ✓ | ✓ | ✓ | |
| 0x40 / 0x61 / 0x01 | LAN_X_BC_TRACK_POWER_ON | ✓ | ✓ | ✓ | |
| 0x40 / 0x61 / 0x02 | LAN_X_BC_PROGRAMMING_MODE | ✓ | ✓ | | |
| 0x40 / 0x61 / 0x08 | LAN_X_BC_TRACK_SHORT_CIRCUIT | ✓ | ✓ | (4) | (4) |
| 0x40 / 0x61 / 0x12 | LAN_X_CV_NACK_SC | ✓ | ✓ | | |
| 0x40 / 0x61 / 0x13 | LAN_X_CV_NACK | ✓ | ✓ | | ✓ |
| 0x40 / 0x61 / 0x82 | LAN_X_UNKNOWN_COMMAND | ✓ | ✓ | ✓ | ✓ |
| 0x40 / 0x62 / 0x22 | LAN_X_STATUS_CHANGED | ✓ | ✓ | ✓ | ✓ |
| 0x40 / 0x63 / 0x21 | Antwort auf LAN_X_GET_VERSION | ✓ | ✓ | ✓ | ✓ |
| 0x40 / 0x64 / 0x14 | LAN_X_CV_RESULT | ✓ | ✓ | | ✓ |
| 0x40 / 0x81 | LAN_X_BC_STOPPED | ✓ | ✓ | | |
| 0x40 / 0xEF | LAN_X_LOCO_INFO | ✓ | ✓ (1) | | |
| 0x40 / 0xF3 / 0x0A | Antwort auf LAN_X_GET_FIRMWARE_VERSION | ✓ | ✓ | ✓ | ✓ |
| 0x51 | Antwort auf LAN_GET_BROADCASTFLAGS | ✓ | ✓ | ✓ | ✓ |
| 0x60 | Antwort auf LAN_GET_LOCOMODE | ✓ | ✓ | | |
| 0x70 | Antwort auf LAN_GET_TURNOUTMODE | ✓ | ✓ | | |
| 0x80 | LAN_RMBUS_DATACHANGED | ✓ | ✓ | | |
| 0x84 | LAN_SYSTEMSTATE_DATACHANGED | ✓ | ✓ | | |
| 0x88 | LAN_RAILCOM_DATACHANGED | ✓ | ✓ | ✓ | |
| 0xA0 | LAN_LOCONET_Z21_RX | ✓ | | | |
| 0xA1 | LAN_LOCONET_Z21_TX | ✓ | ✓ (2) | | |
| 0xA2 | LAN_LOCONET_FROM_LAN | ✓ | ✓ (2) | | |
| 0xA3 | LAN_LOCONET_DISPATCH_ADDR | ✓ | | | |
| 0xA4 | LAN_LOCONET_DETECTOR | ✓ | ✓ (2) | | |
| 0xC4 | LAN_CAN_DETECTOR | ✓ | | | |
| 0xC8 | Antwort LAN_CAN_DEVICE_GET_DESCRIPTION | ✓ | | | |
| 0xCA | LAN_CAN_BOOSTER_SYSTEMSTATE_CHGD | ✓ | | | |
| 0xCD | LAN_FAST_CLOCK_DATA | ✓ | ✓ | | |
| 0xCE | LAN_FAST_CLOCK_SETTINGS_GET | ✓ | ✓ | | |
| 0xB8 | Antwort auf LAN_BOOSTER_GET_DESCRIPTION | | | ✓ | |
| 0xBA | LAN_BOOSTER_SYSTEMSTATE_DATACHANGED | | | ✓ | |
| 0xD8 | Antwort auf LAN_DECODER_GET_DESCRIPTION | | | | ✓ |
| 0xDA | LAN_DECODER_SYSTEMSTATE_DATACHANGED | | | | ✓ |
| 0xE8 / 0x06 | Antwort auf LAN_ZLINK_GET_HWINFO | | | ✓ (5) | ✓ (5) |

**Fußnoten:**
1. z21start: vollfunktionsfähig nur mit Freischaltcode (Artikelnummer 10814 oder 10818).
2. z21, z21start: virtueller LocoNet-Stack (z.B. bei GBM16XN mit XPN-Interface).
3. ab Decoder FW V1.11.
4. Kurzschluss wird im entsprechenden Booster-/Decoder-SystemState gemeldet.
5. Wird vom 10838 Z21 pro LINK beantwortet, nicht vom Endgerät (Booster oder Decoder).

---

## Abbildungs- und Tabellenverzeichnis

**Abbildungen:** 1 Sequenz Kommunikation · 2 Sequenz Lok-Steuerung · 3 DCC Sniff bei Q=0 · 4 DCC Sniff bei Q=1 · 5 Sequenz Weiche schalten · 6 Sequenz CV Lesen · 7 Sequenz Rückmeldemodul programmieren · 8 Sequenz Ethernet/LocoNet Gateway · 9 Sequenz LocoNet Dispatch per LAN-Client.

**Tabellen:** 1 Meldungen vom Client an Z21 · 2 Meldungen von Z21 an Clients.

---

*Konvertiert aus „Z21 LAN Protokoll Spezifikation", Dokumentenversion 1.13 (06.11.2023), Herausgeber Modelleisenbahn GmbH. Diagramm-Abbildungen des Originals sind hier nicht enthalten und im Text als „(Abbildung …)" referenziert.*
