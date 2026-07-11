# Tools

This folder is a mix of one real component and several throwaway technology trials — kept for
completeness, not as a recommendation to build on them.

## Part of the actual system

- **`SimulationTool/`** — OpenDSS circuit parsing (`OpenDSSParser`) and the simulation engine/UI
  (`SimulationEngine`, `OperationalBranchingTool`) used to process the circuit data in
  [`OpenDSSCircuit/`](../OpenDSSCircuit). This is a real dependency of the wider system, not a trial.

## Prototyping / experiments (not used by the core system)

These were spikes to evaluate alternative communication technologies before settling on WCF for
`ResMngNetwork/Server`. They're standalone "Hello World"-level trials, not maintained, and not
wired into the rest of the codebase:

- **`GRPCSol/Trail1/`** — gRPC client/server trial
- **`WCFHosting/`**, **`WCFHosting2/`** — early WCF self-hosting trials
- **`WebAPITools/`** — ASP.NET Web API trial
- **`neo4jexps/`** — Neo4j graph database trial, explored as an alternative to the RDF/OWL
  reasoning approach actually used in `ResMngNetwork/Server/KnowledgeGraph`
