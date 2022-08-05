
using BankApp.Core;
using BankApp.Utilities;

GlobalConfig globalConfig = new();
DataManager dataManager = new(globalConfig);

do
{
    DataManager.Notify("Hello; You are welocme to MyBank", true);
    dataManager.Processor(DataManager.GeneralMenu());
}while(true);


      
                




       