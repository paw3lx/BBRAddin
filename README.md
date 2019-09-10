BBRAddin is a SQL Server Management Studio extension for BBR

## Installation

1. Download the release 
2. Install the extension by running the BBRAddin.vsix file
3. Run init_bbr_addin_registry.reg 

## Getting started

Every time you run the SSMS you have to manually run the extension

``
Tools -> Init BBRAddin
``

### Sql script generator

1. Right click on the table
2. BBR Scripts -> select one of the option to generate sql query

### Copy/Paste helper

1. Copy some data (datetimes, uuids, longs ets)
2. Right click on the editor a select "Paste as CSV"