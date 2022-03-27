function snapAnalysis()
%% initiate COM interface (only need to do once when you open MATLAB)
[DSSCircObj, DSSText, gridpvPath] = DSSStartup;
 % Define the circuit
DSSCircuit = DSSCircObj.ActiveCircuit;
DSSText.command = 'Compile C:\OpenDSSTrails\Ckt24_V2\master_ckt24.dss';
%% Run the simulation in static mode for the peak time 
DSSText.command = 'Set mode=duty number=10080 hour=0 h=60 sec=0';
DSSText.Command = 'Set Controlmode=TIME';
DSSText.command = 'solve';
%%DSSText.command = sprintf('Set mode=duty number=1 hour=%i h=1.0 sec=%i',floor((15061)/3600),round(mod(15061,3600))); 
%%DSSText.Command = 'Set Controlmode=Static'; 
%take control actions immediately without delays 
%%DSSText.command = 'solve';
%% Line Monitor
DSSText.Command = 'export mon fdr_05410_Mon_VI_t1'; 
monitorFile = DSSText.Result; 
MyCSV = importdata(monitorFile); 
%% Second Monitor
DSSText.Command = 'export mon fdr_05410_Mon_PQ_t1'; 
monitorFile = DSSText.Result; 
MyCSV = importdata(monitorFile); 
%% Line Monitor
DSSText.Command = 'export mon fdr_05410_Mon_VI_t2'; 
monitorFile = DSSText.Result; 
MyCSV = importdata(monitorFile); 
%% Second Monitor
DSSText.Command = 'export mon fdr_05410_Mon_PQ_t2'; 
monitorFile = DSSText.Result; 
MyCSV = importdata(monitorFile); 
%% Fourth Monitor
DSSText.Command = 'export mon transformer_05410_G2100FE2600_Mon_VI11'; 
monitorFile = DSSText.Result; 
MyCSV = importdata(monitorFile); 
%% Fourth Monitor
DSSText.Command = 'export mon transformer_05410_G2100FE2600_Mon_VI12'; 
monitorFile = DSSText.Result; 
MyCSV = importdata(monitorFile); 
%% Fifth Monitor
DSSText.Command = 'export mon transformer_05410_G2100FE2600_Mon_PQ1'; 
monitorFile = DSSText.Result; 
MyCSV = importdata(monitorFile); 
%% Fourth Monitor
DSSText.Command = 'export mon transformer_05410_G2100FE2600_Mon_VI21'; 
monitorFile = DSSText.Result; 
MyCSV = importdata(monitorFile); 
%% Fourth Monitor
DSSText.Command = 'export mon transformer_05410_G2100FE2600_Mon_VI22'; 
monitorFile = DSSText.Result; 
MyCSV = importdata(monitorFile); 
%% Fifth Monitor
DSSText.Command = 'export mon transformer_05410_G2100FE2600_Mon_PQ2'; 
monitorFile = DSSText.Result; 
MyCSV = importdata(monitorFile); 
%% Sixth Monitor
DSSText.Command = 'export mon transformer_05410_G2100FE2600_Mon_Tap1'; 
monitorFile = DSSText.Result; 
MyCSV = importdata(monitorFile);
%% Seventh Monitor
DSSText.Command = 'export mon transformer_05410_G2100FE2600_Mon_Tap2'; 
monitorFile = DSSText.Result; 
MyCSV = importdata(monitorFile);
