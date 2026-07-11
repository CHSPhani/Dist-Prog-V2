# Vendored binaries

These DLLs are committed as binaries (not restored via NuGet) because their source is not otherwise obtainable:

- **`Microsoft.GLEE.dll`, `Microsoft.GLEE.Drawing.dll`, `Microsoft.GLEE.GraphViewerGDI.dll`** — Microsoft Research's Graph Layout Execution Engine, used for graph visualization (`RDFGraphWindow`). GLEE was discontinued by Microsoft Research years ago (superseded by [MSAGL](https://github.com/microsoft/automatic-graph-layout)) and is no longer available via NuGet or the original installer. These copies are the only way to build the projects that reference them.
- **`DataSerailizer.dll`** — referenced by `Server.csproj`, `OperationalBranchingTool.csproj`, and `ToolUtilities.csproj`. No source project for it exists in this repository; the original source is lost.

Everything else that used to sit in a `DebugOuts/` folder (project build outputs, NuGet-restorable third-party packages) has been removed — those regenerate automatically from source or `packages.config` on build.
