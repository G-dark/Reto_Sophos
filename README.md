# Reto_Sophos
 A CRUD alike project
This project was develoved for the course Sophos a VIP course of universidad del norte, and it was made using the net core platform what uses c# programing language. 
# Instalation 
1. Make git clone or download the source code from this page.
2. Download sqlserver database engine, this is to use the creation and poblation scripts
# Configuration 
1. Open the creation and poblation scripts files, copy and paste inside of a management database program for sql server.
2. Once created and poblated you must change the user and password for yours in the following configuration line:
          Data Source=ELI\\SQLEXPRESS;Initial Catalog=MiBaseDeDatos; User ID=sa;Password=1234567; Encrypt=False"
   You have to change this in two places in the appsettings.json file and appdbcontext class
# Execution 
1. Run the app in the Reto_sophos2_ profile
   
