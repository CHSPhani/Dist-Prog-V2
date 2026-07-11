# OWL ontology spec — smart local energy system

Class hierarchy, data properties, and object properties for the dynamic ontology used to validate
OpenDSS circuit data (see [`Ontology/opendsst22.owl`](../Ontology/opendsst22.owl) and
[`Ontology/opendsst22-With-Individual.owl`](../Ontology/opendsst22-With-Individual.owl)).

## Classes

Bus, Circuit, Transformer, Winding, Regulator, Capacitor, Load, LoadShape, PVSystem,
CircuitBreaker, Fuse, Switch, Line

## Data properties

```
PU              Domain Circuit                          Range xsd:double  // 1.05
BaseKv           Domain Circuit                          Range xsd:int     // 230
R0 / R1          Domain Circuit                          Range xsd:double  // 4.07 / 0.63
X0 / X1          Domain Circuit                          Range xsd:double  // 15.55 / 6.72
BusSlNo          Domain Bus                               Range xsd:int     // 1

NoOfWindings     Domain Transformer                       Range xsd:int     // 2
Winding1/2       Domain Transformer   Range = Winding
PerNoLoadLoss    Domain Transformer                       Range xsd:double  // 0.18
PerRS1 / PerRS2  Domain Transformer                       Range xsd:double  // 0.12
sub              Domain Transformer                       Range xsd:string  // "yes"
MinTap / MaxTap  Domain Transformer                       Range xsd:double  // 0.9 / 1.1
EmergHkva        Domain Transformer                       Range xsd:long    // 75000
Xhl              Domain Transformer                       Range xsd:double  // 10.63
No               Domain Winding                           Range xsd:int     // 1
PercentageR      Domain Winding                           Range xsd:int

Id / Length      Domain Line                              Range xsd:string / xsd:double

NPTS / MInterval / Mult / Action   Domain LoadShape        Range xsd:long / xsd:int / xsd:string / xsd:string

TransformerName / VReg / Band / PTRation / CTPrim / R / X / DebugTrace / Delay
                 Domain Regulator

Phases           Domain Transformer, Load, PVSystem, Capacitor   Range xsd:int    // 3
Enabled          Domain Regulator, Capacitor                     Range xsd:boolean
WindingConnType  Domain Winding, Load, Capacitor                 Range xsd:string // Wye / Delta
kv               Domain Winding, Load, PVSystem, Capacitor       Range xsd:double
kvar             Domain Load, Capacitor                          Range xsd:double
Bus              Domain Circuit, Winding, Load, PVSystem, Capacitor  Range xsd:string // "SourceBus"

Irradiance       Domain PVSystem                          Range xsd:int
kvA              Domain Winding, Load, PVSystem           Range xsd:double
PF               Domain Load, PVSystem                    Range xsd:double
Pmpp             Domain PVSystem                          Range xsd:double
kw / kwh         Domain Load                              Range xsd:double
xfkVA            Domain Load                              Range xsd:double
Model            Domain Load                              Range xsd:int
CVRWatts/CVRvars Domain Load                              Range xsd:double
Vminpu / Vmaxpu  Domain Load                              Range xsd:double
status           Domain Load                              Range xsd:string

Name             Domain Circuit, Transformer               Range xsd:string  // "ckt24"
Type             Domain Circuit, Transformer                Range xsd:string  // "Substation,Transformer"
Longitude/Latitude  Domain Bus, Transformer                Range xsd:double

MonitoredObj / MonitoredTerm / Ratedcurrent   Domain Fuse   Range xsd:string / xsd:string / xsd:double
```

## Object properties

```
source_to_primary_bus       Domain Circuit   Range Bus
primary_bus_to_source        Domain Bus       Range Circuit
bus_to_winding                Domain Bus       Range Winding
winding_to_bus                Domain Winding   Range Bus
winding_to_regulator          Domain Winding   Range Bus   (no inverse)
bus_to_load / load_to_bus     Domain Bus       Range Bus
load_contains_loadshape       Domain Load      Range LoadShape  (no inverse)
pv_contains_loadshape         Domain PVSystem  Range LoadShape  (no inverse)
```
Characteristics: Functional, Inverse-Functional, Anti-Symmetric, Irreflexive.

```
transformer_contains_winding
```
Characteristics: non-functional (multiple values), inverse-functional (multiple values), anti-symmetric, irreflexive.

```
bus_to_bus   Domain Bus   Range Bus
```
Characteristics: Functional, Inverse-Functional, **Symmetric**, Irreflexive.

Planned but not yet modeled: Recloser (SwitchedObj, SwitchedTerm, NumFast, Shots, Phase, TDPhFast,
TDPhDelayed, Reclose intervals).

## Related: OpenDSS monitor data fields

Per-timestep monitor output fields referenced when validating circuit measurement data against the ontology:

```
hour        int      [0, 24]
t (sec)     double   [0, 3540]
V1..V4, VAngle1..VAngle4     decimal
I1..I4, IAngle1..IAngle4     decimal
S1..S4 (kVA), Ang1..Ang4     decimal
Tap (pu)
```
