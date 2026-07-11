# Early design notes

Original working notes from the start of the project, kept for historical context.

Each process holds:
- a set of resources drawn from a shared database, plus
- an ontology (in RDF/OWL) describing how those resources relate to each other.

Two core tasks:
1. **Consensus on resource updates** — reach agreement across processes before modifying a shared resource.
2. **Dataset validation** — validate a (possibly incomplete) dataset against the ontology.

The initial concept explored Erlang for the distributed process layer, since the problem is naturally message-passing between independent nodes: each process validates datasets, participates in voting when eligible, executes test cases and reports results, and updates its resources to a new version on success. The implementation that followed uses C#/WCF instead (see [`ResMngNetwork/Server`](../ResMngNetwork/Server)), but the underlying process model is the same:

1. Process specification (name/identity)
2. Read initial data
3. State-machine-driven resource updates
4. Inter-process communication rules (who can talk to whom)
5. A master process that tracks communication/network state
